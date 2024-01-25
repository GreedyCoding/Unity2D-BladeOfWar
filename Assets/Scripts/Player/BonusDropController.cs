using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDropController : MonoBehaviour
{
    public BonusDropEnum bonusDropEnum;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite extraSpeedSprite;
    [SerializeField] Sprite extraBulletSprite;
    [SerializeField] Sprite extraLifeSprite;
    [SerializeField] Sprite doubleShotSprite;
    [SerializeField] Sprite tripleShotSprite;
    [SerializeField] Sprite quadShotSprite;

    private PlayerController playerController;

    private void Start()
    {
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
                case BonusDropEnum.extraLife:
                    playerController.IncreaseHitpoints();
                    break;
                case BonusDropEnum.doubleShot:
                    playerController.SetGunType(GunTypeEnum.doubleShot);
                    break;
                case BonusDropEnum.tripleShot:
                    playerController.SetGunType(GunTypeEnum.tripleShot);
                    break;
                case BonusDropEnum.quadShot:
                    playerController.SetGunType(GunTypeEnum.quadShot);
                    break;
            }

            Destroy(gameObject);
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
            case BonusDropEnum.extraLife:
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
