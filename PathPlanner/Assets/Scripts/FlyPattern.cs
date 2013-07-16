using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;
using Vectrosity;

public class FlyPattern : MonoBehaviour {


    public enum PatternMode { Line, Lawnmower, Spiral };

    public float moveSpeed = 10f;
    public List<Vector2> path = new List<Vector2>();
    public bool fly = false;
    public PatternMode flyMode = PatternMode.Line;
    public Vector3[] distVertices;
    public Color[] distColors;
    public int timer = 0;
	
    public VectorLine line;
    public Vector2[] linePoints;
	public Material lineMaterial;
    public int maxPoints = 10000;
    public float lineWidth = 4.0f;
    public double lastSegLeft = 0;

	private double lineLength = 0;
    private int index = 0;
    private int flightDuration = 0;
    private float maxDiff = 0f;
    private Mesh diffMesh;
    private Mesh distMesh;
    private Vector3[] diffVertices;

    private float textureScale = 4.0f;
    private Vector3 UAVPos;
	private Vector2 UAVScreenPos;
    private Vector2 mousePos;
	private Camera curCam;
	private double screenUnit;
	private double unitCount;
	
	private List<UAVState> UAVStates = new List<UAVState>();
	
	// Use this for initialization
	void Start () {

        Initialize();
        SetLine();
	}

    public void Initialize()
    {
        moveSpeed = 10f;
        fly = false;
        flyMode = PatternMode.Line;
        maxPoints = 10000;
        lineWidth = 4.0f;
        lineLength = 0;
        index = 0;
        maxDiff = 0f;
        textureScale = 4.0f;
        lastSegLeft = 0;
		
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
		
        flightDuration = ProjectConstants.intFlightDuration * 60;
        timer = flightDuration;
		// path.Add(ProjectConstants.originalStart);		

        UAVPos = this.gameObject.transform.position;
    }

    void SetLine()
    {
	    // Get rid of previous lines
        VectorLine.Destroy (ref line);
        // Start new line with up to maxPoints points
        linePoints = new Vector2[maxPoints];
        // Create line
		linePoints[0] = Vector2.zero;
    	line = new VectorLine("Line", linePoints, lineMaterial, lineWidth, LineType.Continuous, Joins.Weld);
    }
	
