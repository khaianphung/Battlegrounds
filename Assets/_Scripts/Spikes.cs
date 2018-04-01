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
        //Debug.Log(col.transform.parent.tag);
        if (col.CompareTag("Body"))
        {
            
            player.Damage(200);
            StartCoroutine(player.KnockBack(0.02f, 350, player.transform.position));
            
        }
    }
    
}
