using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KilledBy { Drowning, Fire, Fatigue };

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

    public float RawEnergy
    {
        get
        {
            return Mathf.Clamp01(_energy / maxEnergy);
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
        if (!isAlive)
        {
            return;
        }
        babies.Add(baby);
        baby.SetAttachment(gameObject, attachmentArea);
        baby.gameObject.layer = LayerMask.NameToLayer("Carried");
        baby.DoNotRemove = true;
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
        _energy = 0;
        isAlive = false;
        LooseAllBabies();
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

    [SerializeField]
    float pruneSpeed = 3f;

    void LooseAllBabies()
    {
        while (babies.Count > 0)
        {
            BabyController b = babies[0];
            babies.Remove(b);
            b.FreeAttachment();
        }
    }

    void PruneBunt()
    {
        
        if (Random.value < Time.deltaTime * pruneSpeed)
        {
            int lastBaby = babies.Count - 1;            
            if (lastBaby >= 0)
            {
                BabyController baby = babies[lastBaby];                
                if (baby.Health < .95f)
                {
                    if (baby != null) {
                        baby.Kill();
                    }
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
