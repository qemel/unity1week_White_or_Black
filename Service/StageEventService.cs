using System;
using System.Threading;
using AnnulusGames.LucidTools.Audio;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using u1w_2024_3.Src.Model;
using u1w_2024_3.Src.Model.Message;
using u1w_2024_3.Src.View.Camera;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace u1w_2024_3.Src.Service
{
    public sealed class StageEventService : IInitializable, IAsyncStartable, IDisposable
    {
        [Inject] private readonly ISubscriber<GameStateEvent> _subscriber;
        [Inject] private readonly LevelLoader _levelLoader;
        [Inject] private readonly MainCameraAnimation _mainCameraAnimation;
        [Inject] private readonly SoundRepository _soundRepository;


        private readonly CompositeDisposable _disposable = new();

        public void Initialize()
        {
            Physics2D.gravity = new Vector2(0, -Mathf.Abs(Physics2D.gravity.y));
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            if (Level.CurrentLevel == 0)
            {
                new SoundService().Reset();
                LucidAudio.StopAllBGM();
                LucidAudio.PlayBGM(_soundRepository.GetClip("BGM")).SetLoop();
            }
            else if (Level.IsMaxLevel)
            {
                LucidAudio.StopAllBGM();
                LucidAudio.PlayBGM(_soundRepository.GetClip("LastLevel")).SetLoop();
            }

            _subscriber.Subscribe(async x =>
            {
                if (x.Value == GameState.Clear)
                {
                    LucidAudio.PlaySE(_soundRepository.GetClip("Clear")).SetTimeSamples();
                    await _mainCameraAnimation.DoLeaveLevelAnimation();

                    if (Level.IsMaxLevel)
                    {
                        _levelLoader.LoadEnding();
                    }
                    else
                    {
                        _levelLoader.LoadNext();
                    }
                }
                else if (x.Value == GameState.Retry)
                {
                    LucidAudio.PlaySE(_soundRepository.GetClip("Retry")).SetTimeSamples();
                    await _mainCameraAnimation.DoLeaveLevelAnimation();
                    _levelLoader.Load(Level.CurrentLevel);
                }
                else if (x.Value == GameState.GameOver)
                {
                    LucidAudio.PlaySE(_soundRepository.GetClip("GameOver")).SetVolume(0.7f).SetTimeSamples();
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);
                    _levelLoader.Load(Level.CurrentLevel);
                }
            }).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}