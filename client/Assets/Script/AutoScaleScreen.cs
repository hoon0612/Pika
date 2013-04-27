using UnityEngine;
using System.Collections;

public class AutoScaleScreen : MonoBehaviour {

	// Use this for initialization
	// iPhone 4 Screen size ( Ratio )
	
	public int baseWidth  = 640;
	public int baseHeight = 960; 
	public bool  turnOn = true;
	
	void Awake () {
		
		if(!turnOn) return;
		
		UIRoot rootScript = GetComponent<UIRoot>();
		
		if(rootScript == null) {
			Debug.Log ("To use this component, attach it to the \"UI Root (2D)\" object with UIRoot script");
		}
		else {
			//rootScript.automatic = false;
			// http://www.tasharen.com/ngui/docs/class_u_i_root.html#a16ff3ccbf1a095adf12162bd44c9203d
			// automatic property is obsolete
			
			int newHeight;
			
			if( (((float)baseHeight) / baseWidth) < ((float) Screen.height / Screen.width) ) {
				newHeight = (int) (((float) Screen.height / Screen.width) * baseWidth);
				Debug.Log("aa");
			}
			else {
				newHeight = (int) baseHeight;
				Debug.Log("bb");
			}
			
			rootScript.manualHeight  = newHeight;
			rootScript.minimumHeight = newHeight;
			rootScript.maximumHeight = newHeight;
			
			Debug.Log ("The Screen  size : " + Screen.width + " x " + Screen.height);
			Debug.Log ("Setting new UI height to " + newHeight);
		}
	}
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
