using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour {

    AudioSource speaker;

    [SerializeField, Range(0, 1)]
    float volumeFactor = 0.5f;

    [SerializeField]
    float range = 10f;
	// Use this for initialization
	void Start () {
        speaker = GetComponent<AudioSource>();	
	}
	
	// Update is called once per frame
	void Update () {
        float distance = GameMaster.CameraDistance(transform);
        float volume = Mathf.Clamp01(10 / (distance + 1)) * volumeFactor;
        speaker.volume = volume;
        speaker.mute = volume < 0.1f;
	}
}
