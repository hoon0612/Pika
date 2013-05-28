using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

public class GameManager : MonoBehaviour 
{
	SocketNetworking sn;
	Pika.Game.Control rcvMsg;
	Ball ball;	// volley ball
	Player1 P1;	// user
	Player2 P2;	// opponent
	bool use_networking = false;		//if gamer use networking, this value have true
	bool is_game_started = true;	//if game is started, then this value have true
	int frame_count = 0;
	
	public bool is_P1_vel_changed = false;
	
	float gameTime = 0;
	int receiveTime = 0;
	float P1vel_x = 0, P1vel_y =0, P1loc_x = 0, P1loc_y = 0;
	
	
	public void SettingPlayers()
	{
		P1 = FindObjectOfType(typeof(Player1)) as Player1;
		P2 = FindObjectOfType(typeof(Player2)) as Player2;
	}
	
	public Pika.Game.Control.MotionType SetPlayerMotionType(Player p)
	{
		if(p.pMotion == MotionType.WALK)
			return Pika.Game.Control.MotionType.WALK;
		else if(p.pMotion == MotionType.JUMP)
			return Pika.Game.Control.MotionType.JUMP;
		else if(p.pMotion == MotionType.SLIDE)
			return Pika.Game.Control.MotionType.SLIDE;
		else if(p.pMotion == MotionType.SPIKE)
			return Pika.Game.Control.MotionType.SPIKE;
		else if(p.pMotion == MotionType.WIN)
			return Pika.Game.Control.MotionType.WIN;
		else
			return Pika.Game.Control.MotionType.LOSE;
	}
	
	public MotionType GetPlayerMotionType(Pika.Game.Control.MotionType mt)
	{
		if(mt == Pika.Game.Control.MotionType.WALK)
			return MotionType.WALK;
		else if(mt == Pika.Game.Control.MotionType.JUMP)
			return MotionType.JUMP;
		else if(mt == Pika.Game.Control.MotionType.SLIDE)
			return MotionType.SLIDE;
		else if(mt == Pika.Game.Control.MotionType.SPIKE)
			return MotionType.SPIKE;
		else if(mt == Pika.Game.Control.MotionType.WIN)
			return MotionType.WIN;
		else
			return MotionType.LOSE;
	}
	
	public void MakeP1Info()
	{
		if(use_networking && is_P1_vel_changed)
		{
			var msg = new Pika.Game.Control {
				id = "p1",
				time = (int)gameTime,
				Character = new Pika.Game.Control.Status {
					loc_x = P1.transform.localPosition.x,
					loc_y = P1.transform.localPosition.y,
					vel_x = P1.vel_x,
					vel_y = P1.vel_y
				},
				motion = SetPlayerMotionType(P1)
			};
			sn.revcMsgFromGameManager(msg);	
		}
		is_P1_vel_changed = false;
	}
	
	public void UpdateP1Walking()
	{
		if(P1.pMotion != MotionType.SLIDE && TouchManager.controller == TouchControllerType.BUTTON)
		{
			if(TouchManager.touchEvent == TouchEvent.LEFT)
			{
				P1.vel_x = -P1.walking_speed;
			}
			else if(TouchManager.touchEvent == TouchEvent.RIGHT)
			{
				P1.vel_x = P1.walking_speed;
			}
			else
			{
				P1.vel_x = 0;
			}
		}
	}
	
	public void P1Walking(float movePos)
	{
		//float velx, vely;
		//velx = P1.vel_x;
		//vely = P1.vel_y;
		int sign;	// the sign means + or -
		if(movePos - 5 > P1.player.transform.localPosition.x)
			sign = 1;
		else if(movePos + 5 < P1.player.transform.localPosition.x)
			sign = -1;
		else sign = 0;
		if(P1.can_swipe)
			P1.vel_x = sign * P1.walking_speed;
		//if(velx != P1.vel_x || vely != P1.vel_y)
		//	is_P1_vel_changed = true;
		//MakeP1Info();
	}
	
	public void P1Jumping()
	{
		//float velx, vely;
		//velx = P1.vel_x;
		//vely = P1.vel_y;
		P1.vel_y = P1.jump_speed;
		P1.pMotion = MotionType.JUMP;
		//if(velx != P1.vel_x || vely != P1.vel_y)
		//	is_P1_vel_changed = true;
	}
	
	public void P1Sliding(bool is_right)
	{
		//float velx, vely;
		//velx = P1.vel_x;
		//vely = P1.vel_y;
		if(P1.pMotion == MotionType.JUMP || P1.pMotion == MotionType.SPIKE)
			return;
		
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
		//if(velx != P1.vel_x || vely != P1.vel_y)
		//	is_P1_vel_changed = true;
	}
	
