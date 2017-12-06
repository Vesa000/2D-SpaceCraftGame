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
    public class Part
    {
        public int id = 0;
        public Vector2 pos = new Vector2(0, 0);
        //size in milimeters
        public Vector2 size = new Vector2(1000, 1000);
        public string parentdirection = "center";

        public string type = "none";
        public float mass = 1f;
        public float volume = 0f;
        public float fuel = 0f;
        public string fueltype = "nofuel";

        public bool on = false;
        public bool running = false;
        public int stage = 0;
        public float isp = 0f;
        public float thrust = 0f;

        public float separationthrust = 0f;

        public bool removeme = false;
        public int parent = 0;
        public List<int> neibours = new List<int>();

        public bool dontcalculatemymass = false;
    }
}
