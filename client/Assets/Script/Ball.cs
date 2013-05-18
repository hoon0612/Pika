using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public GameObject ball;
	UISprite ball_sprite;
	Vector3 pos;
	Vector3 vel;
	public float vel_x, vel_y,gravity;
	public bool is_spiked = false;
	bool trigger_col = false;
	
	void OnTriggerEnter(Collider col){
		if(col.name.Equals("net") && !trigger_col)
		{
			
		}
		if(col.name.Equals("Player1")&&!trigger_col){
			is_spiked = false;
			Player1 p = col.gameObject.GetComponent<Player1>();
			if(!p.upperSpike && !p.middleSpike && !p.lowerSpike)
			{
				vel_x = vel_x/4 + p.vel_x/2 + DirVectorElement(ball.transform.localPosition.x, p.transform.localPosition.x);
				if(ball.transform.localPosition.y < p.transform.localPosition.y)
					vel_y = -vel_y + p.vel_y/2;
				else
					vel_y = -vel_y + p.vel_y/2 + DirVectorElement(ball.transform.localPosition.y, p.transform.localPosition.y);
				trigger_col = true;	
			}
			else if(p.upperSpike && !p.middleSpike && !p.lowerSpike)
			{
				Debug.Log("Upper Spike!");
				vel_y = 7f;
				vel_x = -3.5f;
				p.upperSpike = false;
				is_spiked = true;
			}
			else if(!p.upperSpike && p.middleSpike && !p.lowerSpike)
			{
				Debug.Log("Middle Spike!");
				vel_y = -2f;
				vel_x = -7f;
				p.middleSpike = false;
				is_spiked = true;
			}
			else if(!p.upperSpike && !p.middleSpike && p.lowerSpike)
			{
				Debug.Log("Lower Spike!");
				vel_y = -7f;
				vel_x = -3f;
				p.lowerSpike = false;
				is_spiked = true;
			}
		}
		else if(col.name.Equals("Player2")&&!trigger_col){
			Player2 p = col.gameObject.GetComponent<Player2>();
			is_spiked = false;
			vel_x = vel_x/4 + p.vel_x/2 + DirVectorElement(ball.transform.localPosition.x, p.transform.localPosition.x);
			if(ball.transform.localPosition.y < p.transform.localPosition.y)
				vel_y = -vel_y + p.vel_y/2;
			else
				vel_y = -vel_y + p.vel_y/2 + DirVectorElement(ball.transform.localPosition.y, p.transform.localPosition.y);
			trigger_col = true;
		}
	}
	
	
	void OnTriggerStay(Collider col){
		if(trigger_col){
			
		}
	}
	
	void OnTriggerExit(Collider col){
		trigger_col = false;
	}
	
	float DirVectorElement(float elem_1, float elem_2){
		return (elem_1-elem_2)/10;
	}
	
	// Use this for initialization
	void Start () {
		ball = this.gameObject;
		ball_sprite = this.transform.FindChild("ball").GetComponent<UISprite>();
		pos = ball.transform.localPosition;
		
		vel_x = 0f;
		vel_y = -2.5f;
		gravity = -0.1f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!is_spiked)
		{
			if(vel_x > 2.5f) vel_x = 2.5f;
			if(vel_x < -2.5f) vel_x = -2.5f;
			if(vel_y > 6f) vel_y = 6f;
			if(vel_y < -6f) vel_y = -6f;	
		}
		vel_y+=gravity;
		ball.transform.localPosition += new Vector3(vel_x,vel_y,0f);
		pos = ball.transform.localPosition;
		if(pos.x < -196f){
			vel_x = -1 * vel_x;
			ball.transform.localPosition = new Vector3(-196f,ball.transform.localPosition.y, ball.transform.localPosition.z);
		}
		if(pos.x > 196f){
			vel_x = -1 * vel_x;
			ball.transform.localPosition = new Vector3(196f,ball.transform.localPosition.y, ball.transform.localPosition.z);
		}
		if(pos.y < -80f){
			vel_y = -1 * vel_y;	
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, -80f, ball.transform.localPosition.z);
		}
		if(pos.y > 133f){
			vel_y = -1* vel_y + gravity;
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, 132f, ball.transform.localPosition.z);
		}
		if(pos.x >= 14f && pos.x <= 24f && pos.y <= 12f){
			vel_x = -vel_x;
			ball.transform.localPosition = new Vector3(24f, ball.transform.localPosition.y,ball.transform.localPosition.z);
			
		}
		else if(pos.x <= -14f && pos.x >= -24f && pos.y <= 12f){
			vel_x = -vel_x;
			ball.transform.localPosition = new Vector3(-24f, ball.transform.localPosition.y,ball.transform.localPosition.z);
			
		}
		else if(pos.x < 14f && pos.x > -14 && pos.y >= 6.5f && pos.y < 12f){
			vel_y = -vel_y;
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, 12f ,ball.transform.localPosition.z);
			
		}
	}
		
}
