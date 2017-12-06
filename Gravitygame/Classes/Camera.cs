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
    public static class Camera
    {
        public static float zoom = 0.05f;
        public static Vector2 pos = new Vector2(0, 0);
        public static Vector2 screencenter = new Vector2(0,0);

        public static Matrix matrix(Viewport vp)
        {
            screencenter = new Vector2(vp.Width / 2, vp.Height / 2);
            Vector2 translation = -pos + screencenter;
            return
                Matrix.CreateTranslation(new Vector3(-pos, 0.0f)) *
                //Matrix.CreateTranslation(new Vector3(-screencenter, 0.0f)) *
                Matrix.CreateRotationZ(0f) *
                Matrix.CreateScale(zoom, zoom, 1) *
                Matrix.CreateTranslation(new Vector3(screencenter, 0.0f));
        }
    }
}
