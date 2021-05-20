using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IPCA.Monogame;

namespace Tower2
{
    public class AnimatedSprite : GameObject
    {
        protected enum Direction
        {
            Left,
            Right,
        }
        protected Direction _direction = Direction.Right;

        protected List<Texture2D> _textures;
        protected int _currentTexture = 0;
        public Texture2D _texture;

        private int _fps = 10;
        private double _delay => 1.0f / _fps;
        private double _timer = 0f;

        public AnimatedSprite(string name,  Texture2D[] textures, Vector2 pos, Vector2 size) : base(name, pos, size)
        {
            _textures = textures.ToList<Texture2D>();
            _texture = _textures[0];
            size = _texture.Bounds.Size.ToVector2();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (_textures.Count != 1)
            {
                _timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer > _delay)
                {
                    _currentTexture = (_currentTexture + 1) % _textures.Count;
                    _timer = 0.0;
                    _texture = _textures[_currentTexture];
                }
            }
        }

        public virtual void Draw(SpriteBatch sp, GameTime gameTime)
        {
            Vector2 anchor = new Vector2(_texture.Width, _texture.Height) / 2f; //Anchor in the middle

            sp.Draw(_texture, position, null, Color.White,
                _rotation, anchor, size,
                _direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0);
        }
    }
    
}