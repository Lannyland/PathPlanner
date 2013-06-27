using UnityEngine;
using System.Collections;
using System;

public class IncreasingScoreEffect : MonoBehaviour {
	
	public float curScore = 0f;
	public float completionTime = 100f;
	
	private Time start;
	private float initialScore = 0;
	private int counter1 = 0;
	private int counter2 = 0;
	private float frames = 0;
	
	// Use this for initialization
	void Start () {		
	}	
	
	// Update is called once per frame
	void Update () {
		
		float scoreDiff = 0f;
		// Do this once
		counter2++;
		if(counter2==1)
		{
			scoreDiff = curScore - initialScore;		
		}

		// Debug.Log("deltaTime = " + Time.deltaTime + " scoreDiff = " + scoreDiff + " initialScor = " + initialScore);
		if(scoreDiff > 0f)
		{
			// start = Time.time;
			initialScore += scoreDiff / (completionTime * Time.deltaTime);
			counter1++;
			if(counter1 > 5)
			{
				counter1 = 0;
				GameObject.Find("AudioScoreSet").GetComponent<AudioSource>().Play();
			}
				
			if(initialScore > curScore)
			{
				initialScore = curScore;
			}
			this.gameObject.GetComponent<UILabel>().text = Convert.ToInt32(Mathf.Round (initialScore)).ToString();
		}
		else
		{
			counter2=0;
		}
	}
}
