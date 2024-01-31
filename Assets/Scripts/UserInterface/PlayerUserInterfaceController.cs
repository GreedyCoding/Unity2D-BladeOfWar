using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUserInterfaceController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

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
        playerController.OnHealthValueChange += HandleHealthValueChange;
        playerController.OnBulletValueChange += HandleBulletValueChange;
        playerController.OnMovespeedValueChange += HandleMovespeedValueChange;
        playerController.OnMoneyValueChange += HandleMoneyValueChange;
    }

    private void UnsubscribeFromEvents()
    {
        playerController.OnHealthValueChange -= HandleHealthValueChange;
        playerController.OnBulletValueChange -= HandleBulletValueChange;
        playerController.OnMovespeedValueChange -= HandleMovespeedValueChange;
        playerController.OnMoneyValueChange -= HandleMoneyValueChange;
    }

    private void HandleHealthValueChange(object sender, System.EventArgs e)
    {
        switch (playerController.CurrentHitPoints)
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

    private void HandleBulletValueChange(object sender, System.EventArgs e)
    {
        ammoText.text = (playerController.CurrentBullets.ToString() + "/" + playerController.MaxBullets.ToString());
    }

    private void HandleMovespeedValueChange(object sender, EventArgs e)
    {
        speedText.text = playerController.MoveSpeed.ToString();
    }

    private void HandleMoneyValueChange(object sender, EventArgs e)
    {
        moneyText.text = "$" + playerController.Money.ToString();
    }
}
