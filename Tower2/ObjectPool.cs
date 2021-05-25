using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using IPCA.MonoGame;

namespace Tower2
{
    public class ObjectPool
    {
        public static ObjectPool _instance;
        private Game1 game;
        private List<Sprite> _pool;
        private Random rng;
        private int _random;

        public ObjectPool(Game1 game)
        {
            this.game = game;
            _instance = this;
            _pool = new List<Sprite>();
            rng = new Random();

            //Systema para gerar uma seed random no começo do jogo
            //for (float i = 2; i < 20; i = i + 2)
            //{
            //    _random = rng.Next(0, 100);
            //    switch (_random) {
            //        case (_random < 10):
            //            break;






            //    }
            //} 
        }

        
        public void Update()
        {
            rng.Next(0, 100);
        }
    }
}
