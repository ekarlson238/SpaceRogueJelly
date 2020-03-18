using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float speed, jumpForce;

    private Rigidbody rb;

    private bool canJump = false;

    private Vector3 v;
    private Vector3 h;

    private float multiplier;

    [SerializeField]
    private float gravity = 20;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();

        v = Vector3.zero;
        h = Vector3.zero;

        multiplier = 1;

        Physics.gravity = new Vector3(0, -gravity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            v = rb.transform.forward * Input.GetAxis("Vertical");
            h = rb.transform.right * Input.GetAxis("Horizontal");
            multiplier = Mathf.Clamp(multiplier, speed / 3, speed);
        }
        else if (canJump)
        {
            multiplier /= 2;
        }

        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && canJump)
        {
            multiplier *= speed / 5;
        }

        multiplier = Mathf.Clamp(multiplier, 1, speed);
        Vector3 total = Vector3.Normalize(v + h) * multiplier * Time.deltaTime;
        rb.velocity = new Vector3(total.x, rb.velocity.y, total.z);
    }

    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
