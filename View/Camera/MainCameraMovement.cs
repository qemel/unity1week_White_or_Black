using System;
using System.Threading;
using AnnulusGames.LucidTools.Audio;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using MessagePipe;
using R3;
using u1w_2024_3.Src.Model;
using u1w_2024_3.Src.Model.Message;
using u1w_2024_3.Src.Service;
using u1w_2024_3.Src.Service.Input;
using UnityEngine;
using VContainer;

namespace u1w_2024_3.Src.View.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public sealed class MainCameraMovement : MonoBehaviour
    {
        [Inject] private readonly StageField _stageField;
        [Inject] private readonly IGameInputProvider _input;
        [Inject] private readonly IPublisher<CamMovementEvent> _publisher;
        [Inject] private readonly ISubscriber<GameStateEvent> _subscriber;
        [Inject] private readonly SoundRepository _soundRepository;
        [Inject] private readonly CameraMaxMovement _movement;
        
        private UnityEngine.Camera _camera;

        [SerializeField] private float _duration;


        private Vector2 _currentDiff;

        private bool _isAnimating;
        private bool _canInput;

        private CancellationToken _token;


        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            if (_camera != UnityEngine.Camera.main)
            {
                throw new InvalidOperationException("MainCameraMovement must be attached to the main camera.");
            }

            _token = this.GetCancellationTokenOnDestroy();

            // カメラの大きさ、位置から左右の境界を計算
            var leftBound　= _camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
            var rightBound = _camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
            var upperBound = _camera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
            var lowerBound = _camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

            var leftBoundInt = (int)leftBound;
            var rightBoundInt = (int)rightBound - 1;
            var upperBoundInt = (int)upperBound;
            var lowerBoundInt = (int)lowerBound + 1;

            _stageField.SetBoundsGrid(upperBoundInt, lowerBoundInt, leftBoundInt, rightBoundInt);

            if (leftBoundInt == -16 && rightBoundInt == 15 && upperBoundInt == 9 && lowerBoundInt == -8)
            {
                Debug.Log("Camera bounds are correct.");
            }
            else
            {
                Debug.LogWarning($"Camera bounds: {leftBound}, {rightBound}, {upperBound}, {lowerBound}");
            }
        }

        private void Start()
        {
            _subscriber.Subscribe(x =>
            {
                if (x.Value == GameState.CamMovable)
                {
                    _canInput = true;
                }
            }).AddTo(this);

            _input.CamDown
                .DistinctUntilChanged()
                .Where(x => x)
                .Where(_ => _canInput)
                .Subscribe(x =>
                {
                    if (CanMove(new Vector2Int(0, -1)))
                    {
                        Move(new Vector2Int(0, -1)).Forget();
                        LucidAudio.PlaySE(_soundRepository.GetClip("CamMove")).SetVolume(0.8f).SetTimeSamples();
                    }
                }).AddTo(this);

            _input.CamUp
                .DistinctUntilChanged()
                .Where(x => x)
                .Where(_ => _canInput)
                .Subscribe(x =>
                {
                    if (CanMove(new Vector2Int(0, 1)))
                    {
                        Move(new Vector2Int(0, 1)).Forget();
                        LucidAudio.PlaySE(_soundRepository.GetClip("CamMove")).SetVolume(0.8f).SetTimeSamples();
                    }
                }).AddTo(this);

            _input.CamLeft
                .DistinctUntilChanged()
                .Where(x => x)
                .Where(_ => _canInput)
                .Subscribe(x =>
                {
                    if (CanMove(new Vector2Int(-1, 0)))
                    {
                        Move(new Vector2Int(-1, 0)).Forget();
                        LucidAudio.PlaySE(_soundRepository.GetClip("CamMove")).SetVolume(0.8f).SetTimeSamples();
                    }
                }).AddTo(this);

            _input.CamRight
                .DistinctUntilChanged()
                .Where(x => x)
                .Where(_ => _canInput)
                .Subscribe(x =>
                {
                    if (CanMove(new Vector2Int(1, 0)))
                    {
                        Move(new Vector2Int(1, 0)).Forget();
                        LucidAudio.PlaySE(_soundRepository.GetClip("CamMove")).SetVolume(0.8f).SetTimeSamples();
                    }
                }).AddTo(this);
        }

        private bool CanMove(Vector2Int move)
        {
            if (_isAnimating)
            {
                return false;
            }

            if (Mathf.Abs(_currentDiff.x + move.x) > _movement.MaxX)
            {
                return false;
            }

            if (Mathf.Abs(_currentDiff.y + move.y) > _movement.MaxY)
            {
                return false;
            }

            return true;
        }

        private async UniTask Move(Vector2Int move)
        {
            _publisher.Publish(new CamMovementEvent(CamMovementEvent.EventType.MoveStart, move));
            _stageField.Move(move);
            _publisher.Publish(new CamMovementEvent(CamMovementEvent.EventType.StageFieldMoved, move));
            _isAnimating = true;
            var position = transform.position;
            await LMotion.Create(position, position + new Vector3(move.x, move.y), _duration)
                .WithEase(Ease.OutCubic)
                .BindToLocalPosition(transform)
                .ToUniTask(cancellationToken: _token);
            _isAnimating = false;
            _currentDiff += move;
            _publisher.Publish(new CamMovementEvent(CamMovementEvent.EventType.MoveEnd, move));
        }
    }
}