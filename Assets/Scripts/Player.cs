using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [Header("GroundCheck")]
    [SerializeField] Collider2D standingCollider;
    [SerializeField] Transform groundCheckCollider;
    [Header("Crouch")]
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;


    const float groundCheckRadius = 0.05f;
    const float overheadCheckRadius = 0.05f;
    [SerializeField] float speed = 1;
    [SerializeField] float jumpPower = 50;
    float horizontalValue;
    float runSpeedModifier = 1.3f;
    float crouchSpeedModifier = 0.5f;

    bool isGrounded = true;
    bool isRunning;
    bool facingRight = true;
    bool jump;
    bool crouchPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (!CanMove() == true)
        {
            //if LShift is clicked enable isRunning
            if (Input.GetKeyDown(KeyCode.LeftShift))
                isRunning = true;
            //if LShift is released disable isRunning
            if (Input.GetKeyUp(KeyCode.LeftShift))
                isRunning = false;
            return;
        }

            

        //Set the yVelocity in the animator
        animator.SetFloat("yVelocity", rb.velocity.y);

        // Store the horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");

       
        //if we press jump button enable jump
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("Jump", true);
        }
        //Otherwise disable it
        else if (Input.GetButtonUp("Jump"))
            jump = false;

        //if we press Crouch button enable Crouch
        if (Input.GetButtonDown("Crouch"))
            crouchPressed = true;
        //Otherwise disable it
        else if (Input.GetButtonUp("Crouch"))
            crouchPressed = false;
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, jump, crouchPressed);
    }

    void GroundCheck()
    {
        isGrounded = false;

        //Check if the Ground Check Object is colliding with other
        //2D Colliders that are in the "Ground" Layer
        //If yes (isGrounded true) else (isGrounded false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
            isGrounded = true;

        //as long as we are grounded the "Jump" bool
        //in the animator is disabled
        animator.SetBool("Jump", !isGrounded);
    }

    void Move(float dir, bool jumpFlag, bool crouchFlag)
    {
        #region Jump & Crouch

        //if we are crouching and disabled crouching
        //Check overhead for collision with ground items
        //if there  are any, remain crouched, otherwise un-crouch
        if (!crouchFlag)
        {
            if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
            {
                crouchFlag = true;
            }
        }
        

        if(isGrounded)
        {

            //if we press Crouch we disable the standing coliider + animate crouching
            //Reduce the speed
            //if realeased resume the original speed
            //enable the standing collider & disable crouch animation
            standingCollider.enabled = !crouchFlag;

            //If the player is grounded and pressed space Jump
            if (jumpFlag)
            {
                jumpFlag = false;
                //isGrounded = false;
                //Add Jump Force
                rb.AddForce(new Vector2(0f, jumpPower));
            }
        }

        animator.SetBool("Crouch", crouchFlag);
       
        #endregion
        #region Move & Run
        //set value of x using dir and speed
        float xVal = dir * speed * 50 * Time.fixedDeltaTime;
        
        //If we are running multiply with the running modifier
        if (isRunning)
            xVal *= runSpeedModifier;

        //If we are crouch reduce the speed with the crouch modifier
        if (crouchFlag)
            xVal *= crouchSpeedModifier;

        //create Vec2 for the velocity
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        //set the player's velocity
        rb.velocity = targetVelocity;


        //if looking right and clicked left (flip to the left)
        if(facingRight && dir < 0)
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 1);
            facingRight = false;
        }
        //if looking right and clicked left (flip to the left)
        else if(!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(-0.1f, 0.1f, 1);
            facingRight = true;
        }

        //0 idle, 4 walking, 8 running
        //Set the float xVelocity according to the x value
        //of the RigidBody2D velocity
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));

        #endregion


    }

    #region Gizmos
    //setting gizmos for interaction
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(overheadCheckCollider.position, overheadCheckRadius);
    }
    #endregion

    bool CanMove()
    {
        //can move when Interact Something
        bool can = true;

        if (FindObjectOfType<InteractionSystem>().isExamining)
            can = false;

        //can move when Opened Inventory
        if (FindObjectOfType<InventorySystem>().isOpen)

            can = false;
        return can;
    }
}