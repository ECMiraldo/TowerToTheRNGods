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
        private List<Sprite> _bricks;
        private Sprite _texture;
        private Game1 game;
        private float sizemult = 4f;
        public Tower(Game1 game1)
        {
            _instance = this;
            game = game1;
            _bricks = new List<Sprite>();
            _texture = new Sprite("brick", game1.Content.Load<Texture2D>("brick sprite"), new Vector2(0, 0), sizemult);
            _texture._size = new Vector2(1, 2);
            //foreach(Sprite s in _bricks)
            //{
            //    s.AddRectangleBody(game.Services.GetService<World>(), s.Size.X * 1.8f, s.Size.Y * 2.4f); //Some magic numbers cause collider was offset
            //    s.Body.BodyType = BodyType.Static;

            //}
        }
        public void Update(GameTime gametime)
        {
            for (float i = 0; i < Player._instance.Position.Y + 10; i = i + _texture._size.Y)
            {
                _bricks.Add(new Sprite("brick", game.Content.Load<Texture2D>("brick sprite"), new Vector2(0, i)));
                _bricks.Add(new Sprite("brick", game.Content.Load<Texture2D>("brick sprite"), new Vector2(10f, i)));
            }

            foreach (Sprite s in _bricks)
            {
                s.AddRectangleBody(game.Services.GetService<World>(), s._size.X /(sizemult*2), s._size.Y /(sizemult*2)); //Some magic numbers cause collider was offset
                s.Body.BodyType = BodyType.Static;

            }
            for(int i = 0; i<_bricks.Count; i++)
            {
                if (_bricks[i].Position.Y < Player._instance.Position.Y) _bricks.Remove(_bricks[i]);
            }
        }

        public void Draw(SpriteBatch sp, GameTime gametime)
        {
            foreach (Sprite s in _bricks)
            {
                s.Draw(sp, gametime);
            }
        }
    }
}
