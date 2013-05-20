using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour 
{
	GameObject player; //range x : 30~205 range y : -80~
	Player1 player1;
	Player2 player2;
	Player user;
	
	bool is_right_user; //if game player is on right of the device's screen, then this variable will have true, else false.
	float jump_touch_dist = Screen.height/8;
	
	class TouchFrame
	{
		public float size_x;	//the frame's width
		public float size_y; 	//the frame's height
		public float cursor_x; //the frame's x-axis cursor
		public float cursor_y; //the frame's y_axis cursor
		public Vector2 touch_pos;	//user's touch position
		public float boundary_minus_x;	//the frame's left bound
		public float boundary_plus_x;	//the frame's right bound
		public float boundary_minus_y;	//the frame's lower bound
		public float boundary_plus_y;	//the frame's upper bound
		
		public TouchFrame()
		{
			size_x = Screen.width/5;
			size_y = Screen.height/5;
			cursor_x = 0;
			cursor_y = 0;
			touch_pos = Vector2.zero;
			boundary_minus_x = 0;
			boundary_plus_x = 0;
			boundary_minus_y = 0;
			boundary_plus_y = 0;
		}
	}
	TouchFrame touchFrame;
	
	// Use this for initialization
	void Start () 
	{
		GameObject rightUser = GameObject.Find("rightUser").gameObject;
		GameObject leftUser = GameObject.Find("leftUser").gameObject;
		is_right_user = true;
		if(is_right_user)
		{
			rightUser.AddComponent<Player1>();
			leftUser.AddComponent<Player2>();
			player1 = rightUser.GetComponent<Player1>();
			player2 = leftUser.GetComponent<Player2>();
			player1.is_right_user = true;
			player2.is_right_user = false;
			player = rightUser;
		}
		else
		{
			rightUser.AddComponent<Player2>();
			leftUser.AddComponent<Player1>();
			player1 = leftUser.GetComponent<Player1>();
			player2 = rightUser.GetComponent<Player2>();
			player1.is_right_user = false;
			player2.is_right_user = true;
			player = leftUser;
		}
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
		
		if(phase == ContinuousGesturePhase.Started)
		{
			float relativeObjectPosX = 0;
			if(is_right_user)
				relativeObjectPosX = (player.transform.localPosition.x - 30f) / 175f; // max value is 1
			else
				relativeObjectPosX = -(player.transform.localPosition.x + 30f) / 175f; // min value is -1
			//Initialize touch Frame's cursor
			touchFrame.cursor_x = relativeObjectPosX * touchFrame.size_x;
			touchFrame.cursor_y = 0;
			//Initialize touch Frame's boundary
			touchFrame.boundary_minus_x = gesture.Position.x - touchFrame.cursor_x;
			touchFrame.boundary_minus_y = gesture.Position.y;
			touchFrame.boundary_plus_x = touchFrame.boundary_minus_x + touchFrame.size_x;
			touchFrame.boundary_plus_y = touchFrame.boundary_minus_y + touchFrame.size_y;
			player1.walking = true;
		}
		else if(phase == ContinuousGesturePhase.Updated)       
  		{
			float movePos = 0; // player's move position which is on x-axis
			float mst = 1;		//player's moving direction ( + : move right, - : move left)
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
			if(player1.can_swipe)
				player1.vel_x = mst * player1.walking_speed;
			
			//update touch Frame's cursor and touch Frame's boundary
			touchFrame.touch_pos = gesture.Position;
			touchFrame.cursor_x = touchFrame.touch_pos.x - touchFrame.boundary_minus_x;
			touchFrame.cursor_y = touchFrame.touch_pos.y - touchFrame.boundary_minus_y;
			
			/*move touch frame if cursors get out of this frame*/
			if(touchFrame.touch_pos.x > touchFrame.boundary_plus_x)
			{
				touchFrame.boundary_plus_x += touchFrame.cursor_x - touchFrame.size_x;
				touchFrame.boundary_minus_x += touchFrame.cursor_x - touchFrame.size_x;
				touchFrame.cursor_x = touchFrame.size_x;
			}else if(touchFrame.touch_pos.x < touchFrame.boundary_minus_x)
			{
				touchFrame.boundary_minus_x += touchFrame.cursor_x;
				touchFrame.boundary_plus_x += touchFrame.cursor_x;
				touchFrame.cursor_x = 0;
			}
			if(touchFrame.touch_pos.y > touchFrame.boundary_plus_y)
			{
				touchFrame.boundary_plus_y += touchFrame.cursor_y - touchFrame.size_y;
				touchFrame.boundary_minus_y += touchFrame.cursor_y - touchFrame.size_y;
				touchFrame.cursor_y = touchFrame.size_y;
			}
			else if(touchFrame.touch_pos.y < touchFrame.boundary_minus_y)
			{
				touchFrame.boundary_plus_y += touchFrame.cursor_y;
				touchFrame.boundary_minus_y += touchFrame.cursor_y;
				touchFrame.cursor_y = 0;
			}
			//Debug.Log("Touch Pos : " + touchFrame.touch_pos.x +"/t/tCursor : " + touchFrame.cursor + "\nbndMinus : " + touchFrame.boundary_minus_x + "/t/tbndPlus : " + touchFrame.boundary_plus_x);
			
			if(player1.can_swipe)
			{
				if(!player1.jumping && !player1.leftSliding && !player1.rightSliding)
				{
					if(touchFrame.touch_pos.y - touchFrame.boundary_minus_y > jump_touch_dist)//jump touch event
					{
						player1.jumping = true;
						player1.vel_y = player1.jump_speed;
					}
					else if(deltaMove.x < -40) // left sliding touch event
					{
						player1.leftSliding = true;
						player1.vel_x = -player1.sliding_x_speed;
						player1.vel_y = player1.sliding_y_speed;
						player1.can_swipe = false;
					}
					else if(deltaMove.x > 40) // right sliding touch event
					{
						player1.rightSliding = true;
						player1.vel_x = player1.sliding_x_speed;
						player1.vel_y = player1.sliding_y_speed;
						player1.can_swipe = false;
					}
				}
				else if(player1.jumping && !player1.upperSpike && !player1.middleSpike && !player1.lowerSpike)
				{
					if(deltaMove.y > 25)//upper spike
					{
						player1.upperSpike = true;
					}
					else if(deltaMove.y < -25)//lower spike
					{
						player1.lowerSpike = true;
					}
					else if(deltaMove.x > 20)//middle spike
					{
						player1.middleSpike = true;
						player1.vel_x = player1.spike_x_speed;
					}
					else if(deltaMove.x < -20)//middle spike
					{
						player1.middleSpike = true;
						player1.vel_x = -player1.spike_x_speed;
					}
				}
			}
			
		}
		else // it is called when user get off the hand from screen
		{
			if(!player1.upperSpike && !player1.middleSpike && !player1.lowerSpike)
				player.rigidbody.velocity = new Vector3(0, player1.vel_y, 0);
			else
				player.rigidbody.velocity = new Vector3(player1.vel_x, player1.vel_y, 0);
			player1.walking = false;
		}
		
	}
}
