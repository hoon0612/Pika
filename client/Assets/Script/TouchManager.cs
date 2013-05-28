using UnityEngine;
using System.Collections;

public enum TouchControllerType
{
	FRAME = 0,
	BUTTON,
	JOYPAD
}

public enum TouchEvent
	{
		NONE,
		LEFT,
		RIGHT
	}

public class TouchManager : MonoBehaviour 
{
	GameManager gameManager;
	GameObject player; //range x : 30~205 range y : -80~
	GameObject bt_left, bt_right, bt_spike, joystick_bar, joystick_bg;
	Player1 player1;
	Player2 player2;
	Player user;
	public static bool is_touching = false;
	public static bool is_left_touching = false;
	public static bool is_right_touching = false;
	bool is_right_user; //if game player is on right of the device's screen, then this variable will have true, else false.
	bool is_left_clckd = false;
	bool is_right_clckd = false;
	float touched_time = 0;
	int dragFingerIdx;
	float jump_touch_dist = Screen.height/8;
	tk2dSprite sprite_bt_left, sprite_bt_right;
	TouchMotionEffect touchEffect;
	public static TouchControllerType controller = TouchControllerType.JOYPAD;
	
	public static TouchEvent touchEvent = TouchEvent.NONE;
	
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
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
		gameManager.SettingPlayers();
		touchFrame = new TouchFrame();
		bt_left = GameObject.Find("leftButton");
		bt_right = GameObject.Find("rightButton");
		joystick_bar = GameObject.Find("Joystick");
		joystick_bg = GameObject.Find("Joystick_background");
		bt_spike = GameObject.Find("Joystick_spikeButton");
		sprite_bt_left = GameObject.Find("leftButton").GetComponent<tk2dSprite>();
		sprite_bt_right = GameObject.Find("rightButton").GetComponent<tk2dSprite>();
		touchEffect = GameObject.Find("TouchMotionEffect").GetComponent<TouchMotionEffect>();
		if(controller != TouchControllerType.BUTTON)
		{
			GameObject.DestroyObject(bt_left as Object);
			GameObject.DestroyObject(bt_right as Object);
		}
		if(controller != TouchControllerType.JOYPAD)
		{
			GameObject.DestroyObject(bt_spike as Object);
			GameObject.DestroyObject(joystick_bg as Object);
			GameObject.DestroyObject(joystick_bar as Object);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(controller == TouchControllerType.BUTTON && !is_left_touching && !is_right_touching) // if use touch button
			gameManager.P1NoneTouching();
		else if(controller == TouchControllerType.FRAME && !is_touching)
			gameManager.P1NoneTouching();
	}
	
