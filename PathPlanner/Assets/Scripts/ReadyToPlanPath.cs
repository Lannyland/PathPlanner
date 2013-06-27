using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;

public class ReadyToPlanPath : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Debug.Log("ProjectConstants.endPointCounter = " + ProjectConstants.endPointCounter);	
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
        ProjectConstants.curStart = new Vector2(UAV.transform.position.x, UAV.transform.position.z);
		
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
		// Move slider to make sure duration selected is long enough to reach end point
		if(ProjectConstants.boolUseEndPoint &&  ProjectConstants.endPointCounter > 0)
		{
			// Find last endpoint and UAV
			GameObject curEndPoint = GameObject.Find("EndPoint" + ProjectConstants.endPointCounter);
			UILabel curSliderDValue = GameObject.Find("lblDValue").GetComponent<UILabel>();
			int duration = Convert.ToInt16(curSliderDValue.text);			
			UILabel curSliderRValue = GameObject.Find("lblRValue").GetComponent<UILabel>();
			int resolution = Convert.ToInt16(curSliderRValue.text);			
			while(MISCLib.ManhattanDistance(curEndPoint.transform.position, UAV.transform.position)*10 > duration*30)
			{
				duration+=resolution;
			}
			UISlider sliderD = 	GameObject.Find("SliderD").GetComponent<UISlider>();
			sliderD.sliderValue = Mathf.Clamp01(duration/resolution*(1f/(sliderD.numberOfSteps-1)));
		}		


		// Set workerThread factory to ready
		ProjectConstants.stopPathPlanFactory = false;

        // Store a copy of the current vertices
        Mesh distMesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
        Vector3[] copy = new Vector3[distMesh.vertices.Length];
        Array.Copy(distMesh.vertices, copy, copy.Length);
        ProjectConstants.curVertices = copy;
		
        // Start path planning
		if(ProjectConstants.boolUseEndPoint)
		{
			Camera.main.GetComponent<PlanPath>().curEndPointPos = GameObject.Find("EndPoint" + ProjectConstants.endPointCounter).transform.position;
		}
        Camera.main.GetComponent<PlanPath>().PlanMultiplePaths();        
    }
}
