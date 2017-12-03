using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherWithStroller : MonoBehaviour {

	float currentDistance;

	[SerializeField]
	GameObject stroller;

	SpriteRenderer strollerSpriteRenderer;

	float direction = 1;

	[SerializeField]
	float distanceToWalk = 10f;

	[SerializeField]
	float walkSpeed = 1f;

	[SerializeField]
	Rect attachment;

	BabyController baby;

    Vector3 strollerLocalOffset;

	void Start() {

        strollerLocalOffset = stroller.transform.localPosition;
		strollerSpriteRenderer = stroller.GetComponent<SpriteRenderer> ();
  
        baby = GameMaster.instance.CreateBaby();
        baby.transform.position = transform.position;
        baby.SetAttachment(stroller, attachment);
        baby.DoNotRemove = true;
        
    }

	void Update () {
		float toMove = direction * 1 * walkSpeed * Time.deltaTime;
		if (shouldTurn) {
			direction = -direction;
			toMove = -toMove;
            stroller.transform.localPosition = new Vector3(strollerLocalOffset.x * direction, strollerLocalOffset.y, strollerLocalOffset.z);
			strollerSpriteRenderer.flipX = !strollerSpriteRenderer.flipX;
		}
		currentDistance += toMove;
		transform.Translate(toMove, 0,  0);
	}

    bool shouldTurn
    {
        get
        {
            if (Mathf.Sign(distanceToWalk) < 0)
            {
                if (currentDistance < distanceToWalk && direction < 0)
                {
                    return true;
                } else if (currentDistance > 0 && direction > 0)
                {
                    return true;
                }
            } else
            {
                if (currentDistance > distanceToWalk && direction > 0)
                {
                    return true;
                } else if (currentDistance < 0 && direction < 0)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
	