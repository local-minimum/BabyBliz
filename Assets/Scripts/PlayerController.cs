﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerStatus playerStatus;

    public float jumpForce;
    public float walkForce;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spriteRend;

    bool isAlive = true;
    float lastGrounded;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
	}

    public Vector2 maxVelocities;

    [Range(0, 1)]
    public float jumpingWalkForceFactor = 0.5f;
	// Update is called once per frame
	void Update () {

        if (!isAlive)
        {
            return;
        }

        bool grounded = Grounded;        
        float horizontal = Input.GetAxis("Horizontal");

        if (Grounded)
        {
            Walk(horizontal);
        } else if (Submerged)
        {
            Swim(horizontal);
        } else
        {
            JumpingControl(horizontal);
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxVelocities.x, maxVelocities.x), Mathf.Clamp(rb.velocity.y, -maxVelocities.y, maxVelocities.y));        
    }

    [SerializeField, Range(0, 1)]
    float swimFactor = 0.3f;

    void JumpingControl(float horizontal)
    {
        if (Mathf.Abs(horizontal) > 0.01f)
        {
            rb.AddForce(Vector2.right * walkForce * playerStatus.Energy * horizontal * jumpingWalkForceFactor);
        }
    }

    [SerializeField]
    float maxWaterSpeed = 4f;

    [SerializeField]
    float babyBuoyancy = 0.03f;

    void Swim(float horizontal)
    {
        float babies = playerStatus.BabyCount;
        rb.gravityScale = swimGravityScale - babyBuoyancy * babies;
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce * swimFactor * playerStatus.JumpEnergy);

        }

        if (Mathf.Abs(horizontal) > 0.01)
        {
            rb.AddForce(Vector2.right * walkForce * playerStatus.Energy * horizontal);

        }

        float vMagnitude = Mathf.Abs(rb.velocity.x) + Mathf.Min(0, rb.velocity.y) * -1f;
        if (vMagnitude > maxWaterSpeed)
        {
            rb.velocity = rb.velocity / vMagnitude * maxWaterSpeed;
        }
    }

    [SerializeField]
    float xDampening = 0.1f;

    public float VelocityX
    {
        get
        {
            return rb.velocity.x;
        }
    }

    float lastJump;

    void Walk(float horizontal)
    {
        if (Input.GetButtonDown("Jump") && Time.timeSinceLevelLoad - lastJump > 0.35f)
        {
            rb.AddForce(Vector2.up * jumpForce * playerStatus.JumpEnergy);
            anim.SetTrigger("Jump");
            lastJump = Time.timeSinceLevelLoad;
        }

        if (Input.GetButton("Horizontal"))
        {
            rb.AddForce(Vector2.right * walkForce * playerStatus.Energy * horizontal);            
        } else
        {
            float newX = rb.velocity.x * Mathf.Max(1 - xDampening * Time.deltaTime, 0);
            rb.velocity = new Vector2(newX, rb.velocity.y);
        }

        float absX = Mathf.Abs(rb.velocity.x);
        if (absX > 0.01f)
        {
            spriteRend.flipX = rb.velocity.x < 0f;
        }        
        anim.SetFloat("WalkSpeed", absX);
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, 0));
    }

    public float groundingTime = 0.1f;

    public bool Grounded
    {
        get
        {
            return grounders.Count > 0 || (Time.timeSinceLevelLoad - lastGrounded < groundingTime);
        }
    }

    public bool Submerged
    {
        get
        {
            return waters.Count > 0;
        }
    }

    HashSet<Transform> grounders = new HashSet<Transform>();
    HashSet<Transform> waters = new HashSet<Transform>();

    [SerializeField, Range(0, 1)]
    float swimGravityScale = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.tag == "Ground" && !collision.isTrigger)
        {
            grounders.Add(collision.transform);
            if (grounders.Count == 1)
            {
                anim.SetTrigger("Land");
            }
        } else if (collision.gameObject.tag == "Water")
        {
            waters.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            lastGrounded = Time.timeSinceLevelLoad;
            grounders.Remove(collision.transform);
        }
        else if (collision.gameObject.tag == "Water")
        {
            waters.Remove(collision.transform);
            if (waters.Count == 0)
            {
                rb.gravityScale = 1f;
            }
        }
    }

    public void Kill(KilledBy reason)
    {
        if (isAlive)
        {
            anim.SetTrigger("Kill");
            GetComponent<CapsuleCollider2D>().size = new Vector2(0.1f, 0.1f);
        }
        isAlive = false;
        
    }
}
