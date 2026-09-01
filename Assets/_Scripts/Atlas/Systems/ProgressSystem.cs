using System.Collections.Generic;
using Atlas.DB;
using Atlas.Utility;

namespace Atlas.Core
{
    public class ProgressSystem
    {
        private Queue<Enemy> _progression;

        public void Initialize()
        {
            _progression = new Queue<Enemy>();

            Config.Instance.progression.ForEach(enemy => 
            {
                _progression.Enqueue(enemy.data);
            });
            // for(int i = 0; i < Database.Instance.GetEnemies().Count; i++)
            // {
            //     Enemy current = Database.Instance.GetEnemy(i);
            //     if(!current.transformed)
            //         _progression.Enqueue(current);
            // }
        }

        public Enemy GetNextEnemy()
        {
            return _progression.Dequeue();
        }
    }
}