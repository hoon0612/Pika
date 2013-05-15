using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour 
{
	GameObject player; //range x : 30~205 range y : -80~
	Player1 player1;
	Player2 player2;
	Player user;
	tk2dAnimatedSprite player_animation;
	bool motion_change = false;
	bool is_right_user; //if game player is on right of the device's screen, then this variable will have true, else false;
	
	class TouchFrame
	{
		public float size_x;
		public float size_y;
		public float cursor;
		public Vector2 touch_pos;
		public float boundary_minus_x;
		public float boundary_plus_x;
		
		
		public TouchFrame()
		{
			size_x = Screen.width/5;
			size_y = Screen.height/5;
			cursor = 0;
			touch_pos = Vector2.zero;
			boundary_minus_x = 0;
			boundary_plus_x = 0;
		}
	}
	TouchFrame touchFrame;
	
	// Use this for initialization
	void Start () 
	{
		player1 = GameObject.Find("Player1").GetComponent<Player1>();
		player2 = GameObject.Find("Player2").GetComponent<Player2>();
		Debug.Log(player1.isEnemy + " "+ player2.isEnemy);
		if(player1.isEnemy && !player2.isEnemy)
		{
			player = GameObject.Find("Player2").gameObject;
			user = player.GetComponent<Player2>();
			is_right_user = false;
		}
		else if(!player1.isEnemy && player2.isEnemy)
		{
			player = GameObject.Find("Player1").gameObject;
			user = player.GetComponent<Player1>();
			is_right_user = true;
		}
		else
		{
			Debug.Log("Touch Error! Can Not Find the User.");	
		}
		
			
		player_animation = player.transform.FindChild("pikachu").GetComponent<tk2dAnimatedSprite>();
		touchFrame = new TouchFrame();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
	}
	
	void OnDrag(DragGesture gesture)
	{
		ContinuousGesturePhase phase = gesture.Phase;
		Vector2 deltaMove = gesture.DeltaMove;
		float yMove = 0;
		yMove += deltaMove.y;
		
		if(phase == ContinuousGesturePhase.Started)
		{
			float relativeObjectPosX = 0;
			if(is_right_user)
				relativeObjectPosX = (player.transform.localPosition.x - 30f) / 175f; // max value is 1
			else
				relativeObjectPosX = -(player.transform.localPosition.x + 30f) / 175f;
			touchFrame.cursor = relativeObjectPosX * touchFrame.size_x;
			
			touchFrame.boundary_minus_x = gesture.Position.x - touchFrame.cursor;
			touchFrame.boundary_plus_x = touchFrame.boundary_minus_x + touchFrame.size_x;
			user.walking = true;
		}
		else if(phase == ContinuousGesturePhase.Updated)       
  		{
			float movePos = 0;
			float mst = 1;
			if(is_right_user)
			{
				movePos = 30f + ((touchFrame.touch_pos.x - touchFrame.boundary_minus_x) / touchFrame.size_x * 175f);
			}
		 		
			else
			{
				movePos = -30f - ((touchFrame.boundary_plus_x - touchFrame.touch_pos.x) / touchFrame.size_x * 175f);
			}
			if(movePos - 5 > player.transform.localPosition.x)
				mst = 1;
			else if(movePos + 5 < player.transform.localPosition.x)
				mst = -1;
			else mst = 0;
			if(user.can_swipe)
				user.vel_x = mst * 1.2f;
			
			touchFrame.touch_pos = gesture.Position;
			touchFrame.cursor = touchFrame.touch_pos.x - touchFrame.boundary_minus_x;
			if(touchFrame.touch_pos.x > touchFrame.boundary_plus_x)
			{
				touchFrame.boundary_plus_x += touchFrame.cursor - touchFrame.size_x;
				touchFrame.boundary_minus_x += touchFrame.cursor - touchFrame.size_x;
				touchFrame.cursor = touchFrame.size_x;
			}else if(touchFrame.touch_pos.x < touchFrame.boundary_minus_x)
			{
				touchFrame.boundary_minus_x += touchFrame.cursor;
				touchFrame.boundary_plus_x += touchFrame.cursor;
				touchFrame.cursor = 0;
			}
			//Debug.Log("Touch Pos : " + touchFrame.touch_pos.x +"/t/tCursor : " + touchFrame.cursor + "\nbndMinus : " + touchFrame.boundary_minus_x + "/t/tbndPlus : " + touchFrame.boundary_plus_x);
			if(user.can_swipe)
			{
				if(!user.jumping && !user.leftSliding && !user.rightSliding)
				{
					if(yMove > 20 && deltaMove.y > 10)// deltaMove.y > 35)
					{
						Debug.Log(yMove);
						user.jumping = true;
						user.vel_y = 2.3f;
						yMove = 0;
					}
					else if(deltaMove.x < -40)
					{
						user.leftSliding = true;
						user.vel_x = -1.5f;
						user.vel_y = 0.75f;
						user.can_swipe = false;
					}
					else if(deltaMove.x > 40)
					{
						user.rightSliding = true;
						user.vel_x = 1.5f;
						user.vel_y = 0.75f;
						user.can_swipe = false;
					}
				}
				else if(user.jumping && !user.upperSpike && !user.middleSpike && !user.lowerSpike)
				{
					if(deltaMove.y > 40 && yMove > 20)//upper spike
					{
						user.upperSpike = true;
					}
					else if(deltaMove.y < -40 && yMove < -20)//lower spike
					{
						user.lowerSpike = true;
					}
					else if(deltaMove.x > 30  || deltaMove.x < -30)//middle spike
					{
						user.middleSpike = true;
					}
				}
			}
			
		}
		else
		{
			player.rigidbody.velocity = new Vector3(0, user.vel_y, 0);
			user.walking = false;
		}
		
	}
	/*
	void OnSwipe(SwipeGesture gesture)
	{
		Vector2 move = gesture.Move;
		float velocity = gesture.Velocity;
		FingerGestures.SwipeDirection direction = gesture.Direction;
		if(can_swipe)
		{
			if(direction == FingerGestures.SwipeDirection.Up && !jumping && !leftSliding && !rightSliding)
			{
				jumping = true;
				vel_y = 2.3f;
			}
			else if(direction == FingerGestures.SwipeDirection.Left && !jumping && !leftSliding && !rightSliding)
			{
				leftSliding = true;
				vel_x = -1.5f;
				vel_y = 0.75f;
				can_swipe = false;
			}
			else if(direction == FingerGestures.SwipeDirection.Right && !jumping && !leftSliding && !rightSliding)
			{
				rightSliding = true;
				vel_x = 1.5f;
				vel_y = 0.75f;
				can_swipe = false;
			}	
		}
	}
	*/
}
