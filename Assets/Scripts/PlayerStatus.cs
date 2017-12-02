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
        } else
        {
            PruneBunt();
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

    [SerializeField]
    float fireStrength = 1f;

    [SerializeField, Range(0, 1)]
    float fireStrengthDecay = 0.75f;

    void PruneBunt()
    {
        
        if (Random.value < Time.deltaTime)
        {
            int lastBaby = babies.Count - 1;            
            if (lastBaby >= 0)
            {
                BabyController baby = babies[lastBaby];
                if (baby.Health < .95f)
                {
                    baby.Kill();
                    babies.Remove(baby);
                }
            }
        }
    }

    void CauseBurn()
    {
        int lastBaby = babies.Count - 1;
        if (lastBaby < 0)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.black;
            SendMessage("Kill", KilledBy.Fire);
        }
        else
        {
            float fire = fireStrength * Time.deltaTime;
            int lastToBurn = Mathf.Max(0, lastBaby - 10);
            for (int i = lastBaby; i >= lastToBurn; i--)
            {
                BabyController sacrifice = babies[i];
                sacrifice.Health -= fire;

                if (sacrifice.Health < 0.05f)
                {
                    babies.Remove(sacrifice);
                    sacrifice.Kill();
                }
                fire *= fireStrengthDecay;
            }
        }
    }
}
