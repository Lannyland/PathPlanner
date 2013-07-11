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
    public int maxPoints = 2000;
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
        // Start new line with up to 500 points
        linePoints = new Vector2[500];
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
        index = 0;
        // Add UAV current location to be the beginning of the line
        Camera curCam = GameObject.Find("ControlCenter").GetComponent<StartUpPattern>().curCam;
        Vector2 UAVScreenPos = curCam.WorldToScreenPoint(UAVPos);
        linePoints[0] = curCam.WorldToScreenPoint(UAVPos);

        // Mouse position should be a corner of the rectangle
        mousePos = transform.InverseTransformPoint(Input.mousePosition);

        // Determine signs in each axies
        int sign_x = 1;
        int sign_y = 1;
        if (mousePos.x > UAVScreenPos.x)
        {
            sign_x = 1;
        }
        else
        {
            sign_x = -1;
        }
        if (mousePos.y > UAVScreenPos.y)
        {
            sign_y = 1;
        }
        else
        {
            sign_y = -1;
        }

        // Determine column size in screen space
        int columnWidth = 50;
        float remain = Mathf.Abs(mousePos.x - UAVPos.x);

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
        throw new System.NotImplementedException();
    }
	
	void OnMouseUp()
	{
		// Don't do anything if clicking button panel
		mousePos = transform.InverseTransformPoint(Input.mousePosition);
		if(mousePos.x > Screen.width-200)
		{
			return;			
		}
		
		// Erase line
		line.ZeroPoints();
		line.Draw();

        switch (flyMode)
        {
            case PatternMode.Line:
                // Move UAV to mousePos
                Camera curCam = GameObject.Find("ControlCenter").GetComponent<StartUpPattern>().curCam;
                // Debug.Log("angle = " + curCam.transform.eulerAngles.x + " = " + curCam.transform.eulerAngles.x * Math.PI / 180f);
                // Debug.Log("sin(angle) = " + (Math.Sin(curCam.transform.eulerAngles.x * Math.PI / 180f)));
                float z = Convert.ToSingle((curCam.transform.position.y - 4f) / Math.Sin(curCam.transform.eulerAngles.x * Math.PI / 180f));
                Vector3 oldUAVPos = this.gameObject.transform.position;
                Vector3 newUAVPos = curCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, z));
                this.gameObject.transform.position = newUAVPos;
                UAVPos = this.gameObject.transform.position;
                break;

            case PatternMode.Lawnmower:
                
                break;

            case PatternMode.Spiral:
                
                break;

            default:
                break;
        }

		
		// Save Undo States and vacuum following line
		
		
		// Deduct time needed to complete line		
		
		

	}
		
		
}
