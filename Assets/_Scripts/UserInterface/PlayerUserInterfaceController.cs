using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUserInterfaceController : MonoBehaviour
{
    [Header("Event Channel SO")]
    [SerializeField] IntToupleEventChannelSO _bulletChangeVoidEventChannelSO;
    [SerializeField] IntEventChannelSO _healthChangeVoidEventChannelSO;
    [SerializeField] IntEventChannelSO _moneyVoidEventChannelSO;
    [SerializeField] FloatEventChannelSO _movespeedChangeVoidEventChannelSO;

    [Header("Health UI")]
    [SerializeField] Image healthContainerOne;
    [SerializeField] Image healthContainerTwo;
    [SerializeField] Image healthContainerThree;
    [SerializeField] Sprite emptyHealthContainerSprite;
    private Sprite fullHealthContainerSprite;

    [Header("Ammo UI")]
    [SerializeField] TextMeshProUGUI ammoText;

    [Header("Speed UI")]
    [SerializeField] TextMeshProUGUI speedText;

    [Header("Money UI")]
    [SerializeField] TextMeshProUGUI moneyText;


    private void OnEnable()
    {
        fullHealthContainerSprite = healthContainerOne.sprite;
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        _bulletChangeVoidEventChannelSO.OnEventRaised += HandleBulletValueChange;
        _healthChangeVoidEventChannelSO.OnEventRaised += HandleHealthValueChange;
        _moneyVoidEventChannelSO.OnEventRaised += HandleMoneyValueChange;
        _movespeedChangeVoidEventChannelSO.OnEventRaised += HandleMovespeedValueChange;
    }

    private void UnsubscribeFromEvents()
    {
        _bulletChangeVoidEventChannelSO.OnEventRaised -= HandleBulletValueChange;
        _healthChangeVoidEventChannelSO.OnEventRaised -= HandleHealthValueChange;
        _moneyVoidEventChannelSO.OnEventRaised -= HandleMoneyValueChange;
        _movespeedChangeVoidEventChannelSO.OnEventRaised -= HandleMovespeedValueChange;
    }

    private void HandleHealthValueChange(int currentHitPoints)
    {
        switch (currentHitPoints)
        {
            case 3:
                healthContainerOne.sprite = fullHealthContainerSprite;
                healthContainerTwo.sprite = fullHealthContainerSprite;
                healthContainerThree.sprite = fullHealthContainerSprite;
                break;
            case 2:
                healthContainerOne.sprite = fullHealthContainerSprite;
                healthContainerTwo.sprite = fullHealthContainerSprite;
                healthContainerThree.sprite = emptyHealthContainerSprite;
                break;
            case 1:
                healthContainerOne.sprite = fullHealthContainerSprite;
                healthContainerTwo.sprite = emptyHealthContainerSprite;
                healthContainerThree.sprite = emptyHealthContainerSprite;
                break;
            case 0:
                healthContainerOne.sprite = emptyHealthContainerSprite;
                healthContainerTwo.sprite = emptyHealthContainerSprite;
                healthContainerThree.sprite = emptyHealthContainerSprite;
                break;
        }
    }

    private void HandleBulletValueChange(int currentBullets, int maxBullets)
    {
        ammoText.text = (currentBullets.ToString() + "/" + maxBullets.ToString());
    }

    private void HandleMovespeedValueChange(float moveSpeed)
    {
        speedText.text = moveSpeed.ToString();
    }

    private void HandleMoneyValueChange(int moneyAmount)
    {
        moneyText.text = "$" + moneyAmount.ToString();
    }
}
