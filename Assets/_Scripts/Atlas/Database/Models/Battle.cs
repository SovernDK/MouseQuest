using System;
namespace Atlas.DB
{
    [Serializable]
    public class Battle
    {
        public int id;
        public string name;
        public EnemyPrototype enemyPrototype;
    }
}
