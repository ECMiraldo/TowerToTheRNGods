using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IPCA.MonoGame;

namespace Tower2
{
    public class PlatformBig : Sprite
    {

        private Game1 game;
        public PlatformBig(Game1 game, Vector2 position, bool offset = false) :
                    //name              //texture                                           //size(virtual world)  //sizemult(para o draw da sprite) é experimentar e ver
                base("platform big", game.Content.Load<Texture2D>("platform big"), position, new Vector2(5f, 0.5f), 128f)
        {
            this.game = game;
            AddRectangleBody(this.game.Services.GetService<World>(), _size.X, _size.Y);
            Body.BodyType = BodyType.Static;
        }
    }
}
