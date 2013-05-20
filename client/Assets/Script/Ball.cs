using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	public GameObject ball;
	Vector3 netPos;	//the net's position which has upper pivot
	Vector3 pos;
	Vector3 vel;
	public float vel_x, vel_y,gravity;
	public bool is_spiked = false;
	bool trigger_col_player = false;
	bool trigger_col_net_head = false;
	bool trigger_col_net_body = false;
	bool trigger_col_wall = false;
	tk2dAnimatedSprite ball_anim;
	
	void OnTriggerEnter(Collider col)
	{
		if(!trigger_col_wall)
		{
			if(col.name.Equals("ceilingCol"))
			{
				vel_y = -vel_y +gravity;
				ball.transform.localPosition += new Vector3(0,-1,0);
				Debug.Log("c!");
			}
			if(col.name.Equals("floorCol"))
			{
				vel_y = -vel_y;
				ball.transform.localPosition += new Vector3(0,1,0);
				Debug.Log("f");
			}
			if(col.name.Equals("leftCol"))
			{
				vel_x = -vel_x;
				ball.transform.localPosition += new Vector3(1,0,0);
			}
			if(col.name.Equals("rightCol"))
			{
				vel_x = -vel_x;
				ball.transform.localPosition += new Vector3(-1,0,0);
			}
			trigger_col_wall = true;
		}
		if(!trigger_col_net_head && !trigger_col_net_body) //ball <-> net
		{
			if(col.name.Equals("roundCol"))
			{
				vel_y = -vel_y;
				ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, 11f ,ball.transform.localPosition.z);
				trigger_col_net_head = true;
			}
			else if(col.name.Equals("boxCol"))
			{
				if(pos.x > 20)
				{
					vel_x = -vel_x;
					ball.transform.localPosition = new Vector3(25f, ball.transform.localPosition.y,ball.transform.localPosition.z);
				}
				else if(pos.x < -20)
				{
					vel_x = -vel_x;
					ball.transform.localPosition = new Vector3(-25f, ball.transform.localPosition.y,ball.transform.localPosition.z);
				}
				trigger_col_net_body = true;
			}
		}
		if(col.name.Equals("Player1")&&!trigger_col_player) // ball <-> player
		{
			is_spiked = false;
			ball_anim.Play("Idle");
			Player1 p = col.gameObject.GetComponent<Player1>();
			if(!p.upperSpike && !p.middleSpike && !p.lowerSpike)
			{
				vel_x = vel_x/10 + p.vel_x/2 + DirVectorElement(ball.transform.localPosition.x, p.transform.localPosition.x-10);
				if(ball.transform.localPosition.y < p.transform.localPosition.y)
					vel_y = -vel_y + p.vel_y/2;
				else
					vel_y = -vel_y + p.vel_y/2 + DirVectorElement(ball.transform.localPosition.y, p.transform.localPosition.y);
				trigger_col_player = true;	
			}
			else if(p.upperSpike && !p.middleSpike && !p.lowerSpike)
			{
				Debug.Log("Upper Spike!");
				vel_y = 300f;
				vel_x = -350f;
				p.upperSpike = false;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(!p.upperSpike && p.middleSpike && !p.lowerSpike)
			{
				Debug.Log("Middle Spike!");
				vel_y = -50f;
				vel_x = -400f;
				p.middleSpike = false;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(!p.upperSpike && !p.middleSpike && p.lowerSpike)
			{
				Debug.Log("Lower Spike!");
				vel_y = -400f;
				vel_x = -100f;
				p.lowerSpike = false;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
		}
		else if(col.name.Equals("Player2")&&!trigger_col_player)
		{
			is_spiked = false;
			ball_anim.Play("Idle");
			Player2 p = col.gameObject.GetComponent<Player2>();
			vel_x = vel_x/10 + p.vel_x/2 + DirVectorElement(ball.transform.localPosition.x, p.transform.localPosition.x+10);
			if(ball.transform.localPosition.y < p.transform.localPosition.y)
				vel_y = -vel_y + p.vel_y/2;
			else
				vel_y = -vel_y + p.vel_y/2 + DirVectorElement(ball.transform.localPosition.y, p.transform.localPosition.y);
			trigger_col_player = true;
		}
	}
	
	
	void OnTriggerStay(Collider col)
	{
		if(trigger_col_player){
			
		}
		
	}
	
	void OnTriggerExit(Collider col)
	{
		trigger_col_player = false;
		trigger_col_net_head = false;
		trigger_col_net_body = false;
		trigger_col_wall = false;
	}
	
	float DirVectorElement(float elem_1, float elem_2)
	{
		return (elem_1-elem_2)/20;
	}
	
	// Use this for initialization
	void Start () 
	{
		ball = this.gameObject;
		netPos = GameObject.Find("net").transform.localPosition + new Vector3(0, 50, 0);
		Debug.Log(netPos);
		pos = ball.transform.localPosition;
		
		vel_x = 0f;
		vel_y = -250f;
		gravity = -5f;
		ball_anim = GameObject.Find("Ball").transform.FindChild("ball").GetComponent<tk2dAnimatedSprite>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(!is_spiked) //restrict ball's speed
		{
			if(vel_x > 250f) vel_x = 250f;
			if(vel_x < -250f) vel_x = -250f;
			if(vel_y > 400f) vel_y = 400f;
			if(vel_y < -400f) vel_y = -400f;	
		}
		vel_y+=gravity;
		
		//ball.transform.localPosition += new Vector3(vel_x,vel_y,0f);
		ball.rigidbody.velocity = new Vector3(vel_x, vel_y + gravity, 0);
		pos = ball.transform.localPosition;
		
		//correct ball's local position 
		if(pos.x < -196f)
		{
			//vel_x = -1 * vel_x;
			ball.transform.localPosition = new Vector3(-196f,ball.transform.localPosition.y, ball.transform.localPosition.z);
		}
		if(pos.x > 196f)
		{
			//vel_x = -1 * vel_x;
			ball.transform.localPosition = new Vector3(196f,ball.transform.localPosition.y, ball.transform.localPosition.z);
		}
		if(pos.y < -100f)
		{
			//vel_y = -1 * vel_y;	
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, -100f, ball.transform.localPosition.z);
		}
		if(pos.y > 133f)
		{
			//vel_y = -1* vel_y + gravity;
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, 132f, ball.transform.localPosition.z);
		}
		/*if(pos.x >= 14f && pos.x <= 24f && pos.y <= 12f)
		{
			vel_x = -vel_x;
			ball.transform.localPosition = new Vector3(24f, ball.transform.localPosition.y,ball.transform.localPosition.z);
			
		}
		else if(pos.x <= -14f && pos.x >= -24f && pos.y <= 12f)
		{
			vel_x = -vel_x;
			ball.transform.localPosition = new Vector3(-24f, ball.transform.localPosition.y,ball.transform.localPosition.z);
			
		}
		else if(pos.x < 14f && pos.x > -14 && pos.y >= 6.5f && pos.y < 12f)
		{
			vel_y = -vel_y;
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, 12f ,ball.transform.localPosition.z);
			
		}*/
	}
		
}
