using UnityEngine;
using System.Collections;

public class TouchMotionEffect : MonoBehaviour {
	Camera cam;
	GameObject anc;
	// Use this for initialization
	void Start () {
		cam = GameObject.Find("VollyBallCamera").GetComponent<Camera>();
		anc = cam.transform.FindChild("Anchor").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public GameObject spawnParticle(Vector3 spawn_loc)
	{
		GameObject particles = Instantiate(Resources.Load("Prefab/ButtonTouchEffect", typeof(GameObject))) as GameObject;
		Vector3 pos = Camera.main.ScreenToWorldPoint( spawn_loc );
		particles.transform.localPosition = new Vector3(pos.x, pos.y, -8);
		particles.transform.parent = anc.transform;
		Debug.Log(particles.transform.localPosition);
		return particles;
	}
}
