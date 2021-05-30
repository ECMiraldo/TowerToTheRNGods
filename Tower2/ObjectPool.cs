using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;

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
        private List<NPC> _porings;
        private Random rng;
        private int _random;
        private float _height;

        public ObjectPool(Game1 game)
        {
            this.game = game;
            _instance = this;
            _pool = new List<GameObject>();
            _porings = new List<NPC>();
            rng = new Random();

            //Systema para gerar uma seed random no começo do jogo
            for (_height = 3; _height <= 21; _height = _height + 3)
            {
                _random = rng.Next(0, 10);
                if (_random >= 5)  //platform big
                {
                    float Xpos = rng.Next(3, 7);
                    PlatformBig plat = new PlatformBig(game, new Vector2(Xpos, _height));
                    Body aux = plat.Body;
                    _pool.Add(plat);
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        float posY = _height - 0.5f;
                        SpikesBig sB = new SpikesBig(game, new Vector2(Xpos, posY));
                        _pool.Add(sB);
                        Fixture spikes = FixtureFactory.AttachRectangle(sB._size.X, sB._size.Y * 0.1f, 1, new Vector2(0, 0.15f), sB.Body);
                    }
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn poring
                    {
                        float posY = _height + 0.5f;
                        NPC npc = new NPC(game, new Vector2(Xpos, posY));
                        _porings.Add(npc);
                    }
                }
                else
                { //platform small
                    float Xpos = rng.Next(3, 7);
                    PlatformSmall plat = new PlatformSmall(game, new Vector2(Xpos, _height));
                    Body aux = plat.Body;
                    _pool.Add(plat);
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        float posY = _height - 0.5f;
                        SpikesSmall sB = new SpikesSmall(game, new Vector2(Xpos, posY));
                        _pool.Add(sB);
                        Fixture spikes = FixtureFactory.AttachRectangle(sB._size.X, sB._size.Y * 0.2f, 1, new Vector2(0, 0.15f), sB.Body);
                    }
                }
            }
        }

        
        public void Update(GameTime gametime)
        {
            this.SpawnPlatforms(gametime);
            for (int i = 0; i < _porings.Count; i++)
            {
                _porings[i].Update(gametime);
                if (_porings[i].IsDead && 
                    _porings[i].Texture[_porings[i].CurrentTexture] == _porings[i].Texture[_porings[i].Texture.Count - 1])
                {
                    _porings.Remove(_porings[i]);
                }
            }








        }



        public float GetRandomX()
        {
            float aux = rng.Next(0, 10);
            if (aux > 9.5f) aux = aux - 0.5f;
            if (aux < 0.5f) aux = aux + 0.5f;
            return aux;
        }

        public void SpawnPlatforms(GameTime gametime)
        {
            
            while(_height < Camera.Target.Y + 11f)
            {
                _random = rng.Next(0, 10);
                if (_random >= 5)  //platform big
                {
                    float Xpos = rng.Next(3, 7);
                    PlatformBig plat = new PlatformBig(game, new Vector2(Xpos, _height));
                    Body aux = plat.Body;
                    _pool.Add(plat);
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        float posY = _height - 0.5f;
                        SpikesBig sB = new SpikesBig(game, new Vector2(Xpos, posY));
                        _pool.Add(sB);
                        Fixture spikes = FixtureFactory.AttachRectangle(sB._size.X, sB._size.Y * 0.1f, 1, new Vector2(0, 0.15f), sB.Body);
                    }
                    if (_random >= 5)  // Spawn poring
                    {
                        float posY = _height + 0.5f;
                        NPC npc = new NPC(game, new Vector2(Xpos, posY));
                        _porings.Add(npc);
                    }
                }
                else
                { //platform small
                    float Xpos = rng.Next(3, 7);
                    PlatformSmall plat = new PlatformSmall(game, new Vector2(Xpos, _height));
                    Body aux = plat.Body;
                    _pool.Add(plat);
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        float posY = _height - 0.5f;
                        SpikesSmall sB = new SpikesSmall(game, new Vector2(Xpos, posY));
                        _pool.Add(sB);
                        Fixture spikes = FixtureFactory.AttachRectangle(sB._size.X, sB._size.Y * 0.2f, 1, new Vector2(0, 0.15f), sB.Body);
                    }  
                }
                _height = _height + 3;
            }

        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (GameObject s in _pool)
            {
                s.Draw(spriteBatch, gameTime);
            }

            foreach (NPC p in _porings)
            {
                p.Draw(spriteBatch, gameTime);
            }
        }
    }
}
