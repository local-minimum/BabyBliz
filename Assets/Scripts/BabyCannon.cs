using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyCannon : MonoBehaviour {

    [SerializeField]
    float spawnRate = 0.1f;

	void Update () {
	    if (Random.value < spawnRate * Time.deltaTime)
        {
            BirthChild();
        }
	}

    [SerializeField]
    float lateralisation = 5;

    [SerializeField]
    float upScaleMin = 5;

    [SerializeField]
    float upScaleMax = 10;

    [SerializeField]
    float force = 100f;

    public void BirthChild()
    {
		BabyController baby = GameMaster.instance.CreateBaby ();
        baby.transform.position = transform.position;
        baby.dontMove = true;
        Vector2 ejectForce = new Vector2(Random.Range(-1f, 1f) * lateralisation, Random.Range(upScaleMin, upScaleMax)).normalized;
        Rigidbody2D rb = baby.GetComponent<Rigidbody2D>();
        rb.AddTorque(Random.Range(-90, 90));
        rb.AddForce(ejectForce * force);
    }
}
