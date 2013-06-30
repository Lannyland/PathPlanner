using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;
using TCPIPTest;
using Vectrosity;
using System.Threading;
using rtwmatrix;

public class PlanPath : MonoBehaviour
{

    #region Members

    public List<Vector2[]> lstPaths = new List<Vector2[]>();
	public List<Vector3[]> lstVertices = new List<Vector3[]>();
	public List<Color[]> lstColors = new List<Color[]>();
	public List<float> lstCDF = new List<float>();
	public List<float> lstFirstVacuum = new List<float>();
	public VectorLine curLine;	
	public Thread workerThread;
	public Vector3 curEndPointPos;
	public float totalCDF = 0f;
	
	private int resolution;
	private int duration;
    private Vector3 UAVPos;

    #endregion

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
		// Get rid or previous path line
		if(curLine!=null)
		{
			curLine.ZeroPoints();
			curLine.Draw3D();
		}
		VectorLine.Destroy(ref curLine);
		
        // First store UAV current position to object
        GameObject UAV = GameObject.Find("UAV");
        UAVPos = UAV.transform.position;

        // Set things up first
        resolution = Convert.ToInt16(GameObject.Find("lblRValue").GetComponent<UILabel>().text);
        duration = Convert.ToInt16(GameObject.Find("lblDValue").GetComponent<UILabel>().text);
		
		// Debug.Log("resolution = " + resolution);
		// Debug.Log("duration = " + duration);

        // First plan the selected duration path if it has not been planned before
        // (each time step is 2 seconds, so divide by 60 and times 2)
		try
		{{
			if (lstPaths[duration - 1] != null) {}
	        else
	        {
	            // Debug.Log("Doing current path planning");
				// Debug.Log("ProjectConstants.boolUseEndPoint = " + ProjectConstants.boolUseEndPoint);
	            NetworkCall call = new NetworkCall(
                    ProjectConstants.mDistMapCurStepUndo.Clone(),
	                ProjectConstants.mDiffMap,
	                ProjectConstants.curStart,
	                ProjectConstants.curEnd,
	                ProjectConstants.boolUseDiffMap,
	                ProjectConstants.boolUseEndPoint,
	                ProjectConstants.boolPlenty,
	                duration * 30);
				
				// Display any error message if there is any
				GameObject.Find("GUIText").GetComponent<UILabel>().text = call.message;
				// Store everything in List
	            lstPaths[duration - 1] = call.path;
	        	// Debug.Log("Path returned length = " + call.path.Length);
				}
			}
			
			// Next re-draw path each time
			curLine = DrawPath(lstPaths[duration-1]);			
			curLine.Draw3DAuto();

            List<float> CDFGraph = new List<float>();
					
			// Next show vacuumed dist map. If vertices and colors were computed before, simply use that one.
			if(lstVertices[duration-1]!=null){}
			else
			{
				// Store vertices and colors to list
				VacuumHandler vh = ComputeVertices(lstPaths[duration-1]);
				Vector3[] vertices = vh.GetVertices();
				lstVertices[duration-1] = vertices;
				Color[] colors = MISCLib.ApplyDistColorMap(vertices);
				lstColors[duration-1] = colors;
				lstCDF[duration-1] = vh.GetCDF();
				lstFirstVacuum[duration-1] = vh.GetFirstVacuum();

                // Debug info
                CDFGraph = vh.getCDFGraph();
				
				vh = null;
			}
			Mesh mesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
			mesh.vertices = lstVertices[duration-1];
			mesh.colors = lstColors[duration-1];
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

//            // Debug info
//            string output = "";
//            //for (int i = 0; i < CDFGraph.Count; i++)
//			for (int i = 0; i < 50; i++)
//            {
//                output += (CDFGraph[i]).ToString() + " ";
//            }
//          	Debug.Log(output);
		}
		catch(Exception e)
		{
			string message = "Path planning server error: " + e.Message + " Please wait and try again.";
			GameObject.Find("GUIText").GetComponent<UILabel>().text = message;
			return;
		}

        // While the user is not doing anything, just keep planning in a different thread
        ThreadStart threadDelegate = new ThreadStart(this.PathPlannerFactory);
        workerThread = new Thread(threadDelegate);
        workerThread.Start();
    }
	
    // Method to clear all lists and set each member to null
	public void ClearLists ()
	{
        // If lists had things in them, clear them first
        lstPaths.Clear();
        lstVertices.Clear();
        lstColors.Clear();
		lstCDF.Clear();
		lstFirstVacuum.Clear();

		// Fill lists with empty things
	    for (int i = 0; i < ProjectConstants.durationLeft; i++)
       {
	        lstPaths.Add(null);
			lstVertices.Add(null);
			lstColors.Add (null);
			lstCDF.Add (0);
			lstFirstVacuum.Add(0);
		}
	}
	
    // Method to create line representing current path on screen
	VectorLine DrawPath(Vector2[] path)
	{
		DrawPathHandler newDraw = new DrawPathHandler(path);
		return newDraw.GetLine();		
	}
	
    // Method to vacuum dist map using given path
	VacuumHandler ComputeVertices(Vector2[] path)
	{
        Vector3[] copy = new Vector3[ProjectConstants.curVertices.Length];
        Array.Copy(ProjectConstants.curVertices, copy, copy.Length);
    	Mesh diffMesh = GameObject.Find("PlaneDiff").GetComponent<MeshFilter>().mesh;
		VacuumHandler vh = new VacuumHandler(path, copy, diffMesh.vertices);
		return vh;
	}
	
	// Work to be done by a separate thread. And it will stop when a flag is set to false;
	void PathPlannerFactory ()	
	{
		if(!ProjectConstants.stopPathPlanFactory)
		{
			for (int i = resolution; i < ProjectConstants.durationLeft + 1; i += resolution)
	    	{
	    	    if (lstPaths[i - 1] != null)
	    	    {
					// If a path already exists, then no need to plan again
	    	    }
				else if(ProjectConstants.boolUseEndPoint &&  ProjectConstants.endPointCounter > 0)
				{
					// If duration won't allow reaching end point, then no need to plan
					if(MISCLib.ManhattanDistance(curEndPointPos, UAVPos)*10 > i*30)
					{
						continue;
					}					
				}
	    	    else
	    	    {
					Debug.Log("Duration: " + i + ". Doing additional path planning.");
					NetworkCall call = new NetworkCall(
	    	            ProjectConstants.mDistMapCurStepUndo.Clone(),
	    	            ProjectConstants.mDiffMap,
	    	            ProjectConstants.curStart,
	    	            ProjectConstants.curEnd,
	    	            ProjectConstants.boolUseDiffMap,
	    	            ProjectConstants.boolUseEndPoint,
	    	            ProjectConstants.boolPlenty,
	    	            i * 30);
	    	        lstPaths[i - 1] = call.path;
	    	        // Debug.Log("Path returned length = " + call.path.Length);
	    	    }
				if(ProjectConstants.stopPathPlanFactory)
				{
					return;
				}
	    	}
		}
	}
	
}
