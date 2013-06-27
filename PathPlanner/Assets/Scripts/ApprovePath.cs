using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;
using Vectrosity;
using rtwmatrix;

public class ApprovePath : MonoBehaviour {
	
	GameObject UAV;
	
	// Use this for initialization
	void Start () {
		 UAV = GameObject.Find("UAV");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		// For the new path segment if user hasn't clicked plan path button
		if(!ProjectConstants.readyToPlanPath)
		{
			return;
		}
		
		// First set flag
		ProjectConstants.lastPathApproved = true;
		
		// Tell workerThread factory to stop
		ProjectConstants.stopPathPlanFactory = true;
		
		// Find path and add it to total path
		UILabel curSliderDValue = GameObject.Find("lblDValue").GetComponent<UILabel>();
		int duration = Convert.ToInt16(curSliderDValue.text);
		ProjectConstants.AllPathSegments.Add(Camera.main.GetComponent<PlanPath>().lstPaths[duration-1]);
		// Add CDF for current segment to totalCDF
		Camera.main.GetComponent<PlanPath>().totalCDF += Camera.main.GetComponent<PlanPath>().lstCDF[duration-1];
		if(ProjectConstants.AllPathSegments.Count != 1)
		{
			Camera.main.GetComponent<PlanPath>().totalCDF -= Camera.main.GetComponent<PlanPath>().lstFirstVacuum[duration-1];
		}
		GameObject.Find("lblScore").GetComponent<IncreasingScoreEffect>().curScore = Camera.main.GetComponent<PlanPath>().totalCDF;
		
		// Next move UAV to end point of last path segment
		Vector2[] curPath = Camera.main.GetComponent<PlanPath>().lstPaths[duration-1];
		ProjectConstants.curStart = curPath[curPath.Length-1];
		ProjectConstants.curStart.x = ProjectConstants.curStart.x / 10f - 5f;
		ProjectConstants.curStart.y = ProjectConstants.curStart.y / 10f - 5f;
		UAV.transform.position = new Vector3(ProjectConstants.curStart.x,4f,ProjectConstants.curStart.y);
		
		// Also add a new end point and set it's movable to false
		if(!ProjectConstants.boolUseEndPoint)
		{
	        // Create new sphere game object as end point
			GameObject newEndPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			ProjectConstants.endPointCounter++;
			newEndPoint.name = "EndPoint" + ProjectConstants.endPointCounter;
			newEndPoint.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
			newEndPoint.transform.position = new Vector3(ProjectConstants.curStart.x,4f,ProjectConstants.curStart.y);
            // Attach script and set it to not movable
            newEndPoint.AddComponent("MoveUFO");
            newEndPoint.GetComponent<MoveUFO>().movable = false;
		}
		
		// Set ready to plan path mode to false, so moving slider won't change anything.
		ProjectConstants.readyToPlanPath = false;
		
		// Store a copy of the current vertices
        Mesh distMesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
        Vector3[] copy = new Vector3[distMesh.vertices.Length];
        Array.Copy(distMesh.vertices, copy, copy.Length);
        ProjectConstants.curVertices = copy;
		
		// Remember new curDistMapUndo
		// Convert vertices back to matrix map
		RtwMatrix curDistMap = MISCLib.ArrayToMatrix(copy);
		ProjectConstants.mDistMapCurStepUndo = curDistMap;
		ProjectConstants.mDistMapCurStepWorking = curDistMap.Clone();
		
		// Get rid of the line
		VectorLine line = Camera.main.GetComponent<PlanPath>().curLine;
		if(line!=null)
		{
			line.ZeroPoints();
			line.Draw3D();
		}
			
		// Set left duration
		ProjectConstants.durationLeft -= duration;
		/// UILabel dMax = GameObject.Find("lblDMax").GetComponent<UILabel>();
		// dMax.text = ProjectConstants.durationLeft.ToString();
		UISlider sliderR = GameObject.Find("SliderR").GetComponent<UISlider>();
		Debug.Log("Setting sliderR value to triggle OnSliderChange()");
		sliderR.sliderValue = 1f;	// Just to trigger OnSliderChange on SliderR, and then it will take care of itself.
		sliderR.GetComponent<SliderControl>().OnSliderChange(1f);
		
		// Clear all lists of things for next path segment planning		
		Camera.main.GetComponent<PlanPath>().workerThread.Abort();
		Camera.main.GetComponent<PlanPath>().ClearLists();
				
		if(ProjectConstants.durationLeft == 0)
		{
			// We are done planning.
			Debug.Log("We are done planning.");
			
			// Change SliderD value
			UILabel valueD = GameObject.Find("lblDValue").GetComponent<UILabel>();
			valueD.text = "0";
			
			// Disable things
			UIButton b1 = GameObject.Find("btnSetEndPoint").GetComponent<UIButton>();
			b1.isEnabled = false;
			UIButton b2 = GameObject.Find("btnPlanPath").GetComponent<UIButton>();
			b2.isEnabled = false;
			UIButton b3 = GameObject.Find("btnApprove").GetComponent<UIButton>();
			b3.isEnabled = false;
			
			// Enable things
			UIButton b4 = GameObject.Find("btnFly").GetComponent<UIButton>();
			b4.isEnabled = true;
		}
	}	
}
