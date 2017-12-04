using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurtain : MonoBehaviour {

    [SerializeField]
    Image curtain;

    [SerializeField]
    Color color;

	void Update () {
        color.a = GameMaster.EndButtonPressing;
        curtain.color = color;	
	}
}
