using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int BabyCount
    {
        get
        {
            return 0;
        }
    }

    public void PickupBaby(BabyController baby)
    {
        Debug.Log("Hello");
        baby.SetAttachment(gameObject);
    }
}
