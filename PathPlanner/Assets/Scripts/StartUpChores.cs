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
    void Start()
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

        // The following code should be moved to scene specific start up chore scripts
        //Camera.main.transform.GetComponent<DrawFreeLine>().enabled = false;
        //UISlider slider = GameObject.Find("Slider").GetComponent<UISlider>();
        //slider.sliderValue = 0.1f;
        //slider.GetComponent<BrushSize>().OnSliderChange(slider.sliderValue);
    }

    private void LoadMaps()
    {
        // First load dist map to mesh and show on screen
        RtwMatrix distMapIn = MISCLib.LoadMap(ProjectConstants.strDistFileLoad);
        MISCLib.ScaleImageValues(ref distMapIn, 4.0f);
        Vector3[] vertices = MISCLib.MatrixToArray(Assets.Scripts.Common.MISCLib.FlipTopBottom(distMapIn));
        Mesh mesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.colors = MISCLib.ApplyDistColorMap(vertices);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // If use diff map, then load that to memory
        if (ProjectConstants.boolUseDiffMap)
        {
            RtwMatrix diffMapIn = MISCLib.LoadMap(ProjectConstants.strDistFileLoad);
            // Lanny, start here next time.
        }
    }


	// Update is called once per frame
	void Update () {
	}
}
