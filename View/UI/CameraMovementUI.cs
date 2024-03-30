using MessagePipe;
using R3;
using TMPro;
using u1w_2024_3.Src.Model;
using u1w_2024_3.Src.Model.Message;
using UnityEngine;
using VContainer;

namespace u1w_2024_3.Src.View.UI
{
    public sealed class CameraMovementUI : MonoBehaviour
    {
        [Inject] private readonly ISubscriber<CamMovementEvent> _camMovementEventSubscriber;
        [Inject] private readonly CameraMaxMovement _cameraMaxMovement;

        [SerializeField] private TextMeshProUGUI _xMovementMax;
        [SerializeField] private TextMeshProUGUI _yMovementMax;

        [SerializeField] private TextMeshProUGUI _xMovementCurrent;
        [SerializeField] private TextMeshProUGUI _yMovementCurrent;

        private int _currentX;
        private int _currentY;

        private void Start()
        {
             SetMaxMovement(_cameraMaxMovement.MaxX, _cameraMaxMovement.MaxY); 
            _camMovementEventSubscriber.Subscribe(e =>
            {
                if (e.Type == CamMovementEvent.EventType.StageFieldMoved)
                {
                    SetCurrentMovement(e.Move.x, e.Move.y);
                }
            }).AddTo(this);

            SetCurrentMovement(0, 0);
        }

        private void SetMaxMovement(int x, int y)
        {
            _xMovementMax.text = $"{-x} <= x <= {x}";
            _yMovementMax.text = $"{-y} <= y <= {y}";
        }

        private void SetCurrentMovement(int curX, int curY)
        {
            _currentX += curX;
            _currentY += curY;
            
            _xMovementCurrent.text = $"x = {_currentX}";
            _yMovementCurrent.text = $"y = {_currentY}";
        }
    }
}