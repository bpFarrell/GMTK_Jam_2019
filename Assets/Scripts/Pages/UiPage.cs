using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPage : MonoBehaviour
{
    public Image page;
    public Text pageText;

    public void SetActive(string _pageText) {
        if (!page.gameObject.activeInHierarchy)
        {
            pageText.text = _pageText;
            page.gameObject.SetActive(true);
        }
    }
    public void SetDeActive()
    {
        if (page.gameObject.activeInHierarchy)
        {
            pageText.text = "";
            page.gameObject.SetActive(false);
        }
    }
}
