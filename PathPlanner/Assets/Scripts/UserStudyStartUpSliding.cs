using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class UserStudyStartUpSliding : UserStudyStartUpChores
{

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (!Assets.Scripts.ProjectConstants.boolAnyEndPoint)
        {
            // No end point at all, so disable that button.
            GameObject.Find("btnSetEndPoint").GetComponent<UIButton>().enabled = false;
        }

        // Only allow fly path when path is completed.
        UIButton b4 = GameObject.Find("btnFly").GetComponent<UIButton>();
        b4.isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
