using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedDisplayController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TextMeshProUGUI speedText;

    void Update()
    {
        speedText.text = (playerController.MoveSpeed.ToString());
    }
}
