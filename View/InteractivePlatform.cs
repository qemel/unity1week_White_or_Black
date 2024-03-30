using Alchemy.Inspector;
using R3;
using R3.Triggers;
using u1w_2024_3.Src.View.Player;
using UnityEngine;

namespace u1w_2024_3.Src.View
{
    public sealed class InteractivePlatform : MonoBehaviour
    {
        [SerializeField] private Vector2 _triggerDirection;
        [SerializeField, ReadOnly] private Vector2 _startPosition;
        [SerializeField] private Vector2 _endPosition;
        [SerializeField] private BoxCollider2D _triggerArea;

        private void Awake()
        {
            _triggerDirection.Normalize();
            _startPosition = transform.position;
        }

        private void Start()
        {
            _triggerArea.OnTriggerStay2DAsObservable()
                .Subscribe(x =>
                {
                    if (x.gameObject.TryGetComponent(out PlayerMovement movement))
                    {
                        Move(movement.transform.position);
                    }
                }).AddTo(this);
        }

        private void Move(Vector2 playerPos)
        {
            // playerPosに合わせて移動する
            // playerPosが_triggerAreaの範囲内のどこに位置するかを0~1で表す
            // 0: _triggerAreaにて、_triggerDirectionのベクトルの0%の位置にいる
            // 1: _triggerAreaにて、_triggerDirectionのベクトルの100%の位置にいる
            var t = Vector2.Dot(playerPos - _startPosition, _triggerDirection) /
                    Vector2.Distance(_startPosition, _endPosition);
            t = Mathf.Clamp01(t);
            var targetPos = Vector2.Lerp(_startPosition, _endPosition, t);
            transform.position = targetPos;
        }
    }
}