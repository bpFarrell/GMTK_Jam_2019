using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public int pageNumber = 0;
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
                page.SetActive(PageContent.notes[pageNumber]);
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
