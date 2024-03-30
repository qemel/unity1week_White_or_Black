using UnityEngine;

namespace u1w_2024_3.Src.Model
{
    [CreateAssetMenu(menuName = "u1w 2024/PlayerInfoByColor")]
    public class PlayerInfoByColor : ScriptableObject
    {
        public Color Color => _color;

        /// <summary>
        /// 干渉するブロックのレイヤーマスク
        /// </summary>
        public LayerMask LayerMask => _layerMask;

        /// <summary>
        /// ゲームオーバーとなるレイヤーマスク
        /// </summary>
        public LayerMask HitToGameOverLayerMask => _layerMask;
        
        /// <summary>
        /// 当たり判定を無視するレイヤーマスク
        /// </summary>
        public LayerMask IgnoreLayerMask => _ignoreLayerMask;

        [SerializeField] private Color _color;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private LayerMask _ignoreLayerMask;
    }
}