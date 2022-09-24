using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace GM.SceneSwitcher
{
    public class SceneSwitcher : MonoBehaviour
    {
        public static SceneSwitcher instance;
        static SceneSwitchType preType = SceneSwitchType.fade;

        [SerializeField] private Image block;
        [SerializeField] private Animator ani;
        [SerializeField] private float duration;
        [SerializeField] private UnityEvent onChange;
        string scene;

        public float durationTime
        {
            get
            {
                return 1.0f / ani.speed;
            }
            set
            {
                ani.speed = 1.0f / value;
            }
        }

        private void Awake()
        {
            scene = "";
            instance = this;
            durationTime = duration;
            block.gameObject.SetActive(true);
            ani.Play("p " + preType);
        }
        public void Switch()
        {
            scene = SceneManager.GetActiveScene().name;
            Switch(SceneSwitchType.fade);
        }
        public void Switch(string name, SceneSwitchType type = SceneSwitchType.fade)
        {
            scene = name;
            Switch(type);
        }
        public void Switch(int index, SceneSwitchType type = SceneSwitchType.fade)
        {
            Switch(SceneManager.GetSceneAt(index).name, type);
        }
        void Switch(SceneSwitchType type)
        {
            gameObject.SetActive(true);
            block.gameObject.SetActive(true);
            preType = type;
            ani.Play(type.ToString());
        }



        public void AnimationOver(int isStart)
        {
            if (isStart == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                onChange.Invoke();
                SceneManager.LoadScene(scene);
            }
        }
    }

    public enum SceneSwitchType
    {
        fade
    }


}