using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class AutoParticleDestructor : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		if(!particleSystem.IsAlive())
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}
