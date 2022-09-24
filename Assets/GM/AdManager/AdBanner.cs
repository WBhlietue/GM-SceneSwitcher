using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace GM.AdManager
{
    public class AdBanner : MonoBehaviour, IUnityAdsInitializationListener
    {
        static string gameId = "";
        static bool testMode;
        public string placementId;
        public MainEvent mainEvent;
        public OtherEvent otherEvent;
        BannerLoadOptions load;
        BannerOptions show;
        public UnityEngine.UI.Text t;
        private void Awake()
        {
            if (gameId == "")
            {
                AdInitializer ad = (AdInitializer)Resources.Load("Init");
                gameId = ad.gameId;
                testMode = ad.testMode;

            }
            load = new BannerLoadOptions
            {
                loadCallback = Loaded,
                errorCallback = LoadError
            };
            show = new BannerOptions
            {
                showCallback = Showed,
                hideCallback = Hidden,
                clickCallback = Clicked
            };
        }

        public virtual void Start()
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            if (!Advertisement.isInitialized)
            {
                Advertisement.Initialize(gameId, testMode, this);
            }
            else
            {
                t.text = "startLoad";
                Load();
            }

        }

        public void Load()
        {
            Advertisement.Banner.Load(placementId, load);
            t.text = "load";

        }
        public void Show()
        {
            Advertisement.Banner.Show(placementId, show);
        }
        public void Hide()
        {
            Advertisement.Banner.Hide();
        }

        public void Loaded()
        {
            t.text = "loadded";
            Show();
            mainEvent.loadComplete.Invoke();
        }
        public void LoadError(string msg)
        {
            mainEvent.loadFail.Invoke();
        }
        public void Showed()
        {
            t.text = "shoede";
            otherEvent.showStart.Invoke();
        }
        public void Clicked()
        {
            otherEvent.showClick.Invoke();
        }
        public void Hidden()
        {
            otherEvent.showHide.Invoke();
        }

        public void OnInitializationComplete()
        {
            t.text = "init To Load";
            Load();
            otherEvent.initComplete.Invoke();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            otherEvent.initFail.Invoke();
        }
    }
}
