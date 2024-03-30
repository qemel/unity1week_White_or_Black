using u1w_2024_3.Src.Model;
using UnityEngine;

namespace u1w_2024_3.Src.View.Player
{
    public sealed class PlayerSpawnMarker : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public PlayerInfoByColor PlayerInfoByColor => _playerInfoByColor;
        [SerializeField] private PlayerInfoByColor _playerInfoByColor;
        
        /// <summary>
        /// 交代時のプレイヤー情報
        /// </summary>
        public PlayerInfoByColor SubPlayerInfoByColor => _subPlayerInfoByColor;
        [SerializeField] private PlayerInfoByColor _subPlayerInfoByColor;   
    }
}