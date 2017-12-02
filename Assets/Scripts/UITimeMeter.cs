using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeMeter : MonoBehaviour {

    Text myText;
	// Use this for initialization
	void Start () {
        myText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        //myText.text = string.Format("Time: {0}", GameMaster.instance.levelTime);
        myText.text = "Time left: " + GameMaster.instance.levelTime.ToString("0") + "s";

    }
}
