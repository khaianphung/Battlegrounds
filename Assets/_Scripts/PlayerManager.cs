using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
///  PlayerManager class controls the player object and its surrounding environments.
///  The player object is animated, able user to control the character object
///  It also uses sounds that are triggered depending on different events.
/// </summary>

public class PlayerManager : MonoBehaviour 
{
	private Animator _myAnim;					// Player's animator
	private Rigidbody2D _myRigidBody;			// Player's rigidbody
	private AudioSource[] _myAduioArray;		// Player's set of audios stored in an array
	private float _speed, _maxhealth, _currenthealth, _healthbarlength;
	private bool _jumping, _grounded, _dead;
    private int _decay;
	public bool onLadder;
    public bool _facingRight;

    //AttackVariable
    private int previousState;
    private float timer;
    public Transform projectile;
    public bool enableInput;

	BoxCollider2D _myCollider;

	public float speedX, jumpSpeedY;

	public Transform firePoint;

	// Initializing variables
	void Start () 
	{
		_myAnim = GetComponent<Animator> ();
		_myRigidBody = GetComponent<Rigidbody2D> ();
		_myCollider = GetComponent<BoxCollider2D> ();
        _facingRight = false;
        _grounded = true;
		_jumping = _dead = false;
        _maxhealth = 100;
        _currenthealth = 100;
        _decay = 0;
        _healthbarlength = Screen.width / 3;
        _myRigidBody.freezeRotation = true;
        enableInput = true;
		onLadder = false;

    }
    //Should maybe moved to GUI once that module is done
    //Load GUI for healthbar
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, _healthbarlength, 20), _currenthealth + "/" + _maxhealth);
    }
	void Update () 
	{
        //GetHP from other script
        _maxhealth = GetComponent<PlayerHitManager>().maxHP;
        _currenthealth = GetComponent<PlayerHitManager>().currentHP;

        //Overheal?
        if (_currenthealth > _maxhealth)
        {
            _decay++;
            if (_decay == 60)
            {
                GetComponent<PlayerHitManager>().currentHP--;
                _decay = 0;
            }
        }
        
        //Check Dead
        if (_currenthealth <= 0)
        {
            _dead = true;
            _speed = 0;
            Destroy(gameObject);
            Application.LoadLevel(Application.loadedLevel);
        }
	}
	void FixedUpdate()
	{
		
        if (enableInput)
        {
			if (Input.GetKey(KeyCode.UpArrow) && _grounded && !onLadder)
            {
                Jump();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (_facingRight)
                {
                    Flip();
                }
                _speed = -speedX;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (!_facingRight)
                {
                    Flip();
                }
                _speed = speedX;
            }
			else if (Input.GetKey(KeyCode.DownArrow) && _grounded && !onLadder)
            {
                _myAnim.SetInteger("State", 4);
            }
            else
            {
                if (!_grounded)
                {
                    _myAnim.SetInteger("State", 2);
                }
                else
                {
                    _myAnim.SetInteger("State", 0);
                }
            }

            MovePlayer(_speed);

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
                    _myAnim.SetInteger("AttackState", 9);
                }
                else if (previousState == 7)
                {
                    _myAnim.SetInteger("AttackState", 8);
                }
            }

			// Joystick controls
			Vector2 moveVec = new Vector2 (CrossPlatformInputManager.GetAxis ("Horizontal"), CrossPlatformInputManager.GetAxis ("Vertical")) * speedX;
            if (moveVec.x > 0)
            {
				if (!_facingRight)
				{
					Flip();
				}
                _speed = speedX;
            }
            if (moveVec.x < 0)
            {
				if (_facingRight)
				{
					Flip();
				}
                _speed = -speedX;
            }
            if (moveVec.x == 0)
            {
                _speed = 0;
            }
            if (moveVec.y > 10)
            {
                Jump ();
            }
        }
			
	}

    public void healthAdjust(int val)
    {
        if (_currenthealth != _maxhealth * 2)
            _currenthealth += val;
    }
		
	/// <summary>
	/// Player movement and animation
	/// Animator States
	/// State 0 = Idle
	/// State 1 = Walk/Run
	/// State 2 = Jump
	/// State 3 = Attack
	/// State 4 = Crouch
	/// State 5 = Dying
	/// </summary>
	/// <param name="playerSpeed">Player speed.</param>
	void MovePlayer(float playerSpeed)
	{
		_myRigidBody.velocity = new Vector3 (_speed, _myRigidBody.velocity.y, 0);

		// Player walk/run
		if (playerSpeed < 0 && !_jumping || playerSpeed > 0 && !_jumping) 
		{
			_myAnim.SetInteger ("State", 1);
		}

	}
	// Code to flip the player when facing left and right
	void Flip()
	{
		_facingRight = !_facingRight;
		Vector3 temp = transform.localScale;
		temp.x *= -1;
		transform.localScale = temp;
	}
	// Jumping method
	void Jump()
	{
		if (_grounded && !_dead)
		{
			_jumping = true;
			_grounded = false;
			_myRigidBody.velocity = (new Vector2(_myRigidBody.velocity.x, jumpSpeedY));
		}
	}
	/// <summary>
	/// This method is called when the player collides with another object
	/// </summary>
	/// <param name="other"></param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ground")
		{
			_jumping = false;
			_grounded = true;
		}
        if (other.gameObject.tag == "Player")
        {
            _myRigidBody.velocity = Vector2.zero;
        }

		Collider2D collider = other.collider; // collider of object that is gonna collide with the Player
		float rectWidth = this.GetComponentInChildren<Collider2D> ().bounds.size.x; // width of Player box collider
		float rectHeight = this.GetComponentInChildren<Collider2D> ().bounds.size.y; // height of Player box collider

		if (other.gameObject.tag == "Jumper")
		{
			Vector3 contactPoint = other.contacts [0].point;
			Vector3 center =  collider.bounds.center;

			if (contactPoint.y > center.y && (contactPoint.x < center.x + rectWidth / 2 && contactPoint.x > center.x - rectWidth / 2 )) {
				_myRigidBody.AddForce (transform.up * 30000);
				_jumping = false;
				_grounded = true;
			} 
		}


    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _myRigidBody.mass = 1;
        }
		if(other.gameObject.tag == "GoingDownLadder")
		{
			//_myCollider.isTrigger = false;
		}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            _currenthealth = _currenthealth - 10;
        }

		if(collision.gameObject.tag == "GoingDownLadder")
		{
			Debug.Log("Touching GoingDownLadder");
			_myCollider.isTrigger = true;
		}
    }
    void MeleeAttack()
    {
        _myAnim.SetInteger("AttackState", 3);
    }
    void Shoot()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Fire();
            timer = 0.2f;
        }
        _myAnim.SetInteger("AttackState", 7);
    }

    void Fire()
    {
        GameObject obj = GenericObjectPool.current.GetPooledObject();
        if (obj == null)
        {
            return;
        }
        Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), transform.Find("Body").GetComponent<Collider2D>());
        obj.GetComponent<ProjectileControl>().isFacingRight = _facingRight;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);


    }
}
