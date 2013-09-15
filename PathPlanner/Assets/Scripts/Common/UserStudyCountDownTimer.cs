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
    private float flashLength = 0f;
    private float sign = 1f;

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
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.N))
        {
            GameObject.Find("btnNext").GetComponent<UIButton>().isEnabled = true;
        }


        timeLeft = timeLeft - Time.deltaTime;
        if ((int)timeLeft < 0)
        {
            GameOver();
        }
        else
        {
            timeRemain.text = ((int)timeLeft).ToString();
        }

        // Flash time left in last 30 seconds
        // Every half second flash the box
        if (timeLeft < 30)
        {
            timeRemain.color = Color.red;
            if (sign > 0.9f)
            {
                if (flashLength < 0.5f)
                {
                    flashLength += Time.deltaTime;
                    timeRemain.text = "";
                }
                else
                {
                    sign = 0f;
                }
            }
            else
            {
                if (flashLength > 0)
                {
                    flashLength -= Time.deltaTime;
                    timeRemain.text = ((int)timeLeft).ToString();
                }
                else
                {
                    sign = 1f;
                }
            }
        }

    }

    // Something to do with time is up
    void GameOver()
    {
        // Find score to display
        if (!ProjectConstants.boolFlyPath)
        {
            ProjectConstants.score = GameObject.Find("lblScore").GetComponent<UILabel>().text;
        }
        // Load Message Box
        UILabel label = GameObject.Find("lblMessage").GetComponent<UILabel>();
        UISlicedSprite sp = GameObject.Find("spMessage").GetComponent<UISlicedSprite>();
        label.text = "[FFFFFF]Time is up. Please stop!\n\nYour score is: [00FF3E]" + ProjectConstants.score + "\n\n[FFFFFF]Click Next to continue.";
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

        // Enable Next Button
        GameObject.Find("btnNext").GetComponent<UIButton>().isEnabled = true;



    }
}
