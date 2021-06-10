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
        public List<GameObject> _pool;
        public List<GameObject> _platforms;
        public List<NPC> _porings;
        private Random rng;
        private int _random;
        private float _height;
        private float heightaux;

        public ObjectPool(Game1 game)
        {
            this.game = game;
            _instance = this;
            _pool = new List<GameObject>();
            _platforms = new List<GameObject>();
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
                    _platforms.Add(plat);
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        float posY = _height - 0.5f;
                        SpikesBig sB = new SpikesBig(game, new Vector2(Xpos, posY));
                        _platforms.Add(sB);
                        Fixture spikes = FixtureFactory.AttachRectangle(sB._size.X, sB._size.Y * 0.1f, 1, new Vector2(0, 0.15f), sB.Body);
                    }
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn poring
                    {
                        float posY = _height + 0.7f;
                        NPC npc = new NPC(game, new Vector2(Xpos, posY));
                        _porings.Add(npc);
                    }
                }
                else
                { //platform small
                    float Xpos = rng.Next(3, 7);
                    PlatformSmall plat = new PlatformSmall(game, new Vector2(Xpos, _height));
                    Body aux = plat.Body;
                    _platforms.Add(plat);
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        float posY = _height - 0.5f;
                        SpikesSmall sB = new SpikesSmall(game, new Vector2(Xpos, posY));
                        _platforms.Add(sB);
                        Fixture spikes = FixtureFactory.AttachRectangle(sB._size.X, sB._size.Y * 0.2f, 1, new Vector2(0, 0.15f), sB.Body);
                    }
                }
            }
            heightaux = 1.5f;
            
        }

        
        public void Update(GameTime gametime)
        {
            
            this.SpawnPlatforms(gametime);
            this.SpawnObjects(gametime);
            for (int i = 0; i < _porings.Count; i++) //poring die animation
            {
                _porings[i].Update(gametime);
                if (_porings[i].IsDead && 
                    _porings[i].Texture[_porings[i].CurrentTexture] == _porings[i].Texture[_porings[i].Texture.Count - 1])
                {
                    _porings[i]._break.Play();
                    _porings.Remove(_porings[i]);
                }
            }

            Player._instance.Body.OnCollision = (a, b, contact) =>
            {
                if (_pool.Contains(b.GameObject()))
                {
                    if (b.GameObject().Name == "coin")
                    {
                        _pool.Remove(b.GameObject());
                        Player.GetCoin();
                    }
                    if (b.GameObject().Name == "crystal")
                    {
                        _pool.Remove(b.GameObject());
                        Player.GetBullet();
                    }
                    if (b.GameObject().Name == "hourglass")
                    {
                        _pool.Remove(b.GameObject());
                        Camera.lowerSpeed();
                    }
                    if (b.GameObject().Name == "redheart")
                    {
                        _pool.Remove(b.GameObject());
                        Player.GetHP();  
                    }
                    if (b.GameObject().Name == "manaheart")
                    {
                        _pool.Remove(b.GameObject());
                        Player.Getmana();
                    }
                }
                };
            foreach (GameObject s in _pool)
            {
                s.Body.OnCollision = (a, b, contact) =>
                {
                    if (b.GameObject().Name == "platform" || b.GameObject().Name == "spikes")
                    {
                        s.Body.Position = s.Body.Position + Vector2.UnitY;
                    }
                };
                s.Update(gametime);
            }








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
                    _platforms.Add(plat);
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        float posY = _height - 0.5f;
                        SpikesBig sB = new SpikesBig(game, new Vector2(Xpos, posY));
                        _platforms.Add(sB);
                        Fixture spikes = FixtureFactory.AttachRectangle(sB._size.X, sB._size.Y * 0.1f, 1, new Vector2(0, 0.15f), sB.Body);
                    }
                    if (_random >= 5)  // Spawn poring
                    {
                        float posY = _height + 0.7f;
                        NPC npc = new NPC(game, new Vector2(Xpos, posY));
                        _porings.Add(npc);
                    }
                }
                else
                { //platform small
                    float Xpos = rng.Next(3, 7);
                    PlatformSmall plat = new PlatformSmall(game, new Vector2(Xpos, _height));
                    Body aux = plat.Body;
                    _platforms.Add(plat);
                    _random = rng.Next(0, 10);
                    if (_random >= 5)  // Spawn spikes
                    {
                        float posY = _height - 0.5f;
                        SpikesSmall sB = new SpikesSmall(game, new Vector2(Xpos, posY));
                        _platforms.Add(sB);
                        Fixture spikes = FixtureFactory.AttachRectangle(sB._size.X, sB._size.Y * 0.2f, 1, new Vector2(0, 0.15f), sB.Body);
                    }  
                }
                _height = _height + 3;
            }

        }

        public void SpawnObjects(GameTime gametime)
        {
            while (heightaux < Camera.Target.Y + 11f)
            {
                float Xpos = rng.Next(0, 10);
                _random = rng.Next(0, 11);
                if (_random > 2) // 80% to spawn something that helps the player
                {
                    _random = rng.Next(0, 21);
                    switch (_random)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            _pool.Add(new Coin(game, new Vector2(Xpos, heightaux)));
                            break;
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            _pool.Add(new Crystal(game, new Vector2(Xpos, heightaux)));
                            break;
                        case 13:
                        case 14:
                        case 15:
                            _pool.Add(new RedHeart(game, new Vector2(Xpos, heightaux)));
                            break;
                        case 8:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                            _pool.Add(new ManaHeart(game, new Vector2(Xpos, heightaux)));
                            break;
                        case 20:
                            _pool.Add(new Hourglass(game, new Vector2(Xpos, heightaux)));
                            break;
                    }
                }
                heightaux = heightaux + 3;
            }

        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (GameObject s in _platforms) s.Draw(spriteBatch, gameTime);
            foreach (GameObject s in _pool) s.Draw(spriteBatch, gameTime);
            foreach (NPC p in _porings) p.Draw(spriteBatch, gameTime);
        }
    }
}
