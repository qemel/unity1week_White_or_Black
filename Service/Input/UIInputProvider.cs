using R3;
using UnityEngine;
using VContainer.Unity;

namespace u1w_2024_3.Src.Service.Input
{
    public sealed class UIInputProvider : IUIInputProvider, IInitializable, IStartable, ITickable
    {
        public ReadOnlyReactiveProperty<float> RetryPressedTime => _retryPressedTime;
        private readonly ReactiveProperty<float> _retryPressedTime = new();

        private readonly CompositeDisposable _disposable = new();

        public void Initialize()
        {
            _retryPressedTime.AddTo(_disposable);
        }

        public void Start()
        {
            _retryPressedTime.Value = 0;
        }

        public void Tick()
        {
            _retryPressedTime.Value += UnityEngine.Input.GetKey(KeyCode.R) ? Time.deltaTime : 0;
            if (UnityEngine.Input.GetKeyUp(KeyCode.R))
            {
                _retryPressedTime.Value = 0;
            }
        }
    }
}