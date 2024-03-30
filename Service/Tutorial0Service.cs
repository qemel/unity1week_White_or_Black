using System;
using Cysharp.Threading.Tasks;
using R3;
using u1w_2024_3.Src.Service.Input;
using u1w_2024_3.Src.View;
using VContainer;
using VContainer.Unity;

namespace u1w_2024_3.Src.Service
{
    public sealed class Tutorial0Service : IStartable, IDisposable
    {
        [Inject] private readonly IGameInputProvider _inputProvider;
        [Inject] private readonly TutorialSprite _tutorialSprite;

        private readonly CompositeDisposable _disposable = new();

        public void Start()
        {
            _inputProvider.Horizontal
                .DistinctUntilChanged()
                .Where(x => x != 0)
                .Take(1)
                .Subscribe(_ =>
                {
                    _tutorialSprite.FadeOut().Forget();
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}