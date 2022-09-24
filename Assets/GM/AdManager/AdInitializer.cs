using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace GM.AdManager
{

    [CreateAssetMenu(menuName = "AdManager/Create Initializer")]
    public class AdInitializer : ScriptableObject
    {
        public string gameId;
        public bool testMode;
    }

    [System.Serializable]
    public class MainEvent
    {
        public UnityEvent showComplete;
        public UnityEvent loadComplete;
        public UnityEvent loadFail;
    }

    [System.Serializable]
    public class OtherEvent
    {
        public UnityEvent initComplete;
        public UnityEvent initFail;
        public UnityEvent showClick;
        public UnityEvent showFail;
        public UnityEvent showStart;
        public UnityEvent showOver;
        public UnityEvent showHide;
    }
}