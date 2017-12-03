using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sandbox : MonoBehaviour {

	bool init;

	[SerializeField]
	int numberOfBabies;

	void Update () {
		if (!init) {
			init = true;
			for (int i = 0; i < numberOfBabies; ++i) {
				BabyController baby = GameMaster.instance.CreateBaby ();
				baby.transform.position = transform.position;
				baby.BabyState = BabyController.State.Playing;
				baby.DoNotRemove = true;
			}
		}
	}
}
