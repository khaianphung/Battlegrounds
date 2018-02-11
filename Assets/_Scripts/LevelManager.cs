using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public GameObject respawnPoint;

    private PlayerManager player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RespawnPlayer()
    {
        player.transform.position = respawnPoint.transform.position;
        player.healthAdjust(-100);
    }
}
