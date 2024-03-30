using AnnulusGames.LucidTools.Audio;
using Cysharp.Threading.Tasks;
using R3;
using u1w_2024_3.Src.Service;
using u1w_2024_3.Src.View.Camera;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace u1w_2024_3.Src.View.UI
{
    public sealed class TitleScreen : MonoBehaviour
    {
        [Inject] private readonly SoundRepository _soundRepository;
        [Inject] private readonly MainCameraAnimation _mainCameraAnimation;
        
        [SerializeField] private Button _button;

        [FormerlySerializedAs("_settingView")] [SerializeField]
        private Canvas _settingCanvas;

        private void Start()
        {
            SetActive(true);
            _button.OnClickAsObservable().Subscribe(x =>
            {
                LucidAudio.PlaySE(_soundRepository.GetClip("Button")).SetTimeSamples();
                SetActive(false);
                _settingCanvas.gameObject.SetActive(true);
                _mainCameraAnimation.DoEnterLevelAnimation().Forget();
            }).AddTo(this);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            Time.timeScale = active ? 0 : 1;
        }
    }
}