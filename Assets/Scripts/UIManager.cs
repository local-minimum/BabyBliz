using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject distanceMeter;
    public GameObject timeMeter;
   

    public float distanceShowMeterLimit = 5;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //distance meter toggle
        if (GameMaster.instance.distanceWalked > distanceShowMeterLimit) distanceMeter.SetActive(true);
        else distanceMeter.SetActive(false);



    }
}
