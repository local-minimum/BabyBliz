﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour {
    public void StartGame()
    {
        SceneManager.LoadScene("theGame");
    }
}
