using R3;
using u1w_2024_3.Src.Model;
using UnityEngine;

namespace u1w_2024_3.Src.View.Player
{
    [RequireComponent(typeof(PlayerVisual), typeof(PlayerMovement))]
    public sealed class PlayerCore : MonoBehaviour
    {
        public PlayerId PlayerId { get; private set; }
        public PlayerMovement PlayerMovement { get; private set; }

        private PlayerVisual _playerVisual;

        public Vector2 Position => PlayerMovement.PlayerPosition.CurrentValue;

        public PlayerActiveState ActiveState => _activeState.Value;
        private readonly ReactiveProperty<PlayerActiveState> _activeState = new(PlayerActiveState.Inactive);

        private void Awake()
        {
            _playerVisual = GetComponent<PlayerVisual>();
            PlayerMovement = GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            _activeState.Subscribe(isActive => { PlayerMovement.SetThisComponentActive(_activeState.Value); })
                .AddTo(this);
        }

        public void Init(PlayerInfoByColor playerInfoByColor, PlayerId id, PlayerActiveState isActive)
        {
            _playerVisual.SetColor(playerInfoByColor);
            PlayerMovement.Init(playerInfoByColor);
            PlayerId = id;
            SetActive(isActive);
        }

        public void SetActive(PlayerActiveState isActive)
        {
            _activeState.Value = isActive;
        }
    }
}