using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

   
    public float jumpForce;
    public float walkForce;


    Rigidbody2D rb;
    Animation anim;
    
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce);
            
        }
        float horizontal = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontal) > 0.01)
        {
            rb.AddForce(Vector2.right * walkForce * horizontal);

        }

    
    }


}
