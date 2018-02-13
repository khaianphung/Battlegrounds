using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip jumpSound,walkSound, idleSound;
    static AudioSource audioSrc;
	// Use this for initialization
	void Start () {
        jumpSound = Resources.Load<AudioClip>("jump");
        walkSound = Resources.Load<AudioClip>("walk");

        idleSound = Resources.Load<AudioClip>("breathe");
        audioSrc = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "jump":
                audioSrc.PlayOneShot(jumpSound);
                break;
            case "walk":
                audioSrc.PlayOneShot(walkSound);
                break;
            case "idle":
                audioSrc.PlayOneShot(idleSound);
                break;
        }
    }
}
