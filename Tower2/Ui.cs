using System;
using System.Collections.Generic;
using System.Text;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower2
{
    public class Ui
    {
        public static Ui _instance;

        private Game1 game;
        private Texture2D _emptybar;
        private Texture2D _health;
        private Texture2D _mana;
        private List<Sprite> _sprites;
        private Texture2D _crystal;

        public Ui(Game1 game)
        {
            _instance = this;
            this.game = game;
            _sprites = new List<Sprite>();

            _emptybar = game.Content.Load<Texture2D>("Emptybar");
            _health = game.Content.Load<Texture2D>("healthbar");
            _mana = game.Content.Load<Texture2D>("manabar");
            _crystal = game.Content.Load<Texture2D>("crystal/crystal 1");
        }

        public void Update(GameTime gametime)
        {
            
        }

        public void Draw(SpriteBatch sp, GameTime gametime)
        {

            //Windowsize =  512, 1024;
            //worldsize = 10f, 20f
            // 1f, x = 51.2px
            // 1f, y = 51.2px (1024/20f)
            Rectangle hp = new Rectangle(new Point(10,970), new Point(107,26));
            Rectangle mana = new Rectangle(new Point(10, 940), new Point(107, 26));
            Rectangle hpbar = new Rectangle(new Point(13, 976), new Point(Player._instance._hp  , 16));
            Rectangle manabar = new Rectangle(new Point(13, 946), new Point(Player._instance._mana , 16));
            sp.Draw(_emptybar, hp, Color.White);
            sp.Draw(_emptybar, mana, Color.White);
            sp.Draw(_health, hpbar, Color.White);
            sp.Draw(_mana, manabar, Color.White);

            for (int i = 0; i < Player._instance.crystals; i++)
            {
                Rectangle bullets = new Rectangle(new Point(10 + (24*i), 900), new Point(24, 32));
                sp.Draw(_crystal, bullets, Color.White);
            }
        }
    }
}
