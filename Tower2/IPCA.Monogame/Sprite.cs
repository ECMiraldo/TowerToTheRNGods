using System.Collections.Generic;
using System.Linq;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IPCA.Monogame;

namespace Tower2
{
    class Sprite : GameObject
    {
        public Texture2D _texture;

        public Sprite(string name, Texture2D textures, Vector2 pos, Vector2 size) : base(name, pos, size)
        {
            _texture = textures;
            size = _texture.Bounds.Size.ToVector2();
        }

        public virtual void Draw(SpriteBatch sp, GameTime gameTime)
        {
            Vector2 anchor = new Vector2(_texture.Width, _texture.Height) / 2f; //Anchor in the middle
            sp.Draw(_texture, position, null, Color.White, _rotation, anchor, size, SpriteEffects.None, 0);

        }

    }
}