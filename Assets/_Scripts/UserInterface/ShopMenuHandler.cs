using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuHandler : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] TextMeshProUGUI _bulletPrice;
    [SerializeField] TextMeshProUGUI _speedPrice;
    [SerializeField] TextMeshProUGUI _gunPrice;

    [Header("Upgrade Sprites")]
    [SerializeField] List<Image> _bulletUpgradeSprites;
    [SerializeField] List<Image> _movespeedUpgradeSprites;
    [SerializeField] List<Image> _gunUpgradeSprites;

    private int _bulletUpgradeLevel;
    private int _movespeedUpgradeLevel;
    private int _gunUpgradeLevel;
    private int _maxUpgradeLevel = 5;

    private int _upgradeOneCost = 50;
    private int _upgradeTwoCost = 100;
    private int _upgradeThreeCost = 250;
    private int _upgradeFourCost = 500;
    private int _upgradeFiveCost = 1000;

    private int _gunUpgradeOneCost = 500;
    private int _gunUpgradeTwoCost = 1000;
    private int _gunUpgradeThreeCost = 2500;
    private int _gunUpgradeFourCost = 5000;
    private int _gunUpgradeFiveCost = 10000;

    private void Start()
    {
        _bulletUpgradeLevel = PlayerPrefs.GetInt(Constants.BULLET_UPGRADE_LEVEL);
        _movespeedUpgradeLevel = PlayerPrefs.GetInt(Constants.MOVESPEED_UPGRADE_LEVEL);
        _gunUpgradeLevel = PlayerPrefs.GetInt(Constants.GUN_UPGRADE_LEVEL);

        UpdateUIElements();
    }

    private void OnEnable()
    {
        UpdateUIElements();
    }

    private void UpdateUIElements()
    {
        SetUpgradePrices();
        SetUpgradeSprites();
        SetBoughtUpgradeSprites();
        SetMoneyTextElement();
    }

    private void SetMoneyTextElement()
    {
        _moneyText.text = "$" + PlayerPrefs.GetInt(Constants.MONEY_AMOUNT).ToString();
    }

    private void SetUpgradeSprites()
    {
        foreach (var image in _bulletUpgradeSprites)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        }
        foreach (var image in _movespeedUpgradeSprites)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        }
        foreach (var image in _gunUpgradeSprites)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        }
    }

    private void SetBoughtUpgradeSprites()
    {
        for (int i = 0; i < _bulletUpgradeLevel; i++)
        {
            _bulletUpgradeSprites[i].color = new Color(0.75f, 0.75f, 0.75f, 0.85f);
        }
        for (int i = 0; i < _movespeedUpgradeLevel; i++)
        {
            _movespeedUpgradeSprites[i].color = new Color(0.75f, 0.75f, 0.75f, 0.85f);
        }
        for (int i = 0; i < _gunUpgradeLevel; i++)
        {
            _gunUpgradeSprites[i].color = new Color(0.75f, 0.75f, 0.75f, 0.85f);
        }
    }

    private void SetUpgradePrices()
    {
        switch (_bulletUpgradeLevel)
        {
            case 0:
                _bulletPrice.text = "$" + _upgradeOneCost.ToString();
                break;
            case 1:
                _bulletPrice.text = "$" + _upgradeTwoCost.ToString();
                break;
            case 2:
                _bulletPrice.text = "$" + _upgradeThreeCost.ToString();
                break;
            case 3:
                _bulletPrice.text = "$" + _upgradeFourCost.ToString();
                break;
            case 4:
                _bulletPrice.text = "$" + _upgradeFiveCost.ToString();
                break;
            default:
                _bulletPrice.text = "MAX";
                break;
        }

        switch (_movespeedUpgradeLevel)
        {
            case 0:
                _speedPrice.text = "$" + _upgradeOneCost.ToString();
                break;
            case 1:
                _speedPrice.text = "$" + _upgradeTwoCost.ToString();
                break;
            case 2:
                _speedPrice.text = "$" + _upgradeThreeCost.ToString();
                break;
            case 3:
                _speedPrice.text = "$" + _upgradeFourCost.ToString();
                break;
            case 4:
                _speedPrice.text = "$" + _upgradeFiveCost.ToString();
                break;
            default:
                _speedPrice.text = "MAX";
                break;
        }

        switch (_gunUpgradeLevel)
        {
            case 0:
                _gunPrice.text = "$" + _gunUpgradeOneCost.ToString();
                break;
            case 1:
                _gunPrice.text = "$" + _gunUpgradeTwoCost.ToString();
                break;
            case 2:
                _gunPrice.text = "$" + _gunUpgradeThreeCost.ToString();
                break;
            case 3:
                _gunPrice.text = "$" + _gunUpgradeFourCost.ToString();
                break;
            case 4:
                _gunPrice.text = "$" + _gunUpgradeFiveCost.ToString();
                break;
            default:
                _gunPrice.text = "MAX";
                break;
        }
    }

    public void OnAddMoneyButtonClick()
    {
        int money = PlayerPrefs.GetInt(Constants.MONEY_AMOUNT);
        PlayerPrefs.SetInt(Constants.MONEY_AMOUNT, money + 1000);
        UpdateUIElements();
    }

    public void OnResetStatsButtonClick()
    {
        PlayerPrefs.SetInt(Constants.BULLET_UPGRADE_LEVEL, 0);
        PlayerPrefs.SetInt(Constants.MOVESPEED_UPGRADE_LEVEL, 0);
        PlayerPrefs.SetInt(Constants.GUN_UPGRADE_LEVEL, 0);
        PlayerPrefs.SetInt(Constants.MONEY_AMOUNT, 0);


        _bulletUpgradeLevel = PlayerPrefs.GetInt(Constants.BULLET_UPGRADE_LEVEL);
        _movespeedUpgradeLevel = PlayerPrefs.GetInt(Constants.MOVESPEED_UPGRADE_LEVEL);
        _gunUpgradeLevel = PlayerPrefs.GetInt(Constants.GUN_UPGRADE_LEVEL);

        UpdateUIElements();
    }

    public void OnBulletUpgradeButtonClick()
    {
        if (_bulletUpgradeLevel >= _maxUpgradeLevel) return;

        int currentMoney = PlayerPrefs.GetInt(Constants.MONEY_AMOUNT);
        int currentBulletUpgradeLevel = PlayerPrefs.GetInt(Constants.BULLET_UPGRADE_LEVEL);
        int upgradeCost;
        switch (currentBulletUpgradeLevel)
        {
            case 0:
                upgradeCost = _upgradeOneCost;
                break;
            case 1:
                upgradeCost = _upgradeTwoCost;
                break;
            case 2:
                upgradeCost = _upgradeThreeCost;
                break;
            case 3:
                upgradeCost = _upgradeFourCost;
                break;
            case 4:
                upgradeCost = _upgradeFiveCost;
                break;
            default:
                upgradeCost = 0;
                break;
        }
        if (currentMoney >= upgradeCost)
        {
            PlayerPrefs.SetInt(Constants.MONEY_AMOUNT, currentMoney - upgradeCost);
            PlayerPrefs.SetInt(Constants.BULLET_UPGRADE_LEVEL, currentBulletUpgradeLevel + 1);
            _bulletUpgradeLevel = PlayerPrefs.GetInt(Constants.BULLET_UPGRADE_LEVEL);
        }

        UpdateUIElements();
    }

    public void OnMovespeedUpgradeButtonClick()
    {
        if (_movespeedUpgradeLevel >= _maxUpgradeLevel) return;

        int currentMoney = PlayerPrefs.GetInt(Constants.MONEY_AMOUNT);
        int currentMovespeedUpgradeLevel = PlayerPrefs.GetInt(Constants.MOVESPEED_UPGRADE_LEVEL);
        int upgradeCost;
        switch (currentMovespeedUpgradeLevel)
        {
            case 0:
                upgradeCost = _upgradeOneCost;
                break;
            case 1:
                upgradeCost = _upgradeTwoCost;
                break;
            case 2:
                upgradeCost = _upgradeThreeCost;
                break;
            case 3:
                upgradeCost = _upgradeFourCost;
                break;
            case 4:
                upgradeCost = _upgradeFiveCost;
                break;
            default:
                upgradeCost = 0;
                break;
        }
        if (currentMoney >= upgradeCost)
        {
            PlayerPrefs.SetInt(Constants.MONEY_AMOUNT, currentMoney - upgradeCost);
            PlayerPrefs.SetInt(Constants.MOVESPEED_UPGRADE_LEVEL, currentMovespeedUpgradeLevel + 1);
            _movespeedUpgradeLevel = PlayerPrefs.GetInt(Constants.MOVESPEED_UPGRADE_LEVEL);
        }

        UpdateUIElements();
    }

    public void OnGunUpgradeButtonClick()
    {
        if (_gunUpgradeLevel >= _maxUpgradeLevel) return;

        int currentMoney = PlayerPrefs.GetInt(Constants.MONEY_AMOUNT);
        int currentGunUpgradeLevel = PlayerPrefs.GetInt(Constants.GUN_UPGRADE_LEVEL);
        int upgradeCost;
        switch (currentGunUpgradeLevel)
        {
            case 0:
                upgradeCost = _gunUpgradeOneCost;
                break;
            case 1:
                upgradeCost = _gunUpgradeTwoCost;
                break;
            case 2:
                upgradeCost = _gunUpgradeThreeCost;
                break;
            case 3:
                upgradeCost = _gunUpgradeFourCost;
                break;
            case 4:
                upgradeCost = _gunUpgradeFiveCost;
                break;
            default:
                upgradeCost = 0;
                break;
        }
        if (currentMoney >= upgradeCost)
        {
            PlayerPrefs.SetInt(Constants.MONEY_AMOUNT, currentMoney - upgradeCost);
            PlayerPrefs.SetInt(Constants.GUN_UPGRADE_LEVEL, currentGunUpgradeLevel + 1);
            _gunUpgradeLevel = PlayerPrefs.GetInt(Constants.GUN_UPGRADE_LEVEL);
        }

        UpdateUIElements();
    }
}
