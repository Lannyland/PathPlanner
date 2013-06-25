using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class ReadyToPlanPath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        // Set UAV movable to false
        GameObject UAV = GameObject.Find("UAV");
        UAV.GetComponent<MoveUFO>().movable = false;

        // Because UAV should have been moved to the end point location of previous path segment
        ProjectConstants.curStart = new Vector2(UAV.transform.position.x, UAV.transform.position.z);

        // Set current end point movable to false
        GameObject curEndPoint = GameObject.Find("EndPoint" + ProjectConstants.endPointCounter);
        if (curEndPoint.GetComponent<MoveUFO>().movable)
        {
            // End point is used for path planning
            ProjectConstants.curEnd = new Vector2(curEndPoint.transform.position.x, curEndPoint.transform.position.z);
            ProjectConstants.boolUseEndPoint = true;
        }
        curEndPoint.GetComponent<MoveUFO>().movable = false;
        
        // Start path planning
        Camera.main.GetComponent<PlanPath>().PlanMultiplePaths();        
    }
}
