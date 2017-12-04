using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother : MonoBehaviour {

    public Transform door;

    [SerializeField]
    float walkSpeed = 1f;

    SpriteRenderer sRend;

    private void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
    }

    void Update () {
        Vector3 direction = (door.transform.position - transform.position).normalized;
        sRend.flipX = direction.x < 0;
        transform.position +=  direction * walkSpeed * Time.deltaTime;
	}
}
