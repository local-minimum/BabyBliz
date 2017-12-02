using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    [SerializeField]
    Rect attachmentArea;

    HashSet<BabyController> babies = new HashSet<BabyController>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int BabyCount
    {
        get
        {
            return babies.Count;
        }
    }

    public void PickupBaby(BabyController baby)
    {
        babies.Add(baby);
        baby.SetAttachment(gameObject, attachmentArea);
    }
}
