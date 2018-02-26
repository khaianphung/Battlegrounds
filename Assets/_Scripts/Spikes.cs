using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    private PlayerHitManager player;
    private void Start()
   {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHitManager>();
        Debug.Log(player.tag);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision ? "+ col.tag);
        if (col.CompareTag("Body"))
        {
            
            player.Damage(200);
            //Debug.Log("Something");
        }
    }
    
}
