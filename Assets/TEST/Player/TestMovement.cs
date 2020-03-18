using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public float speed = 500f;

    private Rigidbody playerBody;

    private float vertical;
    private float horizontal;

    public float jumpForce = 10;

    private Vector3 velocity;

    bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Jump();
        Move();
        
    }

    /// <summary>
    /// The player moves by setting their rigidbody's velocity rather than using transform.translate or addForce
    /// </summary>
    private void Move()
    {
        Vector3 velocityX = playerBody.velocity;
        Vector3 velocityY = playerBody.velocity;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        velocityX = Camera.main.transform.right * horizontal;
        velocityY = Camera.main.transform.forward * vertical;
        
        velocity = Vector3.Normalize(velocityX + velocityY);

        velocity.y = playerBody.velocity.y / speed / Time.deltaTime;

        playerBody.velocity = velocity * speed * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetAxis("Jump") != 0 && grounded)
        {
            playerBody.velocity = new Vector3(playerBody.velocity.x, jumpForce, playerBody.velocity.y);
            grounded = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}