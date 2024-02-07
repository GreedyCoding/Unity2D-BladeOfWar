using UnityEngine;

public class GameShopMenuController : MonoBehaviour
{
    [SerializeField] GameObject _shopMenu;
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
        _shopMenu.SetActive(true);
    }
}

