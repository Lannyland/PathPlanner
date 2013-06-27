using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Vectrosity;

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
		// Modify VacuumHandler class to include probability volume and first point vacuum.
		// Convert current vertices to mDistMapCurStepUndo so we can do multpile path planning on this.
		// ProjectConstants.mDistMapCurStepUndo = 
		
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
		if(ProjectConstants.durationLeft >= 10)
		{
			sliderR.sliderValue = 1;
            sliderR.GetComponent<SliderControl>().OnSliderChange(sliderR.sliderValue);
		}
		else
		{
			sliderR.sliderValue = ProjectConstants.durationLeft * (1f / (sliderR.numberOfSteps - 1));
		}	
		
		// Clear all lists of things for next path segment planning		
		Camera.main.GetComponent<PlanPath>().workerThread.Abort();
		Camera.main.GetComponent<PlanPath>().ClearLists();
		
		if(ProjectConstants.durationLeft == 0)
		{
			// We are done planning.
			// Disable things
			UIButton b1 = GameObject.Find("btnSetEndPoint").GetComponent<UIButton>();
			b1.GetComponent<SetEndPoint>().enabled = false;
			UIButton b2 = GameObject.Find("btnPlanPath").GetComponent<UIButton>();
			b2.GetComponent<ReadyToPlanPath>().enabled = false;
			UIButton b3 = GameObject.Find("btnApprove").GetComponent<UIButton>();
			b3.GetComponent<ApprovePath>().enabled = false;
			
			// Enable things
			UIButton b4 = GameObject.Find("btnFly").GetComponent<UIButton>();
			b4.GetComponent<FlyPath>().enabled = true;
		}
	}	
}
