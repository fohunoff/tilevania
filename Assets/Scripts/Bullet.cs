using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;

    Rigidbody2D myRigidbody;
    PlayerMovement player;

    float xSpeed;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();

        xSpeed = player.transform.localScale.x * bulletSpeed;

        float xScale = Mathf.Sign(xSpeed);
        float yScale = 1f;
        transform.localScale = new Vector2 (xScale, yScale);
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        
        Destroy(gameObject);
    }
}
