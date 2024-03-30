using AnnulusGames.LucidTools.Audio;

namespace u1w_2024_3.Src.Service
{
    public sealed class SoundService
    {
        public void Reset()
        {
            LucidAudio.BGMVolume = 0.3f;
            LucidAudio.SEVolume = 0.3f;
        }
    }
}