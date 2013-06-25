using UnityEngine;
using System.Collections;

public class RotateSample : MonoBehaviour
{	
	void Start(){
		iTween.RotateBy(gameObject, iTween.Hash("y", .25, "easeType", "linear", "loopType", "loop", "delay", .0));
	}
}

