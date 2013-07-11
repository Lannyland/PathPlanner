using UnityEngine;
using System.Collections;
using Vectrosity;

public class TestCode : MonoBehaviour {

    public VectorLine selectionLine;
    public Vector2[] linePoints;
    public Material lineMaterial;
    public int maxPoints = 2000;
    public float lineWidth = 4.0f;
    private Vector2 originalPos;
    private float textureScale = 4.0f;

	// Use this for initialization
	void Start () {
        linePoints = new Vector2[500];
        selectionLine = new VectorLine("Selection", linePoints, lineMaterial, 4.0f, LineType.Continuous);

        //// Start new line with up to 500 points
        //linePoints = new Vector2[500];
        //// Create line
        //line = new VectorLine("Line", linePoints, lineMaterial, lineWidth, LineType.Continuous, Joins.Weld);
	}

    // Update is called once per frame
    void Update()
    {
        //line.MakeRect(new Vector2(100,100), new Vector2(800,800));
        //line.Draw();	


        if (Input.GetMouseButtonDown(0))
        {
            originalPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            // selectionLine.MakeRect(new Vector2(100, 100), new Vector2(800, 800));
            selectionLine.MakeRect(originalPos, Input.mousePosition);
            selectionLine.Draw();
        }

        selectionLine.SetTextureScale(textureScale, -Time.time * 2.0f % 1);
	}
	
}
