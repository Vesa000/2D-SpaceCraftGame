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
    public class Planet
    {
        public Texture2D texture;
        public float radius;
        public float mass;
        public Vector2 velocity;

        public Vector2 pos;
        public double posx;
        public double posy;

        public bool hasatmosphere;
        public float atmosphereradius;
        public float atmospheredensity;
    }
}
