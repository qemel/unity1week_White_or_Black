using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using MessagePipe;
using u1w_2024_3.Src.Model.Message;
using u1w_2024_3.Src.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace u1w_2024_3.Src.View.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public sealed class MainCameraAnimation : MonoBehaviour
    {
        [Inject] private readonly IPublisher<GameStateEvent> _publisher;


        private UnityEngine.Camera _camera;

        [FormerlySerializedAs("_scaleUpTowardPosition")] [SerializeField]
        private Vector3 _scaleDownFromPosition;

        [SerializeField] private Vector3 _scaleUpTowardPosiiton;

        [SerializeField] private Ease _ease;
        [SerializeField] private float _scaleUpTowardSize;
        [SerializeField] private float _duration;
        [SerializeField] private bool _activateManually;


        private float _defaultSize;
        private CancellationToken _token;
        private const float PositionZ = -10;

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            if (_camera != UnityEngine.Camera.main)
            {
                Debug.LogError("MainCameraAnimation is not MainCameraAnimation");
            }

            _token = this.GetCancellationTokenOnDestroy();
        }

        private async UniTask Start()
        {
            _defaultSize = _camera.orthographicSize;
            
            transform.position = new Vector3(_scaleUpTowardPosiiton.x, _scaleUpTowardPosiiton.y, PositionZ);
            _camera.orthographicSize = _scaleUpTowardSize;
            if (_activateManually) return;

            await DoEnterLevelAnimation();
            _publisher.Publish(new GameStateEvent(GameState.CamMovable));
        }

        public async UniTask DoEnterLevelAnimation()
        {
            _camera.orthographicSize = _scaleUpTowardSize;
            transform.position = new Vector3(_scaleDownFromPosition.x, _scaleDownFromPosition.y, PositionZ);

            LMotion
                .Create(_scaleDownFromPosition, _camera.transform.position, _duration)
                .WithEase(_ease)
                .BindToPosition(transform)
                .AddTo(gameObject);

            await LMotion
                .Create(_scaleUpTowardSize, _defaultSize, _duration)
                .WithEase(_ease)
                .BindToCameraSize(_camera)
                .ToUniTask(cancellationToken: _token);
        }

        public async UniTask DoLeaveLevelAnimation()
        {
            LMotion
                .Create(_camera.transform.position, _scaleUpTowardPosiiton, _duration)
                .WithEase(_ease)
                .BindToPosition(transform)
                .AddTo(gameObject);

            await LMotion
                .Create(_camera.orthographicSize, _scaleUpTowardSize, _duration)
                .WithEase(_ease)
                .BindToCameraSize(_camera)
                .ToUniTask(cancellationToken: _token);
        }
    }
}