using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;

public class ChatHandler : MonoBehaviour
{
    public UITextList textList;
	public string chatTextFile;
	private float timeElapsed;

    UIInput mInput;
    bool mIgnoreNextEnter = false;
	private List<string> chatLines = new List<string>();
	private List<int> timeStamps = new List<int>();
	private int counter = 0;

    void Start()
    {		
		mInput = GameObject.Find ("Input").GetComponent<UIInput>();
		
		// Initialize timeElapased
		timeElapsed = 0;
		
        // Load chat text file to memory
		MISCLib.LoadChatFile(ProjectConstants.chatFiles[ProjectConstants.pageIndex], ref chatLines, ref timeStamps);
    }

    /// <summary>
    /// Pressing 'enter' should immediately give focus to the input field.
    /// </summary>

    void Update()
    {
		// Display chat text file based on time stamp
		timeElapsed = timeElapsed + Time.deltaTime;
        if(counter<timeStamps.Count)
		{
			if ((int)timeElapsed > timeStamps[counter])
	        {
	            textList.Add(chatLines[counter]);
				counter++;
	        }
		}
		
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp("enter"))
        {
			// Give focus to input field
            if (!mIgnoreNextEnter && !mInput.selected)
            {
                mInput.selected = true;
            }
            mIgnoreNextEnter = false;
        }
		
		// Deal with WASD inputs
		if(mInput.selected)
		{
			// Disable orbit
			Camera.main.GetComponent<Orbit>().enabled = false;
		}
		else
		{
			// Enable orbit
			Camera.main.GetComponent<Orbit>().enabled = true;
		}
		
		// Deal with user typing too much
		int lineCount = 1;
		if(mInput.text.Length > 40)
		{
			lineCount = Convert.ToInt16(Mathf.Ceil(mInput.text.Length/42f));
		}
		// Change background size
		Vector3 newScale = this.gameObject.GetComponentInChildren<UISlicedSprite>().transform.localScale;
		newScale.y = 30*lineCount;		
		this.gameObject.GetComponentInChildren<UISlicedSprite>().transform.localScale = newScale;
    }

    /// <summary>
    /// Submit notification is sent by UIInput when 'enter' is pressed or iOS/Android keyboard finalizes input.
    /// </summary>

    void OnSubmit()
    {
        if (textList != null)
        {
            // It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
            string text = NGUITools.StripSymbols(mInput.text);

            if (!string.IsNullOrEmpty(text))
            {
                textList.Add(text);
                mInput.text = "";
                mInput.selected = false;
            }

            // Save user entry with time stamp
            string timeLeft = GameObject.Find("lblTime").GetComponent<UILabel>().text;
            int timeElapsed = ProjectConstants.durations[ProjectConstants.pageIndex] * 60 - Convert.ToInt16(timeLeft);
            string minute = (timeElapsed / 60).ToString();
            string second = (timeElapsed % 60).ToString();
            string timeStamp = minute + ":" + second;
            ProjectConstants.replies.Add(timeStamp + "|" + text);
        }
        mIgnoreNextEnter = true;


    }
}