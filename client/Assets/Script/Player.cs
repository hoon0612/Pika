using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	private GameObject _player;
	public GameObject player{
		get {
			return _player;
		}
		set{
			_player = value;
		}
	}
	public bool is_jumping = false;
	public bool shoot_pressed = false;
	public CapsuleCollider col;
	public float vel_x, vel_y;
	public UISprite player_sprite;
	public tk2dAnimatedSprite player_animation;
	
	
	// Use this for initialization
	void Start () 
	{
		/*player = this.gameObject;
		col = player.GetComponent<CapsuleCollider>();
		player_sprite = player.transform.FindChild("player").GetComponent<UISprite>();
		player_animation = player.transform.FindChild("pikachu").GetComponent<tk2dAnimatedSprite>();*/
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
	}
	
}
