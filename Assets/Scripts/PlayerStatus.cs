using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    [SerializeField]
    Rect attachmentArea;

    [SerializeField]
    AnimationCurve energyCurve;

    [SerializeField]
    float _energy = 40;

    public float Energy
    {
        get
        {
            return energyCurve.Evaluate(_energy);
        }
    }

    HashSet<BabyController> babies = new HashSet<BabyController>();

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    [SerializeField]
    float energyLossRate = .1f;

	void Update () {
        _energy -= Time.deltaTime * BabyCount * energyLossRate;
        _energy = Mathf.Max(_energy, 0);
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
