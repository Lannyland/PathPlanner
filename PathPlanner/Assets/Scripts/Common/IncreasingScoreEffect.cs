using UnityEngine;
using System.Collections;
using System;

public class IncreasingScoreEffect : MonoBehaviour {
	
	public float curScore = 0f;
	public float completionTime = 2;
    public float initialScore = 0;	
	
    private Time start;
	private int counter = 0;
	private float frames = 0;
    private bool firstTime = true;
    private float scoreDiff = 0;  
	
	// Use this for initialization
	void Start () {		
	}	
	
	// Update is called once per frame
	void Update () {
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
			if(counter > 5)
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
}
