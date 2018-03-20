using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour {

    public float damage;
    private void Start()
    {
        damage = 100;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
<<<<<<< HEAD
        
=======
        //Debug.Log("Fist Collision");
>>>>>>> 1e03c18906931efaefdde73cf873ae41159016c0
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Fist Collision");
            col.gameObject.GetComponent<PlayerHitManager>().GetHit(damage, transform.parent.parent.GetComponent<PlayerManager>()._facingRight);
        }
       
    }

}
