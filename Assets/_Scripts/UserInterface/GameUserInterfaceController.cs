using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameUserInterfaceController : MonoBehaviour
{
    [Header("UI Menus")]
    [SerializeField] GameObject _shopMenu;
    [SerializeField] GameObject _pauseMenu;

    [Header("First Selected Option")]
    [SerializeField] GameObject _pauseMenuFirstSelected;
    [SerializeField] GameObject _shopMenuFirstSelected;

    [Header("Player Controller")]
    [SerializeField] PlayerController _playerController;

    [Header("Events")]
    [SerializeField] VoidEventChannelSO _bossDeathEventChannel;



    private void OnEnable()
    {
        _bossDeathEventChannel.OnEventRaised += OpenShopMenu;
    }

    private void OnDisable()
    {
        _bossDeathEventChannel.OnEventRaised -= OpenShopMenu;
    }

    private void OpenShopMenu()
    {
        StartCoroutine(OpenShopMenuCoroutine());
    }

    public void OpenPauseMenu()
    {
        _pauseMenu.SetActive(true);

        GameplayTimer.Instance.StopTimer();
        GameplayTimer.Instance.FreezeGameTime();

        EventSystem.current.SetSelectedGameObject(_pauseMenuFirstSelected);
    }

    private IEnumerator OpenShopMenuCoroutine()
    {
        yield return new WaitForSeconds(5f);

        _playerController.FreezePlayer();

        _shopMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_shopMenuFirstSelected);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
    }
}