	void OnDrag(DragGesture gesture)
	{
		ContinuousGesturePhase phase = gesture.Phase;
		Vector2 deltaMove = gesture.DeltaMove;
		if(controller == TouchControllerType.FRAME)
		{
			if(phase == ContinuousGesturePhase.Started)
			{
				float relativeObjectPosX = 0;
				if(is_right_user)
					relativeObjectPosX = (player.transform.localPosition.x - 30f) / 175f; // max value is 1
				else
					relativeObjectPosX = -(player.transform.localPosition.x + 30f) / 175f; // min value is -1
				//Initialize touch Frame's cursor
				touchFrame = new TouchFrame();
				touchFrame.cursor_x = relativeObjectPosX * touchFrame.size_x;
				touchFrame.cursor_y = 0;
				//Initialize touch Frame's boundary
				touchFrame.boundary_minus_x = gesture.Position.x - touchFrame.cursor_x;
				touchFrame.boundary_minus_y = gesture.Position.y;
				touchFrame.boundary_plus_x = touchFrame.boundary_minus_x + touchFrame.size_x;
				touchFrame.boundary_plus_y = touchFrame.boundary_minus_y + touchFrame.size_y;
				is_touching = true;
				Debug.Log(GameObject.Find("VollyBallCamera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(gesture.Position.x , gesture.Position.y, 0)) - new Vector3(216,153,0));
				//Debug.Log("Touch!");
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
					gameManager.P1Walking(movePos);
				
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
					if(player1.pMotion == MotionType.WALK)//if(!player1.jumping && !player1.leftSliding && !player1.rightSliding &&!is_jumped)
					{
						if(touchFrame.touch_pos.y - touchFrame.boundary_minus_y > jump_touch_dist)//jump touch event
						{
							gameManager.P1Jumping();
						}
						else if(deltaMove.x < -40) // left sliding touch event
						{
							gameManager.P1Sliding(false);
						}
						else if(deltaMove.x > 40) // right sliding touch event
						{
							gameManager.P1Sliding(true);
						}
					}
					else if(player1.pMotion == MotionType.JUMP)//(player1.jumping && !player1.upperSpike && !player1.middleSpike && !player1.lowerSpike)
					{
						if(deltaMove.y > 25 )//&& deltaMove.x < 10 && deltaMove.x > -10)//upper spike
						{
							gameManager.P1Spiking(SpikeType.HIGH, false);
						}
						else if(deltaMove.y < -25)// && deltaMove.x < 10 && deltaMove.x > -10)//lower spike
						{
							gameManager.P1Spiking(SpikeType.LOW, false);
						}
						else if(deltaMove.x > 20)// && deltaMove.y < 10 && deltaMove.y > -10)//middle spike
						{
							gameManager.P1Spiking(SpikeType.MID, false);
						}
						else if(deltaMove.x < -20)// && deltaMove.y < 10 && deltaMove.y > -10)//middle spike
						{
							gameManager.P1Spiking(SpikeType.MID, true);
						}
					}
				}
				
			}
			else // it is called when user get off the hand from screen
			{
				is_touching = false;
			}
		}
		else if(controller == TouchControllerType.BUTTON)
		{
		 	FingerGestures.Finger finger = gesture.Fingers[0];	
			if(gesture.Phase == ContinuousGesturePhase.Started)
			{
				if(gesture.Selection == bt_left && player1.can_swipe)
				{
					if(is_left_clckd && Time.time - touched_time < 0.5f)
					{
						touchEvent = TouchEvent.NONE;
						gameManager.P1Sliding(false);
						is_left_clckd = false;
						if(is_right_clckd)
							is_right_clckd = false;
						Debug.Log("left Sliding!");
					}
					else
					{
						touchEvent = TouchEvent.LEFT;
						sprite_bt_left.SetSprite("bubble_orange");
						//GameObject particle = touchEffect.spawnParticle(new Vector3(gesture.Position.x, gesture.Position.y, -3));
						
						is_left_touching = true;
						touched_time = Time.time;
						is_left_clckd = true;
						if(is_right_clckd)
							is_right_clckd = false;
						Debug.Log("left walking!!!");
					}
					dragFingerIdx = finger.Index;
				}
				else if(gesture.Selection == bt_right && player1.can_swipe)
				{
					if(is_right_clckd && Time.time - touched_time < 0.5f)
					{
						touchEvent = TouchEvent.NONE;
						gameManager.P1Sliding(true);			
						is_right_clckd = false;
						if(is_left_clckd)
						is_left_clckd = false;
						Debug.Log("right Sliding!");
					}
					else
					{
						touchEvent = TouchEvent.RIGHT;
						sprite_bt_right.SetSprite("bubble_orange");
						is_right_touching = true;
						Debug.Log("right Button is selected!");
						touched_time = Time.time;
							is_right_clckd = true;
						if(is_left_clckd)
							is_left_clckd = false;
					}
					dragFingerIdx = finger.Index;
				}
			}
				
		else if(dragFingerIdx == finger.Index && gesture.Phase == ContinuousGesturePhase.Ended)
		{
			if(touchEvent == TouchEvent.LEFT)
			{
				sprite_bt_left.SetSprite("bubble_red");	
				is_left_touching = false;
				touchEvent = TouchEvent.NONE;
			}
			if(touchEvent == TouchEvent.RIGHT)
			{		
				sprite_bt_right.SetSprite("bubble_red");
				is_right_touching = false;
				touchEvent = TouchEvent.NONE;
			}
			else
			{
				sprite_bt_left.SetSprite("bubble_red");	
				is_left_touching = false;
				sprite_bt_right.SetSprite("bubble_red");
				is_right_touching = false;
			}
			
			//is_touching = false;
		}
		}
	}
	
	
	/*void OnLongPress(LongPressGesture gesture)
	{
		Debug.Log(gesture.Selection);
			
			if(gesture.Selection == bt_left && !is_left_clckd)
			{
				//gameManager.TmpP1Walking(true);
				touchEvent = TouchEvent.LEFT;
				sprite_bt_left.SetSprite("bubble_orange");
				is_touching = true;
				Debug.Log("left Button is selected!");
				touched_time = Time.time;
				is_left_clckd = true;
				if(is_right_clckd)
					is_right_clckd = false;
			}
		
			else if(gesture.Selection == bt_left && is_left_clckd && Time.time - touched_time < 0.65f)
			{
				gameManager.P1Sliding(false);			
				is_left_clckd = false;
				if(is_right_clckd)
					is_right_clckd = false;
			}
			else if(gesture.Selection == bt_right && !is_right_clckd)
			{
				//gameManager.TmpP1Walking(false);
				touchEvent = TouchEvent.RIGHT;
				sprite_bt_right.SetSprite("bubble_orange");
				is_touching = true;
				touched_time = Time.time;
				is_right_clckd = true;
				if(is_left_clckd)
					is_left_clckd = false;
			}
			else if(gesture.Selection == bt_right && is_right_clckd && Time.time - touched_time < 0.65f)
			{
				gameManager.P1Sliding(true);
				is_right_clckd = false;
				if(is_left_clckd)
					is_left_clckd = false;
			}
			else 	
				return;
	}*/
	
	void OnSwipe(SwipeGesture gesture)
	{
		if(controller != TouchControllerType.BUTTON)
			return;
		FingerGestures.SwipeDirection dir = gesture.Direction;
		if((dir == FingerGestures.SwipeDirection.Up || dir == FingerGestures.SwipeDirection.UpperLeftDiagonal || dir == FingerGestures.SwipeDirection.UpperRightDiagonal) && player1.pMotion == MotionType.WALK && player1.can_swipe)
		{
			gameManager.P1Jumping();
			
		}
		else if(player1.pMotion == MotionType.JUMP)
		{
			if(dir == FingerGestures.SwipeDirection.Up || dir == FingerGestures.SwipeDirection.UpperLeftDiagonal || dir == FingerGestures.SwipeDirection.UpperRightDiagonal)
			{
				gameManager.P1Spiking(SpikeType.HIGH, false);
			}
			else if(dir == FingerGestures.SwipeDirection.Down || dir == FingerGestures.SwipeDirection.LowerLeftDiagonal || dir == FingerGestures.SwipeDirection.LowerRightDiagonal)
			{
				gameManager.P1Spiking(SpikeType.LOW, false);
			}
			else if(dir == FingerGestures.SwipeDirection.Left)
			{
				gameManager.P1Spiking(SpikeType.MID, true);
			}
			else if(dir == FingerGestures.SwipeDirection.Right)
			{
				gameManager.P1Spiking(SpikeType.MID, false);
			}
		}
	}
	
	/*void OnFingerDown(FingerDownEvent e)
	{
		if(e.Phase == CustomFingerPhase.Down){

			if(e.Selection == bt_left && player1.can_swipe)// e.Finger.Phase != FingerGestures.FingerPhase.None)
			{
				if(is_left_clckd && Time.time - touched_time < 0.5f)
				{
					touchEvent = TouchEvent.NONE;
					gameManager.P1Sliding(false);			
					is_left_clckd = false;
					if(is_right_clckd)
					is_right_clckd = false;
					Debug.Log("left Sliding!");
				}
				else
				{
					touchEvent = TouchEvent.LEFT;
					sprite_bt_left.SetSprite("bubble_orange");
					is_left_touching = true;
					Debug.Log("left Button is selected!");
					touched_time = Time.time;
					is_left_clckd = true;
					if(is_right_clckd)
						is_right_clckd = false;
				}
				
			}
			else if(e.Selection == bt_right && player1.can_swipe)
			{
				if(is_right_clckd && Time.time - touched_time < 0.5f)
				{
					touchEvent = TouchEvent.NONE;
					gameManager.P1Sliding(true);			
					is_right_clckd = false;
					if(is_left_clckd)
					is_left_clckd = false;
					Debug.Log("right Sliding!");
				}
				else
				{
					touchEvent = TouchEvent.RIGHT;
					sprite_bt_right.SetSprite("bubble_orange");
					is_right_touching = true;
					Debug.Log("right Button is selected!");
					touched_time = Time.time;
					is_right_clckd = true;
					if(is_left_clckd)
						is_left_clckd = false;
				}
				
			}
		}
		else
		{
			if(touchEvent == TouchEvent.LEFT)
			{
				sprite_bt_left.SetSprite("bubble_red");	
				is_left_touching = false;
				touchEvent = TouchEvent.NONE;
			}
			if(touchEvent == TouchEvent.RIGHT)
			{		
				sprite_bt_right.SetSprite("bubble_red");
				is_right_touching = false;
				touchEvent = TouchEvent.NONE;
			}
			else
			{
				sprite_bt_left.SetSprite("bubble_red");	
				is_left_touching = false;
				sprite_bt_right.SetSprite("bubble_red");
				is_right_touching = false;
			}
			
			//is_touching = false;
		}
	}*/
	
	
	/*void OnFingerUp(FingerUpEvent e)
	{
		sprite_bt_left.SetSprite("bubble_red");
		sprite_bt_right.SetSprite("bubble_red");
		
		touchEvent = TouchEvent.NONE;
		is_touching = false;
		
	}*/
}
