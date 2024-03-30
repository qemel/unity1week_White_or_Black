using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace u1w_2024_3.Src.View
{
    public sealed class TutorialSprite : MonoBehaviour
    {
        private SpriteRenderer[] _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        }

        public async UniTask FadeOut()
        {
            foreach (var spriteRenderer in _spriteRenderer)
            {
                LMotion.Create(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1f)
                    .BindToColor(spriteRenderer)
                    .AddTo(gameObject);
            }

            await UniTask.Delay(1000);

            foreach (var spriteRenderer in _spriteRenderer)
            {
                spriteRenderer.gameObject.SetActive(false);
            }
        }   
    }
}