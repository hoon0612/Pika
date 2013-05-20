using UnityEngine;
using System.Collections;
using System.IO;
using ProtoBuf;
using Pika.Game;
using System.Net;
using System.Net.Sockets;

public class Player1 : Player 
{
	IPEndPoint ep;
	Socket sock;
	bool motion_change = false;
	tk2dAnimatedSprite playerSprite;
	public IEnumerator WakeUp()
	{
		yield return new WaitForSeconds(0.2f);
		if(is_right_user)
		{
			if(player.transform.FindChild("playerSprite").localRotation.y == 0)
				player.transform.FindChild("playerSprite").localRotation = Quaternion.Euler(new Vector3(0,180,0));	
		}
		else
		{
			if(player.transform.FindChild("playerSprite").localRotation.y != 0)
				player.transform.FindChild("playerSprite").localRotation = Quaternion.Euler(new Vector3(0,0,0));
		}
		
		playerSprite.Play("Idle");
		vel_y = 0;
		vel_x = 0;
		motion_change = false;
		can_swipe = true;
	}
	
	void Awake()
	{
		is_right_user = true;
	}
	
	// Use this for initialization
	void Start () {
		player = this.gameObject;
		col = player.GetComponent<CapsuleCollider>();
		playerSprite = player.transform.FindChild("playerSprite").GetComponent<tk2dAnimatedSprite>();
		ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5567);
		sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(walking&&!leftSliding&&!rightSliding)
		{
			player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);	
			Pika.Game.Control msg = new Pika.Game.Control {
				id = "aad",
				Character = new Pika.Game.Control.Status {
					loc_x = player.transform.localPosition.x,
					loc_y = player.transform.localPosition.y,
					vel_x = player.rigidbody.velocity.x,
					vel_y = player.rigidbody.velocity.y
				}
			};
			
			
			Debug.Log(msg.ToString().Length);
			byte [] buf = new byte [32];
			Stream stream = new MemoryStream(buf);
			Serializer.Serialize<Pika.Game.Control>(stream,msg);
			int bufsize = 0;
			for(int i = 0 ; i < buf.Length ; i++)
			{
				if(buf[i] != null)
					bufsize ++;
			}
			Debug.Log(bufsize);
			byte [] sendBuf = new byte[bufsize];
			sock.SendTo(buf, ep);
			
			
			//GameManager g = GameObject.Find("GameManager").GetComponent<GameManager>();
			//g.SendUDPMessage(buf);
		}
		if(jumping&&!leftSliding&&!rightSliding)
		{
			if(!motion_change)
			{
				motion_change = true;
				playerSprite.Play("Jump");
			}
			vel_y -= jump_reducing_speed;
			player.rigidbody.velocity -= new Vector3(0, jump_reducing_speed, 0);
			
			if(player.transform.localPosition.y < -75f)
			{
				vel_x = 0; // added
				vel_y = 0;
				player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -75f, player.transform.localPosition.z);
				motion_change = false;
				jumping = false;
				playerSprite.Play("Idle");
			}
			if(upperSpike || middleSpike || lowerSpike)
			{
				StartCoroutine(SetSpikeFalse());
			}
		}
		else if(leftSliding&&!jumping&&!rightSliding)
		{
			if(!motion_change)
			{
				motion_change = true;
				playerSprite.Play("Slide");
				if(!is_right_user)
					player.transform.FindChild("playerSprite").localRotation = Quaternion.Euler(new Vector3(0,180,0));
			}
			vel_y -= sliding_reducing_y_speed;
			player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
			if(player.transform.localPosition.y < -75)
			{
				vel_y = 0;
				vel_x = 0;
				player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -75, player.transform.localPosition.z);
				motion_change = false;
				leftSliding = false;
				StartCoroutine(WakeUp());
			}
			
		}
		else if(rightSliding&&!jumping&&!leftSliding)
		{
			if(!motion_change)
			{
				motion_change = true;
				playerSprite.Play("Slide");
				if(is_right_user)
					player.transform.FindChild("playerSprite").localRotation = Quaternion.Euler(new Vector3(0,0,0));
			}
			vel_y -= sliding_reducing_y_speed;
			player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
			if(player.transform.localPosition.y < -75)
			{
				vel_x = 0;
				vel_y = 0;
				player.rigidbody.velocity = new Vector3(vel_x, vel_y, 0);
				player.transform.localPosition = new Vector3(player.transform.localPosition.x, -75, player.transform.localPosition.z);
				motion_change = false;
				rightSliding = false;
				StartCoroutine(WakeUp());
			}
		}
		
		CorrectPlayerPos();
	}
}
