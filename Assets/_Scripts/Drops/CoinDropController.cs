using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropController : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _greyCoinSprite;
    [SerializeField] Sprite _greenCoinSprite;
    [SerializeField] Sprite _blueCoinSprite;
    [SerializeField] Sprite _goldCoinSprite;

    private CoinDropEnum _coinDropEnum;

    private PlayerController _playerController;

    private void Awake()
    {
        SetBonusDropEnum();
        SetBonusDropSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.PLAYER_TAG))
        {
            _playerController = other.GetComponent<PlayerController>();

            switch (_coinDropEnum)
            {

                case CoinDropEnum.greyCoin:
                    _playerController.AddMoney(10);
                    break;
                case CoinDropEnum.greenCoin:
                    _playerController.AddMoney(25);
                    break;
                case CoinDropEnum.blueCoin:
                    _playerController.AddMoney(50);
                    break;
                case CoinDropEnum.goldCoin:
                    _playerController.AddMoney(100);
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
            _coinDropEnum = CoinDropEnum.goldCoin;
        }
        else if (random <= 15)
        {
            _coinDropEnum = CoinDropEnum.blueCoin;
        }
        else if (random <= 30)
        {
            _coinDropEnum = CoinDropEnum.greenCoin;
        }
        else if (random <= 100)
        {
            _coinDropEnum = CoinDropEnum.greyCoin;
        }
    }

    private void SetBonusDropSprite()
    {
        switch (_coinDropEnum)
        {
            case CoinDropEnum.greyCoin:
                _spriteRenderer.sprite = _greyCoinSprite;
                break;
            case CoinDropEnum.greenCoin:
                _spriteRenderer.sprite = _greenCoinSprite;
                break;
            case CoinDropEnum.blueCoin:
                _spriteRenderer.sprite = _blueCoinSprite;
                break;
            case CoinDropEnum.goldCoin:
                _spriteRenderer.sprite = _goldCoinSprite;
                break;
        }
    }
}
