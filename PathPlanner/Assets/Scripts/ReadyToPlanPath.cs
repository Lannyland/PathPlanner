using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class ReadyToPlanPath : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("ProjectConstants.endPointCounter = " + ProjectConstants.endPointCounter);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        // Set UAV movable to false
        GameObject UAV = GameObject.Find("UAV");
        UAV.GetComponent<MoveUFO>().movable = false;

        // Remember start position
        ProjectConstants.curStart = UAV.transform.position;

        // Set current end point movable to false and if there is end point, remember position
        if (ProjectConstants.endPointCounter > 0)
        {
            GameObject curEndPoint = GameObject.Find("EndPoint" + ProjectConstants.endPointCounter);
            if (curEndPoint.GetComponent<MoveUFO>().movable)
            {
                // End point is used for path planning
                ProjectConstants.curEnd = new Vector2(curEndPoint.transform.position.x, curEndPoint.transform.position.z);
                ProjectConstants.boolUseEndPoint = true;
            }
            curEndPoint.GetComponent<MoveUFO>().movable = false;
        }
        

        // Set plan path mode to true, so now moving slider will plan paths
		ProjectConstants.readyToPlanPath = true;
		

        // Because UAV should have been moved to the end point location of previous path segment
        ProjectConstants.curStart = new Vector2(UAV.transform.position.x, UAV.transform.position.z);

		
		// Set workerThread factory to ready
		ProjectConstants.stopPathPlanFactory = false;

        // Stop a copy of the current vertices
        Mesh distMesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
        Vector3[] copy = new Vector3[distMesh.vertices.Length];
        Array.Copy(distMesh.vertices, copy, copy.Length);
        ProjectConstants.curVertices = copy;
		
        // Start path planning
        Camera.main.GetComponent<PlanPath>().PlanMultiplePaths();        
    }
}
