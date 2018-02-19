using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitManager : MonoBehaviour {
    public float maxHP;
    public float currentHP;

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
