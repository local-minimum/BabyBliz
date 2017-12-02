using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {

    public float zoomSize;
    public float zoomDuration;

    [SerializeField]
    BoxCollider2D boxCollider;

    [SerializeField]
    CameraController cam;

    private void Update()
    {
        if (boxCollider.bounds.IntersectRay(cam.Cam.ScreenPointToRay(new Vector3(0.5f * Screen.width, 0.5f * Screen.height))))
        {
            cam.SetZoomArea(this);
        }
    }
}
