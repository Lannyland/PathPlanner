using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;
using rtwmatrix;
using Vectrosity;

public class StartOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// When button is clicked
	void OnClick()
	{
		// For user study
		ProjectConstants.boolFlyPath = false;
		
		// If flying path, stop
		Camera.main.GetComponent<FlyPath>().currentWayPoint = 20000;
		Camera.main.GetComponent<FlyPath>().fly = false;
		
		// Tell workerThread to stop
		if(Camera.main.GetComponent<PlanPath>().workerThread != null)
		{
			Camera.main.GetComponent<PlanPath>().workerThread.Abort();
		}
		
		// Destroy all those endpoints
		GameObject.Find("EndPointView").GetComponent<Transform>().transform.parent = null;
		GameObject.Find("EndPointView").GetComponent<Transform>().transform.position = new Vector3(0,0,0);
		for (int i=1; i< ProjectConstants.endPointCounter+1; i++)
		{
			GameObject go = GameObject.Find("EndPoint" + i);
			Destroy (go);
		}
		
		// Erase line
		VectorLine curLine = Camera.main.GetComponent<PlanPath>().curLine;
		curLine.ZeroPoints();
		curLine.Draw3D();
		
		// Move UAV back to center and make it movable
		GameObject.Find("UAV").transform.position = new Vector3(0f, 4f, 0f);
		GameObject.Find("UAV").GetComponent<MoveUFO>().movable = true;
				
		// Reload distMap to mesh
		RtwMatrix distMapIn = ProjectConstants.mOriginalDistMap.Clone();
		Mesh mesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
        MISCLib.ApplyMatrixToMesh(distMapIn, ref mesh, true);
		
		// Set everything back to default values
		ProjectConstants.mDistMapCurStepUndo = ProjectConstants.mOriginalDistMap.Clone();
		Mesh distMesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
        Vector3[] copy = new Vector3[distMesh.vertices.Length];
        Array.Copy(distMesh.vertices, copy, copy.Length);
        ProjectConstants.curVertices = copy;
		ProjectConstants.resolution = 10;
		ProjectConstants.durationLeft = ProjectConstants.intFlightDuration;
		ProjectConstants.duration = 10;
		ProjectConstants.readyToPlanPath = false;
		ProjectConstants.boolUseEndPoint = false;
		ProjectConstants.lastPathApproved = true;
		ProjectConstants.endPointCounter = 0;
		ProjectConstants.AllPathSegments.Clear();
		Camera.main.GetComponent<PlanPath>().ClearLists();
		
		// Set slider value so the sliders can take care of themselves
		UISlider sliderR = GameObject.Find("SliderR").GetComponent<UISlider>();
		sliderR.sliderValue = 1f;
		sliderR.GetComponent<SliderControl>().OnSliderChange(1f);
		
		// Enable those buttons
		UIButton b1 = GameObject.Find("btnSetEndPoint").GetComponent<UIButton>();
		b1.isEnabled = true;
		UIButton b2 = GameObject.Find("btnPlanPath").GetComponent<UIButton>();
		b2.isEnabled = true;
		UIButton b3 = GameObject.Find("btnApprove").GetComponent<UIButton>();
		b3.isEnabled = true;	

        // reset score
        Camera.main.GetComponent<PlanPath>().totalCDF = 0f;
        Camera.main.GetComponent<IncreasingScoreEffect>().curScore = 0f;
        Camera.main.GetComponent<IncreasingScoreEffect>().initialScore = 0f;
        GameObject.Find("lblScore").GetComponent<UILabel>().text = "0";
		
		// Disable buttons
		if(!Assets.Scripts.ProjectConstants.boolAnyEndPoint)
		{
			// No end point at all, so disable that button.			
			GameObject.Find("btnSetEndPoint").GetComponent<UIButton>().enabled = false;
		}		
		
		UIButton b4 = GameObject.Find("btnFly").GetComponent<UIButton>();
		b4.isEnabled = false;
	}
}
