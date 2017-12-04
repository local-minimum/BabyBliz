using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuThanks : MonoBehaviour {

    [SerializeField]
    float timeToNext = 10f;

    private void Update()
    {
        if (Time.timeSinceLevelLoad > timeToNext)
        {
            SceneManager.LoadScene("menu");
        }
    }
}
