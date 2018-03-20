using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject respawnPoint1, respawnPoint2, respawnPoint3, respawnPoint4;

    private PlayerManager player;

	void Start () {
        player = FindObjectOfType<PlayerManager>();
	}

    public void RespawnPlayer()
    {
		int spawnPoint = Random.Range (1, 4);
		if (spawnPoint == 1) {
			player.transform.position = respawnPoint1.transform.position;
		}
		if (spawnPoint == 2) {
			player.transform.position = respawnPoint2.transform.position;
		}
		if (spawnPoint == 3) {
			player.transform.position = respawnPoint3.transform.position;
		}
		if (spawnPoint == 4) {
			player.transform.position = respawnPoint4.transform.position;
		}
    }
}
