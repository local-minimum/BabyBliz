using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParalax : MonoBehaviour {

    Vector3 origin;
    [SerializeField]
    CameraController cameraController;

    float speed;

	void Start () {
        origin = transform.position;
        speed = Mathf.Exp(origin.z);
	}
	
	void Update () {

        transform.position = origin + new Vector3(
            cameraController.DeltaXFromOrigin / speed, 0, 0);
	}
}
