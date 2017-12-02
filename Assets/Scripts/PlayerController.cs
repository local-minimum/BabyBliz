using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

   
    public float jumpForce;
    public float walkForce;

    public PlayerBag bag;
    Rigidbody2D rb;
    Animation anim;

    float lastGrounded;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    public Vector2 maxVelocities;

    [Range(0, 1)]
    public float jumpingWalkForceFactor = 0.5f;
	// Update is called once per frame
	void Update () {

        bool grounded = Grounded;

	    if (Input.GetButtonDown("Jump") && Grounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            
        }
        float horizontal = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontal) > 0.01)
        {
            rb.AddForce(Vector2.right * walkForce * horizontal * (grounded ? 1f : jumpingWalkForceFactor));

        }

        
        if (grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, 0));
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxVelocities.x, maxVelocities.x), Mathf.Clamp(rb.velocity.y, -maxVelocities.y, maxVelocities.y));
    }

    public float groundingTime = 0.1f;

    public bool Grounded
    {
        get
        {
            return (Time.timeSinceLevelLoad - lastGrounded < groundingTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            lastGrounded = Time.timeSinceLevelLoad;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            lastGrounded = Time.timeSinceLevelLoad;
        }
    }
}
