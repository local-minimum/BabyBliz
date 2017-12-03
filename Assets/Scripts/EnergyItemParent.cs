using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyItemParent : MonoBehaviour {

    public GameObject canvas;
    public Image progres;
    public EnergyItem item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && item != null)
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canvas.SetActive(false);
        }

    }

    private void Update()
    {
        if (canvas.activeSelf)
        {
            progres.fillAmount = item.consumptionProgress;
            if (item == null)
            {
                canvas.SetActive(false);
            }
        }
    }
}
