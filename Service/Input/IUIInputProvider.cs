using R3;

namespace u1w_2024_3.Src.Service.Input
{
    public interface IUIInputProvider
    {
        ReadOnlyReactiveProperty<float> RetryPressedTime { get; }
    }
}