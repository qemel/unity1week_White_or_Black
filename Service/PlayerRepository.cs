using System;
using System.Collections.Generic;
using u1w_2024_3.Src.Model;
using u1w_2024_3.Src.View.Player;

namespace u1w_2024_3.Src.Service
{
    public sealed class PlayerRepository
    {
        public IReadOnlyDictionary<PlayerId, PlayerId> PlayerPairs => _playerPairs;
        public IReadOnlyList<PlayerId> ActivePlayers => _activePlayers;

        private readonly Dictionary<PlayerId, PlayerId> _playerPairs = new();
        private readonly Dictionary<PlayerId, PlayerCore> _players = new();
        private readonly List<PlayerId> _activePlayers = new();

        public PlayerRepository()
        {
            Reset();
        }

        public void AddPlayer(PlayerCore active, PlayerCore inactive)
        {
            _playerPairs.Add(active.PlayerId, inactive.PlayerId);
            _playerPairs.Add(inactive.PlayerId, active.PlayerId);
            _players.Add(active.PlayerId, active);
            _players.Add(inactive.PlayerId, inactive);

            _activePlayers.Add(active.PlayerId);
        }

        public PlayerCore GetPlayer(PlayerId playerId)
        {
            return _players[playerId];
        }

        /// <summary>
        /// ペア相手のプレイヤーを取得
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public PlayerCore GetPairPlayer(PlayerId playerId)
        {
            return _players[_playerPairs[playerId]];
        }

        public bool IsPlayerActive(PlayerId playerId)
        {
            return _players[playerId].ActiveState == PlayerActiveState.Active;
        }

        /// <summary>
        /// アクティブplayerを切り替える。playerが非アクティブの場合は何もしない
        /// </summary>
        /// <param name="playerId"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SwitchActivePlayer(PlayerId playerId)
        {
            var pairPlayer = GetPairPlayer(playerId);
            if (pairPlayer.ActiveState == PlayerActiveState.Active)
            {
                return;
            }

            _players[playerId].SetActive(PlayerActiveState.Inactive);
            _players[pairPlayer.PlayerId].SetActive(PlayerActiveState.Active);

            _activePlayers.Remove(playerId);
            _activePlayers.Add(pairPlayer.PlayerId);
        }

        public void Reset()
        {
            _playerPairs.Clear();
            _players.Clear();
            _activePlayers.Clear();
        }
    }
}