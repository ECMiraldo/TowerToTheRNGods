using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower2
{
    public class GameObject
    {
        protected float _rotation;
        protected Vector2 _position;
        protected Vector2 _size = Vector2.One;
        public Vector2 Position => _position;
        public Vector2 Size
        {
            get;
            set;
        }

        public GameObject(Vector2 position)
        {
            _position = position;
        }

        public virtual void Update()
        {

        }

    }
}
