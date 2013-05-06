using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public GameObject ball;
	UISprite ball_sprite;
	Vector3 pos;
	Vector3 vel;
	public float vel_x, vel_y,gravity;
	public bool is_shooted = false;
	bool trigger_col = false;
	void OnCollisionEnter(Collision col){
		/*Debug.Log("aa");
		Debug.Log(col.gameObject.name);
		Debug.Log(col.collider.name);
		//
		foreach(ContactPoint contact in col.contacts){
			Debug.Log(contact.point);
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}
		float sqr_speed = ball.transform.rigidbody.velocity.x*ball.transform.rigidbody.velocity.x + ball.transform.rigidbody.velocity.y*ball.transform.rigidbody.velocity.y;
		if(sqr_speed < 8f)
			ball.transform.rigidbody.velocity *= 1.2f;
		else
			ball.transform.rigidbody.velocity /= 1.2f;*/
		Debug.Log("xxxx");
		if(trigger_col){
			//ball.transform.rigidbody.velocity = new Vector3(vel.x / 4f, vel.y / 4f, vel.z);
			trigger_col = false;
		}
	}
	
	void OnTriggerEnter(Collider col){
		Debug.Log("col!");
		if(col.name.Equals("Player")&&!trigger_col){
			Player p = col.gameObject.GetComponent<Player>();
			vel_x = vel_x/4 + p.vel_x/2 + DirVectorElement(ball.transform.localPosition.x, p.transform.localPosition.x);
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
	
	float DirVectorElement(float ball, float player){
		return (ball-player)/10;
	}
	
	// Use this for initialization
	void Start () {
		//gravity = 2.5f;
		ball = this.gameObject;
		ball_sprite = this.transform.FindChild("ball").GetComponent<UISprite>();
		pos = ball.transform.localPosition;
		
		vel_x = 0f;
		vel_y = -2.5f;
		gravity = -0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		if(vel_x > 2.5f) vel_x = 2.5f;
		if(vel_x < -2.5f) vel_x = -2.5f;
		if(vel_y > 6f) vel_y = 6f;
		if(vel_y < -6f) vel_y = -6f;
		
		ball.transform.localPosition += new Vector3(vel_x,vel_y,0f);
		vel_y+=gravity;
		pos = ball.transform.localPosition;
		if(pos.x <= -196f){
			vel_x = -1 * vel_x;
			ball.transform.localPosition = new Vector3(-196f,ball.transform.localPosition.y, ball.transform.localPosition.z);
		}
		if(pos.x >= 196f){
			vel_x = -1 * vel_x;
			ball.transform.localPosition = new Vector3(196f,ball.transform.localPosition.y, ball.transform.localPosition.z);
		}
		if(pos.y <= -80f){
			vel_y = -1 * vel_y;	
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, -80f, ball.transform.localPosition.z);
		}
		if(pos.y >= 133f){
			vel_y = -0.45f* vel_y;
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, 133f, ball.transform.localPosition.z);
		}
		if(pos.x >= 14f && pos.x <= 18f && pos.y <= 6.5f){
			vel_x = -vel_x;
			ball.transform.localPosition = new Vector3(18f, ball.transform.localPosition.y,ball.transform.localPosition.z);
			
		}
		else if(pos.x <= -14f && pos.x >= -18f && pos.y <= 6.5f){
			vel_x = -vel_x;
			ball.transform.localPosition = new Vector3(-18f, ball.transform.localPosition.y,ball.transform.localPosition.z);
			
		}
		else if(pos.x < 14f && pos.x > -14 && pos.y <= 6.5f){
			vel_y = -vel_y;
			ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, 10f ,ball.transform.localPosition.z);
			
		}
	}
		
}
