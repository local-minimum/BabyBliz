using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    static GameMaster _instance;

    public static GameMaster instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject player;

    public float distanceWalked
    {
        get
        {
            return Random.value;
        }
    }

    private void Start()
    {
        _instance = this;
    }
}
