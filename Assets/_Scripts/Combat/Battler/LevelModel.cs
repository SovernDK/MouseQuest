namespace CharacterSheet 
{
    public class LevelModel : IModel
    {
        private int _currentLevel;
        private float _currentExp;
        private float _nextLevelExp;

        private bool _levelUp;

        public bool LevelUp { get => _levelUp; set => _levelUp = value; }
        public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
        public float CurrentExp { get => _currentExp; set => _currentExp = value; }
        public float NextLevelExp { get => _nextLevelExp; set => _nextLevelExp = value; }

        public LevelModel(int level, float currentExp, float nextLevelExp)
        {
            _currentLevel = level;
            _currentExp = currentExp;
            _nextLevelExp = nextLevelExp;
        }

        public void Setlevel(int newLevel, float nextLevelExp)
        {
            _currentLevel = newLevel;
            _nextLevelExp = nextLevelExp;

            _levelUp = false;
        }

        public void IncreaseExp(float add)
        {
            _currentExp += add;
            if(_currentExp >= _nextLevelExp)
            {
                _levelUp = true;
            }
        }
    }
}