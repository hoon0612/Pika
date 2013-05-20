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
	IPEndPoint ep;
	Socket sock;
	
	public void SendUDPMessage(byte [] _byte)
	{
		int bytesSent = sock.Send(_byte);
		Console.WriteLine("Sent {0} bytes.", bytesSent);
	}
	
	// Use this for initialization
	void Start () 
	{
		ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5567);
		sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		sock.SendTimeout = 5000;
		sock.ReceiveTimeout = 5000;
		
		if(sock != null)
		{
			try
			{
				sock.Connect(ep);
				if(sock.Connected)
				{
					Debug.Log("connected!");
				}
			}
			catch (SocketException ex)
			{
				System.Console.WriteLine("{0} Error code: {1}.", ex.Message, ex.ErrorCode);

			}
			catch (Exception ex)
			{
				System.Console.WriteLine(ex.Message);
			}
			finally
			{
				sock.Shutdown(SocketShutdown.Both);
				sock.Close();
			}
		}
		ball = GameObject.Find("Ball").GetComponent<Ball>();
		//P1 = GameObject.Find("Player1").GetComponent<Player1>();
		//P2 = GameObject.Find("Player2").GetComponent<Player2>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
