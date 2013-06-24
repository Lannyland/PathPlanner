using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class SetEndPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
				
		// Check if UAV can reach this spot within duration left.
		// if(MISCLib.ManhattanDistance(		

	}
	
	void OnClick()
	{
		// Make sure last segment has already been approved
		if(Assets.Scripts.ProjectConstants.lastPathApproved)
		{
			// Create new sphere game object as end point
			GameObject newEndPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			ProjectConstants.endPointCounter++;
			newEndPoint.name = "EndPoint" + ProjectConstants.endPointCounter;
			newEndPoint.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
			newEndPoint.transform.position = new Vector3(1,4,1);

			// Put new end point next to last end point
			
			
			// Attach EndPointView projector to new end point
			GameObject.Find("EndPointView").GetComponent<Transform>().transform.parent = newEndPoint.transform;
			
			// Set lastPathApproved flag back to false;
			Assets.Scripts.ProjectConstants.lastPathApproved = false;
			
		}
		
	}
}
