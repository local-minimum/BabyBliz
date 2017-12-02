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

    private void Start()
    {
        attachmentJoint = GetComponent<SpringJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.tag == "Player")
        {
            collision.SendMessage("PickupBaby", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void SetAttachment(GameObject go)
    {
        attachmentJoint.connectedBody = go.GetComponent<Rigidbody2D>();
        attachmentJoint.distance = Random.Range(minDistance, maxDistance);
        attachmentJoint.enabled = true;
    }
}
