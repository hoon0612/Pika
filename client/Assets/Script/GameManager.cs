using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	Ball ball;
	Player1 P1;
	Player2 P2;
	// Use this for initialization
	void Start () 
	{
		ball = GameObject.Find("Ball").GetComponent<Ball>();
		P1 = GameObject.Find("Player1").GetComponent<Player1>();
		P2 = GameObject.Find("Player2").GetComponent<Player2>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
