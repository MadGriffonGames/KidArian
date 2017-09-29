﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChestUI : RewardedChest
{
    [SerializeField]
    Image lightCircle;
    [SerializeField]
    GameObject chest;
    [SerializeField]
    Sprite chestOpen;
    [SerializeField]
    Sprite chestClose;
    [SerializeField]
    GameObject activateButton;
    [SerializeField]
    GameObject chestFade;
    [SerializeField]
    Text text;

    Image chestImage;
    bool isSpined;
    Quaternion rotationVector;
    Animator lootAnimator;
    bool isOpened;
    bool isStarsCollected;

    private void Start()
    {
        base.Start();

        chestImage = chest.GetComponent<Image>();
        lootAnimator = loot.gameObject.GetComponent<Animator>();

        isOpened = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_chest") > 0;

        isStarsCollected = (Player.Instance.stars >= 3) || (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_collects") >= 3);

        if (isOpened)
        {
            chestImage.sprite = chestOpen;
            activateButton.SetActive(false);
        }
        else
        {
            chestImage.sprite = chestClose;
            if (isStarsCollected)
            {
                lightCircle.gameObject.SetActive(true);
                isSpined = true;
                activateButton.SetActive(true);
            }
            else
            {
                lightCircle.gameObject.SetActive(false);
                isSpined = false;
                chestImage.color = new Color(chestImage.color.r, chestImage.color.g, chestImage.color.b, 0.55f);
                activateButton.SetActive(false);
            }
        }

        
    }

    private void Update()
    {
        if (isSpined)
        {
            rotationVector = lightCircle.rectTransform.rotation;
            rotationVector.z -= 0.0005f;
            lightCircle.rectTransform.rotation = rotationVector;
        }

        if (AdsManager.Instance.isRewardVideoWatched)
        {
            AdsManager.Instance.isRewardVideoWatched = false;
            if (!PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_chest"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_chest", 1);
            }
            GiveLoot();

            AppMetrica.Instance.ReportEvent("#CHEST 3Stars chest activate");
        }
    }

    public void OpenChestButton()
    {
        AdsManager.Instance.ShowRewardedVideo();
    }

    public void GiveLoot()
    {
        Randomize();
        chestImage.sprite = chestOpen;
        chestFade.SetActive(true);
        loot.gameObject.SetActive(true);
        activateButton.SetActive(false);
    }

    public void CollectLoot()
    {
        chestFade.SetActive(false);
        loot.gameObject.SetActive(false);
    }
}
