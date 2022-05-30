using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5;
    [SerializeField] float jumpSpeed = 10f;

    [SerializeField] AudioPlayer audioPlayer;
   
    Vector2 moveInput;
    
    Rigidbody2D rb;
    BoxCollider2D feetCollider;
    PlayerTransformer playerTransformer;
    
    Animator anim;
    private const string IS_RUNNING = "isRunning";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        feetCollider = GetComponent<BoxCollider2D>();
        playerTransformer = GetComponent<PlayerTransformer>();
    }   

    // Update is called once per frame
    void Update()
    {
        if(playerTransformer.LifeState == PlayerState.Dead) { return; }
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (playerTransformer.LifeState == PlayerState.Dead) { return; }
        if (playerTransformer.LifeState == PlayerState.TemporaryAlive) { return; }
        if((playerTransformer.LifeState == PlayerState.Alive) && (!IsPlayerTouchingGround())) { return; }
        if (value.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpSpeed);
            audioPlayer.PlayJumpSound();
        }
    }

    void Run()
    {
        if (playerTransformer.LifeState == PlayerState.Dead) { return; }
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;

        anim.SetBool(IS_RUNNING, PlayerHasHorizontalSpeed());
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
}
