using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButton : MonoBehaviour {


    [SerializeField]
    float pressTime = 2f;

    float pressStart;
    bool isPressing;
	
    public float EndButtonProgress {
        get
        {
            return playerHere && isPressing ? Mathf.Clamp01((Time.timeSinceLevelLoad - pressStart) / pressTime) : (playerHere ? 0.01f : 0f);
        }
    }

	// Update is called once per frame
	void Update () {
		if (playerHere)
        {
            if (Input.GetButtonDown("Action"))
            {
                pressStart = Time.timeSinceLevelLoad;
                isPressing = true;
            } else if (Input.GetButtonUp("Action"))
            {
                isPressing = false;
            } else if (EndButtonProgress == 1)
            {
                SceneManager.LoadScene("thanks");
            }
        }
	}

    bool playerHere = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHere = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHere = false;
        }
    }
}
