using System;
using Cysharp.Threading.Tasks;
using R3;
using u1w_2024_3.Src.Service.Input;
using u1w_2024_3.Src.View;
using VContainer;
using VContainer.Unity;

namespace u1w_2024_3.Src.Service
{
    public sealed class Tutorial4Service : IStartable, IDisposable
    {
        [Inject] private readonly IGameInputProvider _inputProvider;
        [Inject] private readonly TutorialSprite _tutorialSprite;

        private readonly CompositeDisposable _disposable = new();

        private bool _isMoved = false;

        public void Start()
        {
            _inputProvider.CamDown
                .DistinctUntilChanged()
                .Where(x => x)
                .Take(1)
                .Subscribe(_ =>
                {
                    if (_isMoved) return;
                    _tutorialSprite.FadeOut().Forget();
                    _isMoved = true;
                })
                .AddTo(_disposable);

            _inputProvider.CamUp
                .DistinctUntilChanged()
                .Where(x => x)
                .Take(1)
                .Subscribe(_ =>
                {
                    if (_isMoved) return;
                    _tutorialSprite.FadeOut().Forget();
                    _isMoved = true;
                })
                .AddTo(_disposable);

            _inputProvider.CamRight
                .DistinctUntilChanged()
                .Where(x => x)
                .Take(1)
                .Subscribe(_ =>
                {
                    if (_isMoved) return;
                    _tutorialSprite.FadeOut().Forget();
                    _isMoved = true;
                })
                .AddTo(_disposable);

            _inputProvider.CamLeft
                .DistinctUntilChanged()
                .Where(x => x)
                .Take(1)
                .Subscribe(_ =>
                {
                    if (_isMoved) return;
                    _tutorialSprite.FadeOut().Forget();
                    _isMoved = true;
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}