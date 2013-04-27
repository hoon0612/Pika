using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	Player user_1;
	//Ball ball;
	GameBall ball;
	Vector3 ball_pos;
	Vector3 user1_pos;
	float touch_delay = 0;
	bool touch_once = false;
	
	// Use this for initialization
	void Start () {
		user_1 = GameObject.Find("Player").GetComponent<Player>();
		ball = GameObject.Find("Ball").GetComponent<GameBall>();
	}
	
	// Update is called once per frame
	void Update () {
		if(ball.col_with_character){
			ball.transform.rigidbody.velocity += new Vector3(user_1.vel_x/8,user_1.vel_y/4);
			ball.col_with_character = false;
			if(user_1.shoot_pressed){
				user_1.shoot_pressed = false;
				ball.is_spiked = true;
				ball.spike_col_count = 1;
				Vector3 touch_char = user_1.col.center+new Vector3(0,5,0);
				Vector3 ball_vel = ball.vel;
				Vector3 spike_force = touch_char - ball.transform.localPosition;
				spike_force = new Vector3(spike_force.x/1000,spike_force.y/1000,spike_force.z);
				
				ball.ball.rigidbody.AddForce(spike_force);
				if(ball.BallSpeed()>1.2f)
					ball.SetBallSpeed(1.2f);
				Debug.Log(ball.BallSpeed());
			}
		}
	}
}
