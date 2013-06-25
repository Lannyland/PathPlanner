using UnityEngine;
using System.Collections;

public class RotateUFO : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iTween.RotateBy(gameObject, iTween.Hash("y", 1, "easeType", "linear", "loopType", "loop", "time", 4));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
