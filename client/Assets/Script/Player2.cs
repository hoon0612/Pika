using UnityEngine;
using System.Collections;

public class Player2 : Player {
	bool motion_change = false;
	float walk_speed = 2.5f;
	float jump_speed = 7f;
	float sliding_x_speed = 3f;
	float sliding_y_speed = 3f;
	float jump_reduce = 0.225f;
	enum PlayerStatus
	{
		None,
		Walking, 
		Jumping,
		LeftSliding,
		RightSliding,
		Win,
		Lose
	} 
	PlayerStatus playerStatus;
	IEnumerator WakeUp()
	{
		playerStatus = PlayerStatus.None;
		yield return new WaitForSeconds(0.2f);
		if(player.transform.FindChild("pikachu").localRotation.y != 0){
			player.transform.FindChild("pikachu").localRotation = Quaternion.Euler(new Vector3(0,0,0));
			Debug.Log(player.transform.FindChild("pikachu").localRotation.y == 180);
		}
		vel_y = 0;
		vel_x = 0;
		playerStatus = PlayerStatus.Walking;
		motion_change = false;
	}
	
	IEnumerator SetSpikeFalse()
	{
		yield return new WaitForSeconds(0.15f);	
		shoot_pressed = false;
	}
	
	// Use this for initialization
	void Start () {
		player = this.gameObject;
		col = player.GetComponent<CapsuleCollider>();
		player_sprite = player.transform.FindChild("player").GetComponent<UISprite>();
		player_animation = player.transform.FindChild("pikachu").GetComponent<tk2dAnimatedSprite>();
		playerStatus = PlayerStatus.Walking;
	}
	void CorrectPlayerPos()
	{
		if(player.transform.localPosition.x >= -30f)
		{
			player.transform.localPosition = new Vector3(-30f, player.transform.localPosition.y, player.transform.localPosition.z);
		}
		if(player.transform.localPosition.x <= -205f)
		{
			player.transform.localPosition = new Vector3(-205f, player.transform.localPosition.y, player.transform.localPosition.z);
		}
		if(player.transform.localPosition.y <= -80f)
		{
			player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f , player.transform.localPosition.z);
		}
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		CorrectPlayerPos();
		if(!playerStatus.Equals(PlayerStatus.None))
		{
			if(playerStatus.Equals(PlayerStatus.Walking)&&!motion_change)
			{
				player_animation.Play("Idle");
				motion_change = true;
			}
			if(!Input.anyKey) vel_x = 0;
			if(Input.GetKey("d")&&!playerStatus.Equals(PlayerStatus.LeftSliding)&&!playerStatus.Equals(PlayerStatus.RightSliding))
			{
				if(Input.GetKey("z")&&!playerStatus.Equals(PlayerStatus.Jumping))
				{
					playerStatus = PlayerStatus.LeftSliding;
					motion_change = false;
					vel_x = -sliding_x_speed;
					vel_y = sliding_y_speed;
					player.transform.FindChild("pikachu").localRotation = Quaternion.Euler(new Vector3(0,180,0));
				}
				else
				{
					vel_x = -walk_speed;
					player.transform.localPosition += new Vector3(vel_x,0f,0f);		
					if(player.transform.localPosition.x >= -30f)
					{
						player.transform.localPosition = new Vector3(-30f, player.transform.localPosition.y, player.transform.localPosition.z);
						Debug.Log(player.transform.localPosition);
					}
				}
			}else if(Input.GetKey("g")&&!playerStatus.Equals(PlayerStatus.LeftSliding)&&!playerStatus.Equals(PlayerStatus.RightSliding))
			{
				if(Input.GetKey("z")&&!playerStatus.Equals(PlayerStatus.Jumping))
				{
					playerStatus = PlayerStatus.RightSliding;
					motion_change = false;
					vel_x = sliding_x_speed;
					vel_y = sliding_y_speed;
				}
				else
				{
					vel_x = walk_speed;
					player.transform.localPosition += new Vector3(vel_x,0f,0f);		
					if(player.transform.localPosition.x <= -205f)
					{
						player.transform.localPosition = new Vector3(-205f, player.transform.localPosition.y, player.transform.localPosition.z);
					}
				}
			}
			
			if(Input.GetKey("r")&&!playerStatus.Equals(PlayerStatus.Jumping)&&!playerStatus.Equals(PlayerStatus.LeftSliding)&&!playerStatus.Equals(PlayerStatus.RightSliding)) 
			{
				playerStatus = PlayerStatus.Jumping;
				vel_y = jump_speed;
				motion_change = false;
				is_jumping = true;
			}
			
			if(playerStatus.Equals(PlayerStatus.Jumping))
			{
				if(!motion_change)
				{
					motion_change = true;
					player_animation.Play("Jump");
				}
				player.transform.localPosition += new Vector3(0f, vel_y,0f);
				vel_y -= jump_reduce;
				if(player.transform.localPosition.y <= -80f)
				{
					player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f, player.transform.localPosition.z);
					vel_y = 0;
					playerStatus = PlayerStatus.Walking;
					motion_change = false;
					is_jumping = false;
				}
				if(Input.GetKeyDown("z"))
				{
					shoot_pressed = true;
					StartCoroutine(SetSpikeFalse());
				}
			}
		
			else if(playerStatus.Equals(PlayerStatus.LeftSliding))
			{
				if(!motion_change)
				{
					motion_change = true;
					player_animation.Play("Slide");
					
				}
				player.transform.localPosition += new Vector3(vel_x, vel_y, 0f);
				vel_y -= jump_reduce;
				if(player.transform.localPosition.y <= -80f)
				{
					player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f, player.transform.localPosition.z);
					StartCoroutine(WakeUp());
				}
				if(player.transform.localPosition.x >= -30f)
				{
					player.transform.localPosition = new Vector3(-30f, player.transform.localPosition.y, player.transform.localPosition.z);
				}
			}
			else if(playerStatus.Equals(PlayerStatus.RightSliding))
			{
				if(!motion_change)
				{
					motion_change = true;
					player_animation.Play("Slide");
				}
				player.transform.localPosition += new Vector3(vel_x, vel_y, 0f);
				vel_y = vel_y - jump_reduce;
				if(player.transform.localPosition.y <= -80f)
				{
					player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f, player.transform.localPosition.z);
					StartCoroutine(WakeUp());
				}
				if(player.transform.localPosition.x <= -205f)
				{
					player.transform.localPosition = new Vector3(-205f, player.transform.localPosition.y, player.transform.localPosition.z);
				}
			}	
		}
	}
}
