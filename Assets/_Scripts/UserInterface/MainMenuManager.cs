using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject shopMenuUI;
    [Header("Text Elements")]
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI bulletPrice;
    [SerializeField] TextMeshProUGUI speedPrice;
    [SerializeField] TextMeshProUGUI gunPrice;

    [Header("Upgrade Sprites")]
    [SerializeField] List<Image> bulletUpgradeSprites;
    [SerializeField] List<Image> movespeedUpgradeSprites;
    [SerializeField] List<Image> gunUpgradeSprites;

    private int bulletUpgradeLevel;
    private int movespeedUpgradeLevel;
    private int gunUpgradeLevel;

    private int bulletUpgradeOneCost = 50;
    private int bulletUpgradeTwoCost = 100;
    private int bulletUpgradeThreeCost = 250;
    private int bulletUpgradeFourCost = 500;
    private int bulletUpgradeFiveCost = 1000;

    private int speedUpgradeOneCost = 50;
    private int speedUpgradeTwoCost = 100;
    private int speedUpgradeThreeCost = 250;
    private int speedUpgradeFourCost = 500;
    private int speedUpgradeFiveCost = 1000;

    private int gunUpgradeOneCost = 500;
    private int gunUpgradeTwoCost = 1000;
    private int gunUpgradeThreeCost = 2500;
    private int gunUpgradeFourCost = 5000;
    private int gunUpgradeFiveCost = 10000;

    private void Start()
    {
        bulletUpgradeLevel = PlayerPrefs.GetInt("BulletUpgradeLevel");
        movespeedUpgradeLevel = PlayerPrefs.GetInt("MovespeedUpgradeLevel");
        gunUpgradeLevel = PlayerPrefs.GetInt("GunUpgradeLevel");
        SetUpgradeSprites();
    }
    private void Update()
    {
        SetMoneyTextElement();
        UpdateUpgradeSprites();
        SetUpgradePrices();
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnShopButtonClick()
    {
        mainMenuUI.SetActive(false);
        shopMenuUI.SetActive(true);
    }

    public void OnQuitButtonClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnBackButtonClick()
    {
        shopMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void OnResetStatsButtonClick()
    {
        PlayerPrefs.SetInt("MovespeedUpgradeLevel", 0);
        PlayerPrefs.SetInt("BulletUpgradeLevel", 0);
        PlayerPrefs.SetInt("GunUpgradeLevel", 0);
        PlayerPrefs.SetInt("Money", 0);
    }

    public void OnBulletUpgradeButtonClick()
    {
        int currentMoney = PlayerPrefs.GetInt("Money");
        int currentBulletUpgradeLevel = PlayerPrefs.GetInt("BulletUpgradeLevel");
        int upgradeCost;
        switch (currentBulletUpgradeLevel)
        {
            case 0:
                upgradeCost = bulletUpgradeOneCost;
                break;
            case 1:
                upgradeCost = bulletUpgradeTwoCost;
                break;
            case 2:
                upgradeCost = bulletUpgradeThreeCost;
                break;
            case 3:
                upgradeCost = bulletUpgradeFourCost;
                break;
            case 4:
                upgradeCost = bulletUpgradeFiveCost;
                break;
            default:
                upgradeCost = 0;
                break;
        }
        if (currentMoney >= upgradeCost)
        {
            PlayerPrefs.SetInt("Money", currentMoney - upgradeCost);
            PlayerPrefs.SetInt("BulletUpgradeLevel", currentBulletUpgradeLevel + 1);
            bulletUpgradeLevel = PlayerPrefs.GetInt("BulletUpgradeLevel");
        }
    }

    public void OnMovespeedUpgradeButtonClick()
    {
        int currentMoney = PlayerPrefs.GetInt("Money");
        int currentSpeedUpgradeLevel = PlayerPrefs.GetInt("MovespeedUpgradeLevel");
        int upgradeCost;
        switch (currentSpeedUpgradeLevel)
        {
            case 0:
                upgradeCost = speedUpgradeOneCost;
                break;
            case 1:
                upgradeCost = speedUpgradeTwoCost;
                break;
            case 2:
                upgradeCost = speedUpgradeThreeCost;
                break;
            case 3:
                upgradeCost = speedUpgradeFourCost;
                break;
            case 4:
                upgradeCost = speedUpgradeFiveCost;
                break;
            default:
                upgradeCost = 0;
                break;
        }
        if (currentMoney >= upgradeCost)
        {
            PlayerPrefs.SetInt("Money", currentMoney - upgradeCost);
            PlayerPrefs.SetInt("MovespeedUpgradeLevel", currentSpeedUpgradeLevel + 1);
            movespeedUpgradeLevel = PlayerPrefs.GetInt("MovespeedUpgradeLevel");
        }
    }

    public void OnGunUpgradeButtonClick()
    {
        int currentMoney = PlayerPrefs.GetInt("Money");
        int currentGunUpgradeLevel = PlayerPrefs.GetInt("GunUpgradeLevel");
        int upgradeCost;
        switch (currentGunUpgradeLevel)
        {
            case 0:
                upgradeCost = gunUpgradeOneCost;
                break;
            case 1:
                upgradeCost = gunUpgradeTwoCost;
                break;
            case 2:
                upgradeCost = gunUpgradeThreeCost;
                break;
            case 3:
                upgradeCost = gunUpgradeFourCost;
                break;
            case 4:
                upgradeCost = gunUpgradeFiveCost;
                break;
            default:
                upgradeCost = 0;
                break;
        }
        if (currentMoney >= upgradeCost)
        {
            PlayerPrefs.SetInt("Money", currentMoney - upgradeCost);
            PlayerPrefs.SetInt("GunUpgradeLevel", currentGunUpgradeLevel + 1);
            gunUpgradeLevel = PlayerPrefs.GetInt("GunUpgradeLevel");
        }
    }

    private void SetMoneyTextElement()
    {
        moneyText.text = "$" + PlayerPrefs.GetInt("Money").ToString();
    }

    private void SetUpgradeSprites()
    {
        foreach(var image in bulletUpgradeSprites)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        }
        foreach (var image in movespeedUpgradeSprites)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        }
        foreach (var image in gunUpgradeSprites)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        }
    }

    private void UpdateUpgradeSprites()
    {
        for(int i = 0; i < bulletUpgradeLevel; i++)
        {
            bulletUpgradeSprites[i].color = new Color(0.75f, 0.75f, 0.75f, 0.85f);
        }
        for (int i = 0; i < movespeedUpgradeLevel; i++)
        {
            movespeedUpgradeSprites[i].color = new Color(0.75f, 0.75f, 0.75f, 0.85f);
        }
        for (int i = 0; i < gunUpgradeLevel; i++)
        {
            gunUpgradeSprites[i].color = new Color(0.75f, 0.75f, 0.75f, 0.85f);
        }
    }

    private void SetUpgradePrices()
    {
        switch(bulletUpgradeLevel)
        {
            case 0:
                bulletPrice.text = "$" + bulletUpgradeOneCost.ToString();
                break;
            case 1:
                bulletPrice.text = "$" + bulletUpgradeTwoCost.ToString();
                break;
            case 2:
                bulletPrice.text = "$" + bulletUpgradeThreeCost.ToString();
                break;
            case 3:
                bulletPrice.text = "$" + bulletUpgradeFourCost.ToString();
                break;
            case 4:
                bulletPrice.text = "$" + bulletUpgradeFiveCost.ToString();
                break;
            default:
                bulletPrice.text = "MAX";
                break;
        }

        switch(movespeedUpgradeLevel)
        {
            case 0:
                speedPrice.text = "$" + speedUpgradeOneCost.ToString();
                break;
            case 1:
                speedPrice.text = "$" + speedUpgradeTwoCost.ToString();
                break;
            case 2:
                speedPrice.text = "$" + speedUpgradeThreeCost.ToString();
                break;
            case 3:
                speedPrice.text = "$" + speedUpgradeFourCost.ToString();
                break;
            case 4:
                speedPrice.text = "$" + speedUpgradeFiveCost.ToString();
                break;
            default:
                speedPrice.text = "MAX";
                break;
        }

        switch(gunUpgradeLevel)
        {
            case 0:
                gunPrice.text = "$" + gunUpgradeOneCost.ToString();
                break;
            case 1:
                gunPrice.text = "$" + gunUpgradeTwoCost.ToString();
                break;
            case 2:
                gunPrice.text = "$" + gunUpgradeThreeCost.ToString();
                break;
            case 3:
                gunPrice.text = "$" + gunUpgradeFourCost.ToString();
                break;
            case 4:
                gunPrice.text = "$" + gunUpgradeFiveCost.ToString();
                break;
            default:
                gunPrice.text = "MAX";
                break;
        }
    }
}
