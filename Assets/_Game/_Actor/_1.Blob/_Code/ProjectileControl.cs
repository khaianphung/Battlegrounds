using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour {

    //varaible
    public float speed;
    public bool isFacingRight;
    public float damage;
    Transform projectile;
    SpriteRenderer s;
    void Awake()
    {
        projectile = transform;
        s = GetComponent<SpriteRenderer>();
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        transform.localScale = new Vector3(0.01f, 0.01f, 0);
        speed = 500;
    }

    void FixedUpdate()
    {
        CheckDirection(isFacingRight);
        if (isFacingRight)
        {
            projectile.Translate(Vector2.left * -speed * Time.deltaTime);
        }
        else
        {
            projectile.Translate(Vector2.left * speed * Time.deltaTime);
        }
        
    }

    public void CheckDirection(bool dir)
    {
        
        isFacingRight = dir;
        if (!isFacingRight)
        {
            s.flipX = true; 
        }
        else
        {
            Debug.Log(dir);
            s.flipX = false ; 
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerHitManager>().GetShot(damage, isFacingRight);
        }
            
        //Destroy(gameObject);
    }
    void OnCollisionExit2D(Collision2D col)
    {
        Destroy(gameObject);
    }




}
