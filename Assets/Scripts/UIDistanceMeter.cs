using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDistanceMeter : MonoBehaviour {

    Text myText;
	// Use this for initialization
	void Start () {
        myText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        myText.text = string.Format("Walked: {0}", GameMaster.instance.distanceWalked);
	}
}
