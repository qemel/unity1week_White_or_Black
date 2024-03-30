using LitMotion;
using UnityEngine;

namespace u1w_2024_3.Src.Utils
{
    public static class LitMotionExtension
    {
        public static MotionHandle BindToCameraSize<TOptions, TAdapter>
            (this MotionBuilder<float, TOptions, TAdapter> builder, Camera camera)
            where TOptions : unmanaged, IMotionOptions
            where TAdapter : unmanaged, IMotionAdapter<float, TOptions>
        {
            return builder.BindWithState(camera, (size, cam) => { cam.orthographicSize = size; });
        }
    }
}