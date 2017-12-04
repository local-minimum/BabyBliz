using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    
    static GameMaster _instance;

	int maxNumberOfBabies = 300;

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

    public PlayerStatus player;
    public Transform levelStart;
    public CameraController myCamera;
    public EndButton endButton;


    public static float EndButtonPressing
    {
        get
        {
            return instance.endButton.EndButtonProgress;
        }
    }

    public static float CameraDistance(Transform me)
    {
        return Mathf.Abs(instance.myCamera.transform.position.x - me.position.x);
    }

    public float willToLive
    {
        get
        {
            return player.RawEnergy;
        }
    }

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

    float reloadStart;
    bool reloadPressing;

    public float progressToReload
    {
        get
        {
            return reloadPressing ? Mathf.Min((Time.timeSinceLevelLoad - reloadStart) / 1f, 1f) : 0f;
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Reload"))
        {
            reloadStart = Time.timeSinceLevelLoad;
            reloadPressing = true;
        } else if (Input.GetButtonUp("Reload")) { 
            reloadPressing = false;
        } else if (reloadPressing) {
            if (progressToReload == 1f)
            {
                SceneManager.LoadScene("theGame");
            }
        }

        if (player.isAlive && willToLive == 0)
        {
            player.SendMessage("Kill", KilledBy.Fatigue);
        }
    }


}
