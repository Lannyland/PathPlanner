using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChatHandler : MonoBehaviour
{
    public UITextList textList;
    public bool fillWithDummyData = false;
	public string chatTextFile;

    UIInput mInput;
    bool mIgnoreNextEnter = false;
	private 

    // Load text file to memory.
    void Start()
    {
        mInput = GetComponent<UIInput>();

        if (fillWithDummyData && textList != null)
        {
            for (int i = 0; i < 30; ++i)
            {
                textList.Add(((i % 2 == 0) ? "[FFFFFF]" : "[AAAAAA]") +
                    "This is an example paragraph for the text list, testing line " + i + "[-]");
            }
        }
    }

    /// <summary>
    /// Pressing 'enter' should immediately give focus to the input field.
    /// </summary>

    void Update()
    {
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
        }
        mIgnoreNextEnter = true;
    }
}