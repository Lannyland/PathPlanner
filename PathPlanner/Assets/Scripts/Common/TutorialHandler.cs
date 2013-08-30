using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;

public class TutorialHandler : MonoBehaviour {

    public UITextList textList;
    public string tutorialTextFile;
    private float timeElapsed;

    private List<string> lstTutorialLines = new List<string>();
    private List<int> timeStamps = new List<int>();
    private List<string> lstLocations = new List<string>();
    private int counter = 0;

	// Use this for initialization
	void Start () {
        // Set textList to use
        textList = GameObject.Find("TutorialTextList").GetComponent<UITextList>();

        // Initialize timeElapased
        timeElapsed = 0;

        // Load tutorial text file to memory
        MISCLib.LoadTutorialFile(ProjectConstants.tutorialFiles[ProjectConstants.pageIndex], ref lstTutorialLines, ref timeStamps, ref lstLocations);
	}
	
	// Update is called once per frame
	void Update () {
        // Display tutorial text file based on time stamp
        timeElapsed = timeElapsed + Time.deltaTime;
        if (counter < timeStamps.Count)
        {
            if ((int)timeElapsed > timeStamps[counter])
            {
                textList.Clear();
                textList.Add(lstTutorialLines[counter]);
                counter++;
            }
        }
	}
}
