/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, September 2014
 * 
 * Class to manage the comic and all of it's elements.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Core{

    public class ComicManager : Singleton<ComicManager> {

        const int THUMBMAX = 11; // Maximum number of thumbnails for comic pages
        int viewCenter; // Center of the scroll view
        List<GameObject> ComicPages; // List of all Pages
        List<ScrollContent> ComicThumbs; // List of all Thumbnails

        public int ActivePage { get; set; } // Current page of the comic
        public int FirstPage { get; set; } // First page of the comic
        public int LastPage { get; set; } // Last page of the comic
        
        

        public void StartCheck () {
            if (ActivePage > 0) {
                GUIManager.instance.GetUI("StartScreen").Display = false;
                ActivePage++;
                StartPages();
            }
        }

        public void StartPages () {
            StartCoroutine("StartLoadedPages");
        }
        IEnumerator StartLoadedPages () {
            yield return new WaitForSeconds(1.0f);
            PageActiveAll();
        }

        public void OnComicStart () {
            ActivePage = 0;
            viewCenter = THUMBMAX / 2;
        }

        public void OnPageLoad () {

            ComicPages = new List<GameObject>();
            ComicThumbs = new List<ScrollContent>();

            foreach (Transform child in GUIManager.instance.GetUI("PageRoot").transform) {
                ComicPages.Add(child.gameObject);
            }
            foreach (Transform child in GUIManager.instance.GetUI("ScrollGrid").transform) {
                ComicThumbs.Add(child.GetComponent<ScrollContent>());
            }
            ComicPages[0].SetActive(false);
            FirstPage = 0;
            LastPage = ComicPages.Count - 1;
        }

        public void PageNext () {
            if (ActivePage < LastPage) {
                ComicPages[ActivePage].SetActive(false);
                ActivePage++;
                ComicPages[ActivePage].SetActive(true);
                SoundCheck();
                ThumbActiveNext();
            }
        }

        public void PageBack () {
            if (ActivePage > FirstPage) {
                ComicPages[ActivePage].SetActive(false);
                ActivePage--;
                ComicPages[ActivePage].SetActive(true);
                SoundCheck();
                ThumbActiveBack();
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

        void ThumbActiveBack () {
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
            SoundCheck();
            ComicThumbs[ActivePage].PlayResizeForward();
            ComicThumbs[ActivePage + 1].PlayAlphaForward();
        }

        void PageActiveAll () {
            ComicPages[ActivePage].SetActive(true);
            SoundCheck();
            ComicThumbs[ActivePage].PlayAlphaForward();
            ComicThumbs[ActivePage+1].PlayAlphaForward();
            ComicThumbs[ActivePage].PlayResizeForward();

            for (int i=0; i<ActivePage; i++){
                ComicThumbs[i].PlayAlphaForward();
            }

            UpdateView();
        }

        void SoundCheck() {            
            switch (ActivePage) {
                case 0:
                    GameManager.instance.PlayComic("PlayComic1");
                    break;
                case 1:
                    GameManager.instance.PlayComic("PlayComic2");
                    break;
                case 2:
                    GameManager.instance.PlayComic("PlayComic3");
                    break;
                case 3:
                    GameManager.instance.PlayComic("PlayComic4");
                    break;
                default: 
                    break;
            }
        }

        void UpdateView () {
            ComicThumbs[viewCenter].CenterView();
        }
    }
}
