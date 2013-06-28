using UnityEngine;
using System.Collections;
using System;

public class IncreasingScoreEffect : MonoBehaviour {
	
	public enum EffectStyle {RapidFire, MileStone}
	
	public EffectStyle style = EffectStyle.RapidFire;
	public float curScore = 0f;
	public float completionTime = 1;
	public float initialScore = 0;	
	public int intensity = 5;
	
    private Time start;
	private int counter = 0;
	private float frames = 0;
    private bool firstTime = true;
    private float scoreDiff = 0;  
	private int ringCount = 0;
	
	// Use this for initialization
	void Start () {		
	}	

	// Update is called once per frame
	void Update () {
		if(style == EffectStyle.RapidFire)
		{
			RapidFireStyle();			
		}
		else if(style == EffectStyle.MileStone)
		{
			MileStoneStyle();
		}
	}
	
	void RapidFireStyle()
	{
		float curScoreDiff = 0f;
		// Do this once
		if(firstTime && Mathf.Abs(curScore - initialScore) > 1f)
		{
			scoreDiff = Mathf.Abs(curScore - initialScore);
		          // Debug.Log("scoreDiff = " + scoreDiff);
		          firstTime = false;
		}
      	curScoreDiff = Mathf.Abs(curScore - initialScore);
		
		// Debug.Log("deltaTime = " + Time.deltaTime + " scoreDiff = " + scoreDiff + " initialScor = " + initialScore);
      	if (curScoreDiff > 0.1f)
		{
	    	// Debug.Log("I am not done! scoreDiff = " + scoreDiff + " curScoreDiff = " + curScoreDiff);
			// start = Time.time;
			initialScore += scoreDiff / completionTime * Time.deltaTime;
			counter++;
			if(initialScore > intensity)
			{
				counter = 0;
				GameObject.Find("AudioScoreSet").GetComponent<AudioSource>().Play();
			}
				
	      	if(initialScore > curScore)
	      	{
	        	initialScore = curScore;
	      	}
			GameObject.Find("lblScore").GetComponent<UILabel>().text = Convert.ToInt32(Mathf.Round (initialScore)).ToString();
		    // GameObject.Find("lblScore").GetComponent<UILabel>().text = initialScore.ToString();
		}
		else
		{
			firstTime = true;
		}
	}
	
	void MileStoneStyle()
	{
		if(firstTime)
		{
			ringCount = (int)(curScore - (int)(initialScore/100)*100)/100;
			initialScore = curScore;		
			GameObject.Find("lblScore").GetComponent<UILabel>().text = Convert.ToInt32(Mathf.Round (initialScore)).ToString();			
			firstTime = false;
		}
		
		if(ringCount>0)
		{			
			GameObject.Find("AudioScoreSet").GetComponent<AudioSource>().Play();
			ringCount--;
		}
		else
		{
			firstTime = true;
		}
	}
		
}
