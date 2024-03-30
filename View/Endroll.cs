using System.Threading;
using AnnulusGames.LucidTools.Audio;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using R3;
using u1w_2024_3.Src.Service;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace u1w_2024_3.Src.View
{
    public sealed class Endroll : MonoBehaviour
    {
        [Inject] private SoundRepository _soundRepository;

        [SerializeField] private Button _backToTitleButton;
        [SerializeField] private Image _image;

        private CancellationToken _cancellationToken;

        private async UniTask Awake()
        {
            _backToTitleButton.OnClickAsObservable().Subscribe(async _ =>
            {
                LucidAudio.PlaySE(_soundRepository.GetClip("Button")).SetTimeSamples();
                await FadeScreen();
                await UniTask.Delay(500, cancellationToken: _cancellationToken);
                new LevelLoader().Load(0);
            }).AddTo(this);

            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }

        private async UniTask FadeScreen()
        {
            // imageを先頭に持ってくる
            _image.transform.SetAsLastSibling();
            await LMotion.Create(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 1.0f)
                .BindToColor(_image)
                .ToUniTask(cancellationToken: _cancellationToken);
        }
    }
}