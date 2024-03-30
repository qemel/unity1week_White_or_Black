using u1w_2024_3.Src.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace u1w_2024_3.Src.Service.EntryPoint
{
    public sealed class EndrollConfigure : LifetimeScope
    {
        [SerializeField] private SoundRepository _soundRepository;
        [SerializeField] private Endroll _endroll;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_soundRepository);
            builder.RegisterComponent(_endroll);
        }
    }
}