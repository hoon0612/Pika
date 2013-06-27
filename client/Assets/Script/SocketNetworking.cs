using UnityEngine;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ProtoBuf;

public class SocketNetworking : MonoBehaviour 
{
	IPEndPoint ipep;
	EndPoint ep;
	Socket sock;
	private Pika.Game.Control _msg;
	public byte [] sendBuf = new byte[1];
	Queue q;
	GameManager gameManager;
	int network_bytes = 0;
	public Pika.Game.Control msg
	{
		get
		{
			return _msg;
		}
		set
		{
			_msg = value;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		ipep = new IPEndPoint(IPAddress.Parse("192.168.0.10"), 5567);
		ep = (EndPoint)ipep;
		sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		sock.Blocking = false;
		q = new Queue();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		//sendThread();
		//recvThread();
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*int sendBufSize = q.Count;
		if(sendBufSize > 0) // send some message to server
		{
			for(int i = 0 ; i < q.Count ; i++)
			{
				sock.SendTo(q.Dequeue() as byte[], ipep);
			}
		}
		*/
		//Debug.Log(q.Count);
		while(q.Count > 0)
		{
			sock.SendTo(q.Dequeue() as byte[], ipep);
		}
		byte [] tmp_buf = new byte[64];
		byte [] buf;
		bool isRead = sock.Poll(0,SelectMode.SelectRead);
		if(isRead)
		{
			int recvBufSize = sock.ReceiveFrom(tmp_buf, ref ep);
			buf = new byte[recvBufSize];
			network_bytes += buf.Length;
			//Debug.Log("recv Bytes : " + recvBufSize);
			for(int i = 0 ; i < recvBufSize ; i++)
			{
				buf[i] = tmp_buf[i];
			}
			Stream stream = new MemoryStream(buf);
		 	Pika.Game.Control rcvmsg = Serializer.Deserialize<Pika.Game.Control>(stream);
			gameManager.GetP2Info(rcvmsg);
		 	//Debug.Log("id " + rcvmsg.id + " locx " + rcvmsg.Character.loc_x);
		}
		//Debug.Log(network_bytes + " Bytes are sended or received!");
	}
	
	public void recvThread()
	{
		Thread recvThread = new Thread(new ThreadStart(recvMsg));
		recvThread.Start();
	}
	
	public void recvMsg()
	{
		while(true)
		{
			byte [] tmp_buf = new byte[64];
			byte [] buf;
			bool isRead = sock.Poll(16666,SelectMode.SelectRead);
			if(isRead)
			{
				int recvBufSize = sock.ReceiveFrom(tmp_buf, ref ep);
				buf = new byte[recvBufSize];
				//Debug.Log("recv Bytes : " + recvBufSize);
				for(int i = 0 ; i < recvBufSize ; i++)
				{
					buf[i] = tmp_buf[i];
				}
				Stream stream = new MemoryStream(buf);
			 	Pika.Game.Control rcvmsg = Serializer.Deserialize<Pika.Game.Control>(stream);
				gameManager.GetP2Info(rcvmsg);
		 	//Debug.Log("id " + rcvmsg.id + " locx " + rcvmsg.Character.loc_x);
			
			}
		}
	}
	
	public void sendThread()
	{
		Thread sendThread = new Thread(new ThreadStart(sendMsg));
		sendThread.Start();
	}
	
	public void sendMsg()
	{
		while(true)
		{
			int sendBufSize = q.Count;
			//Debug.Log(q.Count);
			if(sendBufSize > 0) // send some message to server
			{
				for(int i = 0 ; i < q.Count ; i++)
				{
					sock.SendTo(q.Dequeue() as byte[], ipep);
				}
				//sock.SendTo(sendBuf, ipep);
				//sendBuf = null;
			}
		}
		
	}
	
	public void revcMsgFromGameManager(Pika.Game.Control sendmsg)
	{
		byte [] buf;
		using(var ms = new MemoryStream())
		{
			Serializer.Serialize<Pika.Game.Control>(ms, sendmsg);
			buf = ms.ToArray();
			sendmsg = null;
		}
		q.Enqueue(buf);
		network_bytes += buf.Length;
		//sock.SendTo(buf, ep);
		
	}
}
