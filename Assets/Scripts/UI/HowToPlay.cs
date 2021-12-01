using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] List<GameObject> pages = new List<GameObject>();

    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;

    int currPage = 0;

    private void Start() {
        UpdatePage(currPage);
    }

    void UpdatePage(int newPage) {
        if(newPage < 0) {
            return;
        }
        if(newPage > pages.Count) {
            return; 
        }

        previousButton.interactable = true;
        nextButton.interactable = true;

        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        pages[newPage].SetActive(true);

        if(newPage == 0) {
            previousButton.interactable = false;
        }
        if(newPage == pages.Count - 1) {
            nextButton.interactable = false;
        }

        currPage = newPage;
    }

    public void PreviousPage() {
        UpdatePage(currPage - 1);
    }

    public void NextPage() {
        UpdatePage(currPage + 1);
    }
}
