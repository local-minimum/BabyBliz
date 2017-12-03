﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	[SerializeField]
	Transform allTheBabies;

	[SerializeField]
	BabyController babyPrefab;

	[SerializeField]
	List<GameObject> positions;

	[SerializeField]
	float spawnRate = 0.1f;

	float birdSpeed = 5f;

	int index = 0;

	void Update () {
		
		if (Vector3.Distance(transform.position, positions [index].transform.position) < 0.1f) {
			index = (index + 1) % positions.Count;
		}
		Vector3 direction = (positions[index].transform.position - transform.position).normalized;
		transform.position +=  direction * birdSpeed * Time.deltaTime;
	


		if (Random.value < spawnRate * Time.deltaTime)
		{
			BirthChild();
		}
	}

	[SerializeField]
	float lateralisation = 5;

	[SerializeField]
	float force = 100f;

	public void BirthChild()
	{
		BabyController baby = Instantiate(babyPrefab, allTheBabies, true);
		baby.transform.position = transform.position;
		baby.dontMove = true;
		Vector2 ejectForce = new Vector2(Random.Range(0, 0f) * lateralisation, 1).normalized;
		Rigidbody2D rb = baby.GetComponent<Rigidbody2D>();
		rb.AddTorque(Random.Range(-90, 90));
		rb.AddForce(ejectForce * force);
	}
}