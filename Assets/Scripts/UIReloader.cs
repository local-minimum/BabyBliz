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

    [SerializeField]
    Text reloadText;

	void Update () {
        float reload = GameMaster.instance.progressToReload;
        bool hasEnergy = GameMaster.instance.willToLive > 0;
        float endPressing = GameMaster.EndButtonPressing;

        if (endPressing > 0)
        {
            reloadText.text = "\"Abort\"";

            uiRoot.SetActive(true);
            left.fillAmount = 1 - endPressing;
            right.fillAmount = 1 - endPressing;

        }
        else if (reload < 0.05f && hasEnergy)
        {
            uiRoot.SetActive(false);
        } else
        {
            reloadText.text = hasEnergy ? "Reload" : "The End";

            uiRoot.SetActive(true);
            left.fillAmount = 1 - reload;
            right.fillAmount = 1 - reload;
        }
	}
}
