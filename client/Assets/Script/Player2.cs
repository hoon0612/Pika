using UnityEngine;
using System.Collections;

public class Player2 : Player 
{
	bool motion_change = false;
	
	public IEnumerator WakeUp()
	{
		yield return new WaitForSeconds(0.2f);
		if(player.transform.FindChild("pikachu").localRotation.y != 0)
			player.transform.FindChild("pikachu").localRotation = Quaternion.Euler(new Vector3(0,0,0));
		player_animation.Play("Idle");
		vel_y = 0;
		vel_x = 0;
		motion_change = false;
		can_swipe = true;
	}
	
	void Awake()
	{
		isEnemy = true;
		Debug.Log(isEnemy);
	}
	
	// Use this for initialization
	void Start () {
		player = this.gameObject;
		col = player.GetComponent<CapsuleCollider>();
		player_animation = player.transform.FindChild("pikachu").GetComponent<tk2dAnimatedSprite>();
		isEnemy = true;
	}
	void CorrectPlayerPos()
	{
		if(player.transform.localPosition.x > -30f)
		{
			player.transform.localPosition = new Vector3(-30f, player.transform.localPosition.y, player.transform.localPosition.z);
		}
		if(player.transform.localPosition.x < -205f)
		{
			player.transform.localPosition = new Vector3(-205f, player.transform.localPosition.y, player.transform.localPosition.z);
		}
		if(player.transform.localPosition.y < -80f)
		{
			player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f , player.transform.localPosition.z);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(isEnemy)
		{
			
		}
		else
		{
			if(walking&&!leftSliding&&!rightSliding)
			{
				player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);	
			}
			if(jumping&&!leftSliding&&!rightSliding)
			{
				if(!motion_change)
				{
					motion_change = true;
					player_animation.Play("Jump");
				}
				vel_y -= jump_reducing_speed;
				player.rigidbody.velocity -= new Vector3(0, jump_reducing_speed, 0);
				
				if(player.transform.localPosition.y <= -80f)
				{
					vel_x = 0; // added
					vel_y = 0;
					player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
					player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f, player.transform.localPosition.z);
					motion_change = false;
					jumping = false;
					player_animation.Play("Idle");
				}
				if(upperSpike || middleSpike || lowerSpike)
				{
					StartCoroutine(SetSpikeFalse());
				}
			}
			else if(leftSliding&&!jumping&&!rightSliding)
			{
				if(!motion_change)
				{
					motion_change = true;
					player_animation.Play("Slide");
					player.transform.FindChild("pikachu").localRotation = Quaternion.Euler(new Vector3(0,180,0));
				}
				vel_y -= sliding_reducing_y_speed;
				player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
				if(player.transform.localPosition.y <= -80f)
				{
					vel_y = 0;
					vel_x = 0;
					player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
					player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f, player.transform.localPosition.z);
					motion_change = false;
					leftSliding = false;
					StartCoroutine(WakeUp());
				}
				
			}
			else if(rightSliding&&!jumping&&!leftSliding)
			{
				if(!motion_change)
				{
					motion_change = true;
					player_animation.Play("Slide");
				}
				vel_y -= sliding_reducing_y_speed;
				player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
				if(player.transform.localPosition.y <= -80f)
				{
					vel_x = 0;
					vel_y = 0;
					player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
					player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f, player.transform.localPosition.z);
					motion_change = false;
					rightSliding = false;
					StartCoroutine(WakeUp());
				}
			}
		}
		CorrectPlayerPos();
	}
}
