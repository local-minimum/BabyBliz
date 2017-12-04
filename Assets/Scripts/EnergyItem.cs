using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyItem : MonoBehaviour {

    public float energyContent = 100;
    public float consumptionTime = 100;

    GameObject player;
    bool consuming = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            consuming = false;
            player = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            consuming = false;
            player = null;
        }    
    }

    float consumptionStart = 0;

    public float consumptionProgress
    {
        get
        {
            return consuming ? Mathf.Min((Time.timeSinceLevelLoad - consumptionStart) / consumptionTime, 1f) : 0f;
        }
    }

    private void Update()
    {
        if (player != null) {
            if (Input.GetButtonDown("Action")) {
                consuming = true;
                consumptionStart = Time.timeSinceLevelLoad;
            } else if (Input.GetButtonUp("Action"))
            {
                consuming = false;
            }

            if (consuming)
            {
                if (Time.timeSinceLevelLoad - consumptionStart > consumptionTime)
                {
                    player.SendMessage("PickupEnergy", this);
                    consuming = false;
                }
            }

        }
    }
}
