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
    private int maxUpgradeLevel = 5;

    private int upgradeOneCost = 50;
    private int upgradeTwoCost = 100;
    private int upgradeThreeCost = 250;
    private int upgradeFourCost = 500;
    private int upgradeFiveCost = 1000;

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

    public void OnAddMoneyButtonClick()
    {
        int money = PlayerPrefs.GetInt("Money");
        PlayerPrefs.SetInt("Money", money + 1000);
    }

    public void OnResetStatsButtonClick()
    {
        PlayerPrefs.SetInt("BulletUpgradeLevel", 0);
        PlayerPrefs.SetInt("GunUpgradeLevel", 0);
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("MovespeedUpgradeLevel", 0);

        SetUpgradeSprites();
        UpdateUpgradeSprites();
    }

    public void OnBulletUpgradeButtonClick()
    {
        if(bulletUpgradeLevel >= maxUpgradeLevel) return;
        
        int currentMoney = PlayerPrefs.GetInt("Money");
        int currentBulletUpgradeLevel = PlayerPrefs.GetInt("BulletUpgradeLevel");
        int upgradeCost;
        switch (currentBulletUpgradeLevel)
        {
            case 0:
                upgradeCost = upgradeOneCost;
                break;
            case 1:
                upgradeCost = upgradeTwoCost;
                break;
            case 2:
                upgradeCost = upgradeThreeCost;
                break;
            case 3:
                upgradeCost = upgradeFourCost;
                break;
            case 4:
                upgradeCost = upgradeFiveCost;
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
        if (movespeedUpgradeLevel >= maxUpgradeLevel) return;

        int currentMoney = PlayerPrefs.GetInt("Money");
        int currentSpeedUpgradeLevel = PlayerPrefs.GetInt("MovespeedUpgradeLevel");
        int upgradeCost;
        switch (currentSpeedUpgradeLevel)
        {
            case 0:
                upgradeCost = upgradeOneCost;
                break;
            case 1:
                upgradeCost = upgradeTwoCost;
                break;
            case 2:
                upgradeCost = upgradeThreeCost;
                break;
            case 3:
                upgradeCost = upgradeFourCost;
                break;
            case 4:
                upgradeCost = upgradeFiveCost;
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
        if (gunUpgradeLevel >= maxUpgradeLevel) return;

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
                bulletPrice.text = "$" + upgradeOneCost.ToString();
                break;
            case 1:
                bulletPrice.text = "$" + upgradeTwoCost.ToString();
                break;
            case 2:
                bulletPrice.text = "$" + upgradeThreeCost.ToString();
                break;
            case 3:
                bulletPrice.text = "$" + upgradeFourCost.ToString();
                break;
            case 4:
                bulletPrice.text = "$" + upgradeFiveCost.ToString();
                break;
            default:
                bulletPrice.text = "MAX";
                break;
        }

        switch(movespeedUpgradeLevel)
        {
            case 0:
                speedPrice.text = "$" + upgradeOneCost.ToString();
                break;
            case 1:
                speedPrice.text = "$" + upgradeTwoCost.ToString();
                break;
            case 2:
                speedPrice.text = "$" + upgradeThreeCost.ToString();
                break;
            case 3:
                speedPrice.text = "$" + upgradeFourCost.ToString();
                break;
            case 4:
                speedPrice.text = "$" + upgradeFiveCost.ToString();
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
