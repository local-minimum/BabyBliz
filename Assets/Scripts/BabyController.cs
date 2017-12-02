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

    private void Start()
    {
        attachmentJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.tag == "Player")
        {
            collision.SendMessage("PickupBaby", this, SendMessageOptions.DontRequireReceiver);
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

    private void Update()
    {

        if (!attachmentTransform)
        {
            if (nextAction < Time.timeSinceLevelLoad)
            {
                nextAction = Random.Range(minNextAction, maxNextaction) + Time.timeSinceLevelLoad;
                xDirection = Random.Range(-1f, 1f);
            } else
            {
                rb.AddForce(new Vector2(xDirection * crawlSpeed, 0));

            }
        }   
    }
}
