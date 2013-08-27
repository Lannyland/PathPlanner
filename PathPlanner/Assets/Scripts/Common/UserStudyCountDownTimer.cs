using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class UserStudyCountDownTimer : MonoBehaviour
{

    public int timeTotal = 300;
    private float timeLeft;
    UILabel timeRemain;

    // Use this for initialization
    void Start()
    {
        timeTotal = ProjectConstants.durations[ProjectConstants.pageIndex] * 60;
        timeLeft = timeTotal;
        timeRemain = GameObject.Find("lblTime").GetComponent<UILabel>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft = timeLeft - Time.deltaTime;
        if ((int)timeLeft < 0)
        {
            GameOver();
        }
        else
        {
            timeRemain.text = ((int)timeLeft).ToString();
        }
    }

    // Something to do with time is up
    void GameOver()
    {
        // Load Message Box
        UILabel label = GameObject.Find("lblMessage").GetComponent<UILabel>();
        UISlicedSprite sp = GameObject.Find("spMessage").GetComponent<UISlicedSprite>();
        label.transform.position = Vector3.zero;
        sp.transform.position = Vector3.zero;

        // Stop UAV
        FlyManual fm = GameObject.Find("UAV").GetComponent<FlyManual>();
        if (fm != null)
        {
            fm.fly = false;
        }
        FlyPattern fp = GameObject.Find("UAV").GetComponent<FlyPattern>();
        if (fp != null)
        {
            fp.fly = false;
        }
    }
}
