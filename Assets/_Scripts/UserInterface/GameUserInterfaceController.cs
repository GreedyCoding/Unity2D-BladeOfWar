using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUserInterfaceController : MonoBehaviour
{
    [SerializeField] GameObject _shopMenu;
    [SerializeField] GameObject _pauseMenu;
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

    private IEnumerator OpenShopMenuCoroutine()
    {
        yield return new WaitForSeconds(3f);
        _shopMenu.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
    }
}

