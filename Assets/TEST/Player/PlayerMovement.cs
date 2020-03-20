using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float speed, jumpForce;

    private Rigidbody rb;

    private bool canJump = false;
    //TODO fix the name
    private bool chargedJump = false;
    [SerializeField]
    private float chargedJumpingTime = 3;
    private float chargedJumpingCounter = 0;

    private Vector3 v;
    private Vector3 h;

    private float speedMultiplier;
    private float jumpMultiplier;

    [SerializeField]
    private float gravity = 20;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();

        v = Vector3.zero;
        h = Vector3.zero;

        speedMultiplier = 1;
        jumpMultiplier = jumpForce;

        Physics.gravity = new Vector3(0, -gravity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Jump") == 0)
        {
            Move();
        }

        Jump();
    }

    private void Move()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            v = rb.transform.forward * Input.GetAxis("Vertical");
            h = rb.transform.right * Input.GetAxis("Horizontal");
            speedMultiplier = Mathf.Clamp(speedMultiplier, speed / 3, speed);
        }
        else if (canJump)
        {
            speedMultiplier /= 2;
        }

        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && canJump)
        {
            speedMultiplier *= speed / 5;
        }

        speedMultiplier = Mathf.Clamp(speedMultiplier, 1, speed);
        Vector3 total = Vector3.Normalize(v + h) * speedMultiplier * Time.deltaTime;
        rb.velocity = new Vector3(total.x, rb.velocity.y, total.z);
    }

    private void Jump()
    {
        if (canJump && Input.GetAxis("Jump") == 0)
        {
            canJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (jumpMultiplier == jumpForce)
            {
                rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
                jumpMultiplier = jumpForce;
            }
        }
        else if (jumpMultiplier != jumpForce && !canJump && chargedJumpingCounter <= chargedJumpingTime)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpMultiplier, rb.velocity.z);

            chargedJumpingCounter += Time.deltaTime;
        }
        else if (canJump)
        {
            jumpMultiplier += Time.deltaTime * 3;
            jumpMultiplier = Mathf.Clamp(jumpMultiplier, jumpForce, jumpMultiplier * 1.5f);
        }
        else
        {
            jumpMultiplier = jumpForce;
            chargedJumpingCounter = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }
}
