using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    [SerializeField] Image healthContainerOne;
    [SerializeField] Image healthContainerTwo;
    [SerializeField] Image healthContainerThree;

    [SerializeField] Sprite emptyHealthContainerSprite;
    private Sprite fullHealthContainerSprite;


    private void OnEnable()
    {
        playerController.OnHealthValueChange += HandleHealthValueChange;
        fullHealthContainerSprite = healthContainerOne.sprite;
    }

    private void OnDisable()
    {
        playerController.OnHealthValueChange -= HandleHealthValueChange;
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
}
