using System;
using System.Collections.Generic;
using System.Text;
using Genbox.VelcroPhysics.Dynamics;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower2
{
    class Tower
    {
        public static Tower _instance;
        private List<Brick> _bricks;
        private Game1 game;
        private List<PlatformBig> _floor;
        public Tower(Game1 game1)
        {
            _instance = this;
            game = game1;
            _bricks = new List<Brick>();
            _floor = new List<PlatformBig>();

            _floor.Add(new PlatformBig(game1, new Vector2(2.5f, 0.25f)));
            _floor.Add(new PlatformBig(game1, new Vector2(7.5f, 0.25f)));

            for (float i = 0; i < 20f; i++) //brick size is (0.5f, 1) so i raises by one
            {
                _bricks.Add(new Brick(game1, new Vector2(0.25f, i+0.5f)));
                _bricks.Add(new Brick(game1, new Vector2(9.75f, i+0.5f)));
            }

        }

        public void Update(GameTime gametime)
        {
           
        }

        public void Draw(SpriteBatch sp, GameTime gametime)
        {
            foreach (Sprite s in _floor) s.Draw(sp, gametime);
            foreach (Sprite s in _bricks)
            {
                s.Draw(sp, gametime);
            }
        }
    }
}
