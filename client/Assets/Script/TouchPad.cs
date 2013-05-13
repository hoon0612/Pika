using UnityEngine;
using System.Collections;

public class TouchPad : MonoBehaviour {
	GameObject player;
	GameObject TouchLocation;
	tk2dAnimatedSprite player_animation;
	Touch touch;
	float touch_x;
	float touch_time;
	float touch_vel_x;
	float touch_vel_y;
	bool is_touched = false;
	bool is_jumped = false;
	bool is_slided = false;
	bool motion_change = false;
	
	float vel_x;
	float vel_y;
	
	enum PlayerStatus 
	{
		Walking,
		Jumping,
		LeftSliding,
		RightSliding,
		UpperSmash,
		MiddleSmash,
		LowerSmash,
		Win,
		Lose
	}
	
	PlayerStatus playerStatus;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player1");
		TouchLocation = GameObject.Find("PanelBackground").transform.FindChild("TouchLocation").gameObject;
		player_animation = player.transform.FindChild("pikachu").GetComponent<tk2dAnimatedSprite>();
	}
	
	IEnumerator WakeUp()
	{
		yield return new WaitForSeconds(0.2f);
		if(player.transform.FindChild("pikachu").localRotation.y == 0)
			player.transform.FindChild("pikachu").localRotation = Quaternion.Euler(new Vector3(0,180,0));
		vel_y = 0;
		vel_x = 0;
		playerStatus = PlayerStatus.Walking;
		motion_change = false;
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.touchCount > 0 )
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				if(!is_touched)
				{
					is_touched = true;
					touch = Input.touches[0];
					touch_x = touch.position.x;
					TouchLocation.GetComponent<UISprite>().enabled = true;
				}
			}
			
			else if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
			{
				vel_x = 0;
				vel_y = 0;
				for(int i = 0; i<Input.touchCount ; i++)
				{	
					//Debug.Log(Input.GetTouch(i).fingerId+" "+i);
					if(touch.fingerId == Input.GetTouch(i).fingerId)
					{
						Touch now_touch = Input.GetTouch(i);
						touch_time += Time.deltaTime;
						touch_vel_x = (now_touch.position.x - touch_x)/touch_time;
						Debug.Log(touch_vel_x);
						if(touch_vel_x > 2000f||touch_vel_x < -2000f)//sliding
						{
							if(touch_vel_x > 0)//right sliding
							{
								vel_x = 0.9f;
								vel_y = 0.9f;
								playerStatus = PlayerStatus.RightSliding;
								break;
							}
							else//left sliding
							{
								vel_x = -0.9f;
								vel_y = -0.9f;
								playerStatus = PlayerStatus.LeftSliding;
								break;
							}
						}
						else//walking
						{
							if(playerStatus.Equals(PlayerStatus.LeftSliding)&&playerStatus.Equals(PlayerStatus.RightSliding)) break;
							playerStatus = PlayerStatus.Walking;
							if(Input.GetTouch(i).position.x - Screen.width/50f > touch_x)
							{
								vel_x = 0.75f;
								break;
							}
							else if(Input.GetTouch(i).position.x + Screen.width/50f < touch_x)
							{
								vel_x = -0.75f;
								break;
							}	
						}
					}
				}
				player.rigidbody.velocity = new Vector3(vel_x, 0, 0);
			}
		}
		else
		{	
			if(!playerStatus.Equals(PlayerStatus.LeftSliding)&&!playerStatus.Equals(PlayerStatus.RightSliding))
			{
				vel_x = 0;
			}
			if(!playerStatus.Equals(PlayerStatus.Jumping))
			{
				vel_y = 0;
			}
			touch_x = 0;
			touch_time = 0;
			player.rigidbody.velocity = Vector3.zero;
			is_touched = false;
			TouchLocation.GetComponent<UISprite>().enabled = false;
		}
		if(playerStatus.Equals(PlayerStatus.Walking))
		{
			if(!motion_change)
			{
				player_animation.Play("Idle");
				motion_change = true;
				Debug.Log("Walkiing!");
			}
		}
		else if(playerStatus.Equals(PlayerStatus.LeftSliding))
		{
			if(!motion_change)
			{
				motion_change = true;
				player_animation.Play("Slide");
				Debug.Log("LeftSlide!");
			}
			vel_y -= 0.068f;
			if(player.transform.localPosition.y <= -80f)
			{
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f, player.transform.localPosition.z);
				StartCoroutine(WakeUp());
			}
		}
		else if(playerStatus.Equals(PlayerStatus.RightSliding))
		{
			if(!motion_change)
			{
				motion_change = true;
				player_animation.Play("Slide");
				Debug.Log("RightSlide!");
			}
			vel_y -= 0.068f;
			if(player.transform.localPosition.y <= -80f)
			{
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f, player.transform.localPosition.z);
				StartCoroutine(WakeUp());
			}
		}
	}
}
