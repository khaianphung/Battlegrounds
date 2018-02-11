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
    public Transform FiringPoint;
    public GameObject bullet;
    public LevelManager levelManager;

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

	// Update is called once per frame
	void Update () 
	{
		MovePlayer (_speed);
		Flip ();

		// Left player movement
		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			_speed = -speedX;
		} // Idle
		if (Input.GetKeyUp (KeyCode.LeftArrow)) 
		{
			_speed = 0;
		}
			
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        // Shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Create a bullet object at the FiringPoint position
            Instantiate(bullet, FiringPoint.position, FiringPoint.rotation);
        }

        //Overheal?
        //If current health is greater than max health
        if (_currenthealth > _maxhealth)
        {
            //Increment decay counter
            _decay++;
            //When the decay counter is 60, decrease health by 1
            //Equivalent to decrease health by 1 per 60 frames, or once per second
            if (_decay == 60)
            {
                //Decrease health
                _currenthealth--;
                //Reset to 0
                _decay = 0;
            }
        }
        
        //Check Dead
        if (_currenthealth <= 0)
        {
            //Stop the player from jumping by setting dead state to true
            _dead = true;
            //Stop the player from moving by setting their speed to 0
            _speed = 0;
            //Display death message
            Debug.Log("You Died");
            //Begin respawn timer
            respawnTimer();
        }
	}

    //Method to adjust player health, positive or negative
    public void healthAdjust(int val)
    {
        //If the player's current health is not the same as the max
        if (_currenthealth != _maxhealth)
            //Add the adjustment value to the current health
            _currenthealth += val;
        //If the player's health is the same as the max, display message
        else
            Debug.Log("You are at max health");
    }

    //Respawn timer for deaths
    public void respawnTimer()
    {
        //Takes the current time of death
        float timeOfDeath = Time.time;
        //Subtracts it from the current time to find the time elapsed
        float timeElapsed = Time.time - timeOfDeath;
        //Wait for three seconds to pass
        do
        {
            //Update time elapsed
            timeElapsed = Time.time - timeOfDeath;
            //Message to display current task
            Debug.Log("Wait to Respawn");
        } while (timeElapsed < 3);
        //Display that the player has respawned
        Debug.Log("Respawned");
        //Call method to respawn player
        levelManager.RespawnPlayer();
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
