using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;

    [SerializeField]
    Rect noMove;

    [SerializeField]
    float attack = 0.9f;

    private void Start()
    {
        StartCoroutine(_updateTarget());
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

    Vector2 PlayerOnScren()
    {
        Vector2 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector2 relPos = new Vector2(playerPos.x / Screen.width, playerPos.y / Screen.height);
        Vector2 direction = relPos - new Vector2(0.5f, 0.5f);
        return relPos;
    }

    void Update () {

        transform.position = player.position + new Vector3(0, 1f, -10f);
	}
}
