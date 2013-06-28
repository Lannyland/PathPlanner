using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;
using rtwmatrix;

public class FlyPath : MonoBehaviour {

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
        
        // Fly plane following path and then vacuum up as the plane goes and also up the score


	}
}
