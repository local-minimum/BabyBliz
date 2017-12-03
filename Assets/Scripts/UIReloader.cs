using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReloader : MonoBehaviour {

    [SerializeField]
    Image left;

    [SerializeField]
    Image right;

    [SerializeField]
    GameObject uiRoot;


	void Update () {
        float reload = GameMaster.instance.progressToReload;
        if (reload < 0.05f)
        {
            uiRoot.SetActive(false);
        } else
        {
            uiRoot.SetActive(true);
            left.fillAmount = 1 - reload;
            right.fillAmount = 1 - reload;
        }
	}
}
