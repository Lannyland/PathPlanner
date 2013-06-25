using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class SetPlenty : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if (this.gameObject.GetComponent<UICheckbox>().isChecked)
        {
            ProjectConstants.boolPlenty = true;
        }
        else
        {
            ProjectConstants.boolPlenty = false;
        }
    }
}
