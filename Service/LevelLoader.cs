using u1w_2024_3.Src.Model;
using UnityEngine.SceneManagement;

namespace u1w_2024_3.Src.Service
{
    public sealed class LevelLoader
    {
        public void Load(int level)
        {
            Level.CurrentLevel = level;
            SceneManager.LoadScene($"Level{level}");
        }

        public void LoadNext()
        {
            Level.CurrentLevel++;
            Load(Level.CurrentLevel);
        }
        
        public void LoadEnding()
        {
            SceneManager.LoadScene("Ending");
        }
    }
}