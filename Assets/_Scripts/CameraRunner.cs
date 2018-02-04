using UnityEngine;
using System.Collections;

// CameraRunner uses the camera object and follows the player
public class CameraRunner : MonoBehaviour {

	private Transform player; 		// is the transform of the player
	private float offSetX;			// is the offset x axis of the player

	void Start () 
	{
		GameObject player_go = GameObject.FindGameObjectWithTag ("Player");		// finds the player game object using its name tag "Player"

		if (player_go == null) 
		{
			Debug.LogError ("Couldn't find an object with tag 'Player'!");
		} 
		else 
		{
			player = player_go.transform;										// get the transform of the player
			offSetX = transform.position.x - player.position.x;					// get the offset of the player by using the transform of the camera pane - the position of the player's x axis
		}
	}

	void Update () 
	{
		if (player != null) 													
		{
			Vector3 pos = transform.position;				// get the position of the camera
			pos.x = player.position.x + offSetX;			// set the position of the camera to the player and its offset
			transform.position = pos;						// apply the position to the camera
		}
	}
}
