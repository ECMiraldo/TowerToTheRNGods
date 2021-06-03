using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tower2;

namespace IPCA.MonoGame
{
    public class Explosion : AnimatedSprite, ITempObject
    {
        private bool cycled = false;
        public bool IsDead() => _currentTexture == 0 && cycled;

        public Explosion(Game game, Vector2 position) : base(
            "explosion", position, new Vector2(0.75f,0.75f), 32f,
            Enumerable
                .Range(0, 25)
                .Select(
                    n => game.Content.Load<Texture2D>($"Explosion/explosion_{n}")
                )
                .ToArray()
        )
        {
            _fps = 20;
        }

        public override void Update(GameTime gameTime)
        {
            if (_currentTexture > 0) cycled = true;
            base.Update(gameTime);
        }
    }
}