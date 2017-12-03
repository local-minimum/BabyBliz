using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraTrack : MonoBehaviour {

    [SerializeField]
    Transform[] track;

    public Vector3 GetClosestPointOnTrack(Vector3 position)
    {
        Transform[] closest = track.OrderBy(current => (current.position - position).sqrMagnitude).Take(2).ToArray();
        Vector2 origin = closest[0].position;
        Vector2 direction = (closest[1].position - closest[0].position).normalized;
        Vector2 pos2D = position;
        float t = Vector2.Dot((pos2D - origin), direction);
        Vector2 target = origin + direction * t;
        return new Vector3(target.x, target.y, transform.position.z);
    }

}
