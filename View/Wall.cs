using u1w_2024_3.Src.Model;
using UnityEngine;

namespace u1w_2024_3.Src.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class Wall : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        public PlayerInfoByColor PlayerInfoByColor { get; private set; }

        public void Init(PlayerInfoByColor color)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.color = color.Color;
            PlayerInfoByColor = color;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}