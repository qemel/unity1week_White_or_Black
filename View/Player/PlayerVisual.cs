using u1w_2024_3.Src.Model;
using UnityEngine;

namespace u1w_2024_3.Src.View.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PlayerVisual : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetColor(PlayerInfoByColor playerInfoByColor)
        {
            _spriteRenderer.color = playerInfoByColor.Color;
        }
    }
}