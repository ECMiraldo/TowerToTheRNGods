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
        private List<Texture2D> _walkFrames;
        private Vector2 _startingPoint;

        public NPC(Game1 game, Vector2 position) : base ("npc", position, new Vector2(1.2f,0.6f), 64f, 
                                        Enumerable.Range(1,4).Select(n => game.Content.Load<Texture2D>($"Poringsprites/Standing/standing{n}")).ToArray())
        {
            _idleFrames = _textures; // loaded by the base construtor
            _direction = Direction.Right;
            
            _game = game;
            _startingPoint = position;
            

            AddRectangleBody(
                _game.Services.GetService<World>(),
                width: _size.X / 2f
            ); // kinematic is false by default
            Body.LinearVelocity = -Vector2.UnitX;
            Fixture sensor = FixtureFactory.AttachRectangle(_size.X /2, _size.Y,1, new Vector2(0,0),Body);
            sensor.IsSensor = true;
            Body.Friction = 0f;
            
            sensor.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name == "player") Player._instance._hp = Player._instance._hp - 20;
            };
            sensor.OnSeparation = (a, b, contact) =>
            {

            };

            //Fixture BotLeft = FixtureFactory.AttachRectangle(
            //    _size.X * 0.1f, _size.Y * 0.1f,
            //    1, new Vector2(-_size.X /2f + 0.1f, -_size.Y /2f - 0.01f ),
            //    Body);
            //BotLeft.IsSensor = true;

            //BotLeft.OnCollision = (a, b, contact) =>
            //{

            //};
            //BotLeft.OnSeparation = (a, b, contact) =>
            //{
            //    _direction = Direction.Right;
            //    if (b.GameObject().Name == "platform") _direction = Direction.Right;
            //};

            //Fixture BotRight = FixtureFactory.AttachRectangle(
            //    _size.X * 0.1f, _size.Y * 0.1f,
            //    1, new Vector2(_size.X / 2f - 0.1f, -_size.Y / 2f - 0.01f),
            //    Body);
            //BotRight.IsSensor = true;

            //BotRight.OnCollision = (a, b, contact) =>
            //{

            //};
            //BotRight.OnSeparation = (a, b, contact) =>
            //{
            //    if (b.GameObject().Name == "platform") _direction = Direction.Left;
            //};


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
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sp, GameTime gametime)
        {
            base.Draw(sp, gametime);
        }
    }
}