using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;


namespace Tower2
{
    public class ObjectPool
    {
        public static ObjectPool _instance;
        private Game1 game;
        private List<Object> _pool;
        private Random random;

        public ObjectPool(Game1 game)
        {
            this.game = game;
            _instance = this;
            _pool = new List<Object>();
            random = new Random();
        }

        public void Update()
        {
            random.Next(0, 100);
        }
    }
}
