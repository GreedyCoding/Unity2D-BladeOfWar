using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MalusDropController : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite singleShotSprite;
    [SerializeField] Sprite engineMalfunctionSprite;
    [SerializeField] Sprite mirrorControlsSprite;

    private MalusDropEnum malusDropEnum;

    private PlayerController playerController;

    private void Start()
    {
        SetMalusDropEnum();
        SetMalusDropSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.PLAYER_TAG))
        {
            playerController = other.GetComponent<PlayerController>();

            switch (malusDropEnum)
            {
                case MalusDropEnum.singleShot:
                    playerController.SetGunType(GunTypeEnum.singleShot, false, true);
                    break;
                case MalusDropEnum.engineMalfunction:
                    playerController.DebuffMovementSpeed();
                    break;
                case MalusDropEnum.mirrorControls:
                    playerController.DebuffMirrorControls();
                    break;
            }

            Destroy(gameObject);
        }
    }

    private void SetMalusDropEnum()
    {
        float randomNumber = Random.Range(0f, 3f);

        if (randomNumber <= 1f)
        {
            malusDropEnum = MalusDropEnum.singleShot;
        }
        else if(randomNumber <= 2f)
        {
            malusDropEnum = MalusDropEnum.engineMalfunction;
        }
        else if(randomNumber <= 3f)
        {
            malusDropEnum = MalusDropEnum.mirrorControls;
        }
    }

    private void SetMalusDropSprite()
    {
        switch (malusDropEnum)
        {
            case MalusDropEnum.singleShot:
                spriteRenderer.sprite = singleShotSprite;
                break;
            case MalusDropEnum.engineMalfunction:
                spriteRenderer.sprite = engineMalfunctionSprite;
                break;
            case MalusDropEnum.mirrorControls:
                spriteRenderer.sprite = mirrorControlsSprite;
                break;
        }
    }
}
