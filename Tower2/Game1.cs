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
            base.Initialize();
            new Player(this);
            new Tower(this);
            new ObjectPool(this);
            Camera.LookAt(Camera.WorldSize / 2f);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            Player._instance.Update(gameTime);
            Tower._instance.Update(gameTime);
            Camera.Update(gameTime);
            ObjectPool._instance.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            Player._instance.Draw(_spriteBatch, gameTime);
            Tower._instance.Draw(_spriteBatch, gameTime);
            ObjectPool._instance.Draw(_spriteBatch, gameTime);
                _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}