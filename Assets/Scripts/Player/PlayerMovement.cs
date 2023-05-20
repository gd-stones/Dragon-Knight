using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Explain code - https://chat.openai.com/c/8338f62a-d917-4560-9c34-5fcbd93efe98
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //how much time the player can hang in the air before jumping
    private float coyoteCounter; //how much time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    [Header("Sound")]
    [SerializeField] private AudioClip jumpSound;
    protected PlayerActionsExample playerInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerInput = new PlayerActionsExample();
    }

    public void ChangeDirection(float value)
    {
        horizontalInput = value;
    }

    public void ResetDirection()
    {
        horizontalInput = 0f;
    }

    private void Update()
    {
#if UNITY_EDITOR
        horizontalInput = Input.GetAxis("Horizontal");
#endif

        //        Vector2 movement = playerInput.Player.Move.ReadValue<Vector2>();
        //        Debug.Log(movement);

        //#if UNITY_STANDALONE
        //        horizontalInput = Input.GetAxis("Horizontal");
        //#else
        //        horizontalInput = movement.x;
        //#endif

        // Flip player when moving left-right https://youtu.be/Gf8LOFNnils?list=PLgOEwFbvGm5o8hayFB6skAfa8Z-mw4dPV&t=277
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            //print("Jump();");
        }

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
            //print("Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0");
        }

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //reset coyote counter when on the ground
                jumpCounter = extraJumps; //reset jump counter to extra jump value 
            }
            else
            {
                coyoteCounter -= Time.deltaTime; //start decreasing coyote counter when not on the ground
            }
        }
    }


    public bool CanJump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return false;
        return true;
    }

    public void Jump()
    {
        SoundManager.instance.PlaySound(jumpSound);
        anim.SetTrigger("jump");

        if (onWall())
        {
            WallJump();
        }
        else
        {
            if (isGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
            else
            {
                //if not on the ground and coyote counter bigger than 0 do a normal jump
                if (coyoteCounter > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    //print("coyoteCounter > 0");
                }
                else
                {
                    if (jumpCounter > 0) //if we have extra jumps then jump and decrease the jump counter
                    {
                        //print("jumpCounter > 0");
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            //reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
