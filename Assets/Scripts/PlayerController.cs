using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;        // Movement speed of the player
    public float jumpForce = 10f;       // Force applied when the player jumps
    int jumps = 1;
    public float crouchSpeedMultiplier = 0.5f;  // Multiplier for crouch movement speed

    public bool isGrounded;            // Check if the player is grounded
    private bool isCrouching;           // Check if the player is crouching
    private Rigidbody2D rb;             // Reference to the Rigidbody2D component
    public bool facingRight;
    public GameObject flashPivot;
    public GameObject feet;
    public SpriteRenderer blackness;
    public Animator anim;
    public float groundedRange = 0.3f;
    float horizontalInput;
    public float maxClimbAngle = 60f;
    bool isJumping;
    bool lampOn;
    public Slider slide;
    public float stamina = 1000f;
    public float lampSpeed = 0.1f; 
    bool noEnergy;
    public GameObject restartButton;
    public GameObject jumpScare;
    public GameObject bloodParticle;
    //public AudioManager audioManager;
    AudioSource audioSource;
    public AudioClip[] audioClipArray;
    public bool isOnLava = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckFloor();

        if(Input.GetKeyDown(KeyCode.Z))
        {
            lampOn = !lampOn;
            audioSource.PlayOneShot(audioClipArray[2]);
        }

        if (Input.GetKeyDown(KeyCode.X) && !lampOn && horizontalInput == 0)
        {
            audioSource.PlayOneShot(audioClipArray[3]);
            anim.SetBool("isRecharging", true);
            noEnergy = false;
            stamina += 100f;
            slide.value = stamina;
            if(stamina > 1000f)
            {
                stamina = 1000f;
            }
        } else if (Input.GetKeyUp(KeyCode.X))
        {
            anim.SetBool("isRecharging", false);
        }
        
        if(lampOn && !noEnergy || isOnLava)
        {
            stamina -= lampSpeed;
            slide.value = stamina;
            if(stamina < 0.5f)
            {
                noEnergy = true;
            }

            Color roomColor = blackness.color;
            roomColor.a = 0.85f;
            blackness.color = roomColor;
            flashPivot.SetActive(true);
            CheckLamp();
        } else {
            Color roomColor = blackness.color;
            roomColor.a = 0.996f;
            blackness.color = roomColor;
            flashPivot.SetActive(false);
        }

        // Handle player input for movement
        horizontalInput = Input.GetAxis("Horizontal");
        
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        // Move the player horizontally
        MovePlayer(moveDirection);

        // Handle player input for jumping
        if (Input.GetButtonDown("Jump") && jumps > 0f)
        {
            Jump();
            audioSource.PlayOneShot(audioClipArray[1]);
        }

        // Handle player input for crouching
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            audioSource.PlayOneShot(audioClipArray[3]);
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            Uncrouch();
        }
    }

    void CheckFloor()
    {
        //Slope
        RaycastHit2D hit = Physics2D.Raycast(feet.transform.position, Vector2.down,groundedRange, LayerMask.GetMask("Ground"));

        // Check if the player is grounded using a raycast
        isGrounded = hit;

        if(hit)
        {
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
            jumps = 1;
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle <= maxClimbAngle)
            {
                ClimbSlope(rb.velocity, slopeAngle);
            }
        }
    }

    private bool isWalkingSoundActive = false;

    void MovePlayer(Vector2 moveDirection)
    {
        if(isGrounded && horizontalInput != 0)
        {
            anim.SetBool("walk", true);
            
            if(!isWalkingSoundActive)
            {
                isWalkingSoundActive  = true;
                audioSource.PlayOneShot(audioClipArray[0]);
            }
        } else 
        {
            anim.SetBool("walk", false);
            if(isWalkingSoundActive)
            {
                isWalkingSoundActive  = false;
                audioSource.Stop();
            }
        }

        if (horizontalInput > 0f && !facingRight) 
        {
            Flip();
        } else if(horizontalInput < 0f && facingRight) 
        {
            Flip();
        } else if(horizontalInput == 0)
        {
            anim.SetBool("walk", false);
        }

        // Apply horizontal movement
        if (!isCrouching)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * crouchSpeedMultiplier, rb.velocity.y);
        }

        if(!isGrounded)
        {
            if(rb.velocity.y < -0.1)
            {
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            } else
            { 
                anim.SetBool("isFalling", false);
                anim.SetBool("isJumping", true);
            }
        }    
    }

    void CheckLamp()
    {
        

        if(Input.GetAxis("Vertical") > 0f && facingRight)
        {
            flashPivot.transform.rotation = Quaternion.Euler(new Vector3(0,0,90)); 
        } else if(Input.GetAxis("Vertical") == 0f)
        {
            flashPivot.transform.rotation = Quaternion.Euler(new Vector3(0,0,0)); 
        } else if(Input.GetAxis("Vertical") > 0f && !facingRight)
        {
            flashPivot.transform.rotation = Quaternion.Euler(new Vector3(0,0,-90)); 
        }
    }

    void ClimbSlope(Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
    }

    void Jump()
    {
        if (jumps > 0)
        {
            // Apply vertical force for jumping
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumps--;
        }
    }

    void Crouch()
    {
        anim.SetBool("isCrouching", true);
        // Crouch by halving the collider height and adjusting the position
        isCrouching = true;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(collider.size.x, collider.size.y / 2);
        collider.offset = new Vector2(collider.offset.x, collider.offset.y / 2);
    }

    void Uncrouch()
    {
        anim.SetBool("isCrouching", false);
        // Uncrouch by restoring the original collider size and position
        isCrouching = false;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(collider.size.x, collider.size.y * 2);
        collider.offset = new Vector2(collider.offset.x, collider.offset.y * 2);
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            stamina -= 50f;
            slide.value = stamina;
            anim.SetTrigger("Damage");
            Instantiate(bloodParticle, transform.position, Quaternion.identity);
        }
    }

    public void Damage()
    {
        anim.SetTrigger("Damage");
        restartButton.SetActive(true);
        jumpScare.SetActive(true);
        Instantiate(bloodParticle, transform.position, Quaternion.identity);
    }

    public void DamageEnemy()
    {
        audioSource.PlayOneShot(audioClipArray[4]);
    }

    public void LavaFloor()
    {
        audioSource.PlayOneShot(audioClipArray[6]);
        Instantiate(bloodParticle, transform.position, Quaternion.identity);
    }
}
