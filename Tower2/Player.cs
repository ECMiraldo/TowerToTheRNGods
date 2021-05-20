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
    class Player
    {
        public static Player _instance;
        
        enum Status
        {
            Idle, Walk,
        }
        private Status _status = Status.Idle;

        private Game1 _game;
        private Vector2 _size = Vector2.One * 2048; //Como as sprites tem dimensoes diferentes, é preciso experimentar com os sizes para ver quais deles funcionam
        private bool _isGrounded = false;
        private AnimatedSprite _idleAnim;
        private List<Texture2D> _walkFrames;
        private Body _body;
        private bool debug = true;
        private Vector2 _pos;

        public Player(Game1 game1, Vector2 pos)
        {
            _game = game1;
            _instance = this;
            _idleAnim = new AnimatedSprite("idle anim",Enumerable.Range(0, 9).Select(n => _game.Content.Load<Texture2D>(@$"playersprites\stand\{n}")).ToArray(), pos, _size);
            _idleAnim.size = _idleAnim._texture.Bounds.Size.ToVector2() / _size;
 

            //Creates rectangle body
            _body = BodyFactory.CreateRectangle(game1._world, _idleAnim.size.X, _idleAnim.size.Y, 1f, pos);
            _body.UserData = this;
            if (_body.BodyType == BodyType.Kinematic) _body.BodyType = BodyType.Kinematic;
            else
            {
                _body.BodyType = BodyType.Dynamic;
            }
            _body.Friction = 0.5f;
            _body.Restitution = 0.1f;
            _body.FixedRotation = true;

            
            KeyboardManager.Register(
               Keys.Space,
               KeysState.GoingDown,
               () =>
               {
                   _body.ApplyForce(new Vector2(0, -200f));
               });
            KeyboardManager.Register(
                Keys.A,
                KeysState.Down,
                () => { _body.ApplyForce(new Vector2(-5, 0)); });
            KeyboardManager.Register(
                Keys.D,
                KeysState.Down,
                () => { _body.ApplyForce(new Vector2(5, 0)); });
        }

        public static void Update(GameTime gameTime)
        {
            _instance._Update(gameTime);
        }

        private void _Update(GameTime gameTime)
        {
            _pos = _body.Position;
            _idleAnim.position = _pos;
            if (_status == Status.Idle)
            {
                _idleAnim.Update(gameTime);
            }
        }
        public static void Draw(SpriteBatch sp, GameTime gameTime)
        {
            _instance._Draw(sp, gameTime);
        }

        public void _Draw(SpriteBatch sp, GameTime gameTime)
        {
            if (_status == Status.Idle)
            {
                _idleAnim.Draw(sp, gameTime);
            }
        }
    }
}
