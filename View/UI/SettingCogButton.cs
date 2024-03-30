using R3;
using UnityEngine;
using UnityEngine.UI;

namespace u1w_2024_3.Src.View.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class SettingCogButton : MonoBehaviour
    {
        private Button _button;

        [SerializeField] private Button _anotherButton;
        [SerializeField] private SettingView _settingView;

        private bool _isSettingViewActive;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.OnClickAsObservable().Subscribe(x =>
            {
                _isSettingViewActive = !_isSettingViewActive;
                _settingView.SetActive(_isSettingViewActive);
            });
            
            _anotherButton.OnClickAsObservable().Subscribe(x =>
            {
                _isSettingViewActive = !_isSettingViewActive;
                _settingView.SetActive(_isSettingViewActive);
            });
        }
    }
}