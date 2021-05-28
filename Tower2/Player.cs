using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;


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
        private int _lives;
        private int _coins;



        private Game1 _game;
        private bool _isGrounded = false;
        private bool candoublejump = false;
        private bool canwalljump = false;

        private List<Texture2D> _idleFrames;
        private List<Texture2D> _walkFrames;
        private Texture2D _teleport;
        private List<ITempObject> _objects;

        public Player(Game1 game1) : base("idle", new Vector2(5f, 10f), new Vector2(0.5f, 0.5f), 128f, Enumerable.Range(0, 9).Select(n => game1.Content.Load<Texture2D>($"playersprites/Stand/{n}")).ToArray())
        {
            _instance = this;
            _idleFrames = _textures; // loaded by the base construtor

            _walkFrames = Enumerable.Range(0, 9).Select(n => game1.Content.Load<Texture2D>($"playersprites/Walk/{n}")).ToList();

            _game = game1;
            _teleport = _game.Content.Load<Texture2D>("fireball");
            _objects = new List<ITempObject>();

            AddRectangleBody(
                _game.Services.GetService<World>(), _size.X*1.6f, _size.Y*2.4f //Some magic numbers cause collider was offset
            ) ; // kinematic is false by default
            Fixture Bottom = FixtureFactory.AttachRectangle(
                _size.X, _size.Y * 0.05f,
                1, new Vector2(0, -_size.Y -0.1f),
                Body);
            Bottom.IsSensor = true;

            Bottom.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name == "platform")
                    _isGrounded = true;
            };
            Bottom.OnSeparation = (a, b, contact) => _isGrounded = false;


            Fixture left = FixtureFactory.AttachRectangle(
                _size.X *0.2f, _size.Y,
                1, new Vector2(-_size.X * 0.8f, 0),
                Body);
            left.IsSensor = true;

            left.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name == "brick") canwalljump = true;
            };
            left.OnSeparation = (a, b, contact) => canwalljump = false;

            Fixture right = FixtureFactory.AttachRectangle(
                _size.X * 0.2f, _size.Y,
                1, new Vector2(_size.X * 0.8f, 0),
                Body);
            right.IsSensor = true;

            right.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name == "brick") canwalljump = true;
            };
            right.OnSeparation = (a, b, contact) => canwalljump = false;

            KeyboardManager.Register(
                Keys.Space,
                KeysState.GoingDown,
                () =>
                {
                    if (_isGrounded)
                    {
                        Body.ApplyForce(new Vector2(0, 400f));
                        candoublejump = true;
                    }
                    else if (candoublejump)
                    {
                        candoublejump = false;
                        Body.ApplyForce(new Vector2(0, 275f));
                    }
                    else if (canwalljump && KeyboardManager.IsKeyDown(Keys.A)) Body.ApplyForce(new Vector2(-200f, 400f));
                    else  if (canwalljump && KeyboardManager.IsKeyDown(Keys.D)) Body.ApplyForce(new Vector2(200f, 400f));
                });
            KeyboardManager.Register(
                Keys.A,
                KeysState.Down,
                () => { Body.ApplyForce(new Vector2(-12.5f, 0)); });
            KeyboardManager.Register(
                Keys.D,
                KeysState.Down,
                () => { Body.ApplyForce(new Vector2(12.5f, 0)); });

            KeyboardManager.Register(
               Keys.F, KeysState.GoingDown,
               () =>
               {
                   Vector2 pixelClick = Mouse.GetState().Position.ToVector2();
                   Vector2 pixelPlayer = Camera.Position2Pixels(_position);
                   Vector2 delta = pixelClick - pixelPlayer;
                   delta.Normalize();
                   delta.Y = -delta.Y; // Invert for "virtual" world
                    Vector2 dir = 5f * delta;

                   Bullet bullet = new Bullet(_teleport, _position,
                       dir, game1.Services.GetService<World>());
                   _objects.Add(bullet);
               }
               );



        }

        public override void Update(GameTime gameTime)
        {
            foreach (ITempObject obj in _objects)
                obj.Update(gameTime);

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
            _objects.AddRange(_objects
                .Where(obj => obj is Bullet)
                .Cast<Bullet>()
                .Where(b => b.Collided)
                .ToArray()
            );
            _objects = _objects.Where(b => !b.IsDead()).ToList();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            foreach (ITempObject obj in _objects) obj.Draw(spriteBatch, gameTime);
        }
    }
}
