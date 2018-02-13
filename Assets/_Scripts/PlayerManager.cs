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
	private bool _jumping, _facingRight, _grounded, _dead;
    private int _decay;

	public float speedX, jumpSpeedY;

	// Initializing variables
	void Start () 
	{
		_myAnim = GetComponent<Animator> ();
		_myRigidBody = GetComponent<Rigidbody2D> ();
		_facingRight = _grounded = true;
		_jumping = _dead = false;
        _maxhealth = 100;
        _currenthealth = 100;
        _decay = 0;
        _healthbarlength = Screen.width / 3;
	}
	
    //Should maybe moved to GUI once that module is done
    //Load GUI for healthbar
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, _healthbarlength, 20), _currenthealth + "/" + _maxhealth);
    }

	void Update () 
	{
        //Overheal?
        if (_currenthealth > _maxhealth)
        {
            _decay++;
            if (_decay == 60)
            {
                _currenthealth--;
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
		Vector2 moveVec = new Vector2 (CrossPlatformInputManager.GetAxis ("Horizontal"), CrossPlatformInputManager.GetAxis ("Vertical")) * speedX;
		MovePlayer (_speed);
		Flip ();

		// Joystick controls
		if(moveVec.x > 0){
			_speed = speedX;
		}
		if(moveVec.x < 0){
			_speed = -speedX;
		}
		if(moveVec.x == 0){
			_speed = 0;
		}
		if(moveVec.y > 67){
			Jump ();
		}

		// Keyboard controls
		// Left player movement
		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			_speed = -speedX;
		} // Idle
		if (Input.GetKeyUp (KeyCode.LeftArrow)) 
		{
<<<<<<< HEAD
=======
<<<<<<< HEAD
			_speed = 0;
		}

=======
            if (!_jumping)
            {
>>>>>>> origin/testing/beta
                _speed = 0;
        }
			
>>>>>>> origin/testing/beta
		// Right player movement
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			_speed = speedX;
		} // Idle
		if (Input.GetKeyUp (KeyCode.RightArrow)) 
		{
                _speed = 0;
        }

		// Jump
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
		{
			Jump();
		}
	}

    void healthAdjust(int val)
    {
        if (_currenthealth != _maxhealth * 2)
            _currenthealth += val;
    }
		
		
	/// <summary>
	/// Player movement and animation
	/// </summary>
	/// <param name="playerSpeed">Player speed.</param>

	// Animator States
	// State 0 = Idle
	// State 1 = Walk/Run
	// State 2 = Jump
	// State 3 = Attack
	// State 4 = Crouch
	// State 5 = Dying

	void MovePlayer(float playerSpeed)
	{
		_myRigidBody.velocity = new Vector3 (_speed, _myRigidBody.velocity.y, 0);

		// Player walk/run
		if (playerSpeed < 0 && !_jumping || playerSpeed > 0 && !_jumping) 
		{
			_myAnim.SetInteger ("State", 1);
		}

		// Player idle
		if (playerSpeed == 0 && !_jumping) 
		{
			_myAnim.SetInteger ("State", 0);
		}
	}
		
	// Code to flip the player when facing left and right
	void Flip()
	{
		if(_speed > 0 && !_facingRight || _speed < 0 && _facingRight)
		{
			_facingRight = !_facingRight;

			Vector3 temp = transform.localScale;
			temp.x *= -1;
			transform.localScale = temp;
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
			_myAnim.SetInteger("State", 0);
			_jumping = false;
			_grounded = true;
		}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            _currenthealth = _currenthealth - 10;
        }
    }
    // Phone Mobile Interface Button Triggers
    public void WalkLeft()
	{
		_speed = -speedX;
	}
	public void WalkRight()
	{
		_speed = speedX;
	}
	public void Idle()
	{
		_speed = 0;
	}
	public void Jump()
	{
		if (_grounded && !_dead)
		{
			_jumping = true;
			_grounded = false;
			_myRigidBody.velocity = (new Vector2(_myRigidBody.velocity.x, jumpSpeedY));
			_myAnim.SetInteger("State", 2);
		}
	}
}
