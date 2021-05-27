using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower2
{
    public class ObjectPool
    {
        //Largura da torre = 10f - bricks.size*2
        //Bricks.size = 0.5
        // Largura da torre = 9f
        // Range -- 0.5f ate 9.5f
        // Quando spawnar um objecto pos necessita ser esse numero - size.x



        public static ObjectPool _instance;
        private Game1 game;
        private List<GameObject> _pool;
        private Random rng;
        private int _random;

        public ObjectPool(Game1 game)
        {
            this.game = game;
            _instance = this;
            _pool = new List<GameObject>();
            rng = new Random();

            //Systema para gerar uma seed random no começo do jogo
            for (float i = 2; i < 20; i = i + 2)
            {
                _random = rng.Next(0, 10);
                if (_random >= 5)  //platform big
                {
                    float Xpos = GetRandomX();
                    _pool.Add(new PlatformBig(game, new Vector2(Xpos, i)));
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        _pool.Add(new SpikesBig(game, new Vector2(Xpos, i-0.5f)));
                    }
                }
                else { //platform small
                    float Xpos = GetRandomX();
                    _pool.Add(new PlatformSmall(game, new Vector2(Xpos, i)));
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        _pool.Add(new SpikesSmall(game, new Vector2(Xpos, i-0.5f)));
                    }
                }
            }
        }

        
        public void Update()
        {
            rng.Next(0, 100);
        }



        public float GetRandomX()
        {
            float aux = rng.Next(0, 10);
            if (aux > 9.5f) aux = aux - 0.5f;
            if (aux < 0.5f) aux = aux + 0.5f;
            return aux;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (GameObject s in _pool)
            {
                s.Draw(spriteBatch, gameTime);
            }
        }
    }
}
