using System.Collections;
using UnityEngine;

public class BossDeathMenuController : MonoBehaviour
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
        StartCoroutine(OpenShopMenuCoroutine());
    }

    private IEnumerator OpenShopMenuCoroutine()
    {
        yield return new WaitForSeconds(3f);
        _shopMenu.SetActive(true);
    }
}

