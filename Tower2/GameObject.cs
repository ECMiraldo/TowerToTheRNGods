using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower2
{
    public class GameObject
    {
        private string name;
        protected float _rotation;
        public Vector2 position;
        public Vector2 size = Vector2.One;

        public GameObject(string name, Vector2 position, Vector2 size)
        {
            this.name = name;
            this.position = position;
            this.size = size;
        }

    }
}
