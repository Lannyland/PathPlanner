using UnityEngine;
using System.Collections;

public class StartPatternFlight : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnClick()
	{
        // Make UAV not movable
        GameObject.Find("UAV").GetComponent<FlyPattern>().fly = true;
		GameObject.Find("UAV").GetComponent<MoveUFO>().movable = false;
        this.gameObject.GetComponent<UIButton>().isEnabled = false;
        GameObject.Find("UAV").GetComponent<FlyPattern>().path.Clear();
        GameObject.Find("UAV").GetComponent<FlyPathPattern>().path.Clear();
	}		
}