	// Update is called once per frame
	void Update () {
        if(!fly)
		{
			return;
		}			
		
		// Update UAV position
 		curCam = GameObject.Find("ControlCenter").GetComponent<StartUpPattern>().curCam;		
		UAVPos = this.gameObject.transform.position;                
		UAVScreenPos = curCam.WorldToScreenPoint(UAVPos);
		screenUnit = GetOffsetDistance(curCam);	
		lastSegLeft = screenUnit * unitCount;
		// Debug.Log("lastSegLeft = " + lastSegLeft);
		
		// Clear previous lines
		ClearLine();
		
        // Enable special key combinations
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.M))
        {
            flyMode = PatternMode.Lawnmower;
        }
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.L))
        {
            flyMode = PatternMode.Line;
        }
        if ((Input.GetKey(KeyCode.LeftAlt)  || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.S))
        {
            flyMode = PatternMode.Spiral;
        }

		switch (flyMode)
        {
            case PatternMode.Line:
                DrawLine();               
                break;
            case PatternMode.Lawnmower:
                DrawLawnmower();               
                break;
            case PatternMode.Spiral:
                DrawSpiral();
                break;
            default:
                break;
        }
	}

    private void DrawLine()
    {
        // Add UAV current location to be the beginning of the line
		linePoints[0] = curCam.WorldToScreenPoint(UAVPos);
		index = 1;

		// The current line point should always be where the mouse is
		mousePos = transform.InverseTransformPoint(Input.mousePosition);
		linePoints[index] = mousePos;
		line.maxDrawIndex = index;
		
		// Make sure line won't exceed remaining time
		lineLength = GetLineLength(line, linePoints);
		int timeNeeded = Convert.ToInt32(lineLength / screenUnit*2);
		if(timeNeeded > timer)
		{
			// Find point along the line with exact length
			float ratio = timer *1f / timeNeeded;
			float new_x = UAVScreenPos.x + (mousePos.x - UAVScreenPos.x)*ratio;
			float new_y = UAVScreenPos.y + (mousePos.y - UAVScreenPos.y)*ratio;
			linePoints[index] = new Vector2(new_x, new_y);
		}		
		
		// Draw line
		line.Draw ();
        line.SetTextureScale(textureScale, -Time.time * 2.0f % 1);

		if(Input.GetMouseButtonUp(0))
		{
			OnMouseUp();
		}
		
    }

    private void DrawLawnmower()
    {
        // Add UAV current location to be the beginning of the line
        Camera curCam = GameObject.Find("ControlCenter").GetComponent<StartUpPattern>().curCam;
        linePoints[0] = curCam.WorldToScreenPoint(UAVPos);

        // Mouse position should be a corner of the rectangle
        mousePos = transform.InverseTransformPoint(Input.mousePosition);

        // Determine signs in x axis
        int sign_x = 1;
        if (mousePos.x > UAVScreenPos.x)
        {
            sign_x = 1;
        }
        else
        {
            sign_x = -1;
        }

        float columnWidth = GetOffsetDistance(curCam);
        // Debug.Log("columnWidth = " + columnWidth);

        float remain = Mathf.Abs(mousePos.x - UAVScreenPos.x);
		// Debug.Log("Original remain = " + remain );
				
        // Draw first segment
        index++;
        linePoints[index] = new Vector2(linePoints[index - 1].x, mousePos.y);
        // Debug.Log("point1 = " + linePoints[0] + " Poin2 = " + linePoints[1]);        

        int flagU = 0;
        int flagM = 1;
        while (remain > columnWidth)
        {
            index++;
            linePoints[index] = new Vector2(linePoints[index - 1].x + columnWidth * sign_x, UAVScreenPos.y * flagU + mousePos.y * flagM);
            int temp = flagM;
            flagM = flagU;
            flagU = temp;
            index++;
            linePoints[index] = new Vector2(linePoints[index - 1].x, UAVScreenPos.y * flagU + mousePos.y * flagM);
            remain -= columnWidth;
			// Debug.Log("Next remain = " + remain );
        }
        line.maxDrawIndex = index;

        // Make sure line won't exceed remaining time
        lineLength = 0;
        int lastTimeNeeded = 0;
        int timerLeft = 0;
        for (int i = 0; i < line.maxDrawIndex; i++)
        {
            lineLength += (linePoints[i + 1] - linePoints[i]).magnitude;

            int timeNeeded = Convert.ToInt16(lineLength / screenUnit * 2);
            if (timeNeeded > timer)
            {
                // Find point along the line with exact length
                int timeNeededCurSeg = timeNeeded - lastTimeNeeded;
                float ratio = timerLeft * 1f / timeNeededCurSeg;
                float new_x = linePoints[i].x + (linePoints[i + 1].x - linePoints[i].x) * ratio;
                float new_y = linePoints[i].y + (linePoints[i + 1].y - linePoints[i].y) * ratio;
                linePoints[i + 1] = new Vector2(new_x, new_y);
                line.maxDrawIndex = i + 1;
                break;
            }
            lastTimeNeeded = timeNeeded;
            timerLeft = timer - lastTimeNeeded;
        }
        
        line.Draw();
        line.SetTextureScale(textureScale, -Time.time * 2.0f % 1);

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
    }

    private void DrawSpiral()
    {
        // Add UAV current location to be the beginning of the line
        Camera curCam = GameObject.Find("ControlCenter").GetComponent<StartUpPattern>().curCam;
        Vector2 UAVScreenPos = curCam.WorldToScreenPoint(UAVPos);
        linePoints[0] = curCam.WorldToScreenPoint(UAVPos);

        // One degree:
        double oneDegree = Math.PI / 180f;
        double step = 5 * oneDegree;
        // a is the radius of the center empty area
        double a = 0f;
        // b is the distance between spines (0.1 world unit)
        double b = GetOffsetDistance(curCam) / (2 * Math.PI);
        // Compute hao many total degrees we need.
        mousePos = transform.InverseTransformPoint(Input.mousePosition);
        Vector2 v = mousePos - UAVScreenPos;
        float dist = v.magnitude;
        float angle = Mathf.Atan2(v.y, v.x);
        if (angle<0)
        {
            angle = angle + 2 * Mathf.PI;
        }
		// Debug.Log("v = " + v + " angle = " + angle * Mathf.Rad2Deg);

        // Debug.Log("dist = " + dist);
        double end = dist / b;
        // Debug.Log("count = " + end/Math.PI/2);
        // Draw points
        for (double theta = 0; theta < end; theta += step)
        {
            double r = a + b * theta; // Radius of current point
            float x = Convert.ToSingle(Math.Cos(theta - end + angle) * r) + UAVScreenPos.x;
            float y = Convert.ToSingle(Math.Sin(theta - end + angle) * r) + UAVScreenPos.y;
            index++;
            linePoints[index] = new Vector2(x, y);
        }
        line.maxDrawIndex = index;

        // Make sure line won't exceed remaining time
        lineLength = 0;
        int lastTimeNeeded = 0;
        int timerLeft = 0;
        for (int i = 0; i < line.maxDrawIndex; i++)
        {
            lineLength += (linePoints[i + 1] - linePoints[i]).magnitude;

            int timeNeeded = Convert.ToInt16(lineLength / screenUnit * 2);
            if (timeNeeded > timer)
            {
                // Find point along the line with exact length
                int timeNeededCurSeg = timeNeeded - lastTimeNeeded;
                float ratio = timerLeft * 1f / timeNeededCurSeg;
                float new_x = linePoints[i].x + (linePoints[i + 1].x - linePoints[i].x) * ratio;
                float new_y = linePoints[i].y + (linePoints[i + 1].y - linePoints[i].y) * ratio;
                linePoints[i + 1] = new Vector2(new_x, new_y);
                line.maxDrawIndex = i + 1;
                break;
            }
            lastTimeNeeded = timeNeeded;
            timerLeft = timer - lastTimeNeeded;
        }

        line.Draw();
        line.SetTextureScale(textureScale, -Time.time * 2.0f % 1);

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
    }
	
	void OnMouseUp()
	{
		// Don't do anything if start button is not clicked yet.
        if (!fly)
        {
            return;
        }
        
        // Don't do anything if clicking button panel
		mousePos = transform.InverseTransformPoint(Input.mousePosition);
		if(mousePos.x > Screen.width-200)
		{
			return;			
		}
		
        Camera curCam = GameObject.Find("ControlCenter").GetComponent<StartUpPattern>().curCam;
        // Debug.Log("angle = " + curCam.transform.eulerAngles.x + " = " + curCam.transform.eulerAngles.x * Math.PI / 180f);
        // Debug.Log("sin(angle) = " + (Math.Sin(curCam.transform.eulerAngles.x * Math.PI / 180f)));
		float z = Convert.ToSingle((curCam.transform.position.y - 4f) / Math.Sin(curCam.transform.eulerAngles.x * Math.PI / 180f));
        Vector3 oldUAVPos = this.gameObject.transform.position;
		Vector3 newUAVPos = Vector3.zero;
		
        newUAVPos = curCam.ScreenToWorldPoint(new Vector3(linePoints[line.maxDrawIndex].x, linePoints[line.maxDrawIndex].y, z));
        this.gameObject.transform.position = newUAVPos;
        UAVPos = this.gameObject.transform.position;

        // First find all points along line based on fixed speeed. This path is in screen space
        Vector2[] screenPath = new Vector2[ProjectConstants.intFlightDuration * 60 / 2 + 1];
        // Add start point to path
        int counter = 0;
		if(UAVStates.Count == 0)
		{
			screenPath[0] = linePoints[0];
			counter = 1;
		}

        for (int i = 0; i < line.maxDrawIndex; i++)
        {
            Vector2 v = linePoints[i + 1] - linePoints[i];
            float dist = v.magnitude;
            double curSegLeft = dist;
            int curSegPointCounter = 0;

			// Get points for current segment
            float angle = Mathf.Atan2(v.y, v.x);
            if (angle < 0)
            {
                angle = angle + 2 * Mathf.PI;
            }
			
			if(curSegLeft + lastSegLeft >= screenUnit)
			{
				while (curSegLeft + lastSegLeft >= screenUnit)
	            {
	                if(curSegPointCounter == 0)
					{
						double r = screenUnit - lastSegLeft;
		                float x = Convert.ToSingle(Math.Cos(angle) * r) + linePoints[i].x;
		                float y = Convert.ToSingle(Math.Sin(angle) * r) + linePoints[i].y;
		                screenPath[counter] = new Vector2(x, y);
						curSegLeft -= screenUnit - lastSegLeft;
						lastSegLeft = 0;
					}
					else
					{
						double r = screenUnit;
		                float x = Convert.ToSingle(Math.Cos(angle) * r) + screenPath[counter - 1].x;
		                float y = Convert.ToSingle(Math.Sin(angle) * r) + screenPath[counter - 1].y;
		                screenPath[counter] = new Vector2(x, y);
	                	curSegLeft -= screenUnit;					
					}
	                counter++;
	                curSegPointCounter++;
	            }
	            lastSegLeft = curSegLeft;
			}
			else
			{
				lastSegLeft += curSegLeft;
			}
        }
		unitCount = lastSegLeft / screenUnit;
		
//		Debug.Log("Path:");
//		for(int i=0; i<20; i++)
//		{
//			Debug.Log (screenPath[i]);
//		}

//      // Draw points to make sure it looks right.
//      VectorPoints myPoints = new VectorPoints("Points", screenPath, lineMaterial, 10.0f);
//      myPoints.Draw();
		
		// Convert screen path to real path
		List<Vector3> realPath = new List<Vector3>();
		for(int i=0; i<counter; i++)
		{
			Vector3 point = curCam.ScreenToWorldPoint(new Vector3(screenPath[i].x, screenPath[i].y, z));
			realPath.Add(point);
            path.Add(new Vector2(point.x, point.z));
			// Debug.Log(point);
        }
		
		// Save Undo States
		UAVState curState = new UAVState();
		curState.lastSegLeft = lastSegLeft;
		curState.path = path;
		curState.distVertices = (Vector3[])distVertices.Clone();
		curState.distColors = (Color[])distColors.Clone();
		curState.timer = timer;
		curState.UAVPos = oldUAVPos;
		curState.Score = GameObject.Find("ControlCenter").GetComponent<IncreasingScoreEffect>().curScore;
		UAVStates.Add (curState);

        // Vacuum following line
		UAVPos = this.gameObject.transform.position;
		for(int i=0; i<realPath.Count; i++)
		{
			this.gameObject.transform.position = realPath[i];
			GameObject.Find("ControlCenter").GetComponent<IncreasingScoreEffect>().curScore += VacuumCells(realPath[i]);	
		}
		this.gameObject.transform.position = UAVPos;
   		distMesh.vertices = distVertices;
		distMesh.colors = distColors;

		// Debug.Log("screenUnit = " + screenUnit + " lastSegLeft = " + lastSegLeft);
        
        // Deduct time needed to complete line		
		if(timer>0)
		{
			lineLength = GetLineLength(line, linePoints);
			// Debug.Log("T_used = " + lineLength + "/" + screenUnit + "*2 = " + (lineLength/screenUnit*2));
			timer -= Convert.ToInt16(Math.Round (lineLength / screenUnit * 2));
		}
		int second = timer % 60;
		int minute = timer / 60;
		GameObject.Find("lblFlightTime").GetComponent<UILabel>().text = minute.ToString() + ":" + second.ToString("00");

		// Erase line
		line.ZeroPoints();
		line.Draw();

        if (timer == 0)
        {
            GameObject.Find("btnFly").GetComponent<UIButton>().isEnabled = true;
            fly = false;
        }		
	}

    // Compute 0.1 world unit length in screen space
    private static float GetOffsetDistance(Camera curCam)
    {
        // Determine column size in screen space
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = new Vector3(0f, 0f, 0.1f);
        Vector2 p3 = curCam.WorldToScreenPoint(p1);
        Vector2 p4 = curCam.WorldToScreenPoint(p2);
        float width = (p3 - p4).magnitude;
        return width;
    }
	
    // Compute line total length
	private static float GetLineLength(VectorLine l, Vector2[] points)
	{
		float length = 0;
		for(int i=0; i<l.maxDrawIndex; i++)
		{
			length += (points[i+1] - points[i]).magnitude;
		}
		return length;
	}
	
	// Erase line
	public void ClearLine()
	{
		line.ZeroPoints();
        line.Draw();
        index = 0;
		lineLength = 0;
	}
	
	// Undo and set things back
	public void Undo()
	{
        int last = UAVStates.Count - 1;
        if (last < 0)
        {
            // Already back at the very beginning. Cannot undo anymore.
            return;
        }
        UAVState lastState = UAVStates[last];
        UAVStates.RemoveAt(last);

	    lastSegLeft = lastState.lastSegLeft;
        path = lastState.path;
        distVertices = (Vector3[])lastState.distVertices.Clone();
        distColors = (Color[])lastState.distColors.Clone();
        timer = lastState.timer;
        UAVPos = lastState.UAVPos;
        GameObject.Find("ControlCenter").GetComponent<IncreasingScoreEffect>().curScore = lastState.Score;
        distMesh.vertices = distVertices;
        distMesh.colors = distColors;
        this.gameObject.transform.position = UAVPos;		
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
		
		// distMesh.vertices = distVertices;
		// distMesh.colors = distColors;
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
		
		// string log = "";		
		if(skip)
		{
			p = 1;
            point.x = c.x;
            point.y = c.z;
		}
		else
		{
			// Use monte carlo method to estimate portion of cell that's overlapping		
			int counter = 0;		
			for(int i=-5; i<5; i++)
			{
				for(int j=-5; j<5;j++)
				{
					point.x = c.x + 0.01f*j;
					point.y = c.z + 0.01f*i;
					// log+="(" + point.x.ToString() + "," + point.y.ToString() + ")";
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
		if(i>ProjectConstants.intMapWidth*ProjectConstants.intMapWidth-1)
		{
			// UAV flew outside of map.
			return 0f;
		}
        if (i > diffVertices.Length)
        {
            Debug.Log("Stop!");
        }
		if(maxDiff == 0f)
		{
			diffVertices[i].y = 1;
		}
		else
		{
	        diffVertices[i].y = (maxDiff + 1 - diffVertices[i].y) * (1.0f / (maxDiff + 1));
		}
		
		float v = distVertices[i].y * diffVertices[i].y * p;
		distVertices[i].y -= v;
		
		distColors[i] = MISCLib.HeightToDistColor(distVertices[i].y, 4f);
		return v;
	}	
}

public class UAVState
{
	public double lastSegLeft;
	public List<Vector2> path;
    public Vector3[] distVertices;
    public Color[] distColors;
	public int timer;
	public Vector3 UAVPos;	
	public float Score;
			
	public UAVState()
	{
	}
			
	~UAVState()
	{
	}
}