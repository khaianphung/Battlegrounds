using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorlManager : MonoBehaviour {
    //Private Varaible
    Rigidbody2D rigidbody;
    Animator anima;
    bool isGrounded;
    int previousState;
    float timer;
    float gravityForce;


    //Public Varaibles
    public bool enableInput;
    public float speedX, speedY;
    public bool isFacingRight;
    public Transform projectile;
    float changeSpeed; 

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.sharedMaterial.friction = 5;
        rigidbody.mass = 1;
        speedX = 5000;
        speedY = 75f;
        rigidbody.freezeRotation = true;
        anima = GetComponent<Animator>();
        enableInput = true;
        rigidbody.gravityScale = 20;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (enableInput)
        {
            //Movement and Animation Control
            if (Input.GetKey(KeyCode.UpArrow) && isGrounded)
            {
                changeSpeed = speedY;
                Jump();
                isGrounded = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveRight();
            }
            else if (Input.GetKey(KeyCode.DownArrow) && isGrounded)
            {
                Crouch();
            }
            else
            {
                if (!isGrounded)
                {
                    anima.SetInteger("State", 2);
                }
                else
                {
                    Idle();
                    rigidbody.velocity = new Vector2(0, 0);
                }
            }

            //Attack and Animation Control
            if (Input.GetKey(KeyCode.M))
            {
                MeleeAttack();
                previousState = 3;
            }
            else if (Input.GetKey(KeyCode.N))
            {
                Shoot();
                previousState = 7;
            }
            else
            {
                if (previousState == 3)
                {
                    anima.SetInteger("AttackState", 9);
                }
                else if (previousState == 7)
                {
                    anima.SetInteger("AttackState", 8);
                }
            }
        }
    }

    void MoveLeft()
    {
        if (isFacingRight)
        {
            Flip();
        }
        rigidbody.velocity = new Vector2(-speedX*Time.deltaTime , rigidbody.velocity.y);
        if (isGrounded)
        {
            anima.SetInteger("State", 1);
        }
    }
    void MoveRight()
    {
        if (!isFacingRight)
        {
            Flip();
        }
        rigidbody.velocity = new Vector2(speedX * Time.deltaTime, rigidbody.velocity.y);
        if (isGrounded)
        {
            anima.SetInteger("State", 1);
        }
        
    }
    void Jump()
    {
        rigidbody.gravityScale = 20;
        rigidbody.velocity = Vector2.up * speedY*100*Time.deltaTime;
        

    }
    void Crouch()
    {
        anima.SetInteger("State", 4);
    }
    void Idle()
    {
        anima.SetInteger("State", 0);
    }

    void MeleeAttack()
    {
        anima.SetInteger("AttackState", 3);

    }

    void Shoot()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Vector3 pos = transform.position;
            var bullet = Instantiate(projectile, pos, Quaternion.identity);
            bullet.GetComponent<ProjectileControl>().isFacingRight = isFacingRight;
            bullet.GetComponent<ProjectileControl>().speed = 500;
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.Find("Body").GetComponent<Collider2D>());
            timer = 0.2f;
        }
        anima.SetInteger("AttackState", 7);
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag==("Ground") && isGrounded == false)
        {
            isGrounded = true;
            rigidbody.gravityScale = 1;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector2 newPos = new Vector2(transform.localScale.x, transform.localScale.y);
        newPos.x = newPos.x * -1;
        transform.localScale = newPos;
    }
}
