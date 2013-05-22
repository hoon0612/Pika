using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpikeType
{
	NONE = 0,
	HIGH = 1,
	MID = 2,
	LOW = 3
}

public enum MotionType
{
	WALK = 0,
	JUMP = 1,
	SLIDE = 2,
	SPIKE = 3,
	WIN = 4,
	LOSE = 5
}

public class Player : MonoBehaviour 
{
	private GameObject _player;
	private float _vel_x, _vel_y;
	public SpikeType pSpike = SpikeType.NONE;
	public MotionType pMotion = MotionType.WALK;
	public GameObject player
	{
		get 
		{
			return _player;
		}
		set
		{
			_player = value;
		}
	}
	public bool is_right_user = false;
	public bool shoot_pressed = false;
	public CapsuleCollider col;
	
	public tk2dAnimatedSprite playerSprite;
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
	public bool can_swipe = true;
	public bool motion_change = false;
	public bool motion_change_spike = false;
	public bool is_reversed = false;
	
	public float jump_speed = 330f;
	public float sliding_x_speed = 180f;
	public float sliding_y_speed = 140f;
	public float jump_reducing_speed = 9f;
	public float sliding_reducing_y_speed = 9f;
	public float walking_speed = 150f;
	public float spike_x_speed = 180f;
	
	public IEnumerator WakeUp()
	{
		yield return new WaitForSeconds(0.2f);
		if(is_reversed)
		{
			if(is_right_user)
			{
				player.transform.FindChild("playerSprite").localRotation = Quaternion.Euler(new Vector3(0,180,0));	
			}
			else
			{
				player.transform.FindChild("playerSprite").localRotation = Quaternion.Euler(new Vector3(0,0,0));
			}
		}
		
		playerSprite.Play("Idle");
		vel_y = 0;
		vel_x = 0;
		motion_change = false;
		can_swipe = true;
		is_reversed = false;
	}
	
	public	IEnumerator SetSpikeFalse()
	{
		yield return new WaitForSeconds(0.25f);
		if(player.transform.localPosition.y > -74f)
			pMotion = MotionType.JUMP;
		else
		{
			pMotion = MotionType.WALK;
			playerSprite.Play("Idle");
		}
		pSpike = SpikeType.NONE;	
		motion_change = false;
		motion_change_spike = false;
	}
	
	public void CorrectPlayerPos()
	{
		if(is_right_user)
		{
			if(player.transform.localPosition.x < 30f)
			{
				player.transform.localPosition = new Vector3(30f, player.transform.localPosition.y, player.transform.localPosition.z);
			}
			if(player.transform.localPosition.x > 205f)
			{
				player.transform.localPosition = new Vector3(205f, player.transform.localPosition.y, player.transform.localPosition.z);
			}
			if(player.transform.localPosition.y < -75f)
			{
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -75f , player.transform.localPosition.z);
			}	
		}
		else
		{
			if(player.transform.localPosition.x > -30f)
			{
				player.transform.localPosition = new Vector3(-30f, player.transform.localPosition.y, player.transform.localPosition.z);
			}
			if(player.transform.localPosition.x < -205f)
			{
				player.transform.localPosition = new Vector3(-205f, player.transform.localPosition.y, player.transform.localPosition.z);
			}
			if(player.transform.localPosition.y < -75f)
			{
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -75f , player.transform.localPosition.z);
			}
		}
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
