using UnityEngine;
using System.Collections;
 
public class NotPenetrateThings : MonoBehaviour 
{
	public LayerMask layerMask; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed 
 
	private float minimumExtent; 
	private float partialExtent; 
	private float sqrMinimumExtent; 
	private Vector3 previousPosition;
	private GameObject this_obj; 
	
	//initialize values 
	void Awake() 
	{  
	   this_obj = this.gameObject;
	   previousPosition = this_obj.transform.localPosition;
	   //minimumExtent = Mathf.Min(Mathf.Min(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z); 
		minimumExtent = 20;
	   partialExtent = minimumExtent * (1.0f - skinWidth); 
	   sqrMinimumExtent = minimumExtent * minimumExtent; 
	} 
 
	void FixedUpdate() 
	{ 
	   //have we moved more than our minimum extent? 
	   Vector3 movementThisStep = this_obj.transform.localPosition - previousPosition; 
	   float movementSqrMagnitude = movementThisStep.sqrMagnitude;
 
	   if (movementSqrMagnitude > sqrMinimumExtent) 
		{ 
	      float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
	      RaycastHit hitInfo; 
 		  
	      //check for obstructions we might have missed 
	      if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value)) 
			{
				this_obj.transform.localPosition = hitInfo.point - (movementThisStep/movementMagnitude)*partialExtent; 
				Debug.Log(layerMask.value);
			}
			if(collider.Raycast(new Ray(previousPosition, movementThisStep), out hitInfo, movementSqrMagnitude)){
				this_obj.transform.localPosition = hitInfo.point - (movementThisStep/movementMagnitude)*partialExtent; 
				Debug.Log(layerMask.value);
				Debug.Log("ax");
			}
			
	   } 
 
	   previousPosition = this_obj.transform.localPosition;
	}
	
	void OnTriggerEnter(Collider col){
		
		Vector3 movementThisStep = this_obj.transform.localPosition - previousPosition; 
	   float movementSqrMagnitude = movementThisStep.sqrMagnitude;
 
	   if (movementSqrMagnitude > sqrMinimumExtent) 
		{ 
	      float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
	      //RaycastHit hitInfo; 
 		  //this_obj.transform.localPosition = hitInfo.point - (movementThisStep/movementMagnitude)*partialExtent; 	
	   }
	}
}