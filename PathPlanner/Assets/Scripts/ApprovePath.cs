using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class ApprovePath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		// First set flag
		ProjectConstants.lastPathApproved = true;	
		
		// Next move UAV to end point of last path segment
		
		// Also add a new end point and set it's movable to false
		
		// Set ready to plan path mode to false, so moving slider won't change anything.
		ProjectConstants.readyToPlanPath = false;
		
		// Set workerThread factory to ready
		ProjectConstants.stopPathPlanFactory = true;
		
		// Next add approved path segment to total path
		
		// 
	}
	
}
