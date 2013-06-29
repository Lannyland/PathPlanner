using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;
using rtwmatrix;

public class FlyPathClicked : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		Debug.Log("Fly Path button is clicked.");	
        // First set things back to starting position
        // Move UAV back to original position
        GameObject.Find("UAV").transform.position = new Vector3(ProjectConstants.originalStart.x, 4.0f, ProjectConstants.originalStart.y);
        // Reload distMap to mesh
        RtwMatrix distMapIn = ProjectConstants.mOriginalDistMap.Clone();
        Mesh mesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
        MISCLib.ApplyMatrixToMesh(distMapIn, ref mesh, true);

        // Connect all path segments together. Get rid of the overlapping points and construct final path.
        Vector2[] path = new Vector2[ProjectConstants.intFlightDuration * 30 + 1];
        int index = 0;
        for (int i = 0; i < ProjectConstants.AllPathSegments.Count; i++)
        {
            Array.Copy(ProjectConstants.AllPathSegments[i], 0, path, index, ProjectConstants.AllPathSegments[i].Length);
            index += ProjectConstants.AllPathSegments[i].Length;
            index--;
        }

        // reset score
        Camera.main.GetComponent<PlanPath>().totalCDF = 0f;
        Camera.main.GetComponent<IncreasingScoreEffect>().curScore = 0f;
        Camera.main.GetComponent<IncreasingScoreEffect>().initialScore = 0f;
        GameObject.Find("lblScore").GetComponent<UILabel>().text = "0";

        // Get diff map max
        Mesh diffMesh = GameObject.Find("PlaneDiff").GetComponent<MeshFilter>().mesh;
        Vector3[] diffVertices = diffMesh.vertices;
        float maxDiff = 0f;
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
        Mesh distMesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;			
        Camera.main.GetComponent<FlyPath>().maxDiff = maxDiff;
        Camera.main.GetComponent<FlyPath>().distVertices = distMesh.vertices;
        Camera.main.GetComponent<FlyPath>().distColors = distMesh.colors;				


        // Fly plane following path and then vacuum up as the plane goes and also up the score
        Camera.main.GetComponent<FlyPath>().path = path;
        Camera.main.GetComponent<FlyPath>().currentWayPoint = 1;
        Camera.main.GetComponent<FlyPath>().fly = true;
    }
}
