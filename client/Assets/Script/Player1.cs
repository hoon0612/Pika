using UnityEngine;
using System.Collections;

public class Player1 : Player 
{
	void Awake()
	{
		is_right_user = false;
	}
	
	// Use this for initialization
	void Start () {
		player = this.gameObject;
		col = player.GetComponent<CapsuleCollider>();
		playerSprite = player.transform.FindChild("playerSprite").GetComponent<tk2dAnimatedSprite>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(pMotion == MotionType.WALK || pMotion == MotionType.JUMP || pMotion == MotionType.SPIKE)
		{	
			can_swipe = true;
			if(pMotion == MotionType.WALK)
			{	
				vel_y = 0;
			}
			player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);	
		}
		
		if(pMotion == MotionType.JUMP && pMotion != MotionType.SLIDE)
		{
			if(!motion_change)
			{
				motion_change = true;
				playerSprite.Play("Jump");
			}
			vel_x = 0;
			vel_y -= jump_reducing_speed;
			player.rigidbody.velocity -= new Vector3(0, jump_reducing_speed, 0);
			
			if(player.transform.localPosition.y < -75f)
			{
				vel_x = 0; // added
				vel_y = 0;
				player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -75f, player.transform.localPosition.z);
				motion_change = false;
				pMotion = MotionType.WALK;
				playerSprite.Play("Idle");
			}
		}
		
		else if(pMotion == MotionType.SPIKE && pMotion != MotionType.SLIDE)
		{
			if(!motion_change_spike)
			{
				motion_change_spike = true;
				playerSprite.Play("Jump");
				//Debug.Log("spike!");
			}
			vel_y -= jump_reducing_speed;
			player.rigidbody.velocity -= new Vector3(0, jump_reducing_speed, 0);
			StartCoroutine(SetSpikeFalse());
			if(player.transform.localPosition.y < -75f)
			{
				vel_x = 0; // added
				vel_y = 0;
				player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -75f, player.transform.localPosition.z);
				motion_change_spike = false;
				motion_change = false;
				pMotion = MotionType.WALK;
				playerSprite.Play("Idle");
			}
		}
		
		else if(pMotion == MotionType.SLIDE && pMotion != MotionType.JUMP)
		{
			if(!motion_change)
			{
				motion_change = true;
				playerSprite.Play("Slide");
				if(vel_x > 0) // right slide
				{
					if(is_right_user)
					{
						playerSprite.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
						is_reversed = true;
					}
				}
				else if(vel_x < 0)//left slide
				{
					if(!is_right_user)
					{
						playerSprite.transform.localRotation = Quaternion.Euler(new Vector3(0,180,0));
						is_reversed = true;
					}
				}
			}
			vel_y -= sliding_reducing_y_speed;
			player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
			if(player.transform.localPosition.y < -75)
			{
				vel_y = 0;
				vel_x = 0;
				player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -75, player.transform.localPosition.z);
				motion_change = false;
				StartCoroutine(WakeUp());
			}
		}
		CorrectPlayerPos();
	}
}
