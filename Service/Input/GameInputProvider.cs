using R3;
using VContainer.Unity;
using UnityEngine;

namespace u1w_2024_3.Src.Service.Input
{
    public sealed class GameInputProvider : IGameInputProvider, IInitializable, IStartable, ITickable
    {
        public ReadOnlyReactiveProperty<bool> Jump => _jump;
        private readonly ReactiveProperty<bool> _jump = new();

        public ReadOnlyReactiveProperty<float> Horizontal => _horizontal;
        private readonly ReactiveProperty<float> _horizontal = new();
        
        public ReadOnlyReactiveProperty<bool> CamUp => _camUp;
        private readonly ReactiveProperty<bool> _camUp = new();
        
        public ReadOnlyReactiveProperty<bool> CamDown => _camDown;
        private readonly ReactiveProperty<bool> _camDown = new();
        
        public ReadOnlyReactiveProperty<bool> CamLeft => _camLeft;
        private readonly ReactiveProperty<bool> _camLeft = new();
        
        public ReadOnlyReactiveProperty<bool> CamRight => _camRight;
        private readonly ReactiveProperty<bool> _camRight = new();

        private readonly CompositeDisposable _disposable = new();

        public void Initialize()
        {
            _jump.AddTo(_disposable);
            _horizontal.AddTo(_disposable);
            _camUp.AddTo(_disposable);
            _camDown.AddTo(_disposable);
            _camLeft.AddTo(_disposable);
            _camRight.AddTo(_disposable);
        }

        public void Start()
        {
            _jump.Value = false;
            _horizontal.Value = 0;
            _camUp.Value = false;
            _camDown.Value = false;
            _camLeft.Value = false;
            _camRight.Value = false;
        }

        public void Tick()
        {
            _jump.Value = UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow);
            _horizontal.Value = UnityEngine.Input.GetAxis("Horizontal");
            _camUp.Value = UnityEngine.Input.GetKey(KeyCode.I);
            _camDown.Value = UnityEngine.Input.GetKey(KeyCode.K);
            _camLeft.Value = UnityEngine.Input.GetKey(KeyCode.J);
            _camRight.Value = UnityEngine.Input.GetKey(KeyCode.L);
        }
    }
}