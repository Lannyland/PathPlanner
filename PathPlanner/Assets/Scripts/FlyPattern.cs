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
        // Add UAV current location to be the beginning of the line
        linePoints[0] = new Vector2(transform.position.x, transform.position.z);
        // Create line
    	line = new VectorLine("Line", linePoints, lineWidth, lineType, joins);
	endReached = false;
	index = 0;

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
        
    }

    private void DrawLawnmower()
    {
        throw new System.NotImplementedException();
    }

    private void DrawSpiral()
    {
        throw new System.NotImplementedException();
    }
}
