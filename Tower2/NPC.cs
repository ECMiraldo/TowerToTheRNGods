using System;
using System.Collections.Generic;
using System.Linq;
using Genbox.VelcroPhysics.Collision.RayCast;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Tower2
{
    public class NPC : AnimatedSprite
    {
        private Game1 _game;
     
        private List<Texture2D> _idleFrames;
        private List<Texture2D> _deadFrames;
        private Vector2 _startingPoint;
        private bool isDead = false;
        public bool IsDead => isDead;

        public NPC(Game1 game, Vector2 position) : base ("npc", position, new Vector2(1.2f,0.6f), 64f, 
                                        Enumerable.Range(1,4).Select(n => game.Content.Load<Texture2D>($"Poringsprites/Standing/standing{n}")).ToArray())
        {
            _idleFrames = _textures; // loaded by the base construtor
            _deadFrames = Enumerable.Range(1, 5).Select(n => game.Content.Load<Texture2D>($"Poringsprites/Dying/hurt{n}")).ToList();
            _direction = Direction.Right;
            
            _game = game;
            _startingPoint = position;
           
            AddRectangleBody(_game.Services.GetService<World>(),width: _size.X / 2f); // kinematic is false by default
            Body.LinearVelocity = -Vector2.UnitX;

            Fixture sensor = FixtureFactory.AttachRectangle(_size.X /2, _size.Y,1, new Vector2(0,0),Body);
            sensor.IsSensor = true;
            Body.Friction = 0f;
            
            sensor.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name == "bullet") isDead = true;
            };
            sensor.OnSeparation = (a, b, contact) =>
            {

            };

            Fixture top = FixtureFactory.AttachRectangle(_size.X /3f, _size.Y * 0.1f, 1, new Vector2(0, _size.Y / 2f - 0.01f),Body);
            top.IsSensor = true;

            top.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name == "player") isDead = true;
            };

        }

        public override void Update(GameTime gameTime)
        {
            // Patrolling
            float _patroldistance = 1.8f;
            if (_direction == Direction.Left)
            {
                if (Body.Position.X > _startingPoint.X + _patroldistance)
                {
                    _direction = Direction.Right;
                    Body.LinearVelocity = -Vector2.UnitX;
                }

            }
            else if (_direction == Direction.Right)
            {
                if (Body.Position.X < _startingPoint.X - _patroldistance)
                {
                    _direction = Direction.Left;
                    Body.LinearVelocity = Vector2.UnitX;
                }
            }
            if (Body.LinearVelocity.LengthSquared() < 1) Body.LinearVelocity = Vector2.UnitX;
            if (isDead)
            {
                Body.Enabled = false;
                Body.ResetDynamics();
                _textures = _deadFrames;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sp, GameTime gametime)
        {
            base.Draw(sp, gametime);
        }
    }
}