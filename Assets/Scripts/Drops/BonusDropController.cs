using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDropController : MonoBehaviour
{

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite extraSpeedSprite;
    [SerializeField] Sprite extraBulletSprite;
    [SerializeField] Sprite extraLifeSprite;
    [SerializeField] Sprite greyCoinSprite;
    [SerializeField] Sprite greenCoinSprite;
    [SerializeField] Sprite blueCoinSprite;
    [SerializeField] Sprite goldCoinSprite;
    [SerializeField] Sprite doubleShotSprite;
    [SerializeField] Sprite tripleShotSprite;
    [SerializeField] Sprite quadShotSprite;

    private BonusDropEnum bonusDropEnum;

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

            switch(bonusDropEnum)
            {
                case BonusDropEnum.extraSpeed:
                    playerController.IncreaseSpeed();
                    break;
                case BonusDropEnum.extraBullet:
                    playerController.IncreaseBullet();
                    break;
                case BonusDropEnum.healHitPoints:
                    playerController.ProvideHealing(1f);
                    break;
                case BonusDropEnum.doubleShot:
                    playerController.SetGunType(GunTypeEnum.doubleShot, false);
                    break;
                case BonusDropEnum.tripleShot:
                    playerController.SetGunType(GunTypeEnum.tripleShot, false);
                    break;
                case BonusDropEnum.quadShot:
                    playerController.SetGunType(GunTypeEnum.quadShot, false);
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
            bonusDropEnum = BonusDropEnum.extraSpeed;
        }
        else if (random <= 40)
        {
            bonusDropEnum = BonusDropEnum.extraBullet;
        }
        else if (random <= 60)
        {
            bonusDropEnum = BonusDropEnum.healHitPoints;
        }
        else if (random <= 80)
        {
            bonusDropEnum = BonusDropEnum.doubleShot;
        }
        else if (random <= 95)
        {
            bonusDropEnum = BonusDropEnum.tripleShot;
        }
        else if (random <= 100)
        {
            bonusDropEnum = BonusDropEnum.quadShot;
        }
    }

    private void SetBonusDropSprite()
    {
        switch (bonusDropEnum)
        {
            case BonusDropEnum.extraSpeed:
                spriteRenderer.sprite = extraSpeedSprite;
                break;
            case BonusDropEnum.extraBullet:
                spriteRenderer.sprite = extraBulletSprite;
                break;
            case BonusDropEnum.healHitPoints:
                spriteRenderer.sprite = extraLifeSprite;
                break;
            case BonusDropEnum.doubleShot:
                spriteRenderer.sprite = doubleShotSprite;
                break;
            case BonusDropEnum.tripleShot:
                spriteRenderer.sprite = tripleShotSprite;
                break;
            case BonusDropEnum.quadShot:
                spriteRenderer.sprite = quadShotSprite;
                break;
        }
    }
}
