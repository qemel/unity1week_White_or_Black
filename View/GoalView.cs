using MessagePipe;
using R3;
using R3.Triggers;
using u1w_2024_3.Src.Model.Message;
using u1w_2024_3.Src.View.Player;
using UnityEngine;
using VContainer;

namespace u1w_2024_3.Src.View
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class GoalView : MonoBehaviour
    {
        [Inject] private readonly IPublisher<GameStateEvent> _publisher;

        private ParticleSystem _particleSystem;
        private Collider2D _collider2D;
        private bool _isTriggered;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            _collider2D.OnTriggerEnter2DAsObservable().Subscribe(x =>
            {
                if (x.TryGetComponent<PlayerCore>(out _))
                {
                    if (_isTriggered) return;
                    _particleSystem.Play();
                    _publisher.Publish(new GameStateEvent(GameState.Clear));
                    _isTriggered = true;
                }
            }).AddTo(this);
        }
    }
}