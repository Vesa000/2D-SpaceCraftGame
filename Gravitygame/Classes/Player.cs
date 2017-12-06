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
    public static class Player
    {
        public static Texture2D texture;
        public static Vector2 pos;
        public static Vector2 vel;
        public static float rot;
        public static float drot;
        
        public static float throttle;
        public static float thrust;
    }
}
