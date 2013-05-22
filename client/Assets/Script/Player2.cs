using UnityEngine;
using System.Collections;

public class Player2 : Player 
{
	void Awake()
	{
		is_right_user = false;
	}
	
	// Use this for initialization
	void Start () {
		player = this.gameObject;
		col = player.GetComponent<CapsuleCollider>();
		playerSprite = player.transform.FindChild("playerSprite").GetComponent<tk2dAnimatedSprite>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		CorrectPlayerPos();
	}
}
