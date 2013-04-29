using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	Player user_1;
	GameBall ball;
	
	// Use this for initialization
	void Start () 
	{
		user_1 = GameObject.Find("Player").GetComponent<Player>();
		ball = GameObject.Find("Ball").GetComponent<GameBall>();
	}
	
	Vector3 SetSpikePower(Vector3 direction, float power)
	{
		float scala = Mathf.Sqrt((direction.x*direction.x)+(direction.y*direction.y));
		return new Vector3(power * direction.x / scala, power * direction.y / scala, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(ball.col_with_character)
		{
			ball.col_with_character = false;
			ball.transform.rigidbody.velocity += new Vector3(user_1.vel_x/8,user_1.vel_y/4);
			Vector3 touch_char = user_1.col.center+new Vector3(0,5,0);
			Vector3 ball_vel = ball.vel;	
			Vector3 force = touch_char - ball.transform.localPosition;
			if(user_1.shoot_pressed && user_1.is_jumping)
			{
				user_1.shoot_pressed = false;
				ball.is_spiked = true;
				ball.spike_col_count = 1;
				
				ball.rigidbody.velocity = SetSpikePower(force,2);
				Debug.Log(ball.BallSpeed());
			}
			/*else
			{
				ball.transform.rigidbody.velocity += new Vector3(user_1.vel_x/8,user_1.vel_y/4);
				if(ball.transform.rigidbody.velocity.y < 0)
				{
					ball.transform.rigidbody.velocity = new Vector3(ball.transform.rigidbody.velocity.x,-ball.transform.rigidbody.velocity.y,0);
				}
			}*/		
		}
	}
}
