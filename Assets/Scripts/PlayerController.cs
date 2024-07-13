using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;        // Movement speed of the player
    public float jumpForce = 10f;       // Force applied when the player jumps
    public float crouchSpeedMultiplier = 0.5f;  // Multiplier for crouch movement speed

    public bool isGrounded;            // Check if the player is grounded
    private bool isCrouching;           // Check if the player is crouching
    private Rigidbody2D rb;             // Reference to the Rigidbody2D component
    public bool facingRight;
    public GameObject flashPivot;
    public GameObject feet;
    public Animator anim;
    public float groundedRange = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is grounded using a raycast
        isGrounded = Physics2D.Raycast(feet.transform.position, Vector2.down,groundedRange, LayerMask.GetMask("Ground"));

        // Handle player input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);

        if (horizontalInput > 0f && !facingRight) 
        {
            anim.SetBool("walk", true);
            Flip();
        } else if(horizontalInput < 0f && facingRight) 
        {
            anim.SetBool("walk", true);
            Flip();
        } else if(horizontalInput == 0)
        {
            anim.SetBool("walk", false);
        }
 
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



        // Move the player horizontally
        MovePlayer(moveDirection);

        // Handle player input for jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Handle player input for crouching
        if (Input.GetKeyDown(KeyCode.X))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            Uncrouch();
        }
    }

    void MovePlayer(Vector2 moveDirection)
    {
        // Apply horizontal movement
        if (!isCrouching)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * crouchSpeedMultiplier, rb.velocity.y);
        }
    }

    void Jump()
    {
        // Apply vertical force for jumping
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void Crouch()
    {
        // Crouch by halving the collider height and adjusting the position
        isCrouching = true;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(collider.size.x, collider.size.y / 2);
        collider.offset = new Vector2(collider.offset.x, collider.offset.y / 2);
    }

    void Uncrouch()
    {
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
}
