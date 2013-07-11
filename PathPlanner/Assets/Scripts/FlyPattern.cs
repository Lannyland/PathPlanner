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

    //public float turnSpeed = 50f;
    //public int timer = 0;
    //public int curWaypoint = 1;
    //private float TimeStep = 0f;


	// Use this for initialization
	void Start () {
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
		Vector3 UAVPos = this.gameObject.transform.position;
		linePoints[0] = curCam.WorldToScreenPoint(UAVPos);
		index = 1;

		// The current line point should always be where the mouse is
		Vector2 mousePos = transform.InverseTransformPoint(Input.mousePosition);
		linePoints[index] = mousePos;
		line.maxDrawIndex = index;

		// Draw line
		line.Draw ();
		
		if(Input.GetMouseButtonUp(0))
		{
			OnMouseUp();
		}
		
    }

    private void DrawLawnmower()
    {
        throw new System.NotImplementedException();
    }

    private void DrawSpiral()
    {
        throw new System.NotImplementedException();
    }
	
	void OnMouseUp()
	{
		// Don't do anything if clicking button panel
		Vector2 mousePos = transform.InverseTransformPoint(Input.mousePosition);
		if(mousePos.x > Screen.width-200)
		{
			return;			
		}
		
		// Erase line
		line.ZeroPoints();
		line.Draw();
		
		// Move UAV to mousePos
		Camera curCam = GameObject.Find("ControlCenter").GetComponent<StartUpPattern>().curCam;		
		// Debug.Log("angle = " + curCam.transform.eulerAngles.x + " = " + curCam.transform.eulerAngles.x * Math.PI / 180f);
		// Debug.Log("sin(angle) = " + (Math.Sin(curCam.transform.eulerAngles.x * Math.PI / 180f)));
		float z = Convert.ToSingle((curCam.transform.position.y - 4f) / Math.Sin(curCam.transform.eulerAngles.x * Math.PI / 180f));
		Vector3 oldUAVPos = this.gameObject.transform.position;
		Vector3 newUAVPos = curCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, z));
		this.gameObject.transform.position = newUAVPos;
		
		// Save Undo States and vacuum following line
		
		
		// Deduct time needed to complete line		
		
		

	}
		
		
}
