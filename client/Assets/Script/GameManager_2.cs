using UnityEngine;
using System.Collections;

public class GameManager_2 : MonoBehaviour {
	Ball ball;
	Player P1;
	float dist_p1;
	float dist_p2;
	// Use this for initialization
	void Start () {
		ball = GameObject.Find("Ball").GetComponent<Ball>();
		P1 = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		dist_p1 = Vector3.Distance(ball.transform.localPosition, P1.transform.localPosition - new Vector3(10,0,0));
		/*if(dist_p1 <= 40){
			ball.vel_x = -ball.vel_x + P1.vel_x;
			ball.vel_y = -ball.vel_y + P1.vel_y;
		}*/
	}
}
