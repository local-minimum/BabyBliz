using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerStatus playerStatus;

    public float jumpForce;
    public float walkForce;

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
        float energy = playerStatus.Energy;

	    if (Input.GetButtonDown("Jump") && Grounded)
        {
            rb.AddForce(Vector2.up * jumpForce * energy);
            
        }
        float horizontal = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontal) > 0.01)
        {
            rb.AddForce(Vector2.right * walkForce * energy * horizontal * (grounded ? 1f : jumpingWalkForceFactor));

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
            return grounders.Count > 0 || (Time.timeSinceLevelLoad - lastGrounded < groundingTime);
        }
    }

    HashSet<Transform> grounders = new HashSet<Transform>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounders.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            lastGrounded = Time.timeSinceLevelLoad;
            grounders.Remove(collision.transform);
        }
    }
}
