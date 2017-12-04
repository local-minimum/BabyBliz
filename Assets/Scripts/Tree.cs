using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    public Rect ClingArea;

    public int babies = 3;

	[SerializeField]
	GameMaster gameMaster;

    List<BabyController> babyFruits = new List<BabyController>();

    Transform player;

	void Start () {
        player = GameMaster.instance.player.transform;

        for (int i=0; i<babies; i++)
        {
            AdoptBaby();
        }    		
	}
	
    void AdoptBaby()
    {
		BabyController baby = GameMaster.instance.CreateBaby ();
        baby.transform.position = GlobalAdoptionCenter;
        baby.SetAttachment(gameObject, ClingArea);
        babyFruits.Add(baby);
    }

    Vector2 GlobalAdoptionCenter
    {
        get
        {
            return transform.TransformPoint(ClingArea.center);
        }
    }

    [SerializeField]
    float dropRange = 0.1f;
    private void Update()
    {
        
        for (int i=babyFruits.Count - 1; i>=0; i--)
        {
            BabyController baby = babyFruits[i];
            if (baby == null)
            {
                babyFruits.RemoveAt(i);
            } else if (Mathf.Abs(player.position.x - baby.transform.position.x) < dropRange)
            {
                baby.FreeAttachment();
                babyFruits.Remove(baby);
            }
        }
    }


}
