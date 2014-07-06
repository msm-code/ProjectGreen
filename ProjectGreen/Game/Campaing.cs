using System.Collections.Generic;

namespace ProjectGreen.Game
{
    class Campaign
    {
        readonly List<string> levels;
        IGameState previousState;
        int currentLevel;

        public Campaign(params string[] levels)
        {
            if (levels.Length == 0)
            { throw new System.ArgumentException("No levels in campaign"); }

            this.levels = new List<string>(levels);
        }

        public LevelState Begin(IGameState previousState)
        {
            this.previousState = previousState;
            this.currentLevel = 0;
            return new LevelState(this, levels[0]);
        }

        public LevelState Continue(IGameState previousState)
        {
            return Begin(previousState); 
        }

        public IGameState MoveToNextLevel()
        {
            currentLevel++;

            if (currentLevel >= levels.Count)
            { return previousState; }

            return new LevelState(this, levels[currentLevel]);
        }

        public static readonly Campaign Main = new Campaign("map", "map", "map");
    }
}
