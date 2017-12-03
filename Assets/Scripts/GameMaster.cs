using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    static GameMaster _instance;

	int maxNumberOfBabies = 200;

	[SerializeField]
	Transform allTheBabies;

	[SerializeField]
	BabyController babyPrefab;

	List<BabyController> babies = new List<BabyController> ();

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

	public BabyController CreateBaby() {
		if (babies.Count > maxNumberOfBabies) {
			List<BabyController> toRemove = new List<BabyController>();
			int maxRemove = babies.Count - maxNumberOfBabies;
			foreach (BabyController controller in babies) {
				if (controller.DoNotRemove) {
					continue;
				}
				if (toRemove.Count >= maxRemove) {
					break;
				}
				if (controller.Killed) {
					Debug.Log ("Removing killed");
					toRemove.Add(controller);
				}
			}
			List<BabyController> visible = new List<BabyController> ();
			foreach (BabyController controller in babies) {
				if (controller.DoNotRemove) {
					continue;
				}
				if (toRemove.Count >= maxRemove) {
					break;
				}
				if (controller.VisibleTime == null) {
					Debug.Log ("Removing invisible");
					toRemove.Add (controller);
				} else {
					visible.Add (controller);
				}
			}
			visible.Sort ((x, y) => x.VisibleTime.Value.CompareTo (y.VisibleTime.Value));
			foreach (BabyController controller in babies) {
				if (controller.DoNotRemove) {
					continue;
				}
				if (toRemove.Count >= maxRemove) {
					break;
				}
					Debug.Log ("Removing visible");
					toRemove.Add (controller);
			}

			foreach (BabyController controller in toRemove) {				
				Debug.Log ("Removing baby");
				babies.Remove (controller);
				Destroy (controller.gameObject);
			}
		}
		BabyController baby = Instantiate(babyPrefab, allTheBabies, true);
		babies.Add (baby);
		Debug.Log(babies.Count + " number of babies");
	
		return baby;
	}

    

    private void Start()
    {
        _instance = this;
        
    }


}
