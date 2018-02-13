using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spike : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
