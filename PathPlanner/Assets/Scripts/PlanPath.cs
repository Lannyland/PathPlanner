using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;
using TCPIPTest;
using Vectrosity;
using System.Threading;

public class PlanPath : MonoBehaviour {

    public List<Vector2[]> lstPaths = new List<Vector2[]>();
	public List<VectorLine> lstLines = new List<VectorLine>();
	public List<Vector3[]> lstVertices = new List<Vector3[]>();
	public List<Color[]> lstColors = new List<Color[]>();
	public VectorLine curLine;	
	
	private int resolution;
	private int duration;
	
	// Use this for initialization
	void Start () {
		ClearLists();
	}

    // Update is called once per frame
    void Update()
    {
	
	}

    // The most important part of the tool: actually plan paths
    public void PlanMultiplePaths()
    {
        // Set things up first
        resolution = Convert.ToInt16(GameObject.Find("lblRValue").GetComponent<UILabel>().text);
        duration = Convert.ToInt16(GameObject.Find("lblDValue").GetComponent<UILabel>().text);
		
		Debug.Log("resolution = " + resolution);
		Debug.Log("duration = " + duration);

        // First plan the selected duration path if it has not been planned before
        // (each time step is 2 seconds, so divide by 60 and times 2)
        if (lstPaths[duration - 1] != null) {}
        else
        {
            Debug.Log("Doing current path planning");
			Debug.Log("ProjectConstants.boolUseEndPoint = " + ProjectConstants.boolUseEndPoint);
            NetworkCall call = new NetworkCall(
                ProjectConstants.mDistMapCurStepWorking,
                ProjectConstants.mDiffMap,
                ProjectConstants.curStart,
                ProjectConstants.curEnd,
                ProjectConstants.boolUseDiffMap,
                ProjectConstants.boolUseEndPoint,
                ProjectConstants.boolPlenty,
                duration * 30);
			
			// Store everything in List
            lstPaths[duration - 1] = call.path;
        	Debug.Log("Path returned length = " + call.path.Length);
		}
		
		// Next re-draw path each time
		if(curLine!=null)
		{
			curLine.ZeroPoints();
			curLine.Draw3D();
		}
		curLine = DrawPath(lstPaths[duration-1]);			
		curLine.Draw3DAuto();
				
		// Next show vacuumed dist map. If vertices and colors were computed before, simply use that one.
		if(lstVertices[duration-1]!=null){}
		else
		{
			Vector3[] vertices = ComputeVertices(lstPaths[duration-1]);
			lstVertices[duration-1] = vertices;
			Color[] colors = MISCLib.ApplyDistColorMap(vertices);
			lstColors[duration-1] = colors;
		}
		Mesh mesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
		mesh.vertices = lstVertices[duration-1];
		mesh.colors = lstColors[duration-1];
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

        // While the user is not doing anything, just keep planning in a different thread
		ThreadStart threadDelegate = new ThreadStart(this.PathPlannerFactory);
		Thread workerThread = new Thread(threadDelegate);
		workerThread.Start();
    }
	
	void ClearLists ()
	{
		// Fill lists with empty things
	    for (int i = 0; i < ProjectConstants.durationLeft; i++)
	    {
	        lstPaths.Add(null);
			lstLines.Add(null);
			lstVertices.Add(null);
			lstColors.Add (null);
		}
	}
	
	VectorLine DrawPath(Vector2[] path)
	{
		DrawPathHandler newDraw = new DrawPathHandler(path);
		return newDraw.GetLine();		
	}
	
	Vector3[] ComputeVertices(Vector2[] path)
	{
        Mesh distMesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
		Mesh diffMesh = GameObject.Find("PlaneDiff").GetComponent<MeshFilter>().mesh;
		VacuumHandler vh = new VacuumHandler(path, distMesh.vertices, diffMesh.vertices);
		return vh.GetVertices();
	}
	
	// Work to be done by a separate thread. And it will stop when a flag is set to false;
	void PathPlannerFactory ()	
	{
		if(!ProjectConstants.stopPathPlanFactory)
		{
			for (int i = resolution; i < ProjectConstants.durationLeft + 1; i += resolution)
	    	{
	    	    Debug.Log("Doing additional path planning");
	    	    // If a path already exists, then no need to plan again
	    	    if (lstPaths[i - 1] != null)
	    	    { 
	    	    }
	    	    else
	    	    {
	    	        NetworkCall call = new NetworkCall(
	    	            ProjectConstants.mDistMapCurStepWorking,
	    	            ProjectConstants.mDiffMap,
	    	            ProjectConstants.curStart,
	    	            ProjectConstants.curEnd,
	    	            ProjectConstants.boolUseDiffMap,
	    	            ProjectConstants.boolUseEndPoint,
	    	            ProjectConstants.boolPlenty,
	    	            i * 30);
	    	        lstPaths[i - 1] = call.path;
	    	        Debug.Log("Path returned length = " + call.path.Length);
	    	    }
				if(ProjectConstants.stopPathPlanFactory)
				{
					return;
				}
	    	}
		}
	}
	
}
