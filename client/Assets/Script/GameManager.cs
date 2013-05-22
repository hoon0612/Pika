using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;

public class GameManager : MonoBehaviour 
{
	
	Ball ball;
	Player1 P1;
	Player2 P2;
	bool use_networking = true;
	bool is_game_started = true;
	
	public void SettingPlayers()
	{
		P1 = FindObjectOfType(typeof(Player1)) as Player1;
		P2 = FindObjectOfType(typeof(Player2)) as Player2;
	}
	
	public void P1Walking(float movePos)
	{
		int sign;	// the sign means + or -
		if(movePos - 5 > P1.player.transform.localPosition.x)
			sign = 1;
		else if(movePos + 5 < P1.player.transform.localPosition.x)
			sign = -1;
		else sign = 0;
		if(P1.can_swipe)
			P1.vel_x = sign * P1.walking_speed;
	}
	
	public void P1Jumping()
	{
		P1.vel_y = P1.jump_speed;
		P1.pMotion = MotionType.JUMP;
	}
	
	public void P1Sliding(bool is_right)
	{
		if(is_right)
		{
			P1.vel_x = P1.sliding_x_speed;
			P1.vel_y = P1.sliding_y_speed;
			P1.can_swipe = false;
			P1.pMotion = MotionType.SLIDE;
		}
		else
		{
			P1.vel_x = -P1.sliding_x_speed;
			P1.vel_y = P1.sliding_y_speed;
			P1.can_swipe = false;
			P1.pMotion = MotionType.SLIDE;
		}
	}
	
	public void P1Spiking(SpikeType st, bool isMidLeft) // isMidLeft means that if user swipe left then this value gets true else false
	{
		if(st == SpikeType.HIGH)
		{
			P1.pSpike = SpikeType.HIGH;
		}
		else if(st == SpikeType.LOW)
		{
			P1.pSpike = SpikeType.LOW;
		}
		else
		{
			P1.pSpike = SpikeType.MID;
			if(isMidLeft)
			{
				P1.vel_x = -P1.spike_x_speed;
			}
			else
			{
				P1.vel_x = P1.spike_x_speed;
			}
		}
		P1.pMotion = MotionType.SPIKE;
	}
	
	// Use this for initialization
	void Start () 
	{
		ball = GameObject.Find("Ball").GetComponent<Ball>();
		if(is_game_started)
		{
			//ball.MakeBallThread();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
