﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndUI : MonoBehaviour
{
    int coinsCollected;
    [SerializeField]
    Text coinsText;
    [SerializeField]
    GameObject[] starsFull;
    [SerializeField]
    GameObject fade;
    [SerializeField]
    GameObject controlsUI;
    [SerializeField]
    GameObject pauseUI;
    [SerializeField]
    ChestUI chest;

    void Start ()
    {
        controlsUI.SetActive(false);
        pauseUI.SetActive(false);

        SoundManager.PlayMusic("victory sound", false);
        fade.SetActive(true);

        coinsCollected = GameManager.lvlCollectedCoins;
        coinsText.text = "" + coinsCollected;
        StartCoroutine(ShowStars(Player.Instance.stars));
    }

    private void Update()
    {
        if (AdsManager.Instance.isInterstitialClosed)
        {
            AdsManager.Instance.isInterstitialClosed = false;
            SceneManager.LoadScene("Loading");
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Continue()
    {
        if (GameManager.currentLvl == "Level20")
        {
            GameManager.nextLevelName = "MainMenu";
        }
        SceneManager.LoadScene("Loading");
    }

    public void Restart()
    {
        GameManager.nextLevelName = SceneManager.GetActiveScene().name;

#if UNITY_EDITOR
        AdsManager.Instance.isInterstitialClosed = true;
        AdsManager.Instance.fromShowfunction = true;

#elif UNITY_ANDROID
        AdsManager.Instance.ShowAdsAtLevelEnd();//check if ad was showed in update()

#elif UNITY_IOS
        AdsManager.Instance.ShowAdsAtLevelEnd();//check if ad was showed in update()

#endif


        GameManager.collectedCoins = Player.Instance.startCoinCount;
        GameManager.nextLevelName = SceneManager.GetActiveScene().name;
    }

    public IEnumerator ShowStars(int value)
    {
        for (int i = 0; i < value; i++)
        {
            starsFull[i].SetActive(true);
            yield return new WaitForSeconds(1f);
        }
    }
}
