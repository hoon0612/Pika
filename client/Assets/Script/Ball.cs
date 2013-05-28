using UnityEngine;
using System.Collections;
using System.Threading;

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
	tk2dAnimatedSprite ball_anim;
	GameManager gameManager;
	
	void OnTriggerEnter(Collider col)
	{
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
				if(pos.x > 0)
				{
					vel_x = -vel_x;
					ball.transform.localPosition = new Vector3(25f, ball.transform.localPosition.y,ball.transform.localPosition.z);
				}
				else if(pos.x < 0)
				{
					vel_x = -vel_x;
					ball.transform.localPosition = new Vector3(-25f, ball.transform.localPosition.y,ball.transform.localPosition.z);
				}
				trigger_col_net_body = true;
			}
		}
		if(col.name.Equals("rightUser")&&!trigger_col_player) // ball <-> player
		{
			is_spiked = false;
			ball_anim.Play("Idle");
			Player p = col.gameObject.GetComponent<Player>();
			if(p.pMotion != MotionType.SPIKE)
			{
				vel_x = vel_x/5 + p.vel_x/4 + DirVectorElement(ball.transform.localPosition.x, p.transform.localPosition.x-10);
				if(ball.transform.localPosition.y < p.transform.localPosition.y)
					vel_y = -vel_y + p.vel_y/4;
				else
					vel_y = -vel_y + p.vel_y/4 + DirVectorElement(ball.transform.localPosition.y, p.transform.localPosition.y);
				trigger_col_player = true;	
			}
			else if(p.pSpike == SpikeType.HIGH)
			{
				Debug.Log("Upper Spike!");
				vel_y = 300f;
				vel_x = -350f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(p.pSpike == SpikeType.MID)
			{
				Debug.Log("Middle Spike!");
				vel_y = -50f;
				vel_x = -400f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(p.pSpike == SpikeType.LOW)
			{
				Debug.Log("Lower Spike!");
				vel_y = -400f;
				vel_x = -160f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
		}
		else if(col.name.Equals("leftUser")&&!trigger_col_player)
		{
			is_spiked = false;
			ball_anim.Play("Idle");
			Player p = col.gameObject.GetComponent<Player>();
			if(p.pMotion != MotionType.SPIKE)
			{
				vel_x = vel_x/5 + p.vel_x/4 + DirVectorElement(ball.transform.localPosition.x, p.transform.localPosition.x+10);
				if(ball.transform.localPosition.y < p.transform.localPosition.y)
					vel_y = -vel_y + p.vel_y/4;
				else
					vel_y = -vel_y + p.vel_y/4 + DirVectorElement(ball.transform.localPosition.y, p.transform.localPosition.y);
				trigger_col_player = true;	
			}
			else if(p.pSpike == SpikeType.HIGH)
			{
				Debug.Log("Upper Spike!");
				vel_y = 300f;
				vel_x = 350f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(p.pSpike == SpikeType.MID)
			{
				Debug.Log("Middle Spike!");
				vel_y = -50f;
				vel_x = 400f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(p.pSpike == SpikeType.LOW)
			{
				Debug.Log("Lower Spike!");
				vel_y = -400f;
				vel_x = 160f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
		}
	}
	
	
	void OnTriggerStay(Collider col)
	{
		if(col.name.Equals("rightUser")&&trigger_col_player) // ball <-> player
		{
			Player p = col.gameObject.GetComponent<Player>();
			if(p.pSpike == SpikeType.HIGH)
			{
				Debug.Log("Upper Spike!");
				vel_y = 300f;
				vel_x = -350f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(p.pSpike == SpikeType.MID)
			{
				Debug.Log("Middle Spike!");
				vel_y = -50f;
				vel_x = -400f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(p.pSpike == SpikeType.LOW)
			{
				Debug.Log("Lower Spike!");
				vel_y = -400f;
				vel_x = -160f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
		}
		else if(col.name.Equals("leftUser")&&trigger_col_player)
		{
			Player p = col.gameObject.GetComponent<Player>();
			if(p.pSpike == SpikeType.HIGH)
			{
				Debug.Log("Upper Spike!");
				vel_y = 300f;
				vel_x = 350f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(p.pSpike == SpikeType.MID)
			{
				Debug.Log("Middle Spike!");
				vel_y = -50f;
				vel_x = 400f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
			else if(p.pSpike == SpikeType.LOW)
			{
				Debug.Log("Lower Spike!");
				vel_y = -400f;
				vel_x = 160f;
				p.pSpike = SpikeType.NONE;
				is_spiked = true;
				ball_anim.Play("Spike");
			}
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		trigger_col_player = false;
		trigger_col_net_head = false;
		trigger_col_net_body = false;
	}
	
	float DirVectorElement(float elem_1, float elem_2)
	{
		return (elem_1-elem_2)*5f;
	}
	
	void CorrectBallPos()
	{
		Vector3 pos = GetBallPos();
		if(pos.x < -195f)
		{
			vel_x = -1 * vel_x;
			SetBallPos(new Vector3(-195f,pos.y, pos.z));
		}
		if(pos.x > 195f)
		{
			vel_x = -1 * vel_x;
			SetBallPos(new Vector3(195f,pos.y, pos.z));
		}
		if(pos.y < -85f)
		{
			vel_y = -1 * vel_y;	
			SetBallPos(new Vector3(pos.x, -85f, pos.z));
			//Debug.Log(vel_y);
		}
		if(pos.y > 130f)
		{
			vel_y = -1* vel_y + gravity;
			SetBallPos(new Vector3(pos.x, 130f, pos.z));
		}
	}
	
	Vector3 GetBallPos()
	{
		return ball.transform.localPosition;
	}
	
	void SetBallPos(Vector3 arg)
	{
		ball.transform.localPosition = arg;
	}
	
	// Use this for initialization
	void Start () 
	{
		ball = this.gameObject;
		netPos = GameObject.Find("net").transform.localPosition + new Vector3(0, 50, 0);
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		
		pos = ball.transform.localPosition;
		
		vel_x = 0f;
		vel_y = -300f;
		gravity = -3.5f;
		ball_anim = GameObject.Find("Ball").transform.FindChild("ball").GetComponent<tk2dAnimatedSprite>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		CorrectBallPos();
		if(!is_spiked) //restrict ball's speed
		{
			if(vel_x > 100f) vel_x = 100f;
			if(vel_x < -100f) vel_x = -100f;
			if(vel_y > 250f) vel_y = 250f;
			if(vel_y < -250f) vel_y = -250f;	
		}
		vel_y+=gravity;
		
		//ball.transform.localPosition += new Vector3(vel_x,vel_y,0f);
		ball.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
		pos = ball.transform.localPosition;
		
		//correct ball's local position 
		/*if(ball.transform.localPosition.x < -196f)
		{
			//vel_x = -1 * vel_x;
			ball.transform.localPosition = new Vector3(-196f,ball.transform.localPosition.y, ball.transform.localPosition.z);
		}
		if(ball.transform.localPosition.x > 196f)
		{
			//vel_x = -1 * vel_x;
			ball.transform.localPosition = new Vector3(196f,ball.transform.localPosition.y, ball.transform.localPosition.z);
		}
		if(ball.transform.localPosition.y < -85f)
		{
			//vel_y = -1 * vel_y;	
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, -85f, ball.transform.localPosition.z);
		}
		if(ball.transform.localPosition.y > 133f)
		{
			//vel_y = -1* vel_y + gravity;
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, 132f, ball.transform.localPosition.z);
		}*/
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