	public void P1Spiking(SpikeType st, bool isMidLeft) // isMidLeft means that if user swipe left then this value gets true else false
	{
		//float velx, vely;
		//velx = P1.vel_x;
		//vely = P1.vel_y;
		if(P1.transform.localPosition.y < -70f) // if Player1's y-axis position is smaller than -70f, then just return
			return;
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
			/*if(isMidLeft)
			{
				P1.vel_x = -P1.spike_x_speed;
			}
			else
			{
				P1.vel_x = P1.spike_x_speed;
			}*/
		}
		P1.pMotion = MotionType.SPIKE;
		//if(P1vel_x != P1.vel_x || P1vel_y != P1.vel_y)
		//	is_P1_vel_changed = true;
	}
	
	public void P1NoneTouching()
	{
		if(P1.pMotion == MotionType.WALK)
		{
			P1.vel_x = 0;
			P1.vel_y = 0;
			is_P1_vel_changed = true;
		}
	}
	
	
	public void GetP2Info(Pika.Game.Control msg)
	{
		rcvMsg = msg;
		if(msg.time >= receiveTime)
		{
			receiveTime = msg.time;
		}
			
		else
		{
			return;
		}
			
		
		SetP2Moving();
	}
	
	public void SetP2Moving()
	{
		//if(is_recved_msg)
		{
			MotionType mt = P2.pMotion;
			SetP2Pos(new Vector2(rcvMsg.Character.loc_x, rcvMsg.Character.loc_y));
			if(rcvMsg.motion == Pika.Game.Control.MotionType.WALK)
				P2Walking(rcvMsg.Character.vel_x);
			else if(rcvMsg.motion == Pika.Game.Control.MotionType.JUMP)
				P2Jumping(rcvMsg.Character.vel_x , rcvMsg.Character.vel_y);
			else if(rcvMsg.motion == Pika.Game.Control.MotionType.SLIDE)
				P2Sliding(rcvMsg.Character.vel_x , rcvMsg.Character.vel_y);
			else if(rcvMsg.motion == Pika.Game.Control.MotionType.SPIKE)
				P2Spiking(rcvMsg.Character.vel_x , rcvMsg.Character.vel_y);
			//if(GetPlayerMotionType(rcvMsg.motion) != mt)
			//	P2.pMotion = GetPlayerMotionType(rcvMsg.motion);
			//is_recved_msg = false;
		}
	}
	
	public void SetP2Pos(Vector2 pos)
	{	
		P2.transform.localPosition = new Vector3(pos.x, pos.y, 0);
	}
	
	void P2Walking(float vel_x)
	{
		P2.vel_x = vel_x;
		//if(
		P2.pMotion = MotionType.WALK;
	}
	
	void P2Jumping(float vel_x, float vel_y)
	{
		P2.vel_x = vel_x;
		P2.vel_y = vel_y;
		P2.pMotion = MotionType.JUMP;
	}
	
	void P2Spiking(float vel_x, float vel_y)
	{
		P2.vel_x = vel_x;
		P2.vel_y = vel_y;
		//if(P2.transform.localPosition.y < -70f) // if Player1's y-axis position is smaller than -70f, then just return
		//	return;
		
		P2.pMotion = MotionType.SPIKE;
	}
	
	void P2Sliding(float vel_x, float vel_y)
	{
		P2.vel_x = vel_x;
		P2.vel_y = vel_y;
		P2.pMotion = MotionType.SLIDE;
	}
	
	void IgnoreBallPhysics()
	{
		switch(TouchManager.controller)
		{
			case TouchControllerType.BUTTON :
				Physics.IgnoreCollision(GameObject.Find("leftButton").collider, ball.collider);
				Physics.IgnoreCollision(GameObject.Find("rightButton").collider, ball.collider);
				break;
			case TouchControllerType.JOYPAD :
				Physics.IgnoreCollision(GameObject.Find("Joystick").collider, ball.collider);
				Physics.IgnoreCollision(GameObject.Find("Joystick_background").collider, ball.collider);
				Physics.IgnoreCollision(GameObject.Find("Joystick_spikeButton").collider, ball.collider);
				break;
			default :
				break;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		sn = GameObject.Find("SocketNetwork").GetComponent<SocketNetworking>();
		ball = GameObject.Find("Ball").GetComponent<Ball>();
		IgnoreBallPhysics();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		UpdateP1Walking();
		frame_count = frame_count + 1;
		gameTime += Time.deltaTime;
		if((P1vel_x != P1.vel_x || P1vel_y < P1.vel_y))// || P1loc_x != P1.transform.localPosition.x))
		{
			//Debug.Log(P1vel_x + " " + P1.vel_x + "\n" + P1vel_y + " " + P1.vel_y);
			is_P1_vel_changed = true;
			P1vel_x = P1.vel_x;
			P1vel_y = P1.vel_y;
			P1loc_x = P1.transform.localPosition.x;
			P1loc_y = P1.transform.localPosition.y;
			//frame_count = 0;
			//Debug.Log("After! " + P1vel_x + " " + P1.vel_x + "\n" + P1vel_y + " " + P1.vel_y);
			//Debug.Log("s");
		}
		else if(frame_count > 20 && Vector2.Distance(new Vector2(P1loc_x, P1loc_y), new Vector2(P1.transform.localPosition.x,P1.transform.localPosition.y)) > 5)//else if((P1loc_x != P1.transform.localPosition.x || P1loc_y != P1.transform.localPosition.y))
		{
			is_P1_vel_changed = true;
			P1vel_x = P1.vel_x;
			P1vel_y = P1.vel_y;
			P1loc_x = P1.transform.localPosition.x;
			P1loc_y = P1.transform.localPosition.y;
			//Debug.Log("sss");
			frame_count = 0;
		}
		else 
		{
			is_P1_vel_changed = false;
		}
		MakeP1Info();
	}
}
