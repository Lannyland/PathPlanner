using UnityEngine;
using System.Collections;

public class ChatHandler2 : MonoBehaviour {
	
	private UIInput mInput;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		mInput = GameObject.Find ("Input").GetComponent<UIInput>();
		mInput.selected = true;
	}
}
