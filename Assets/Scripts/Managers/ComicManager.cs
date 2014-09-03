using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Core{

    public class ComicManager : Singleton<ComicManager> {

        List<GameObject> ComicPages;
        UIScrollBar ScrollFocus { get; set; }
        float ScrollOffset = 0.153f;

        public int ActivePage { get; set; } //Current page of the comic
        public int FirstPage { get; set; }
        public int LastPage { get; set; }

        public void OnComicStart(){
            ActivePage = 0;
            ComicPages = new List<GameObject>();
        }        
                
        public void OnPageLoad () {
            foreach (Transform child in GUIManager.instance.GetUI("PageRoot").transform) {
                ComicPages.Add(child.gameObject);
            }

            ComicPages[ActivePage].SetActive(true);
            ScrollFocus = FindObjectOfType<UIScrollBar>();
            FocusPage(ActivePage);
            FirstPage = 0;
            LastPage = ComicPages.Count-1;
        }

        public void OnPageNext () {
            if (ActivePage < LastPage) {
                ComicPages[ActivePage].SetActive(false);
                ActivePage++;
                ComicPages[ActivePage].SetActive(true);
                FocusPage(ActivePage);
            }
        }

        public void OnPagePrevious () {
            if (ActivePage > FirstPage) {
                ComicPages[ActivePage].SetActive(false);
                ActivePage--;
                ComicPages[ActivePage].SetActive(true);
                FocusPage(ActivePage);
            }
        }

        void FocusPage (int page) {
            ScrollFocus.value = ScrollOffset * page;
        }
    }
}
