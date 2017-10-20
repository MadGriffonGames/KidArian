﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementBox : MonoBehaviour {


    [SerializeField]
    public string achievementName;

    [SerializeField]
    GameObject getBtn;
    [SerializeField]
    GameObject getBtn1;
    [SerializeField]
    GameObject getBtn2;

    [SerializeField]
    string[] descriptionText;

    [SerializeField]
    GameObject description;


    [SerializeField]
    GameObject text;
    [SerializeField]
    GameObject levelText;


    [SerializeField]
    GameObject inProgress;
    [SerializeField]
    GameObject doneImg;


    [SerializeField]
    GameObject gold;
    [SerializeField]
    GameObject silver;
    [SerializeField]
    GameObject bronze;

    [SerializeField]
    GameObject rewardFade;
    [SerializeField]
    GameObject loot;

    [SerializeField]
    Sprite coins;

    [SerializeField]
    Sprite crystals;

    [SerializeField]
    RectTransform status;

    [SerializeField]
    GameObject fadeButton;


    [SerializeField]
    GameObject lootVolume;

    string recordName;


    int gotReward;
    const string btn = "btn";
    const string medal = "medal";
    


	// Use this for initialization
	void Start () {

        recordName = text.GetComponent<Text>().text;

        PlayerPrefs.SetInt(achievementName + medal, 0);
        PlayerPrefs.SetInt(achievementName + btn, 0);

        if (!PlayerPrefs.HasKey(achievementName + btn))
            PlayerPrefs.SetInt(achievementName + btn, 0);
        GetInfo();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void GetInfo()
    {
        int weight = PlayerPrefs.GetInt(achievementName + "weight");
        int currentValue = PlayerPrefs.GetInt(achievementName);
        int targetValue0 = PlayerPrefs.GetInt(achievementName + "targetValue0");
        int targetValue1 = PlayerPrefs.GetInt(achievementName + "targetValue1");
        int targetValue2 = PlayerPrefs.GetInt(achievementName + "targetValue2");

        if (PlayerPrefs.GetInt(achievementName + btn) == 0)         // Get button has never been pressed
        {
            Debug.Log(achievementName);
            Debug.Log("never pressed");
            description.gameObject.GetComponent<Text>().text = descriptionText[0];
            text.GetComponent<Text>().text += " " + PlayerPrefs.GetInt(achievementName) + "/" + PlayerPrefs.GetInt(achievementName + "targetValue0");
            UpdateStatus(PlayerPrefs.GetInt(achievementName), PlayerPrefs.GetInt(achievementName + "targetValue0"));
        }

        if (PlayerPrefs.GetInt(achievementName + btn) == 1)         // Get button has been pressed once
        {
            description.gameObject.GetComponent<Text>().text = descriptionText[1];
            text.GetComponent<Text>().text += " " + PlayerPrefs.GetInt(achievementName) + "/" + PlayerPrefs.GetInt(achievementName + "targetValue1");
            UpdateStatus(PlayerPrefs.GetInt(achievementName), PlayerPrefs.GetInt(achievementName + "targetValue1"));
            bronze.SetActive(true);
        }

        if (PlayerPrefs.GetInt(achievementName + btn) == 2)         // Get butten has been pressed twice
        {
            Debug.Log(achievementName);
            Debug.Log("twice pressed");
            description.gameObject.GetComponent<Text>().text = descriptionText[2];
            text.GetComponent<Text>().text += " " + PlayerPrefs.GetInt(achievementName) + "/" + PlayerPrefs.GetInt(achievementName + "targetValue2");
            UpdateStatus(PlayerPrefs.GetInt(achievementName), PlayerPrefs.GetInt(achievementName + "targetValue2"));
            bronze.SetActive(false);
            silver.SetActive(true);
        }

        if (PlayerPrefs.GetInt(achievementName + btn) == 3)
        {
            bronze.SetActive(false);
            silver.SetActive(false);
            gold.SetActive(true);
            description.gameObject.GetComponent<Text>().text = descriptionText[2];
            doneImg.SetActive(true);
        }


        //    if (PlayerPrefs.GetInt(achievementName + medal) == 1)   // 
        //        bronze.SetActive(true);

        //    if (PlayerPrefs.GetInt(achievementName + medal) == 2)
        //    {
        //        bronze.SetActive(false);
        //        silver.SetActive(true);
        //    }

        //if (PlayerPrefs.GetInt(achievementName + medal) == 3)
        //{
        //    bronze.SetActive(false);
        //    silver.SetActive(false);
        //    gold.SetActive(true);
        //}

        //if (currentValue < targetValue0)
        //{
        //    doneImg.gameObject.SetActive(true);
        //}

        //if (currentValue < targetValue1 && PlayerPrefs.GetInt(achievementName + btn) == 1)
        //{
        //    doneImg.gameObject.SetActive(true);
        //}

        //if (currentValue < targetValue2 && PlayerPrefs.GetInt(achievementName + btn) == 2)
        //{
        //    doneImg.gameObject.SetActive(true);
        //}


        //if (currentValue >= targetValue2 && PlayerPrefs.GetInt(achievementName + btn) == 3)
        //{
        //    doneImg.gameObject.SetActive(true);
        //}

        //if (currentValue < targetValue2)
        //    doneImg.SetActive(true);
        //else
        //    doneImg.SetActive(false);

        if (currentValue < targetValue0)
        {
            inProgress.gameObject.SetActive(true);
        }

        if (currentValue < targetValue1 && currentValue >= targetValue0)
        {
            if (achievementName == "Treasure Hunter")
            {
                Debug.Log("inProgress");
            }
            inProgress.gameObject.SetActive(true);
        }

        if (currentValue < targetValue2 && currentValue >= targetValue1)
        {
            inProgress.gameObject.SetActive(true);
        }


            if (currentValue >= targetValue0 && PlayerPrefs.GetInt(achievementName + btn) == 0)
            {
                inProgress.gameObject.SetActive(false);
                getBtn.gameObject.SetActive(true);
                getBtn.gameObject.GetComponent<Button>().enabled = true;
            }

            if (currentValue >= targetValue1 && PlayerPrefs.GetInt(achievementName + btn) == 1)
            {
                inProgress.gameObject.SetActive(false);
                getBtn1.gameObject.SetActive(true);
                getBtn1.gameObject.GetComponent<Button>().enabled = true;
            }

            if (currentValue >= targetValue2 && PlayerPrefs.GetInt(achievementName + btn) == 2)
            {
                inProgress.gameObject.SetActive(false);
                getBtn2.gameObject.SetActive(true);
                getBtn2.gameObject.GetComponent<Button>().enabled = true;
            }
        }



    public void GetFirstReward()
    {

        int currentValue = PlayerPrefs.GetInt(achievementName);
        int targetValue1 = PlayerPrefs.GetInt(achievementName + "targetValue1");
        UpdateValue(1);
        PlayerPrefs.SetInt(achievementName + btn, 1);
        description.gameObject.GetComponent<Text>().text = descriptionText[1];
        UpdateStatus(currentValue, targetValue1);
        getBtn.gameObject.SetActive(false);
        rewardFade.SetActive(true);
        if (PlayerPrefs.GetString(achievementName + "rewardType0") == "Coins")
        {
            StartCoroutine(ShowCoinLoot(0));
        }

        if (currentValue >= targetValue1)
        {

            inProgress.SetActive(false);
            getBtn1.gameObject.SetActive(true);
        }
        else
            inProgress.SetActive(true);
        bronze.gameObject.SetActive(true);

    }

    public void GetSecondReward()
    {
        int currentValue = PlayerPrefs.GetInt(achievementName);
        int targetValue2 = PlayerPrefs.GetInt(achievementName + "targetValue2");
        UpdateValue(2);
        UpdateStatus(currentValue, targetValue2);
        PlayerPrefs.SetInt(achievementName + btn, 2);
        description.gameObject.GetComponent<Text>().text = descriptionText[2];
        getBtn1.gameObject.SetActive(false);
        if (PlayerPrefs.GetString(achievementName + "rewardType1") == "Coins")
        {
            StartCoroutine(ShowCoinLoot(1));
        }

        if (currentValue < targetValue2)
        {
            inProgress.SetActive(true);
        }
        else
        {
            inProgress.SetActive(false);
            getBtn2.SetActive(true);
        }
        bronze.SetActive(false);
        silver.SetActive(true);
    }

    public void GetThirdReward()
    {
        Debug.Log(3);
        PlayerPrefs.SetInt(achievementName + btn, 3);
        getBtn2.gameObject.SetActive(false);
        doneImg.gameObject.SetActive(true);
        if (PlayerPrefs.GetString(achievementName + "rewardType2") == "Coins")
        {
            StartCoroutine(ShowCoinLoot(2));
        }

        UpdateStatus(PlayerPrefs.GetInt(achievementName), PlayerPrefs.GetInt(achievementName + "targetValue2"));
        description.gameObject.GetComponent<Text>().text = descriptionText[2];
        bronze.SetActive(false);
        silver.SetActive(false);
        gold.SetActive(true);
    }

    public void UpdateValue(int level)
    {
        text.GetComponent<Text>().text = recordName;
        text.GetComponent<Text>().text += " " + PlayerPrefs.GetInt(achievementName) + "/" + PlayerPrefs.GetInt(achievementName + "targetValue" + level.ToString());
    }

    void UpdateStatus(int currentValue, int currenTargetValue)
    {
        float newX;

        if (currentValue >= currenTargetValue)
            newX = 1;
        else
        {
            newX = (float)currentValue / (float)currenTargetValue;
        }
        status.localScale = new Vector3(newX, status.localScale.y, status.localScale.z);
    }

    public void FadeOut()
    {
        Debug.Log("Fade out");
        fadeButton.SetActive(false);
        rewardFade.SetActive(false);
        loot.SetActive(false);
    }

    IEnumerator ShowCoinLoot(int level)
    {
        Debug.Log(PlayerPrefs.GetInt(achievementName + level.ToString()));
        lootVolume.GetComponent<Text>().text = PlayerPrefs.GetInt(achievementName + level.ToString()).ToString();
        rewardFade.gameObject.SetActive(true);
        loot.gameObject.GetComponent<Image>().sprite = coins;
        loot.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        fadeButton.SetActive(true);
    }
}