using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	private GameObject _player;
	private float _vel_x, _vel_y;
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
	public bool can_swipe = true;
	
	public float jump_speed = 330f;
	public float sliding_x_speed = 180f;
	public float sliding_y_speed = 170f;
	public float jump_reducing_speed = 9f;
	public float sliding_reducing_y_speed = 9f;
	public float walking_speed = 150f;
	public float spike_x_speed = 180f;
	
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
