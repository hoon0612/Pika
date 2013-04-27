using UnityEngine;
using System.Collections;

public class GameBall : MonoBehaviour {
	public GameObject ball;
	Vector3 pos;
	public Vector3 vel;
	bool is_col = false;
	public bool col_with_character = false;
	public bool is_spiked = false;
	void OnCollisionEnter(Collision col){
		if(!is_col){
			is_col = true;
			
			foreach(ContactPoint con in col.contacts){
				//Debug.Log(con.otherCollider.name);
				//is_spiked = false;
				
				if(con.otherCollider.name.Equals("Player")&&!col_with_character){
					col_with_character = true;
					/*if(is_spiked){
						spike_col_count ++;
						if(spike_col_count > 1){
							is_spiked = false;
							spike_col_count = 0;
						}
					}*/
				}
			}
 		}else return;	
	}
	
	void OnCollisionStay(Collision col){
		if(is_col) {
			
		}
	}
	
	void OnCollisionExit(Collision col){
		is_col = false;
	}
	
	public float BallSpeed(){
		return Mathf.Sqrt((vel.x*vel.x)+(vel.y*vel.y));
	}
	
	public void SetBallSpeed(float set_val){
		float vel_x = set_val * vel.x / BallSpeed();
		float vel_y = set_val * vel.y / BallSpeed();
		ball.transform.rigidbody.velocity = new Vector3(vel_x,vel_y,0);
	}
	
	void CorrectBallSpeed(){
		if(!is_spiked)
		{
			Debug.Log("XX");
			if(BallSpeed() > 2){
				SetBallSpeed(2);
			}
			if(vel.y>1.8) {
				ball.transform.rigidbody.velocity = new Vector3(vel.x, 1.8f, vel.z);
			}
			else if(vel.y<-1.8){
				ball.transform.rigidbody.velocity = new Vector3(vel.x, -1.8f, vel.z);
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		ball = GameObject.Find("Ball").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		pos = ball.transform.localPosition;
		vel = ball.transform.rigidbody.velocity;
		CorrectBallSpeed();
	}
}
