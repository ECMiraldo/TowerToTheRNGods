using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Genbox.VelcroPhysics.Dynamics;
using IPCA.MonoGame;

namespace Tower2
{
    //Questions: Player Collider is offset


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public World _world;
        public float playersizemult = 1024f; //useful for player constructor in order to softcode sprite size
        double _timer = 0;
        public bool playernotdead = true;
        private SpriteFont _font;
        private bool newGame = false;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            new KeyboardManager(this);
            _world = new World(new Vector2(0, -10f));
            Services.AddService(_world);
            
            
            
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 1024;
            _graphics.PreferredBackBufferWidth = 512;
            _graphics.ApplyChanges();
            Debug.SetGraphicsDevice(GraphicsDevice);
            new Camera(GraphicsDevice, 10f, 20f);
            new Player(this);
            new Tower(this);
            new ObjectPool(this);
            new Ui(this);
            base.Initialize();
            Camera.LookAt(Camera.WorldSize / 2f);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = this.Content.Load<SpriteFont>("EndGame");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Player._instance.Update(gameTime);
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);     
            ObjectPool._instance.Update(gameTime);
            if (playernotdead)
            {
                Tower._instance.Update(gameTime);
                Ui._instance.Update(gameTime);
                _timer = _timer + gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer > 5)
                {
                    Camera.Update(gameTime);
                }
            }
            else {
                if (KeyboardManager.IsGoingDown(Keys.Escape)) this.Exit();
                if (KeyboardManager.IsGoingDown(Keys.Enter)) Initialize();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(); 
            Player._instance.Draw(_spriteBatch, gameTime);
            ObjectPool._instance.Draw(_spriteBatch, gameTime);
            Tower._instance.Draw(_spriteBatch, gameTime);
            Ui._instance.Draw(_spriteBatch, gameTime);
            if (!playernotdead)
            {
                int highscore = (int)Ui._instance._highscore;
                Vector2 scoresize = _font.MeasureString("Your highscore was" + highscore.ToString());
                Vector2 scorepos = (Camera.WindowSize - scoresize) / 2f;
                Vector2 m1size = _font.MeasureString("Game over");
                Vector2 m1pos = new Vector2((Camera.WindowSize.X - m1size.X) / 2f, scorepos.Y + m1size.Y + 10);
                Vector2 m2size = _font.MeasureString("Press enter to play again or ESC to exit");
                Vector2 m2pos = new Vector2((Camera.WindowSize.X - m2size.X) / 2f, scorepos.Y - m2size.Y - 10);
                _spriteBatch.DrawString(_font, "Your highscore was: " + highscore.ToString(), scorepos, Color.White);
                _spriteBatch.DrawString(_font, "Game over", m1pos, Color.White);
                _spriteBatch.DrawString(_font, "Press enter to play again or ESC to exit", m2pos, Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}