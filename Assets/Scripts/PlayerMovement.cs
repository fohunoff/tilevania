using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float defaultGravityScale = 8f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 20f);

    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimatior;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimatior = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();

        myRigidbody.gravityScale = defaultGravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        ClimbLadder();

        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void Run()
    {
        float xVelocity = moveInput.x * runSpeed;
        float yVelocity = myRigidbody.velocity.y;

        Vector2 playerVelocity = new Vector2 (xVelocity, yVelocity);
        myRigidbody.velocity = playerVelocity;

        // для фикса игры на геймпаде
        if (Mathf.Abs(myRigidbody.velocity.x) < 0.02f)
        {
            myAnimatior.SetBool("isRunning", false);
            return;
        }

        bool isRunning = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimatior.SetBool("isRunning", isRunning);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            float xScale = Mathf.Sign(myRigidbody.velocity.x);
            float yScale = 1f;

            transform.localScale = new Vector2 (xScale, yScale);
        }
    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladders"))) {
            myRigidbody.gravityScale = defaultGravityScale;
            myAnimatior.SetBool("isClimbing", false);
            return;
        }

        myRigidbody.gravityScale = 0;

        float xVelocity = myRigidbody.velocity.x;
        float yVelocity = moveInput.y * climbSpeed;

        Vector2 climbVelocity = new Vector2 (xVelocity, yVelocity);
        myRigidbody.velocity = climbVelocity;

        bool isClimbing = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimatior.SetBool("isClimbing", isClimbing);
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimatior.SetTrigger("Dying");

            myRigidbody.velocity = deathKick;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
