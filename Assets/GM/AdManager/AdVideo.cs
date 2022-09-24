using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

namespace GM.AdManager
{
    public class AdVideo : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        static string gameId = "";
        static bool testMode;
        public string placementId;
        public UnityEvent initEvent;
        public MainEvent mainEvent;
        public OtherEvent otherEvent;
        private void Awake()
        {
            if (gameId == "")
            {
                AdInitializer ad = (AdInitializer)Resources.Load("Init");
                gameId = ad.gameId;
                testMode = ad.testMode;
            }
            initEvent.Invoke();

        }

        public virtual void Start()
        {
            if (!Advertisement.isInitialized)
            {
                Advertisement.Initialize(gameId, testMode, this);
            }
            else
            {
                Load();
            }
        }

        public void Load()
        {
            initEvent.Invoke();
            Advertisement.Load(placementId, this);
        }
        public virtual void Show()
        {
            Advertisement.Show(placementId, this);
        }

        public void OnInitializationComplete()
        {
            Load();
            otherEvent.initComplete.Invoke();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            otherEvent.initFail.Invoke();
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            mainEvent.loadComplete.Invoke();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            mainEvent.loadFail.Invoke();
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            otherEvent.showClick.Invoke();
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                mainEvent.showComplete.Invoke();
            }
            otherEvent.showOver.Invoke();
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            otherEvent.showFail.Invoke();
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Load();
            otherEvent.showStart.Invoke();
        }

    }
}

