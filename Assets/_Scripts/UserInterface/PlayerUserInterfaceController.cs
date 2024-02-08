using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUserInterfaceController : MonoBehaviour
{
    [Header("Event Channel SO")]
    [SerializeField] IntToupleEventChannelSO _bulletChangeIntToupleEventChannelSO;
    [SerializeField] IntEventChannelSO _healthChangeIntEventChannelSO;
    [SerializeField] IntEventChannelSO _moneyChangeIntEventChannelSO;
    [SerializeField] IntEventChannelSO _stageChangeIntEventChannelSO;
    [SerializeField] FloatEventChannelSO _movespeedChangeFloatEventChannelSO;

    [Header("Health UI")]
    [SerializeField] Image _healthContainerOne;
    [SerializeField] Image _healthContainerTwo;
    [SerializeField] Image _healthContainerThree;
    [SerializeField] Sprite _emptyHealthContainerSprite;

    private Sprite _fullHealthContainerSprite;

    [Header("Ammo UI")]
    [SerializeField] TextMeshProUGUI _ammoText;

    [Header("Speed UI")]
    [SerializeField] TextMeshProUGUI _speedText;

    [Header("Money UI")]
    [SerializeField] TextMeshProUGUI _moneyText;

    [Header("Stage UI")]
    [SerializeField] TextMeshProUGUI _stageText;


    private void OnEnable()
    {
        _fullHealthContainerSprite = _healthContainerOne.sprite;
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        _bulletChangeIntToupleEventChannelSO.OnEventRaised += HandleBulletValueChange;
        _healthChangeIntEventChannelSO.OnEventRaised += HandleHealthValueChange;
        _moneyChangeIntEventChannelSO.OnEventRaised += HandleMoneyValueChange;
        _stageChangeIntEventChannelSO.OnEventRaised += HandleStageValueChange;
        _movespeedChangeFloatEventChannelSO.OnEventRaised += HandleMovespeedValueChange;
    }

    private void UnsubscribeFromEvents()
    {
        _bulletChangeIntToupleEventChannelSO.OnEventRaised -= HandleBulletValueChange;
        _healthChangeIntEventChannelSO.OnEventRaised -= HandleHealthValueChange;
        _moneyChangeIntEventChannelSO.OnEventRaised -= HandleMoneyValueChange;
        _stageChangeIntEventChannelSO.OnEventRaised -= HandleStageValueChange;
        _movespeedChangeFloatEventChannelSO.OnEventRaised -= HandleMovespeedValueChange;
    }

    private void HandleHealthValueChange(int currentHitPoints)
    {
        switch (currentHitPoints)
        {
            case 3:
                _healthContainerOne.sprite = _fullHealthContainerSprite;
                _healthContainerTwo.sprite = _fullHealthContainerSprite;
                _healthContainerThree.sprite = _fullHealthContainerSprite;
                break;
            case 2:
                _healthContainerOne.sprite = _fullHealthContainerSprite;
                _healthContainerTwo.sprite = _fullHealthContainerSprite;
                _healthContainerThree.sprite = _emptyHealthContainerSprite;
                break;
            case 1:
                _healthContainerOne.sprite = _fullHealthContainerSprite;
                _healthContainerTwo.sprite = _emptyHealthContainerSprite;
                _healthContainerThree.sprite = _emptyHealthContainerSprite;
                break;
            case 0:
                _healthContainerOne.sprite = _emptyHealthContainerSprite;
                _healthContainerTwo.sprite = _emptyHealthContainerSprite;
                _healthContainerThree.sprite = _emptyHealthContainerSprite;
                break;
        }
    }

    private void HandleBulletValueChange(int currentBullets, int maxBullets)
    {
        _ammoText.text = (currentBullets.ToString() + "/" + maxBullets.ToString());
    }

    private void HandleMovespeedValueChange(float moveSpeed)
    {
        _speedText.text = moveSpeed.ToString();
    }

    private void HandleMoneyValueChange(int moneyAmount)
    {
        _moneyText.text = "$" + moneyAmount.ToString();
    }

    private void HandleStageValueChange(int stageNumber)
    {
        _stageText.text = stageNumber.ToString();
    }
}
