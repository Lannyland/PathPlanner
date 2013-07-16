using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;
using Vectrosity;

public class FlyPattern : MonoBehaviour {


    public enum PatternMode { Line, Lawnmower, Spiral };

    public float moveSpeed = 10f;
    public Vector2[] path;
    public bool fly = false;
    public PatternMode flyMode = PatternMode.Line;
    public Vector3[] distVertices;
    public Color[] distColors;
    public int timer = 0;
	
    public VectorLine line;
    public Vector2[] linePoints;
	public Material lineMaterial;
    public int maxPoints = 1800;
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
    
    //public float turnSpeed = 50f;
    //public int timer = 0;
    //public int curWaypoint = 1;
    //private float TimeStep = 0f;


	// Use this for initialization
	void Start () {

        Initialize();
        SetLine();
	}

    private void Initialize()
    {
        moveSpeed = 10f;
        fly = false;
        flyMode = PatternMode.Line;
        maxPoints = 1800;
        lineWidth = 4.0f;
        lineLength = 0;
        index = 0;
        maxDiff = 0f;
        textureScale = 4.0f;
        lastSegLeft = 0;

        flightDuration = ProjectConstants.intFlightDuration * 60;
        timer = flightDuration;
        path = new Vector2[flightDuration + 1];

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
		double screenUnit = GetOffsetDistance(curCam);
		int timeNeeded = Convert.ToInt16(lineLength / screenUnit*2);
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
        Debug.Log("columnWidth = " + columnWidth);

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
        double screenUnit = GetOffsetDistance(curCam);
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

        //if (Input.GetMouseButton(0))
        //{
        //    line.MakeRect(UAVPos, Input.mousePosition);
        //    line.Draw();
        //}

        //selectionLine.SetTextureScale(textureScale, -Time.time * 2.0 % 1);

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
        double screenUnit = GetOffsetDistance(curCam);
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

		// Save Undo States
        
        // Save path segment 

        // Vacuum following line
        // First find all points along line based on fixed speeed.
        double screenUnit = GetOffsetDistance(curCam);
        // This path is in screen space
        Vector2[] screenPath = new Vector2[ProjectConstants.intFlightDuration * 60 / 2 + 1];
        // Add start point to path
        screenPath[0] = linePoints[0];

        // lineLength = 0;
        int counter = 1;
        for (int i = 0; i < line.maxDrawIndex; i++)
        {
            Vector2 v = linePoints[i + 1] - linePoints[i];
            float dist = v.magnitude;
            double curSegLeft = dist;
            int curSegPointCounter = 0;
            // lineLength += dist;
            while (curSegLeft + lastSegLeft >= screenUnit)
            {
                // Get points for current segment
                float angle = Mathf.Atan2(v.y, v.x);
                if (angle < 0)
                {
                    angle = angle + 2 * Mathf.PI;
                }
                if (curSegPointCounter == 0)
                {
                    double r = screenUnit - lastSegLeft;
                    float x = Convert.ToSingle(Math.Cos(angle) * r) + linePoints[i].x;
                    float y = Convert.ToSingle(Math.Sin(angle) * r) + linePoints[i].y;
                    screenPath[counter] = new Vector2(x, y);
                }
                else
                {
                    float x = Convert.ToSingle(Math.Cos(angle) * screenUnit) + screenPath[counter - 1].x;
                    float y = Convert.ToSingle(Math.Sin(angle) * screenUnit) + screenPath[counter - 1].y;
                    screenPath[counter] = new Vector2(x, y);
                }
                curSegLeft -= screenUnit;
                counter++;
                curSegPointCounter++;
            }
            lastSegLeft = curSegLeft;
        }

        // Draw points to make sure it looks right.
        VectorPoints myPoints = new VectorPoints("Points", screenPath, lineMaterial, lineWidth);
        myPoints.Draw();

        //    int timeNeeded = Convert.ToInt16(lineLength / screenUnit * 2);
        //    if (timeNeeded > timer)
        //    {
        //        // Find point along the line with exact length
        //        int timeNeededCurSeg = timeNeeded - lastTimeNeeded;
        //        float ratio = timerLeft * 1f / timeNeededCurSeg;
        //        float new_x = linePoints[i].x + (linePoints[i + 1].x - linePoints[i].x) * ratio;
        //        float new_y = linePoints[i].y + (linePoints[i + 1].y - linePoints[i].y) * ratio;
        //        linePoints[i + 1] = new Vector2(new_x, new_y);
        //        line.maxDrawIndex = i + 1;
        //        break;
        //    }
        //    lastTimeNeeded = timeNeeded;
        //    timerLeft = timer - lastTimeNeeded;
        //}


        //int lastTimeNeeded = 0;
        //int timerLeft = 0;
		

        






        
        
        
        
        
        // Deduct time needed to complete line		
		if(timer>0)
		{
			lineLength = GetLineLength(line, linePoints);
			Debug.Log("T_used = " + lineLength + "/" + screenUnit + "*2 = " + (lineLength/screenUnit*2));
			timer -= Convert.ToInt16(Math.Round (lineLength / screenUnit * 2));
		}
		int second = timer % 60;
		int minute = timer / 60;
		GameObject.Find("lblFlightTime").GetComponent<UILabel>().text = minute.ToString() + ":" + second.ToString("00");

		// Erase line
		line.ZeroPoints();
		line.Draw();
		
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
}
