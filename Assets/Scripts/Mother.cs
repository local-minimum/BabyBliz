using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother : MonoBehaviour {

    public Transform door;

    [SerializeField]
    float walkSpeed = 1f;

	void Update () {
        Vector3 direction = (door.transform.position - transform.position).normalized;
        transform.position +=  direction * walkSpeed * Time.deltaTime;
	}
}
