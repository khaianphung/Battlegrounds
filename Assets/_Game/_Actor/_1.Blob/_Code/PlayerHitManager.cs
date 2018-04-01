﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitManager : MonoBehaviour {
    public float maxHP;
    public float currentHP;
    public GameObject Player;
    //PlayerManager PM;

   

    float meleeKnockBackX, meleeKnockBackY, projectileKnockBackX, projectileKnockBackY;

    Animator anima;
    Rigidbody2D rigidbody;
    public bool isGrounded;
    public string spawnPoint;
	// Use this for initialization
	void Start () {
        maxHP = 1000;
        currentHP = maxHP;
        meleeKnockBackX = 1000;
        meleeKnockBackY = 100;
        projectileKnockBackX = 100;
        projectileKnockBackY = 100;
        anima = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.freezeRotation = true;
        spawnPoint = "SpawnPoint";

        //PM = Player.GetComponent<PlayerManager>();
    }
    private void FixedUpdate()
    {
        if (currentHP<=0)
        {
            StartCoroutine(MyCoroutDying());

        }
    }
    public void GetHit(float damage, bool dir)
    {
        currentHP -= damage;
        anima.SetInteger("State", 6);
        if (dir)
        {
            rigidbody.velocity = new Vector2(meleeKnockBackX, meleeKnockBackY);
        }
        else
        {
            rigidbody.velocity = new Vector2(-meleeKnockBackX, meleeKnockBackY);
        }
        
        isGrounded = false;

    }
    public void GetShot(float damage, bool dir)
    {
        currentHP -= damage;
        if (dir)
        {
            rigidbody.velocity = new Vector2(projectileKnockBackX, projectileKnockBackY);
        }
        else
        {
            rigidbody.velocity = new Vector2(-projectileKnockBackX, projectileKnockBackY);
        }
            
        anima.SetInteger("State", 6);
        isGrounded = false;
    }

    
    public void Damage(float dmg)
    {
        currentHP  = currentHP - dmg;
        Debug.Log("Damage Works");
    }
    
    public IEnumerator KnockBack(float knockDur, float knockBackPwr, Vector3 knockBackDir) // dont erase im trying to separate
    {

        float timer = 0;
        while (knockDur > timer)
        {
            timer += Time.deltaTime;
            rigidbody.velocity = new Vector2(0, 0);   
            rigidbody.AddForce(new Vector3(knockBackDir.x * -100, knockBackDir.y * knockBackPwr, transform.position.z));


        }
        yield return 0;

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == ("Ground") && isGrounded == false)

        {
            isGrounded = true;
            anima.SetInteger("State", 0);

        }

    }
    IEnumerator MyCoroutDying()
    {
        transform.GetComponent<PlayerManager>().enableInput = false;
        //anima.SetInteger("State", 5);
        yield return new WaitForSeconds(0f);
        transform.position = GameObject.Find(spawnPoint).transform.position;
        //anima.SetInteger("State", 0);
        transform.GetComponent<PlayerManager>().enableInput = true;
        transform.GetComponent<Rigidbody2D>().gravityScale = 20;
        currentHP = maxHP;
    }

}
