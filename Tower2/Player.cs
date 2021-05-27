using System.Collections.Generic;
using System.Linq;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IPCA.MonoGame;


namespace Tower2
{
    class Player : AnimatedSprite
    {
        enum Status
        {
            Idle, Walk,
        }
        private Status _status = Status.Idle;

        public static Player _instance;

        private Game1 _game;
        private bool _isGrounded = false;
        private bool canjump = true;

        private List<Texture2D> _idleFrames;
        private List<Texture2D> _walkFrames;

        public Player(Game1 game1) : base("idle", new Vector2(5f, 10f), new Vector2(0.5f, 0.5f), 128f, Enumerable.Range(0, 9).Select(n => game1.Content.Load<Texture2D>($"playersprites/Stand/{n}")).ToArray())
        {
            _instance = this;
            _idleFrames = _textures; // loaded by the base construtor

            _walkFrames = Enumerable.Range(0, 9).Select(n => game1.Content.Load<Texture2D>($"playersprites/Walk/{n}")).ToList();

            _game = game1;


            AddRectangleBody(
                _game.Services.GetService<World>(), _size.X*1.8f, _size.Y*2.4f //Some magic numbers cause collider was offset
            ) ; // kinematic is false by default
            Fixture sensor = FixtureFactory.AttachRectangle(
                _size.X / 3f, _size.Y * 0.05f,
                1, new Vector2(0, -_size.Y -0.1f),
                Body);
            sensor.IsSensor = true;

            sensor.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name != "bullet")
                    _isGrounded = true;
            };
            sensor.OnSeparation = (a, b, contact) => _isGrounded = false;

            KeyboardManager.Register(
                Keys.Space,
                KeysState.GoingDown,
                () =>
                {
                    if (_isGrounded)
                    {
                        Body.ApplyForce(new Vector2(0, 375f));
                        canjump = true;

                    }
                    else if(canjump == true)
                    {
                        canjump = false;
                        Body.ApplyForce(new Vector2(0, 300f));
                    }
                });
            KeyboardManager.Register(
                Keys.A,
                KeysState.Down,
                () => { Body.ApplyForce(new Vector2(-12.5f, 0)); });
            KeyboardManager.Register(
                Keys.D,
                KeysState.Down,
                () => { Body.ApplyForce(new Vector2(12.5f, 0)); });



        }

        public override void Update(GameTime gameTime)
        {
            if (_status == Status.Idle && Body.LinearVelocity.LengthSquared() > 0.001f)
            {
                _status = Status.Walk;
                _textures = _walkFrames;
                _currentTexture = 0;
            }

            if (_status == Status.Walk && Body.LinearVelocity.LengthSquared() <= 0.001f)
            {
                _status = Status.Idle;
                _textures = _idleFrames;
                _currentTexture = 0;
            }

            if (Body.LinearVelocity.X < 0f) _direction = Direction.Left;
            else if (Body.LinearVelocity.X > 0f) _direction = Direction.Right;

            base.Update(gameTime);
            //Camera.LookAt(_position);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}
