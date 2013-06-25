using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;
using System;
using rtwmatrix;

public class StartUpChores : MonoBehaviour {

    public Plane plane;
	public WWW www;
	// public Vector3[] vertices;

    // Use this for initialization
    protected virtual void Start()
    {
        // Load terrain image file
        StartCoroutine(LoadMesh());
        
        // Scene Initialization
        SceneInit();

    }

    // Method to load terrain material into Projector
    IEnumerator LoadMesh()
    {
		// Sets material catelog TerrainImage texture to something so at least it's not empty.
		GameObject go = GameObject.Find("MaterialHolder");		
		go.GetComponent<MaterialCatelog>().catelog[4].mainTexture = go.GetComponent<MaterialCatelog>().catelog[5].mainTexture;
		
		// Deal with web vs. local files
		string fileLoc = ProjectConstants.strTerrainImage;
		if(fileLoc.Substring(0, 7) != "http://")
		{
			fileLoc = @"file:///" + fileLoc;				
		}
		
		// Load file
		www = new WWW(fileLoc);
	    yield return www;
	
	    // Stored loaded texture to a material in material catelog
		if(www.isDone && www !=null)
		{
			go.GetComponent<MaterialCatelog>().catelog[4].mainTexture = www.texture;
		}
		else
		{
			Debug.Log("Error loading file.");
		}
    }

    // Initialize all assets parameters to set scene ready
    private void SceneInit()
    {
        LoadMaps();
    }

    private void LoadMaps()
    {
        // First load dist map to mesh and show on screen
        RtwMatrix distMapIn = MISCLib.LoadMap(ProjectConstants.strDistFileLoad);
        MISCLib.ScaleImageValues(ref distMapIn, 4.0f);
        distMapIn = MISCLib.FlipTopBottom(distMapIn);

        // Store a copy at global data store
        ProjectConstants.mOriginalDistMap = distMapIn.Clone();
        ProjectConstants.mDistMapCurStepUndo = distMapIn.Clone();
        ProjectConstants.mDistMapCurStepWorking = distMapIn;

        // Show dist map on screen
        Mesh mesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
        MISCLib.ApplyMatrixToMesh(distMapIn, ref mesh, true);

        // If use diff map, then load that to memory
        if (ProjectConstants.boolUseDiffMap)
        {
            RtwMatrix diffMapIn = MISCLib.LoadMap(ProjectConstants.strDiffFileLoad);
            diffMapIn = MISCLib.FlipTopBottom(diffMapIn);
            ProjectConstants.mDiffMap = diffMapIn;
	        Mesh mesh2 = GameObject.Find("PlaneDiff").GetComponent<MeshFilter>().mesh;
			MISCLib.ApplyMatrixToMesh(diffMapIn, ref mesh2, false);
        }
    }


	// Update is called once per frame
	void Update () {
	}
}
