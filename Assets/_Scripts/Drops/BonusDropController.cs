using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDropController : MonoBehaviour
{

    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _extraSpeedSprite;
    [SerializeField] Sprite _extraBulletSprite;
    [SerializeField] Sprite _extraLifeSprite;
    [SerializeField] Sprite _greyCoinSprite;
    [SerializeField] Sprite _greenCoinSprite;
    [SerializeField] Sprite _blueCoinSprite;
    [SerializeField] Sprite _goldCoinSprite;
    [SerializeField] Sprite _doubleShotSprite;
    [SerializeField] Sprite _tripleShotSprite;
    [SerializeField] Sprite _quadShotSprite;

    private BonusDropEnum _bonusDropEnum;

    private PlayerController _playerController;

    private void Start()
    {
        SetBonusDropEnum();
        SetBonusDropSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.PLAYER_TAG))
        {
            _playerController = other.GetComponent<PlayerController>();

            switch(_bonusDropEnum)
            {
                case BonusDropEnum.extraSpeed:
                    _playerController.IncreaseSpeed(true);
                    break;
                case BonusDropEnum.extraBullet:
                    _playerController.IncreaseBullet(true);
                    break;
                case BonusDropEnum.healHitPoints:
                    _playerController.ProvideHealing(1);
                    break;
                case BonusDropEnum.doubleShot:
                    _playerController.SetGunType(GunTypeEnum.doubleShot, false, true);
                    break;
                case BonusDropEnum.tripleShot:
                    _playerController.SetGunType(GunTypeEnum.tripleShot, false, true);
                    break;
                case BonusDropEnum.quadShot:
                    _playerController.SetGunType(GunTypeEnum.quadShot, false, true);
                    break;
            }
            Destroy(gameObject);
        }
    }

    private void SetBonusDropEnum()
    {
        int random = Random.Range(0, 101);
        if (random <= 20)
        {
            _bonusDropEnum = BonusDropEnum.extraSpeed;
            return;
        }
        else if (random <= 40)
        {
            _bonusDropEnum = BonusDropEnum.extraBullet;
            return;
        }
        else if (random <= 60)
        {
            _bonusDropEnum = BonusDropEnum.healHitPoints;
            return;
        }
        else if (random <= 80)
        {
            _bonusDropEnum = BonusDropEnum.doubleShot;
            return;
        }
        else if (random <= 95)
        {
            _bonusDropEnum = BonusDropEnum.tripleShot;
            return;
        }
        else if (random <= 100)
        {
            _bonusDropEnum = BonusDropEnum.quadShot;
            return;
        }
    }

    private void SetBonusDropSprite()
    {
        switch (_bonusDropEnum)
        {
            case BonusDropEnum.extraSpeed:
                _spriteRenderer.sprite = _extraSpeedSprite;
                break;
            case BonusDropEnum.extraBullet:
                _spriteRenderer.sprite = _extraBulletSprite;
                break;
            case BonusDropEnum.healHitPoints:
                _spriteRenderer.sprite = _extraLifeSprite;
                break;
            case BonusDropEnum.doubleShot:
                _spriteRenderer.sprite = _doubleShotSprite;
                break;
            case BonusDropEnum.tripleShot:
                _spriteRenderer.sprite = _tripleShotSprite;
                break;
            case BonusDropEnum.quadShot:
                _spriteRenderer.sprite = _quadShotSprite;
                break;
        }
    }
}
