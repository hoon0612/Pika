using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour 
{
	GameObject player; //range x : 30~205 range y : -80~
	tk2dAnimatedSprite player_animation;
	bool motion_change = false;
	
	bool walking = false;
	bool jump = false;
	bool leftSliding = false;
	bool rightSliding = false;
	
	float vel_x;
	float vel_y;
	
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
		player = GameObject.Find("Player1").gameObject;
		player_animation = player.transform.FindChild("pikachu").GetComponent<tk2dAnimatedSprite>();
		touchFrame = new TouchFrame();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(walking)
		{
			player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);	
			Debug.Log("walking!:);");
		}
		if(jump)
		{
			if(!motion_change)
			{
				motion_change = true;
				player_animation.Play("Jump");
			}
			vel_y -= 0.06f;
			player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
			if(player.transform.localPosition.y <= -80f)
			{
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -80f, player.transform.localPosition.z);
				player.rigidbody.velocity = new Vector3(vel_x, 0, 0);
				vel_y = 0;
				motion_change = false;
				jump = false;
				player_animation.Play("Idle");
			}
		}
	}
	
	void OnDrag(DragGesture gesture)
	{
		ContinuousGesturePhase phase = gesture.Phase;
		Vector2 deltaMove = gesture.DeltaMove;
		Vector2 totalMove = gesture.TotalMove;
		
		if(phase == ContinuousGesturePhase.Started)
		{
			float relativeObjectPosX = (player.transform.localPosition.x - 30f) / 175f; // max value is 1
			touchFrame.cursor = relativeObjectPosX * touchFrame.size_x;
			
			touchFrame.boundary_minus_x = gesture.Position.x - touchFrame.cursor;
			touchFrame.boundary_plus_x = touchFrame.boundary_minus_x + touchFrame.size_x;
			walking = true;
			
		}
		else if(phase == ContinuousGesturePhase.Updated)
		{
			float movePos = 30f + ((touchFrame.touch_pos.x - touchFrame.boundary_minus_x) / touchFrame.size_x * 175f);
			float mst = 1;
			
			if(movePos - 5 > player.transform.localPosition.x)
				mst = 1;
			else if(movePos + 5 < player.transform.localPosition.x)
				mst = -1;
			else mst = 0;
			//player.rigidbody.velocity = new Vector3(mst * 1.2f, player.rigidbody.velocity.y , 0);
			vel_x = mst * 1.2f;
			
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
			//Debug.Log("cursor : "+ touchFrame.cursor + "\nlower bound : " + touchFrame.boundary_minus_x + "\nupper bound : " + touchFrame.boundary_plus_x);
			if(!jump)
			{
				if(deltaMove.y > 35 && totalMove.y >= 5)
				{
					//Swipe UP!
					jump = true;
					vel_y = 2.3f;
				}
			}
		}
		else
		{
			player.rigidbody.velocity = Vector3.zero;
			walking = false;
			vel_x = 0;
		}
		
	}
	
	/*void OnSwipe(SwipeGesture gesture)
	{
		Vector2 move = gesture.Move;
		float velocity = gesture.Velocity;
		FingerGestures.SwipeDirection direction = gesture.Direction;
		if(direction == FingerGestures.SwipeDirection.Up && !jump)
		{
			jump = true;
			//player.rigidbody.velocity += new Vector3(0, 2.3f, 0);
			vel_y = 2.3f;
		}
		
	}*/
}
