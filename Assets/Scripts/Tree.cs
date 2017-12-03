using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    public BabyController babyPrefab;

    public Rect ClingArea;

    public int babies = 3;

    public Transform AllBabies;

	void Start () {
        for (int i=0; i<babies; i++)
        {
            AdoptBaby();
        }    		
	}
	
    void AdoptBaby()
    {
        BabyController baby =  Instantiate(babyPrefab, AllBabies, true);
        baby.transform.position = GlobalAdoptionCenter;
        baby.SetAttachment(gameObject, ClingArea);
    }

    Vector2 GlobalAdoptionCenter
    {
        get
        {
            return transform.TransformPoint(ClingArea.center);
        }
    }
}
