using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoDisplayController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TextMeshProUGUI ammoText;

    private void Awake()
    {
        playerController.OnBulletValueChange += HandleBulletValueChange;
    }

    private void OnDestroy()
    {
        playerController.OnBulletValueChange -= HandleBulletValueChange;
    }

    private void HandleBulletValueChange(object sender, System.EventArgs e)
    {
        ammoText.text = (playerController.CurrentBullets.ToString() + "/" + playerController.MaxBullets.ToString());
    }
}
