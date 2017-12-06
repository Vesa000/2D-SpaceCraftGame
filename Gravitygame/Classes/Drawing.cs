using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gravitygame
{
    public static class Drawing
    {

        public static Texture2D planet;
        public static Texture2D atmosphere;
        public static Texture2D white;
        public static Texture2D arrow;
        public static Texture2D flame;

        public static Texture2D commandpod;
        public static Texture2D fueltank;
        public static Texture2D engine;
        public static Texture2D separator;

        public static SpriteFont menufont;
        public static SpriteFont font;
        public static Texture2D plusbutton;
        public static Texture2D minusbutton;
        public static Texture2D redxbutton;

        public static Texture2D apoapsis;
        public static Texture2D periapsis;


        public static Dictionary<string, Texture2D> dictionary = new Dictionary<string, Texture2D>();


        public static void drawgame(SpriteBatch sb, GraphicsDevice gd, Matrix matrix, GameTime gameTime, List<Craft> craftlist)
        {
            if (Play.mainmenu)
            {
                gd.Clear(Color.Black);
                sb.Begin();
                sb.Draw(Drawing.white, new Rectangle(200, 120, 400, 30), Color.Green);
                sb.DrawString(Drawing.menufont, "Start Game", new Vector2(250, 120), Color.White);
                sb.Draw(Drawing.white, new Rectangle(200, 160, 400, 30), Color.Red);
                sb.DrawString(Drawing.menufont, "Exit", new Vector2(350, 160), Color.White);
                sb.End();
            }
            if (Play.escmenu)
            {
                sb.Begin();
                sb.Draw(white, new Rectangle((int)Camera.screencenter.X - 210, 110, 420, 130), Color.Gray);
                sb.Draw(Drawing.white, new Rectangle((int)Camera.screencenter.X-200, 120, 400, 30), Color.Green);
                sb.DrawString(Drawing.menufont, "Continue", new Vector2((int)Camera.screencenter.X - 200, 120), Color.White);
                sb.Draw(Drawing.white, new Rectangle((int)Camera.screencenter.X - 200, 160, 400, 30), Color.Yellow);
                sb.DrawString(Drawing.menufont, "Exit to mainmenu", new Vector2((int)Camera.screencenter.X - 200, 160), Color.White);
                sb.Draw(Drawing.white, new Rectangle((int)Camera.screencenter.X - 200, 200, 400, 30), Color.Red);
                sb.DrawString(Drawing.menufont, "Exit game", new Vector2((int)Camera.screencenter.X - 200, 200), Color.White);
                sb.End();
            }
            else if (Play.editor)
            {
                DrawEditor.draw(sb, gd, matrix, gameTime,craftlist);
            }
            else if (Play.flight)
            {
                Drawflight.draw(sb, gd, matrix, gameTime,craftlist);
            }
        }

        public static string getsiprefix(float f)
        {
            string s = f.ToString();
            if (f >= 1000000000000)
            {
                s = (f / 1000000000000).ToString() + "T";
            }
            else if (f >= 1000000000)
            {
                s = (f / 1000000000).ToString() + "G";
            }
            else if (f >= 1000000)
            {
                s = (f / 1000000).ToString() + "M";
            }
            else if (f>=1000)
            {
                s = (f/1000).ToString() + "K";
            }
            return s;
        }
    }
}
