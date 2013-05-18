using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerStatus
{
	None,
	Walking, 
	Jumping,
	LeftSliding,
	RightSliding,
	Win,
	Lose
} 

public class Player : MonoBehaviour {
	private GameObject _player;
	private float _vel_x, _vel_y;
	public GameObject player{
		get {
			return _player;
		}
		set{
			_player = value;
		}
	}
	public bool shoot_pressed = false;
	public CapsuleCollider col;
	public UISprite player_sprite;
	public tk2dAnimatedSprite player_animation;
	public float vel_x
	{
		get
		{
			return _vel_x;
		}
		set
		{
			_vel_x = value;
		}
	}
	public float vel_y
	{
		get
		{
			return _vel_y;
		}
		set
		{
			_vel_y = value;
		}
	}
	public bool walking = false;
	public bool jumping = false;
	public bool leftSliding = false;
	public bool rightSliding = false;
	public bool upperSpike = false;
	public bool middleSpike = false;
	public bool lowerSpike = false;
	private bool _isEnemy;
	public bool isEnemy
	{
		get{
			return _isEnemy;
		}
		set{
			_isEnemy = value;
		}
	}
	public bool can_swipe = true;
	
	public float jump_speed = 2.3f;
	public float sliding_x_speed = 1.5f;
	public float sliding_y_speed = 0.75f;
	public float jump_reducing_speed = 0.07f;
	public float sliding_reducing_y_speed = 0.09f;
	public float walking_speed = 1.2f;
	public float spike_x_speed = 1f;
	
	public IEnumerator WakeUp()
	{
		yield return new WaitForSeconds(0);
	}
	
	public	IEnumerator SetSpikeFalse()
	{
		yield return new WaitForSeconds(0.25f);	
		upperSpike = false;
		middleSpike = false;
		lowerSpike = false;
	}
	
	public void CorrectPlayerPos()
	{
		
	}
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
	}
	
}
