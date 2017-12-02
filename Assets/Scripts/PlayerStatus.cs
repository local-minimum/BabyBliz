using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KilledBy { Drowning, Fire };

public class PlayerStatus : MonoBehaviour {

    [SerializeField]
    Rect attachmentArea;

    [SerializeField]
    AnimationCurve energyCurve;

    [SerializeField]
    AnimationCurve jumpEnergyCurve;

    [SerializeField]
    float _energy = 40;

    [SerializeField]
    float maxEnergy = 80;

    public float Energy
    {
        get
        {
            return energyCurve.Evaluate(_energy);
        }
    }

    public float JumpEnergy
    {
        get
        {
            return jumpEnergyCurve.Evaluate(_energy);
        }
    }

    List<BabyController> babies = new List<BabyController>();

    [SerializeField]
    float energyLossRate = .1f;

	void Update () {
        _energy -= Time.deltaTime * (BabyCount + 1) * energyLossRate;
        _energy = Mathf.Max(_energy, 0);

        if (burning)
        {
            CauseBurn();
        }
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
        baby.gameObject.layer = LayerMask.NameToLayer("Carried");
    }

    public void PickupEnergy(EnergyItem item)
    {
        _energy += item.energyContent;
        _energy = Mathf.Min(_energy, maxEnergy);
        Destroy(item.gameObject);
    }

    public bool isAlive = true;

    public void Kill(KilledBy reason)
    {
        Debug.Log(reason);
        isAlive = false;
    }

    bool burning = false;

    public void SetBurning(bool value)
    {
        burning = value;
    }

    void CauseBurn()
    {
        int babyIndex = babies.Count - 1;
        if (babyIndex < 0)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.black;
            SendMessage("Kill", KilledBy.Fire);
        }
        else
        {
            BabyController sacrifice = babies[babyIndex];
            sacrifice.Health -= .05f;

            if (sacrifice.Health == 0)
            {
                babies.Remove(sacrifice);
                sacrifice.Kill();
            }
        }
    }
}
