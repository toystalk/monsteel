using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Core{

    public class ComicManager : Singleton<ComicManager> {

        const int THUMBMAX = 11; //maximum number of thumbnails for comic pages
        int viewCenter;
        List<GameObject> ComicPages;
        List<ScrollContent> ComicThumbs;

        public int ActivePage { get; set; } //Current page of the comic
        public int FirstPage { get; set; }
        public int LastPage { get; set; }
        
        public void OnComicStart(){
            ActivePage = 0;
            viewCenter = THUMBMAX / 2;
            ComicPages = new List<GameObject>();
            ComicThumbs = new List<ScrollContent>();
        }        
                
        public void OnPageLoad () {
            foreach (Transform child in GUIManager.instance.GetUI("PageRoot").transform) {
                ComicPages.Add(child.gameObject);
            }
            foreach (Transform child in GUIManager.instance.GetUI("ScrollGrid").transform) {
                ComicThumbs.Add(child.GetComponent<ScrollContent>());
            }

            FirstPage = 0;
            LastPage = ComicPages.Count-1;
            PageActiveAll();
        }

        public void OnPageNext () {
            if (ActivePage < LastPage) {
                ComicPages[ActivePage].SetActive(false);
                ActivePage++;
                ComicPages[ActivePage].SetActive(true);
                ThumbActiveNext();
            }
        }

        public void OnPagePrevious () {
            if (ActivePage > FirstPage) {
                ComicPages[ActivePage].SetActive(false);
                ActivePage--;
                ComicPages[ActivePage].SetActive(true);
                ThumbActivePrevious();
            }
        }

        void ThumbActiveNext () {
            ComicThumbs[ActivePage-1].PlayResizeReverse();
            ComicThumbs[ActivePage].PlayResizeForward();
            ComicThumbs[ActivePage + 1].PlayAlphaForward();

            if (ActivePage % THUMBMAX == 0 && ActivePage > 0) {
                viewCenter += THUMBMAX;
                UpdateView();
            }
        }

        void ThumbActivePrevious () {
            ComicThumbs[ActivePage].PlayResizeForward();
            ComicThumbs[ActivePage + 1].PlayResizeReverse();

            if ((ActivePage+1) % THUMBMAX == 0 && ActivePage > 0) {
                viewCenter -= THUMBMAX;
                UpdateView();
            }
        }

        public void PageActive (int pageNum) {
            ComicPages[ActivePage].SetActive(false);
            ComicThumbs[ActivePage].PlayResizeReverse();
            ActivePage = pageNum;

            ComicPages[ActivePage].SetActive(true);
            ComicThumbs[ActivePage].PlayResizeForward();
            ComicThumbs[ActivePage + 1].PlayAlphaForward();
        }

        void PageActiveAll () {
            ComicPages[ActivePage].SetActive(true);
            ComicThumbs[ActivePage].PlayAlphaForward();
            ComicThumbs[ActivePage+1].PlayAlphaForward();
            ComicThumbs[ActivePage].PlayResizeForward();

            for (int i=0; i<ActivePage; i++){
                ComicThumbs[i].PlayAlphaForward();
            }

            UpdateView();
        }

        void UpdateView () {
            ComicThumbs[viewCenter].CenterView();
        }
    }
}
