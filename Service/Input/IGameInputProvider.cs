using R3;

namespace u1w_2024_3.Src.Service.Input
{
    public interface IGameInputProvider
    {
        ReadOnlyReactiveProperty<bool> Jump { get; }
        ReadOnlyReactiveProperty<float> Horizontal { get; }
        ReadOnlyReactiveProperty<bool> CamUp { get; }
        ReadOnlyReactiveProperty<bool> CamDown { get; }
        ReadOnlyReactiveProperty<bool> CamLeft { get; }
        ReadOnlyReactiveProperty<bool> CamRight { get; }
    }
}