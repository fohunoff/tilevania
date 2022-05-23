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

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimatior;
    CapsuleCollider2D myCapsuleCollider;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimatior = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();

        myRigidbody.gravityScale = defaultGravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed);
        }
    }

    void Run()
    {
        float xVelocity = moveInput.x * runSpeed;
        float yVelocity = myRigidbody.velocity.y;

        Vector2 playerVelocity = new Vector2 (xVelocity, yVelocity);
        myRigidbody.velocity = playerVelocity;

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
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladders"))) {
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
}
