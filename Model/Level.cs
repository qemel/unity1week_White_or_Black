namespace u1w_2024_3.Src.Model
{
    public static class Level
    {
        public static int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                _currentLevel = value;
                if (AchievedLevel < value)
                {
                    AchievedLevel = value;
                }
            }
        }
        
        public static bool IsMaxLevel => CurrentLevel >= MaxLevel;
        
        /// <summary>
        /// 実際に表示されるレベル
        /// </summary>
        public static int CurrentDisplayLevel => CurrentLevel + 1;

        private static int _currentLevel = 0;
        
        public static int MaxLevel { get; private set; } = 7;

        public static int AchievedLevel { get; private set; } = 0;

        public static int LevelCount { get; private set; } = 1;

        public static void Reset()
        {
            CurrentLevel = 0;
        }
    }
}