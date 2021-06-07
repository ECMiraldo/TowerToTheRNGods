using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using IPCA.MonoGame;
using System;

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
        private Texture2D _background;
        private Song _song;
        private float _volume = 0.5f;
        bool isnotplaying = true;


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
            playernotdead = true;
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
            _background = this.Content.Load<Texture2D>("Full-Background");
            _song = Content.Load<Song>("ES_Gallop - Jerry Lacey");
            MediaPlayer.Volume = 0.25f;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (isnotplaying && _timer > 4)
            {
                isnotplaying = false;
                MediaPlayer.Play(_song);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) _volume += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) _volume -= 0.1f;
            _volume = (float)Math.Clamp(_volume, 0.0, 1.0);
            MediaPlayer.Volume = _volume;


           

            Player._instance.Update(gameTime);
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);     
            ObjectPool._instance.Update(gameTime);

            if (Player._instance.Position.Y < Camera.Target.Y - 11f) playernotdead = false;
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
            if (!playernotdead) isnotplaying = true;
            
            if (KeyboardManager.IsGoingDown(Keys.Escape)) this.Exit();
                //We wanted to make a reset function but taking into account our game is running with a pretty good amount of singletons
                //Then we might just learn out lesson and make it better next time
                //if (KeyboardManager.IsGoingDown(Keys.Enter)) Reset();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            Rectangle background = new Rectangle(new Point(0, 0), new Point(512, 1024));
            _spriteBatch.Draw(_background, background, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, layerDepth: 1);
            _spriteBatch.End();


            _spriteBatch.Begin();
            ObjectPool._instance.Draw(_spriteBatch, gameTime);
            Tower._instance.Draw(_spriteBatch, gameTime);
            Player._instance.Draw(_spriteBatch, gameTime);
            Ui._instance.Draw(_spriteBatch, gameTime);
            if (!playernotdead)
            {
                int highscore = (int)Ui._instance._highscore;
                Vector2 scoresize = _font.MeasureString("Your highscore was" + highscore.ToString());
                Vector2 scorepos = (Camera.WindowSize - scoresize) / 2f;
                Vector2 m1size = _font.MeasureString("Game over");
                Vector2 m1pos = new Vector2((Camera.WindowSize.X - m1size.X) / 2f, scorepos.Y + m1size.Y + 10);
                Vector2 m2size = _font.MeasureString("Press ESC to exit");
                Vector2 m2pos = new Vector2((Camera.WindowSize.X - m2size.X) / 2f, scorepos.Y - m2size.Y - 10);
                _spriteBatch.DrawString(_font, "Your highscore was: " + highscore.ToString(), scorepos, Color.Black);
                _spriteBatch.DrawString(_font, "Game over", m1pos, Color.Black);
                _spriteBatch.DrawString(_font, "Press ESC to exit", m2pos, Color.Black);
            }
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        //public void Reset()
        //{
        //    ObjectPool._instance._platforms.Clear();
        //    ObjectPool._instance._pool.Clear();
        //    ObjectPool._instance._porings.Clear();
        //    Player._instance = null;
        //    Camera._camera = null;
        //    ObjectPool._instance = null;
        //    Ui._instance = null;
        //    Tower._instance = null;
            
        //    Initialize();
        //}
    }
}