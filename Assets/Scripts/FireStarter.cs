using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStarter : MonoBehaviour {

    List<ParticleSystem> emitters = new List<ParticleSystem>();

	// Use this for initialization
	void Start () {
        emitters.AddRange(GetComponentsInChildren<ParticleSystem>());
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Baby")
        {
            //Fire(true);
            collision.SendMessage("SetBurning", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Baby")
        {
            //Fire(false);
            collision.SendMessage("SetBurning", false);
        }
    }

    void Fire(bool on)
    {
        foreach(ParticleSystem emitter in emitters)
        {
            if (on)
            {
                emitter.Play();
            } else
            {
                emitter.Stop();
            }
            
        }
    }
}
