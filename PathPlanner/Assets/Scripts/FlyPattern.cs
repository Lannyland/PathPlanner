using UnityEngine;
using System;
using System.Collections;
using Vectrosity;

public class FlyPattern : MonoBehaviour {


    public enum PatternMode { Line, Lawnmower, Spiral };

    public float moveSpeed = 10f;
    public Vector2[] path;
    public bool fly = false;
    public PatternMode flyMode = PatternMode.Line;
    public Vector3[] distVertices;
    public Color[] distColors;
    public VectorLine line;
    public Vector2[] linePoints;
	public Material lineMaterial;
    public int maxPoints = 16000;
    public float lineWidth = 4.0f;
	
    private int index = 0;
    private int flightDuration = 0;
    private float maxDiff = 0f;
    private Mesh diffMesh;
    private Mesh distMesh;
    private Vector3[] diffVertices;

    private float textureScale = 4.0f;
    private Vector3 UAVPos;
    private Vector2 mousePos;
    
    //public float turnSpeed = 50f;
    //public int timer = 0;
    //public int curWaypoint = 1;
    //private float TimeStep = 0f;


	// Use this for initialization
	void Start () {
        UAVPos = this.gameObject.transform.position;        
        SetLine();
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
		Camera curCam = GameObject.Find("ControlCenter").GetComponent<StartUpPattern>().curCam;
		linePoints[0] = curCam.WorldToScreenPoint(UAVPos);
		index = 1;

		// The current line point should always be where the mouse is
		mousePos = transform.InverseTransformPoint(Input.mousePosition);
		linePoints[index] = mousePos;
		line.maxDrawIndex = index;

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
        // Clear previous lines
		line.ZeroPoints();
		line.Draw ();
		
		index = 0;
        // Add UAV current location to be the beginning of the line
        Camera curCam = GameObject.Find("ControlCenter").GetComponent<StartUpPattern>().curCam;
        Vector2 UAVScreenPos = curCam.WorldToScreenPoint(UAVPos);
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

        int columnWidth = GetOffsetDistance(curCam);
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
        // Clear previous lines
        line.ZeroPoints();
        line.Draw();

        index = 0;
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
        float dist = (UAVScreenPos - mousePos).magnitude;
        float angle = Vector2.Angle(UAVScreenPos, mousePos);
        Vector3 cross = Vector3.Cross(UAVScreenPos, mousePos);
        if (cross.z > 0)
        {
            angle = 360 - angle;
        }
        double rotate = angle * oneDegree;

        // Debug.Log("dist = " + dist);
        double end = dist / b;
        // Debug.Log("count = " + end/Math.PI/2);
        // Draw points
        for (double theta = 0; theta < end; theta += step)
        {
            double r = a + b * theta; // Radius of current point
            float x = Convert.ToSingle(Math.Cos(theta - angle) * r) + UAVScreenPos.x;
            float y = Convert.ToSingle(Math.Sin(theta - angle) * r) + UAVScreenPos.y;
            index++;
            linePoints[index] = new Vector2(x, y);
        }
        line.maxDrawIndex = index;

        line.Draw();
        line.SetTextureScale(textureScale, -Time.time * 2.0f % 1);
    }
	
	void OnMouseUp()
	{
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
		
		switch (flyMode)
        {
            case PatternMode.Line:
                // Move UAV to mousePos                
                newUAVPos = curCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, z));
                break;

            case PatternMode.Lawnmower:
				// Move UAV to mousePos\
				// Debug.Log("linePoints = " + linePoints[0] + " " + linePoints[1] + " " + linePoints[2] + " " + linePoints[3] + " " + linePoints[4] + " " +linePoints[5]);
				// Debug.Log("maxDrawIndex = " + line.maxDrawIndex);
				// Debug.Log("linePoints[max] = " + linePoints[line.maxDrawIndex]);
                newUAVPos = curCam.ScreenToWorldPoint(new Vector3(linePoints[line.maxDrawIndex].x, linePoints[line.maxDrawIndex].y, z));
                break;

            case PatternMode.Spiral:
                
                break;

            default:
                break;
        }
        this.gameObject.transform.position = newUAVPos;
        UAVPos = this.gameObject.transform.position;

		// Erase line
		line.ZeroPoints();
		line.Draw();
		
		// Save Undo States and vacuum following line
		
		
		// Deduct time needed to complete line		
		
		

	}

    // Compute 0.1 world unit length in screen space
    private static int GetOffsetDistance(Camera curCam)
    {
        // Determine column size in screen space
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = new Vector3(0f, 0f, 0.1f);
        Vector2 p3 = curCam.WorldToScreenPoint(p1);
        Vector2 p4 = curCam.WorldToScreenPoint(p2);
        int width = Convert.ToInt16((p3 - p4).magnitude);
        return width;
    }
		
}
