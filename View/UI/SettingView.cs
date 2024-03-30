using AnnulusGames.LucidTools.Audio;
using LitMotion;
using LitMotion.Extensions;
using R3;
using R3.Triggers;
using u1w_2024_3.Src.Service;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace u1w_2024_3.Src.View.UI
{
    public sealed class SettingView : MonoBehaviour
    {
        [Inject] private readonly SoundRepository _soundRepository;

        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _seSlider;

        private void Start()
        {
            _bgmSlider.onValueChanged
                .AsObservable()
                .Subscribe(volume => { LucidAudio.BGMVolume = volume; }).AddTo(this);

            _seSlider.onValueChanged
                .AsObservable()
                .Subscribe(volume => { LucidAudio.SEVolume = volume; }).AddTo(this);

            _bgmSlider.value = LucidAudio.BGMVolume;
            _seSlider.value = LucidAudio.SEVolume;

            _seSlider.OnPointerUpAsObservable()
                .Subscribe(_ => { LucidAudio.PlaySE(_soundRepository.GetClip("TestSFX")).SetTimeSamples(); })
                .AddTo(this);
        }

        private void PlayCloseAnimation()
        {
            // TODO
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            Time.timeScale = active ? 0 : 1;

            if (!active)
            {
                PlayCloseAnimation();
            }
        }
    }
}