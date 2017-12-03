using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

	void Start() {
		strollerSpriteRenderer = stroller.GetComponent<SpriteRenderer> ();
	}

	void Update () {
		if (baby == null) {
			baby = GameMaster.instance.CreateBaby ();
			baby.transform.position = transform.position;
			baby.SetAttachment (stroller, attachment);
			baby.DoNotRemove = true;
		}
		float toMove = direction * 1 * walkSpeed * Time.deltaTime;
		if (Math.Abs (currentDistance) > distanceToWalk) {
			direction = -direction;
			toMove = -toMove;
			stroller.transform.Translate (direction*2, 0, 0);
			strollerSpriteRenderer.flipX = !strollerSpriteRenderer.flipX;
		}
		currentDistance += toMove;
		transform.Translate(toMove, 0,  0);
	}
}
	