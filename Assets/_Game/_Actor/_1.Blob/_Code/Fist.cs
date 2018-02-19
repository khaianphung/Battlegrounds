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
        Debug.Log("Fist Collision");
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerHitManager>().GetHit(damage, transform.parent.parent.GetComponent<PlayerManager>()._facingRight);
        }
       
    }

}
