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
    public Transform levelStart;


    public float distanceWalked
    {
        get
        {
            return Mathf.Abs(player.transform.position.x - levelStart.transform.position.x);
        }
    }

    public float levelTime
    {
        get
        {
            return  Time.timeSinceLevelLoad;
        }
    }

    

    private void Start()
    {
        _instance = this;
        
    }


}
