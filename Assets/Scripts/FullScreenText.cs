using Framework.Singleton;
using TMPro;
using UnityEngine;

public class FullScreenText : SingletonBehaviour<FullScreenText>
{
    public TMP_Text text;
    public GameObject container;

    public static void ShowText(string message)
    {
        if (Instance.text != null)
        {
            Instance.text.text = message;
            Instance.container.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Text component is not assigned.");
        }
    }

    public static void HideText()
    {
        if (Instance.text != null)
        {
            Instance.text.text = "";
            Instance.container.SetActive(false);
        }
        else
        {
            Debug.LogError("Text component is not assigned.");
        }
    }
}