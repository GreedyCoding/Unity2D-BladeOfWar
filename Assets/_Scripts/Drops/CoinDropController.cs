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

    private void Awake()
    {
        SetBonusDropEnum();
        SetBonusDropSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.PLAYER_TAG))
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
        if(random <= 5)
        {
            coinDropEnum = CoinDropEnum.goldCoin;
        }
        else if (random <= 15)
        {
            coinDropEnum = CoinDropEnum.blueCoin;
        }
        else if (random <= 30)
        {
            coinDropEnum = CoinDropEnum.greenCoin;
        }
        else if (random <= 100)
        {
            coinDropEnum = CoinDropEnum.greyCoin;
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
