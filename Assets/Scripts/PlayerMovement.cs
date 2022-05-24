using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5;
    [SerializeField] float jumpSpeed = 10f;
   
    Vector2 moveInput;
    
    Rigidbody2D rb;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    
    Animator anim;
    private const string IS_RUNNING = "isRunning";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
    }   

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        Die();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!IsPlayerTouchingGround()) { return; }
        if (value.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;

        anim.SetBool("isRunning", PlayerHasHorizontalSpeed());
    }

    void FlipSprite()
    {
        if (PlayerHasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    bool PlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
    }

    bool IsPlayerTouchingGround()
    {
        return feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void Die()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            Debug.Log("I'm dead");
        }
    }
}
