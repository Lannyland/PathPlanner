using UnityEngine;
using System.Collections;
using Vectrosity;

public class DrawPathHandler {

    public Material lineMaterial;
    public int maxPoints = 2000;
    public float lineWidth = 4.0f;
    public int minPixelMove = 1;	// Must move at least this many pixels per sample for a new segment to be recorded
    public bool useEndCap = false;
    public Texture2D capTex;
    public Material capMaterial;
    public VectorLine line;	

	private Vector2[] path;	
    private Vector3[] linePoints;
    private int lineIndex = 0;
    private Vector2 previousPosition;
    private int sqrMinPixelMove;
    private bool canDraw = false;

	// Contructor
	public DrawPathHandler(Vector2[] _path)
	{
		path = _path;
		CreateLine();
	}
	
	// Destructor
	~DrawPathHandler()
	{
		path = null;
	}
	
	// Method to create a 3D line.
	public void CreateLine()
	{
		linePoints = new Vector3[path.Length];
		for(int i=0; i<path.Length; i++)
		{
			linePoints[i] = new Vector3(path[i].x/10f - 5f, 4f, path[i].y/10f - 5f);
		}		
        line = new VectorLine("Path", linePoints, lineMaterial, lineWidth, LineType.Continuous, Joins.Weld);
	}
	
	// Line getter
	public VectorLine GetLine()
	{
		return line;
	}
}
