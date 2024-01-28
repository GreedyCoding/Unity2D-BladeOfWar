using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropController : MonoBehaviour
{

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite greyCoinSprite;
    [SerializeField] Sprite greenCoinSprite;
    [SerializeField] Sprite blueCoinSprite;
    [SerializeField] Sprite goldCoinSprite;

    private CoinDropEnum coinDropEnum;

    private PlayerController playerController;

    private void Start()
    {
        SetBonusDropEnum();
        SetBonusDropSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();

            switch (coinDropEnum)
            {

                case CoinDropEnum.greyCoin:
                    playerController.AddMoney(10);
                    break;
                case CoinDropEnum.greenCoin:
                    playerController.AddMoney(25);
                    break;
                case CoinDropEnum.blueCoin:
                    playerController.AddMoney(50);
                    break;
                case CoinDropEnum.goldCoin:
                    playerController.AddMoney(100);
                    break;

            }
            Destroy(gameObject);
        }
    }

    private void SetBonusDropEnum()
    {
        int random = Random.Range(0, 101);
        if(random <= 50)
        {
            coinDropEnum = CoinDropEnum.greyCoin;
        }
        else if (random <= 85)
        {
            coinDropEnum = CoinDropEnum.greenCoin;
        }
        else if (random <= 95)
        {
            coinDropEnum = CoinDropEnum.blueCoin;
        }
        else if (random <= 100)
        {
            coinDropEnum = CoinDropEnum.goldCoin;
        }
    }

    private void SetBonusDropSprite()
    {
        switch (coinDropEnum)
        {
            case CoinDropEnum.greyCoin:
                spriteRenderer.sprite = greyCoinSprite;
                break;
            case CoinDropEnum.greenCoin:
                spriteRenderer.sprite = greenCoinSprite;
                break;
            case CoinDropEnum.blueCoin:
                spriteRenderer.sprite = blueCoinSprite;
                break;
            case CoinDropEnum.goldCoin:
                spriteRenderer.sprite = goldCoinSprite;
                break;
        }
    }
}
