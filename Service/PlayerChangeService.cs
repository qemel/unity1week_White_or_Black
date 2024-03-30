using u1w_2024_3.Src.Model;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace u1w_2024_3.Src.Service
{
    public sealed class PlayerChangeService : ITickable
    {
        [Inject] private readonly PlayerRepository _playerRepository;
        [Inject] private readonly StageField _stageField;


        public void Tick()
        {
            var activePlayerIds = _playerRepository.ActivePlayers;
            foreach (var activeId in activePlayerIds)
            {
                var active = _playerRepository.GetPlayer(activeId);
                var inactive = _playerRepository.GetPairPlayer(activeId);
                if (_stageField.IsOutOfBoundsFloat(active.Position))
                {
                    _playerRepository.SwitchActivePlayer(activeId);
                    inactive.PlayerMovement.SetPosition(_stageField.GetInversePosition(active.Position));
                    inactive.PlayerMovement.SetVelocity(active.PlayerMovement.Velocity);
                    break;
                }
                else if (_stageField.IsOutOfBoundsWithPlayerSize(active.Position, active.PlayerMovement.Size))
                {
                    inactive.PlayerMovement.SetPosition(
                        _stageField.GetInversePositionWithPlayerMargin(active.Position, active.PlayerMovement.Size));
                    break;
                }
                // activeが完全にステージ内に入ったとき
                else if (_stageField.IsInBoundsWithPlayerSize(active.Position, active.PlayerMovement.Size))
                {
                    inactive.PlayerMovement.SetPosition(new Vector2(-100, -100));
                    break;
                }
            }
        }
    }
}