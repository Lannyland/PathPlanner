using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;
using rtwmatrix;

public class FlyManual : MonoBehaviour {

    public enum FlyMode { Turn, Strafe };

    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
	public Vector2[] path;
	public bool fly = false;
    public FlyMode flyMode = FlyMode.Turn;
    public Vector3[] distVertices;
    public Color[] distColors;
    public int timer = 0;
	public int curWaypoint = 1;
	
	private int flightDuration = 0;
	private float maxDiff = 0f;		
	private Mesh diffMesh;			
	private Mesh distMesh;			
	private Vector3[] diffVertices;
	private float TimeStep = 0f;
	
	// Use this for initialization
	void Start () {	
		flightDuration = ProjectConstants.intFlightDuration * 60;
		timer = flightDuration;
		path = new Vector2[flightDuration+1];
		path[0] = ProjectConstants.originalStart;
		
        // Get diff map max
        diffMesh = GameObject.Find("PlaneDiff").GetComponent<MeshFilter>().mesh;
        diffVertices = diffMesh.vertices;
        for (int i = 0; i < diffVertices.Length; i++)
        {
            if (maxDiff < diffVertices[i].y)
            {
                maxDiff = diffVertices[i].y;
            }
        }
        if (maxDiff < 0.001f)
        {
            // No Diff map is used
            maxDiff = 0f;
            Debug.Log("No diff map is used based on maxDiff.");
        }
        distMesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;			
        distVertices = distMesh.vertices;
        distColors = distMesh.colors;						
	}
	
