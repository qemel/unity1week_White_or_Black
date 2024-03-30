using System;
using System.Linq;
using AnnulusGames.LucidTools.Audio;
using MessagePipe;
using R3;
using u1w_2024_3.Src.Model;
using u1w_2024_3.Src.Model.Message;
using u1w_2024_3.Src.Service;
using u1w_2024_3.Src.Service.Input;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace u1w_2024_3.Src.View.Player
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [Inject] private readonly IGameInputProvider _input;
        [Inject] private readonly ISubscriber<ChangeGravityDirectionEvent> _subscriber;
        [Inject] private readonly ISubscriber<GameStateEvent> _subscriberGameState;
        [Inject] private readonly IPublisher<GameStateEvent> _publisher;
        [Inject] private readonly ISubscriber<CamMovementEvent> _camAnimationEventSubscriber;
        [Inject] private readonly SoundRepository _soundRepository;


        [FormerlySerializedAs("_particleSystem")] [SerializeField]
        private ParticleSystem _gameOverParticle;


        public ReadOnlyReactiveProperty<Vector2> PlayerPosition => _playerPosition;
        private readonly ReactiveProperty<Vector2> _playerPosition = new(Vector2.zero);

        public Vector2 Velocity => _rigidbody.velocity;
        public float Size => _boxCollider.size.x;

        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;

        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private float _jumpingFirstVelocity;

        private const float MaxSpeed = 15f;

        /// <summary>
        /// 干渉するレイヤーマスク
        /// </summary>
        private LayerMask _layerMask;

        /// <summary>
        /// 当たったらゲームオーバーになるレイヤーマスク
        /// </summary>
        private LayerMask _hitToGameOverLayerMask;

        private Vector2 _gravityDirection = new(0, -1);
        private const float RayMargin = 0.05f;

        private float _currentHorizontalInput;
        private bool _isGameOver;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            if (Math.Abs(_boxCollider.size.x - _boxCollider.size.y) > 0.01f)
            {
                Debug.LogError("BoxCollider2Dのサイズが正方形でないと正常に動作しません");
            }

            _playerPosition.AddTo(this);
        }

        public void Init(PlayerInfoByColor playerInfoByColor)
        {
            _layerMask = playerInfoByColor.LayerMask;
            _hitToGameOverLayerMask = playerInfoByColor.HitToGameOverLayerMask;
            _boxCollider.includeLayers = _layerMask;
            _boxCollider.excludeLayers = playerInfoByColor.IgnoreLayerMask;
        }

        private void Start()
        {
            _subscriber.Subscribe(_ =>
            {
                _gravityDirection *= -1;
                Physics2D.gravity = Physics2D.gravity.magnitude * _gravityDirection.normalized;
            }).AddTo(this);
            _subscriberGameState.Subscribe(x =>
            {
                if (x.Value == GameState.Clear)
                {
                    _rigidbody.simulated = false;
                    _boxCollider.enabled = false;
                }
            }).AddTo(this);
            _camAnimationEventSubscriber.Subscribe(x =>
            {
                if (x.Type == CamMovementEvent.EventType.MoveStart)
                {
                    _rigidbody.simulated = false;
                    _boxCollider.enabled = false;
                }
                else if (x.Type == CamMovementEvent.EventType.MoveEnd)
                {
                    _rigidbody.simulated = true;
                    _boxCollider.enabled = true;
                }
            }).AddTo(this);

            _playerPosition.Value = transform.position;
        }


        private void Update()
        {
            _currentHorizontalInput = _input.Horizontal.CurrentValue;
            if (_input.Jump.CurrentValue && IsGrounded())
            {
                var velocityY = -_jumpingFirstVelocity * _gravityDirection.normalized.y;
                SetVelocity(new Vector2(_rigidbody.velocity.x, velocityY));
                LucidAudio.PlaySE(_soundRepository.GetClip("Jump")).SetTimeSamples();
            }

            if (IsInsideGameOverLayer())
            {
                if (!_isGameOver)
                {
                    _publisher.Publish(new GameStateEvent(GameState.GameOver));
                    _gameOverParticle.Play();
                    _rigidbody.simulated = false;
                    _boxCollider.enabled = false;
                    _isGameOver = true;
                }
            }

            _playerPosition.Value = transform.position;
        }

        private void FixedUpdate()
        {
            SetVelocity(new Vector2(_currentHorizontalInput * _horizontalSpeed, _rigidbody.velocity.y));
        }

        private bool IsGrounded()
        {
            var pos = transform.position;
            var size = _boxCollider.size;
            var posLeft = new Vector2(pos.x - size.x * 0.5f + RayMargin, pos.y);
            var posRight = new Vector2(pos.x + size.x * 0.5f - RayMargin, pos.y);
            var rayLength = size.y * 0.5f + 0.1f;
            var hitsDown = Physics2D.RaycastAll(pos, _gravityDirection.normalized, rayLength, _layerMask);
            var hitsLeft = Physics2D.RaycastAll(posLeft, _gravityDirection.normalized, rayLength, _layerMask);
            var hitsRight = Physics2D.RaycastAll(posRight, _gravityDirection.normalized, rayLength, _layerMask);
            
            if (hitsDown.Any(hit => hit.collider != _boxCollider)) return true;
            if (hitsLeft.Any(hit => hit.collider != _boxCollider)) return true;
            if (hitsRight.Any(hit => hit.collider != _boxCollider)) return true;

            return false;
        }

        private bool IsInsideGameOverLayer()
        {
            // detect if player is inside the game over layer of composite collider
            var pos = transform.position;

            // returns if position is inside the game over layer
            return Physics2D.OverlapPoint(pos, _hitToGameOverLayerMask);
        }

        public void SetThisComponentActive(PlayerActiveState active)
        {
            _rigidbody.simulated = active == PlayerActiveState.Active;
            enabled = active == PlayerActiveState.Active;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetVelocity(Vector2 velocity)
        {
            var magnitude = velocity.magnitude;

            if (magnitude > MaxSpeed)
            {
                velocity = velocity.normalized * MaxSpeed;
            }

            _rigidbody.velocity = velocity;
        }
    }
}