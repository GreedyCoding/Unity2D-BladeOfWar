using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject shopMenuUI;

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnShopButtonClick()
    {
        mainMenuUI.SetActive(false);
        shopMenuUI.SetActive(true);
    }

    public void OnQuitButtonClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnBackButtonClick()
    {
        shopMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
