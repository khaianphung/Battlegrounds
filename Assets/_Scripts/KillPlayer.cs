using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {
	LevelManager levelManager;

	void Start () {
		levelManager = FindObjectOfType < LevelManager >();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log ("Respawned");
		levelManager.RespawnPlayer();
    }
}
