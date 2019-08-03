using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public string body = "";
    Canvas canvas;
    UiPage page;


    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null) {
            Debug.LogError("Need a page Canvas in scene");
            return;
        }

        page = canvas.gameObject.GetComponent<UiPage>();         
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canvas != null)
        {
            if (other.gameObject.layer == 8)
            {
                page.SetActive(body);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (canvas != null)
        {
            if (other.gameObject.layer == 8)
            {
                page.SetDeActive();
            }
        }
    }
}
