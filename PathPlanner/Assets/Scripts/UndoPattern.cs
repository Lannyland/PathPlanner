using UnityEngine;
using System.Collections;

public class UndoPattern : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        GameObject.Find("UAV").GetComponent<FlyPattern>().Undo();
    }
}
