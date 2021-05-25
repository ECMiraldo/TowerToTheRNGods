using System;
using System.Collections.Generic;
using System.Text;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Genbox.VelcroPhysics.Utilities;
using Genbox.VelcroPhysics.Dynamics;

namespace Tower2
{
    public class Brick : Sprite
    {
        private Game1 game;
        public Brick(Game1 game, Vector2 position, bool offset = false) : 
                base("brick", game.Content.Load<Texture2D>("brick sprite"), position, new Vector2(0.5f, 1f))
        {
            this.game = game;
            AddRectangleBody(this.game.Services.GetService<World>(), _size.X, _size.Y);
            Body.BodyType = BodyType.Static;
        }





    }
}