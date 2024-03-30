using System.Threading;
using AnnulusGames.LucidTools.Audio;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using MessagePipe;
using R3;
using R3.Triggers;
using u1w_2024_3.Src.Model.Message;
using u1w_2024_3.Src.Service;
using u1w_2024_3.Src.View.Player;
using UnityEngine;
using VContainer;

namespace u1w_2024_3.Src.View
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public sealed class GravityChangerView : MonoBehaviour
    {
        [Inject] private readonly IPublisher<ChangeGravityDirectionEvent> _publisher;
        [Inject] private readonly SoundRepository _soundRepository;

        private Collider2D _collider2D;
        private SpriteRenderer _spriteRenderer;
        private CancellationToken _token;

        private bool _isActivated = true;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            _collider2D
                .OnTriggerEnter2DAsObservable()
                .Where(_ => _isActivated)
                .Subscribe(x =>
                {
                    if (x.TryGetComponent<PlayerCore>(out _))
                    {
                        _publisher.Publish(new ChangeGravityDirectionEvent());
                        LucidAudio.PlaySE(_soundRepository.GetClip("GravityChange")).SetTimeSamples();
                        SetCoolTime().Forget();
                    }
                }).AddTo(this);
        }

        /// <summary>
        /// 消滅させて、一定時間後に復活させる
        /// </summary>
        private async UniTask SetCoolTime()
        {
            _isActivated = false;
            await LMotion
                .Create(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.1f)
                .WithEase(Ease.InSine)
                .BindToColor(_spriteRenderer)
                .ToUniTask(_token);
            await UniTask.Delay(2500, cancellationToken: _token);
            await LMotion
                .Create(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.5f)
                .WithEase(Ease.OutSine)
                .BindToColor(_spriteRenderer)
                .ToUniTask(_token);
            _isActivated = true;
        }
    }
}