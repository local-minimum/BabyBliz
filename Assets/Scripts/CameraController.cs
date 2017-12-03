using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    CameraTrack track;

    public PlayerController player;

    [SerializeField]
    Rect noMove;

    [SerializeField]
    float attack = 0.9f;

    Camera myCam;

    public Camera Cam
    {
        get
        {
            return myCam;
        }
    }

    private void Start()
    {
        myCam = GetComponent<Camera>();
        orthoGraphicSizeAreaTarget = myCam.orthographicSize;
    }

    Vector3 playerTrackPosition;
    Vector2 playerMovement;
    Vector3 playerPosition;

    [SerializeField]
    float aimAttack = 0.4f;

    public float DeltaXFromOrigin
    {
        get
        {
            return transform.parent.position.x - transform.position.x;
        }
    }

    Vector3 camOffset = new Vector3(0, 1f, -10f);
    [SerializeField]
    AnimationCurve xLookAhead;

    void Update () {
        Vector3 trackPoint = track.GetClosestPointOnTrack(player.transform.position);
        float yError = Mathf.Max(player.transform.position.y - transform.position.y, 1);
        float xFactor = player.VelocityX;
        transform.position = Vector3.Lerp(transform.position, trackPoint + camOffset + Vector3.up * Mathf.Max(yError - 1) * 5f + Vector3.right * xLookAhead.Evaluate(xFactor), aimAttack);
        myCam.orthographicSize = orthoGraphicSizeAreaTarget * Mathf.Min(Mathf.Pow(yError, 0.1f), 1.5f);
	}

    CameraZone camZone;

    public void SetZoomArea(CameraZone zone)
    {
        if (camZone != zone)
        {
            camZone = zone;
            StartCoroutine(_animateSize(zone.zoomSize, zone.zoomDuration, zone));
        }
    }
    float orthoGraphicSizeAreaTarget;

    IEnumerator<WaitForSeconds> _animateSize(float size, float duration, CameraZone zone)
    {
        float startT = Time.timeSinceLevelLoad;
        float progress = 0;
        float startSize = orthoGraphicSizeAreaTarget;
        while (progress < 1f && zone == camZone)
        {
            progress = Mathf.Clamp01( (Time.timeSinceLevelLoad - startT) / duration );
            orthoGraphicSizeAreaTarget = Mathf.Lerp(startSize, size, progress);
            yield return new WaitForSeconds(0.0015f);
        }
        orthoGraphicSizeAreaTarget = size;
    }
}
