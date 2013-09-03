using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;
using Vectrosity;

public class TutorialHandler : MonoBehaviour {

    public UITextList textList;
    public string tutorialTextFile;
    private float timeElapsed;

    private List<string> lstTutorialLines = new List<string>();
    private List<int> timeStamps = new List<int>();
    private List<string> lstLocations = new List<string>();
    private int counter = 0;

    private VectorLine myRec;
	private Material lineMaterial;
	
	// Use this for initialization
	void Start () {
        // Get rectangle ready
		myRec = new VectorLine("Rec", new Vector2[8], lineMaterial, 4.0f);
		
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
        if(lstTutorialLines.Count < 1)
		{
			return;
		}
		if (counter < timeStamps.Count)
        {
            if ((int)timeElapsed > timeStamps[counter])
            {
                // Display tutorial text
                textList.Clear();
                textList.Add(lstTutorialLines[counter]);

                // Draw rectangle
                string[] strings = lstLocations[counter].Split(',');
                List<int> ints = new List<int>();
                ints.Add(Convert.ToInt16(strings[0]));
                ints.Add(Convert.ToInt16(strings[1]));
                ints.Add(Convert.ToInt16(strings[2]));
                ints.Add(Convert.ToInt16(strings[3]));
				myRec.MakeRect(new Rect(ints[0], ints[1], ints[2], ints[3]));
                myRec.Draw();

                counter++;
            }
        }
	}
}