	// Update is called once per frame
	void Update () {
		if(fly)
		{
            // Move around
            if (Input.GetKey(KeyCode.UpArrow))
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.DownArrow))
                transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
            // Different fly mode
            if (flyMode == FlyMode.Turn)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                    transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);

                if (Input.GetKey(KeyCode.RightArrow))
                    transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
            }
            if (flyMode == FlyMode.Strafe)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

                if (Input.GetKey(KeyCode.RightArrow))
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
		}
	}
	
	// Update at fixed time
	void FixedUpdate()
	{
		if(fly)
		{
			// Set speed
			TimeStep += Time.deltaTime;
			if(TimeStep > 0.05)
			{			
				// Do this only once. Vacuum starting point
				Vector3 UAVPos;
				if(curWaypoint == 1)
				{
					UAVPos = this.transform.position;
                    Debug.Log("UAVPos = " + UAVPos);
					GameObject.Find("ControlCenter").GetComponent<IncreasingScoreEffect>().curScore += VacuumCells(UAVPos);
					// Debug.Log("First visit score: " + Camera.main.GetComponent<IncreasingScoreEffect>().curScore);
				}
				
				if(curWaypoint < flightDuration + 1)
				{
					// Vacuum probability and change score
					UAVPos = this.transform.position;
					// Debug.Log("UAVPos = " + UAVPos);
                    GameObject.Find("ControlCenter").GetComponent<IncreasingScoreEffect>().curScore += VacuumCells(UAVPos);
					
					// Change timer remaining time
					if(timer>0)
					{
						timer--;
					}
					int second = timer % 60;
					int minute = timer / 60;
					GameObject.Find("lblFlightTime").GetComponent<UILabel>().text = minute.ToString() + ":" + second.ToString("00");
					
					// Remember Path
					// Debug.Log("curWaypoint = " + curWaypoint);
					path[curWaypoint] = UAVPos;
					curWaypoint++;
				}
				if(curWaypoint == flightDuration + 1)
				{
					// Manual flight is over!
                    // Debug.Log("Setting UAV movable to false.");
					fly = false;
                    // Disable the Start/Pause button.
                    GameObject.Find("btnStart").GetComponent<UIButton>().isEnabled= false;
				}
				TimeStep = 0f;
			}
		}
	}

	// Method to do partial Vacuum
	float VacuumCells(Vector3 UAVPos)
	{
		// Get square corners
		Vector3 TL = GameObject.Find("TL").transform.position;
		Vector3 TR = GameObject.Find("TR").transform.position;
		Vector3 BR = GameObject.Find("BR").transform.position;
		Vector3 BL = GameObject.Find("BL").transform.position;
		Vector2[] square = new Vector2[4];
		square[0] = new Vector2(TL.x, TL.z);
		square[1] = new Vector2(TR.x, TR.z);
		square[2] = new Vector2(BR.x, BR.z);
		square[3] = new Vector2(BL.x, BL.z);
//		Debug.Log("Sqaure: (" + square[0].x + "," + square[0].y + ")"
//					   + " (" + square[1].x + "," + square[1].y + ")"
//					   + " (" + square[2].x + "," + square[2].y + ")" 
//					   + " (" + square[3].x + "," + square[3].y + ")");
		
		float cellCDF = 0f;
		List<Vector3> cells = FindOverlapCells(UAVPos);
		if(cells.Count>1)
		{
			foreach(Vector3 c in cells)
			{
				cellCDF += PartialVacuum(c, square, false);
			}
		}
		else
		{
			cellCDF += PartialVacuum(cells[0], square, true);
		}
		
		distMesh.vertices = distVertices;
		distMesh.colors = distColors;
		// distMesh.RecalculateNormals();
		// distMesh.RecalculateBounds();

		// Debug.Log("Total partial vacuum = " + cellCDF);		
		return cellCDF;
	}
	
	// Method to find all partially overlapping cells
	List<Vector3> FindOverlapCells(Vector3 UAVPos)
	{
		Vector3 closestVetex = new Vector3(Mathf.Round(UAVPos.x * 10f)/10f, UAVPos.y, Mathf.Round(UAVPos.z * 10f)/10f);

		List<Vector3> neighbors = new List<Vector3>();
		List<Vector3> overlapNeighbors = new List<Vector3>();
		
		// Find eight neighbors
		Vector3 new1 = closestVetex + new Vector3(-0.1f, 0f, 0.1f);
		neighbors.Add(new1);
		Vector3 new2 = closestVetex + new Vector3(0f, 0f, 0.1f);
		neighbors.Add(new2);
		Vector3 new3 = closestVetex + new Vector3(0.1f, 0f, 0.1f);
		neighbors.Add(new3);
		Vector3 new4 = closestVetex + new Vector3(0.1f, 0f, 0f);
		neighbors.Add(new4);
		Vector3 new5 = closestVetex + new Vector3(0.1f, 0f, -0.1f);
		neighbors.Add(new5);
		Vector3 new6 = closestVetex + new Vector3(0f, 0f, -0.1f);
		neighbors.Add(new6);
		Vector3 new7 = closestVetex + new Vector3(-0.1f, 0f, -0.1f);
		neighbors.Add(new7);
		Vector3 new8 = closestVetex + new Vector3(-0.1f, 0f, 0f);
		neighbors.Add(new8);		
			
		// Determine overlapping neighbors
		// The closest one must be valid
		overlapNeighbors.Add(closestVetex);	
		foreach(Vector3 n in neighbors)
		{
			if(Overlap(UAVPos, n))
			{
				overlapNeighbors.Add(n);
			}
		}
		return overlapNeighbors;		
	}
	
	// Method to check if overlap
	bool Overlap(Vector3 v1, Vector3 v2)
	{
		bool result = false;
		float r = Mathf.Sqrt (0.05f*0.05f*2);
		List<Vector3> corners = new List<Vector3>();
		corners.Add (new Vector3(-0.05f, 0f, 0.05f));	// Top Left
		corners.Add (new Vector3(0.05f, 0f, 0.05f));	// Top Right	
		corners.Add (new Vector3(0.05f, 0f, - 0.05f));	// Bottom Right
		corners.Add (new Vector3(-0.05f, 0f, -0.05f));	// Bottom Left		
		foreach(Vector3 v in corners)
		{
			Vector3 corner = v2 + v;
			Vector3 temp = corner - v1;
			float distance = Mathf.Sqrt (temp.x*temp.x + temp.z*temp.z);
			if( distance + 0.00001 < r)
			{
				return true;
			}
		}		
		return result;
	}
	
	// Method to partially vacuum a cell
	// This is where the vertex height and color actually gets changed
	float PartialVacuum(Vector3 c, Vector2[] square, bool skip)
	{
		float p = 0f;
		float cellCDF = 0f;
		Vector2 point = Vector2.zero;
		
		if(skip)
		{
			p = 1;
            point.x = c.x;
            point.y = c.z;
		}
		else
		{
			// Use monte carlo method to estimate portion of cell that's overlapping		
			string log = "";
			int counter = 0;		
			for(int i=-5; i<5; i++)
			{
				for(int j=-5; j<5;j++)
				{
					point.x = c.x + 0.01f*j;
					point.y = c.z + 0.01f*i;
					log+="(" + point.x.ToString() + "," + point.y.ToString() + ")";
					if(MISCLib.PointInPolygon(point, square))
					{
						counter++;
					}
				}
			}		
			p = counter/100f;
		}
		// Debug.Log("p = " + p );		
		// Debug.Log(log);
		
		// Now let's vacuum
		int Y = Convert.ToInt16(Mathf.Round (point.y * 10)+50);
		int X = Convert.ToInt16(Mathf.Round (point.x * 10)+50);
		int index = Convert.ToInt16(Y * ProjectConstants.intMapWidth + X);
        // Don't vacuum if flying out of map
        if (Y < 0 || Y > ProjectConstants.intMapHeight - 1
            || X < 0 || X > ProjectConstants.intMapWidth - 1)
        {
            index = -1;
        }
        // Don't vacuum if outside of map
        if (index < ProjectConstants.intMapWidth * ProjectConstants.intMapHeight
            && index > 0)
        {
            cellCDF = PointVacuum(index, p);
        }
		
		// Change vetext height and color
		// Debug.Log("Partial vacuum cell = " + cellCDF);		
		return cellCDF;
	}
	
	// Method to vacuum the point visited on path.
	float PointVacuum(int i, float p)
	{
        float diff = 0f;
        if (i > ProjectConstants.intMapWidth * ProjectConstants.intMapWidth - 1)
        {
            // UAV flew outside of map.
            return 0f;
        }
        if (i > diffVertices.Length)
        {
            Debug.Log("Stop!");
        }
        if (maxDiff == 0f)
        {
            diff = 1;
        }
        else
        {
            diff = (maxDiff + 1 - diffVertices[i].y) * (1.0f / (maxDiff + 1));
        }

        float v = distVertices[i].y * diff * p;
        distVertices[i].y -= v;

        distColors[i] = MISCLib.HeightToDistColor(distVertices[i].y, 4f);
        return v;
    }	
}
