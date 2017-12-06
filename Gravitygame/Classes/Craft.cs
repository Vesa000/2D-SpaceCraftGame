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
    public class Craft
    {
        public RenderTarget2D rendertarget;
        public int needsrerendering = 2;
        public List<Part> partlist = new List<Part>();
        public int stage = 0;
        public int partnumber = 0;
        //public Rectangle collisionbox = new Rectangle(0, 0, 0, 0);

        public float throttle = 0f;
        public float mass = 0f;
        public Vector2 pos = new Vector2(0,-10);
        public Vector2 size = new Vector2(0, 0);
        public Vector2 posoffset = new Vector2(0, 0);
        public Vector2 vel = new Vector2(0, 0);
        public float rotation = 0f;
        public float rotationspeed = 0f;
    }
}
