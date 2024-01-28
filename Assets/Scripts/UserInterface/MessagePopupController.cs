using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class MessagePopupController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void PlayMessage(string input)
    {
        string message = "> ----------- " + input + " ----------- <";
        messageText.text = message;
        gameObject.SetActive(true);
        StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage()
    {
        string message = messageText.text;
        yield return new WaitForSeconds(0.4f);
        messageText.text = " ";
        yield return new WaitForSeconds(0.4f);
        messageText.text = message;
        yield return new WaitForSeconds(0.4f);
        messageText.text = " ";
        yield return new WaitForSeconds(0.4f);
        messageText.text = message;
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }
}
