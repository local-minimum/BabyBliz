using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnergyBar : MonoBehaviour {

    [SerializeField]
    Image left;

    [SerializeField]
    Image right;

    [SerializeField]
    GameObject uiRoot;


    void Update()
    {
        float progress = GameMaster.instance.willToLive;

        left.fillAmount = progress;
        right.fillAmount = progress;
  
    }
}
