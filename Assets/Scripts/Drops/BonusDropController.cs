using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDropController : MonoBehaviour
{

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite extraSpeedSprite;
    [SerializeField] Sprite extraBulletSprite;
    [SerializeField] Sprite extraLifeSprite;
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
        float randomNumber = Random.Range(0f, 6f);
        if (randomNumber <= 1f)
        {
            bonusDropEnum = BonusDropEnum.extraSpeed;
        }
        else if (randomNumber <= 2f)
        {
            bonusDropEnum = BonusDropEnum.extraBullet;
        }
        else if (randomNumber <= 3f)
        {
            bonusDropEnum = BonusDropEnum.healHitPoints;
        }
        else if (randomNumber <= 4.5f)
        {
            bonusDropEnum = BonusDropEnum.doubleShot;
        }
        else if (randomNumber <= 5.5f)
        {
            bonusDropEnum = BonusDropEnum.tripleShot;
        }
        else if (randomNumber <= 6f)
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
