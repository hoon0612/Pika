using UnityEngine;
using System.Collections;

public class GameBall : MonoBehaviour {
	public GameObject ball;
	Vector3 pos;
	public Vector3 vel;
	bool is_col = false;
	public bool col_with_character = false;
	public bool is_spiked = false;
	public int spike_col_count = 0;
	void OnCollisionEnter(Collision col)
	{
		if(!is_col)
		{
			is_col = true;
			
			foreach(ContactPoint con in col.contacts){
				//Debug.Log(con.otherCollider.name);
				//is_spiked = false;
				
				if(con.otherCollider.name.Equals("Player")&&!col_with_character)
				{
					col_with_character = true;
					if(is_spiked)
					{
						spike_col_count ++;
						if(spike_col_count > 1)
						{
							is_spiked = false;
							spike_col_count = 0;
						}
					}
				}
			}
 		}
		else return;	
	}
	
	void OnCollisionExit(Collision col)
	{
		is_col = false;
	}
	
	public float BallSpeed()
	{
		return Mathf.Sqrt((vel.x*vel.x)+(vel.y*vel.y));
	}
	
	public void SetBallSpeed(float set_val)
	{
		float vel_x = set_val * vel.x / BallSpeed();
		float vel_y = set_val * vel.y / BallSpeed();
		ball.transform.rigidbody.velocity = new Vector3(vel_x,vel_y,0);
	}
	
	/*void CorrectBallSpeed()
	{
		if(!is_spiked)
		{
			if(vel.y>1.4f) 
			{
				ball.transform.rigidbody.velocity = new Vector3(vel.x*1.4f / Mathf.Abs(vel.y), 1.4f, vel.z);
			}
			else if(vel.y<-1.4f)
			{
				ball.transform.rigidbody.velocity = new Vector3(vel.x*1.4f / Mathf.Abs(vel.y), -1.4f, vel.z);
			}
			if(vel.x>1.4f) 
			{
				ball.transform.rigidbody.velocity = new Vector3(1.4f, vel.y*1.4f / Mathf.Abs(vel.x), vel.z);
			}
			else if(vel.x<-1.4f)
			{
				ball.transform.rigidbody.velocity = new Vector3(-1.4f, vel.y*1.4f / Mathf.Abs(vel.x), vel.z);
			}
			if(BallSpeed() > 2f)
			{
				SetBallSpeed(2f);
			}
		}
	}*/
	void CorrectBallSpeed()
	{
		float limit;
		float sqrt_2limit;
		if(!is_spiked)
		{
			limit = 1;
			sqrt_2limit = Mathf.Sqrt(2*limit);
			if(vel.y>limit) 
			{
				ball.transform.rigidbody.velocity = new Vector3(vel.x*limit / Mathf.Abs(vel.y), limit, vel.z);
			}
			else if(vel.y<-limit)
			{
				ball.transform.rigidbody.velocity = new Vector3(vel.x*limit / Mathf.Abs(vel.y), -limit, vel.z);
			}
			if(vel.x>limit) 
			{
				ball.transform.rigidbody.velocity = new Vector3(limit, vel.y*limit / Mathf.Abs(vel.x), vel.z);
			}
			else if(vel.x<-limit)
			{
				ball.transform.rigidbody.velocity = new Vector3(-limit, vel.y*limit / Mathf.Abs(vel.x), vel.z);
			}
			if(BallSpeed() > sqrt_2limit)
			{
				SetBallSpeed(sqrt_2limit);
			}
		}
		else
		{
			limit = 2;
			sqrt_2limit = Mathf.Sqrt(2*limit);
			if(vel.y>limit) 
			{
				ball.transform.rigidbody.velocity = new Vector3(vel.x*limit / Mathf.Abs(vel.y), limit, vel.z);
			}
			else if(vel.y<-limit)
			{
				ball.transform.rigidbody.velocity = new Vector3(vel.x*limit / Mathf.Abs(vel.y), -limit, vel.z);
			}
			if(vel.x>limit) 
			{
				ball.transform.rigidbody.velocity = new Vector3(limit, vel.y*limit / Mathf.Abs(vel.x), vel.z);
			}
			else if(vel.x<-limit)
			{
				ball.transform.rigidbody.velocity = new Vector3(-limit, vel.y*limit / Mathf.Abs(vel.x), vel.z);
			}
			if(BallSpeed() > sqrt_2limit)
			{
				SetBallSpeed(sqrt_2limit);
			}
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		ball = GameObject.Find("Ball").gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		pos = ball.transform.localPosition;
		vel = ball.transform.rigidbody.velocity;
		ball.transform.rigidbody.velocity -= new Vector3(0, 0.02f, 0);
		CorrectBallSpeed();
	}
}
