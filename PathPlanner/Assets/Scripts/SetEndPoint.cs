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
			// Make sure no path is planned for current stage yet
			if(ProjectConstants.readyToPlanPath)
			{
				return;
			}			
			
            // Create new sphere game object as end point
			GameObject newEndPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			ProjectConstants.endPointCounter++;
			newEndPoint.name = "EndPoint" + ProjectConstants.endPointCounter;
			newEndPoint.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
			// newEndPoint.transform.position = new Vector3(1,4,1);

            // Where to put the new end point?
            Vector3 offset = new Vector3(0.1f, 0, 0f);
            Vector3 objectPosition = new Vector3();
            if (ProjectConstants.endPointCounter > 1)
            {
                // Put new end point next to last end point
                GameObject oldEndPoint = GameObject.Find("EndPoint" + (ProjectConstants.endPointCounter - 1));
                objectPosition = oldEndPoint.transform.position;
                // Debug.Log("Put new point next to previous end point.");
            }
            else
            {
                // put it next to UAV
                GameObject UAV = GameObject.Find("UAV");
                objectPosition = UAV.transform.position;
                // Debug.Log("Put new point next to UAV.");
            }
            // Debug.Log( (ProjectConstants.intMapWith - 1)/10.0f - 5.0f);
            if (Mathf.Abs((ProjectConstants.intMapWidth - 1)/10.0f - 5.0f - objectPosition.x) < 0.0001)
            {
                offset.x = -0.1f;
            }
            newEndPoint.transform.position = objectPosition + offset;

            // Attach EndPointView projector to new end point
            GameObject.Find("EndPointView").GetComponent<Transform>().transform.position = newEndPoint.transform.position;
            GameObject.Find("EndPointView").GetComponent<Transform>().transform.parent = newEndPoint.transform;

            // Attach MoveUFO script to this end point
            newEndPoint.AddComponent("MoveUFO");
            newEndPoint.GetComponent<MoveUFO>().UAV = newEndPoint.transform;

			
			// Set lastPathApproved flag back to false;
			Assets.Scripts.ProjectConstants.lastPathApproved = false;
			
		}
		
	}
}
