using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IPCA.Monogame;

namespace Tower2
{
    class Platform
    {
        private Sprite _sprite;
        private Vector2 pos;
        private Vector2 size;

        public Platform(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;
        }
    }
}
