using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class MessagePopupController : MonoBehaviour
{
    public static MessagePopupController Instance;

    [SerializeField] GameObject _messageTextGO;
    [SerializeField] TextMeshProUGUI _messageText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        _messageTextGO.SetActive(false);
    }

    public void PlayMessage(string input, int cycles = 2)
    {
        string message = "> ----------- " + input + " ----------- <";
        _messageText.text = message;
        _messageTextGO.SetActive(true);
        StartCoroutine(ShowMessage(cycles));
    }

    private IEnumerator ShowMessage(int cycles)
    {
        string message = _messageText.text;
        for (int i = 0; i < cycles; i++)
        {
            _messageText.text = " ";
            yield return new WaitForSeconds(0.4f);
            _messageText.text = message;
            yield return new WaitForSeconds(0.4f);
        }
        _messageTextGO.SetActive(false);
    }
}
