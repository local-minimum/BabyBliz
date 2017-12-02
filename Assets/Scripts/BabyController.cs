using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyController : MonoBehaviour {
    
    Transform attachmentTransform;
    SpringJoint2D attachmentJoint;

    [SerializeField]
    float minDistance = 0.05f;

    [SerializeField]
    float maxDistance = 0.3f;

    Rigidbody2D rb;

    public bool dontMove;
    public bool floating;

    private void Start()
    {
        attachmentJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !floating)
        {
            collision.SendMessage("PickupBaby", this, SendMessageOptions.DontRequireReceiver);
        } else if (collision.tag == "Water")
        {
            rb.gravityScale = -.1f;           
            floating = true;
            dontMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            floating = false;
            rb.gravityScale = 1f;            
        }
    }

    public void SetAttachment(GameObject go, Rect attachmentArea)
    {
        attachmentJoint.connectedBody = go.GetComponent<Rigidbody2D>();
        attachmentJoint.distance = Random.Range(minDistance, maxDistance);
        attachmentJoint.enabled = true;
        attachmentJoint.connectedAnchor =
            new Vector2(
                Mathf.Lerp(attachmentArea.min.x, attachmentArea.max.x, Random.value),
                Mathf.Lerp(attachmentArea.min.y, attachmentArea.max.y, Random.value)
            );

        floating = false;
        rb.gravityScale = 1f;
    }

    Vector2 aim;
    [SerializeField]
    float minNextAction = 5f;

    [SerializeField]
    float maxNextaction = 20f;

    float nextAction;
    float xDirection = 0;

    [SerializeField]
    float crawlSpeed;

    [SerializeField]
    CapsuleCollider2D capsulCollider;

    private void Update()
    {

        if (attachmentTransform == null)
        {
            if (floating)
            {
                rb.velocity = rb.velocity * 0.9f;
            }

            if (dontMove != true)
            {
                if (nextAction < Time.timeSinceLevelLoad)
                {
                    nextAction = Random.Range(minNextAction, maxNextaction) + Time.timeSinceLevelLoad;
                    xDirection = Random.Range(-1f, 1f);
                }
                else
                {
                    rb.AddForce(new Vector2(xDirection * crawlSpeed, 0));
                    if (Random.value < 0.1f)
                        capsulCollider.size = new Vector2(Random.Range(0.7f, 0.9f), Random.Range(0.8f, 1.0f));
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!floating && collision.gameObject.tag == "Ground")
        {
            dontMove = false;
        }
    }
}
