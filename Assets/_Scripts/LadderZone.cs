using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LadderZone : MonoBehaviour {
	private PlayerManager thePlayer;
	// Use this for initialization
	void Start ()
	{
		GameObject player_go = GameObject.FindGameObjectWithTag ("Player");
		thePlayer = player_go.GetComponent<PlayerManager> ();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			thePlayer.onLadder = true;
		}        

		Debug.Log(other.transform.gameObject.name);
	}
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				other.transform.GetComponent<Rigidbody2D>().gravityScale = 30;
			}
			else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				other.transform.GetComponent<Rigidbody2D>().gravityScale = -30;
			}
			else if(Input.GetKeyUp(KeyCode.UpArrow))
			{
				other.transform.GetComponent<Rigidbody2D>().gravityScale = 0;
			}
			else if (Input.GetKeyUp(KeyCode.DownArrow))
			{
				other.transform.GetComponent<Rigidbody2D>().gravityScale = 0;
			}
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			thePlayer.onLadder = false;
			other.transform.GetComponent<Rigidbody2D>().gravityScale = 30;
		}
	}
}