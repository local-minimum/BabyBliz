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
    Text message;

    private void Start()
    {
        float progress = GameMaster.instance.willToLive;

        SetProgress(progress);
    }

    void Update()
    {
        float progress = GameMaster.instance.willToLive;

        if (progress < 0.1f)
        {
            message.text = "No will to live";
        } else if (progress < 0.3f)
        {
            message.text = "Need for coffee";
        } else
        {
            message.text = "Will to live";
        }
        SetProgress(Mathf.Lerp(left.fillAmount, progress, 0.1f));
  
    }

    void SetProgress(float progress)
    {
        left.fillAmount = progress;
        right.fillAmount = progress;
    }
}
