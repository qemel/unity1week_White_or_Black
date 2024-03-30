using System;
using u1w_2024_3.Src.Model;
using u1w_2024_3.Src.View;
using u1w_2024_3.Src.View.Player;
using UnityEngine;
using VContainer;

namespace u1w_2024_3.Src.Service
{
    public sealed class PlayerSpawner : MonoBehaviour
    {
        [Inject] private readonly Func<PlayerInfoByColor, Vector3, PlayerActiveState, PlayerCore> _playerFactory;
        [Inject] private readonly PlayerRepository _playerRepository;

        [SerializeField] private PlayerSpawnMarker[] _playerSpawnMarkers;
        [SerializeField] private SubPlayerRefuge _subPlayerRefuge;


        private void Start()
        {
            for (var i = 0; i < _playerSpawnMarkers.Length; i++)
            {
                var playerSpawnMarker = _playerSpawnMarkers[i];
                var active = _playerFactory(playerSpawnMarker.PlayerInfoByColor, playerSpawnMarker.Position,
                    PlayerActiveState.Active);
                var inactive = _playerFactory(playerSpawnMarker.SubPlayerInfoByColor, _subPlayerRefuge.Position,
                    PlayerActiveState.Inactive);
                _playerRepository.AddPlayer(active, inactive);
            }
        }
    }
}