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
            Idle, Walk, Cast,
        }
        private Status _status = Status.Idle;

        public static Player _instance;
        public int _hp = 100;
        public int _mana = 100;
        public int _coins;
        public int crystals;

        private double _timer = 0;
        public bool _damaged = false;

        private Game1 _game;
        private bool _isGrounded = false;
        private bool candoublejump = false;
        private bool canwalljump = false;

        private List<Texture2D> _idleFrames;
        private List<Texture2D> _walkFrames;
        private List<Texture2D> _castFrames;
        private Texture2D _bullet;
        private List<ITempObject> _objects;
        private AnimatedSprite Aura;

        public Player(Game1 game1) : base("player", new Vector2(5f, 10f), new Vector2(0.5f, 0.5f), 128f, Enumerable.Range(0, 9).Select(n => game1.Content.Load<Texture2D>($"playersprites/Stand/{n}")).ToArray())
        {
            _instance = this;
            _idleFrames = _textures; // loaded by the base construtor

            _walkFrames = Enumerable.Range(0, 9).Select(n => game1.Content.Load<Texture2D>($"playersprites/Walk/{n}")).ToList();
            _castFrames = Enumerable.Range(0, 9).Select(n => game1.Content.Load<Texture2D>($"playersprites/Cast1H/{n}")).ToList();

            _game = game1;
            _bullet = _game.Content.Load<Texture2D>("fireball");
            _objects = new List<ITempObject>();


            crystals = 3;
            _coins = 0;

            AddRectangleBody(
                _game.Services.GetService<World>(), _size.X*1.6f, _size.Y*2.4f //Some magic numbers cause collider was offset
            ) ; // kinematic is false by default

            Fixture top = FixtureFactory.AttachRectangle(
                _size.X, _size.Y * 0.05f,
                1, new Vector2(0, +_size.Y + 0.1f),
                Body);
            top.IsSensor = true;
            top.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name == "spikes big" || b.GameObject().Name == "spikes small") {
                    if (!_damaged)
                    {
                        _damaged = true;
                        _hp = _hp - 10;
                    }
                }
            };

            Fixture Bottom = FixtureFactory.AttachRectangle(
                _size.X, _size.Y * 0.05f,
                1, new Vector2(0, -_size.Y -0.1f),
                Body);
            Bottom.IsSensor = true;

            Bottom.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name == "platform") _isGrounded = true;
                if (b.GameObject().Name == "npc") Body.ApplyForce(new Vector2(0, 150f));
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
                if (b.GameObject().Name == "npc" &&  !_damaged)
                {
                    _hp = _hp - 30;
                    _damaged = true;
                }
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
                if (b.GameObject().Name == "npc" && !_damaged)
                {
                    _hp = _hp - 30;
                    _damaged = true;
                }
            };
            right.OnSeparation = (a, b, contact) => canwalljump = false;

            KeyboardManager.Register(
                Keys.Space,
                KeysState.GoingDown,
                () =>
                {
                    if (_isGrounded)
                    {
                        Body.ApplyForce(new Vector2(0, 375f));
                        candoublejump = true;
                    }
                    else if (candoublejump)
                    {
                        candoublejump = false;
                        Body.ApplyForce(new Vector2(0, 275f));
                    }
                    else if (canwalljump && KeyboardManager.IsKeyDown(Keys.A)) Body.ApplyForce(new Vector2(-80f, 550f));
                    else  if (canwalljump && KeyboardManager.IsKeyDown(Keys.D)) Body.ApplyForce(new Vector2(80f, 550f));
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
                   if (crystals > 0)
                   {
                       Vector2 pixelClick = Mouse.GetState().Position.ToVector2();
                       Vector2 pixelPlayer = Camera.Position2Pixels(_position);
                       Vector2 delta = pixelClick - pixelPlayer;
                       delta.Normalize();
                       delta.Y = -delta.Y; // Invert for "virtual" world
                       Vector2 dir = 5f * delta;

                       Bullet bullet = new Bullet(_bullet, _position,
                           dir, game1.Services.GetService<World>());
                       _objects.Add(bullet);
                       crystals--;
                   }
               }
               );
            KeyboardManager.Register(
                Keys.R,
                KeysState.Down,
                () => {
                    if (_mana > 0)
                    {
                        Body.ApplyForce(new Vector2(0, 30f));
                        _status = Status.Cast;
                    }
                });
            KeyboardManager.Register(
               Keys.R,
               KeysState.GoingUp,
               () => {
                   _status = Status.Idle;
               });

            Aura = new AnimatedSprite("aura", _position, _size, 32f, Enumerable.Range(0,31).Select(n => game1.Content.Load<Texture2D>($"aura/aura{n}")).ToArray());
        }

        public override void Update(GameTime gameTime)
        {
            if (_damaged)
            {
                _timer = _timer + gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer > 2)
                {
                    _damaged = false;
                    _timer = 0;
                }
            }
            if (_status == Status.Cast)
            {
                _mana = _mana - ((int)gameTime.ElapsedGameTime.Ticks / 100000 );
            }

            foreach (ITempObject obj in _objects) obj.Update(gameTime);
            Aura._position = Body.Position;
            if (_status == Status.Cast)
            {
                _textures = _castFrames;
                _currentTexture = 0;
                Aura.Update(gameTime);
            }

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
            if (_status == Status.Cast) Aura.Draw(spriteBatch, gameTime);

        }
    }
}
