using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MalusDropController : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _singleShotSprite;
    [SerializeField] Sprite _engineMalfunctionSprite;
    [SerializeField] Sprite _mirrorControlsSprite;

    private MalusDropEnum _malusDropEnum;

    private PlayerController _playerController;

    private void Start()
    {
        SetMalusDropEnum();
        SetMalusDropSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.PLAYER_TAG))
        {
            _playerController = other.GetComponent<PlayerController>();

            switch (_malusDropEnum)
            {
                case MalusDropEnum.singleShot:
                    _playerController.SetGunType(GunTypeEnum.singleShot, false, true);
                    break;
                case MalusDropEnum.engineMalfunction:
                    _playerController.DebuffMovementSpeed();
                    break;
                case MalusDropEnum.mirrorControls:
                    _playerController.DebuffMirrorControls();
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
            _malusDropEnum = MalusDropEnum.singleShot;
        }
        else if(randomNumber <= 2f)
        {
            _malusDropEnum = MalusDropEnum.engineMalfunction;
        }
        else if(randomNumber <= 3f)
        {
            _malusDropEnum = MalusDropEnum.mirrorControls;
        }
    }

    private void SetMalusDropSprite()
    {
        switch (_malusDropEnum)
        {
            case MalusDropEnum.singleShot:
                _spriteRenderer.sprite = _singleShotSprite;
                break;
            case MalusDropEnum.engineMalfunction:
                _spriteRenderer.sprite = _engineMalfunctionSprite;
                break;
            case MalusDropEnum.mirrorControls:
                _spriteRenderer.sprite = _mirrorControlsSprite;
                break;
        }
    }
}
