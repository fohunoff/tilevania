using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;

    Vector2 moveInput;
    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Run();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        Debug.Log(moveInput);
    }

    void Run()
    {
        float xVelocity = moveInput.x * runSpeed;
        float yVelocity = rb2d.velocity.y;

        Vector2 playerVelocity = new Vector2(xVelocity, yVelocity);
        
        rb2d.velocity = playerVelocity;
    }
}
