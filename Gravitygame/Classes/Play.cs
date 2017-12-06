using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Gravitygame
{
    public static class Play
    {
        public static bool gameon = true;
        public static bool editor = false;
        public static bool flight = false;
        public static bool mainmenu = true;
        public static bool escmenu = false;

        public static KeyboardState state = Keyboard.GetState();
        public static KeyboardState oldstate = Keyboard.GetState();
        public static MouseState mousestate = Mouse.GetState();
        public static MouseState oldmousestate = Mouse.GetState();

        public static void game(List<Craft> craftlist)
        {
            oldstate = state;
            oldmousestate = mousestate;
            state = Keyboard.GetState();
            mousestate = Mouse.GetState();

            if (state.IsKeyDown(Keys.Escape)&!mainmenu) { escmenu = true; }

            if (mainmenu)
            {
                if (mousestate.LeftButton == ButtonState.Released & oldmousestate.LeftButton == ButtonState.Pressed)
                {
                    if (isinside(new Vector2(mousestate.X, mousestate.Y), new Vector2(200, 120), new Vector2(400, 30)))
                    {
                        mainmenu = false;
                        editor = true;
                        Camera.zoom = 0.05f;
                        Camera.pos = new Vector2(0, 0);
                    }
                    if (isinside(new Vector2(mousestate.X, mousestate.Y), new Vector2(200, 160), new Vector2(400, 30)))
                    {
                        gameon = false;
                    }
                }
            }

            else if (escmenu)
            {
                if (mousestate.LeftButton == ButtonState.Released & oldmousestate.LeftButton == ButtonState.Pressed)
                {
                    if (isinside(new Vector2(mousestate.X, mousestate.Y), new Vector2((int)Camera.screencenter.X - 200, 120), new Vector2(400, 30)))
                    {
                        escmenu = false;
                    }
                    if (isinside(new Vector2(mousestate.X, mousestate.Y), new Vector2((int)Camera.screencenter.X - 200, 160), new Vector2(400, 30)))
                    {
                        mainmenu = true;
                        editor = false;
                        flight = false;
                        escmenu = false;
                    }
                    if (isinside(new Vector2(mousestate.X, mousestate.Y), new Vector2((int)Camera.screencenter.X - 200, 200), new Vector2(400, 30)))
                    {
                        gameon = false;
                    }
                }
            }

            else if (editor)
            {
                Editor.edit(craftlist);
            }

            else if (flight)
            {
                Flight.fly(craftlist);
            }
        }



        //public static bool isinside(Vector2 v, Rectangle r)
        //{
        //    bool b = false;
        //    if (v.X > r.X & v.X < r.X + r.Width)
        //    {
        //        if (v.Y > r.Y & v.Y < r.Y + r.Height)
        //        {
        //            b = true;
        //        }

        //    }
        //    return b;
        //}
        public static bool isinside(Vector2 p, Vector2 s, Vector2 e)
        {
            bool b = false;
            if (p.X > s.X & p.X < s.X + e.X)
            {
                if (p.Y > s.Y & p.Y < s.Y + e.Y)
                {
                    b = true;
                }

            }
            return b;
        }
    }
}
