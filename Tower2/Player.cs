using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private int _size = 2048; //Como as sprites tem dimensoes diferentes, é preciso experimentar com os sizes para ver quais deles funcionam
        private bool _isGrounded = false;
        private Vector2 _pos;
        private AnimatedSprite _idleAnim;
        private List<Texture2D> _walkFrames;
        

        public Player(Game1 game1, Vector2 pos)
        {
            _game = game1;
            _instance = this;
            _pos = pos;
            _idleAnim = new AnimatedSprite(Enumerable.Range(0, 9).Select(n => _game.Content.Load<Texture2D>(@$"playersprites\stand\{n}")).ToArray(), pos);
            _idleAnim.Size *= _size;
        }

        public static void Update(GameTime gameTime)
        {
            _instance._Update(gameTime);
        }

        private void _Update(GameTime gameTime)
        {
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
