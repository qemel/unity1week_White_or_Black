using MessagePipe;
using R3;
using u1w_2024_3.Src.Model.Message;
using u1w_2024_3.Src.Service.Input;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using SingleAssignmentDisposable = R3.SingleAssignmentDisposable;

namespace u1w_2024_3.Src.View.UI
{
    public sealed class RetryUI : MonoBehaviour
    {
        [Inject] private readonly IUIInputProvider _uiInputProvider;
        [Inject] private readonly IPublisher<GameStateEvent> _publisher;


        [SerializeField] private Slider _retryTimeSlider;
        [SerializeField] private float _retryTime;


        private bool _isDone = false;

        private void Start()
        {
            // _uiInputProvider.RetryPressedTime.DistinctUntilChanged().Subscribe(time =>
            // {
            //     SetRetryTime(time);
            //     SetActive(time > 0);
            //     if (time >= _retryTime)
            //     {
            //         if (_isDone) return;
            //         _publisher.Publish(new GameStateEvent(GameState.Retry));
            //         _isDone = true;
            //     }
            // }).AddTo(this);

            var disposable = new SingleAssignmentDisposable();
            disposable.Disposable = _uiInputProvider.RetryPressedTime.DistinctUntilChanged().Subscribe(time =>
            {
                SetRetryTime(time);
                SetActive(time > 0);
                if (time >= _retryTime)
                {
                    _publisher.Publish(new GameStateEvent(GameState.Retry));
                    disposable.Dispose(); // 購読の解除
                }
            });
        }

        private void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void SetRetryTime(float time)
        {
            _retryTimeSlider.value = time;
        }
    }
}