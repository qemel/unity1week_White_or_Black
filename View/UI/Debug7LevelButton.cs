using R3;
using u1w_2024_3.Src.Service;
using UnityEngine;
using UnityEngine.UI;

namespace u1w_2024_3.Src.View.UI
{
    public sealed class Debug7LevelButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.OnClickAsObservable().Subscribe(x =>
            {
                new LevelLoader().Load(7);
                Time.timeScale = 1;
            }).AddTo(this);
        }
    }
}