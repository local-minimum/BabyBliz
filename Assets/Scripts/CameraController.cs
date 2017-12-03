using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    CameraTrack track;

    public Transform player;

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
        //StartCoroutine(_updateTarget());
    }

    Vector3 playerTrackPosition;
    Vector2 playerMovement;
    Vector3 playerPosition;

    [SerializeField]
    float aimAttack = 0.4f;

    IEnumerator<WaitForSeconds> _updateTarget()
    {
        while (true)
        {
            playerMovement = player.position - playerPosition;
            playerTrackPosition = Vector3.Lerp(
                playerTrackPosition, player.position, aimAttack);
            playerPosition = player.position;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public float DeltaXFromOrigin
    {
        get
        {
            return transform.parent.position.x - transform.position.x;
        }
    }

    Vector2 PlayerOnScren()
    {
        Vector2 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector2 relPos = new Vector2(playerPos.x / Screen.width, playerPos.y / Screen.height);
        Vector2 direction = relPos - new Vector2(0.5f, 0.5f);
        return relPos;
    }



    void Update () {

        transform.position = Vector3.Lerp(transform.position, track.GetClosestPointOnTrack(player.position) + new Vector3(0, 1f, -10f), 0.2f);
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

    IEnumerator<WaitForSeconds> _animateSize(float size, float duration, CameraZone zone)
    {
        float startT = Time.timeSinceLevelLoad;
        float progress = 0;
        float startSize = myCam.orthographicSize;
        while (progress < 1f && zone == camZone)
        {
            progress = Mathf.Clamp01( (Time.timeSinceLevelLoad - startT) / duration );
            myCam.orthographicSize = Mathf.Lerp(startSize, size, progress);
            yield return new WaitForSeconds(0.0015f);
        }
        myCam.orthographicSize = size;
    }
}
