using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace u1w_2024_3.Src.View.UI
{
    public sealed class FloatingUI : MonoBehaviour
    {
        [SerializeField] private float _height;
        [SerializeField] private float _duration;
        
        private void Start()
        {
            LMotion.Create(new Vector3(0, 0, 0), new Vector3(0, _height, 0), _duration)
                .WithLoops(-1, LoopType.Yoyo)
                .WithEase(Ease.InOutSine)
                .BindToPosition(transform)
                .AddTo(gameObject);
        }
    }
}