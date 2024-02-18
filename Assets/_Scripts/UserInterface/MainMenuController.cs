using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject _mainMenuUI;
    [SerializeField] GameObject _shopMenuUI;

    [Header("First Selected Option")]
    [SerializeField] GameObject _mainMenuFirstSelected;
    [SerializeField] GameObject _shopMenuFirstSelected;

    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_mainMenuFirstSelected);
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnShopButtonClick()
    {
        _mainMenuUI.SetActive(false);
        _shopMenuUI.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_shopMenuFirstSelected);
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
        _shopMenuUI.SetActive(false);
        _mainMenuUI.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirstSelected);
    }
}
