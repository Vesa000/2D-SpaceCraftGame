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
    public static class Flight
    {
        public static Planet[] planet = new Planet[1];
        public static int timewarp = 1;

        public static void fly(List<Craft> craftlist)
        {
            #region controlls
            #region camera
            if (Play.mousestate != Play.oldmousestate)
            {
                Camera.zoom *= 1 + (Play.mousestate.ScrollWheelValue - Play.oldmousestate.ScrollWheelValue) * 0.001f;
            }
            #endregion
            if (Play.state.IsKeyDown(Keys.A)) { craftlist[0].rotation -= 0.01f; }
            if (Play.state.IsKeyDown(Keys.D)) { craftlist[0].rotation += 0.01f; }

            if (Play.state.IsKeyDown(Keys.Z)) { craftlist[0].throttle = 1; }
            if (Play.state.IsKeyDown(Keys.X)) { craftlist[0].throttle = 0; }
            if (Play.state.IsKeyDown(Keys.LeftShift)) { craftlist[0].throttle += 0.01f; }
            if (Play.state.IsKeyDown(Keys.LeftControl)) { craftlist[0].throttle -= 0.01f; }
            if (craftlist[0].throttle > 1) { craftlist[0].throttle = 1; }
            if (craftlist[0].throttle < 0) { craftlist[0].throttle = 0; }

            if (Play.state.IsKeyUp(Keys.OemPeriod) & Play.oldstate.IsKeyDown(Keys.OemPeriod)) { timewarp *= 2; }
            if (Play.state.IsKeyUp(Keys.OemComma) & Play.oldstate.IsKeyDown(Keys.OemComma)) { timewarp /= 2; }
            if (timewarp <= 0) { timewarp = 1; }

            #region next stage
            if (Play.state.IsKeyUp(Keys.Space) & Play.oldstate.IsKeyDown(Keys.Space))
            {
                foreach (Part p in craftlist[0].partlist)
                {
                    if (p.type == "engine" || p.type == "separator")
                    {
                        if (p.stage == craftlist[0].stage)
                        {
                            if (p.type == "engine")
                            {
                                p.on = true;
                            }
                            if (p.type == "separator")
                            {
                                List<Part> lista = new List<Part>();
                                List<int> used = new List<int>();
                                foreach (Part pa in craftlist[0].partlist)
                                {
                                    if (pa.id != p.parent)
                                    {
                                        foreach (int n in p.neibours)
                                        {
                                            if (n == pa.id) { lista.Add(pa); }
                                        }
                                    }
                                }

                                for (int i = 0; i < lista.Count(); i++)
                                {
                                    bool partused = false;
                                    foreach (int j in used)
                                    {
                                        if (lista[i].id == j) { partused = true; }
                                    }
                                    if (!partused)
                                    {
                                        foreach (Part pa in craftlist[0].partlist)
                                        {
                                            foreach (int n in lista[i].neibours)
                                            {
                                                if (n == pa.id) { lista.Add(pa); used.Add(pa.id); }
                                            }
                                        }

                                    }
                                }
                                //separate the stages
                                Craft c = new Craft();
                                c.pos = craftlist[0].pos + Vector2.Transform(p.pos, Matrix.CreateRotationZ(craftlist[0].rotation));
                                c.rotation = craftlist[0].rotation;
                                c.vel = craftlist[0].vel;
                                c.needsrerendering=2;
                                foreach (Part pa in lista)
                                {
                                    c.partlist.Add(pa);
                                    pa.removeme = true;
                                }
                                c = recalculatecollisionbox(c);
                                craftlist.Add(c);
                                craftlist[0] = recalculatecollisionbox(craftlist[0]);
                                craftlist[0].needsrerendering=2;
                            }
                        }
                    }
                }

                craftlist[0].stage++;
            }
            //had to remove parts outside of the foreach loop
            for (int i = 0; i < craftlist[0].partlist.Count(); i++)
            {
                if (craftlist[0].partlist[i].removeme) { craftlist[0].partlist.Remove(craftlist[0].partlist[i]); }
            }
            #endregion
            #endregion

            #region movement
            for (int w = 0; w < timewarp; w++)
            {
                foreach (Craft c in craftlist)
                {
                    if (craftlist[0].throttle > 0)
                    {
                        foreach (Part p in c.partlist)
                        {
                            if (p.type == "engine" & p.on)
                            {
                                float fuelusage = (p.thrust / (9.81f * p.isp)) / 60;
                                p.running = usefuel(c, p.id, fuelusage);
                                if (p.running)
                                {
                                    c.mass = 0;
                                    foreach (Part p1 in c.partlist)
                                    {
                                        c.mass += p1.mass;
                                        c.mass += p1.fuel;
                                    }
                                    c.vel += (new Vector2((float)(((c.throttle * p.thrust) / c.mass) * Math.Sin(c.rotation)), -(float)(((c.throttle * p.thrust) / c.mass) * Math.Cos(c.rotation)))) / (60);
                                }
                            }
                        }
                    }
                }
                #region gravity
                foreach (Craft c in craftlist)
                {
                    float r = (planet[0].pos - c.pos).Length();
                    Vector2 v = (planet[0].pos - c.pos);
                    double a = Math.Atan2(v.Y, v.X);
                    float G = (0.00000000006674f * (planet[0].mass) / (r * r));
                    c.vel.X += (float)(G * Math.Cos(a) / 60);
                    c.vel.Y += (float)(G * Math.Sin(a) / 60);
                }
                #endregion
                #region collisions
                foreach (Craft c in craftlist)
                {
                    Vector2 v = (planet[0].pos - c.pos);
                    float a = (float)Math.Atan2(v.Y, v.X);

                    double ang = Math.Atan2(planet[0].pos.Y, planet[0].pos.X) - Math.Atan2(c.vel.Y, c.vel.X);

                    if ((planet[0].pos - c.pos).Length() <= planet[0].radius&(ang<Math.PI/2))
                    {
                        c.pos = planet[0].pos + Vector2.Normalize(c.pos - planet[0].pos) * (planet[0].radius);
                        c.vel = new Vector2(0, 0);
                        c.rotation = (float)(a - Math.PI / 2);
                    }
                }
                #endregion
                foreach (Craft c in craftlist)
                {
                    c.pos += c.vel/60;
                }
                if (craftlist[0].pos != new Vector2(0, 0))
                {
                    Vector2 offset = craftlist[0].pos;
                    foreach(Craft c in craftlist)
                    {
                        c.pos -= offset;
                    }
                    foreach(Planet p in Flight.planet)
                    {
                        p.posx -= offset.X;
                        p.posy -= offset.Y;
                        p.pos = new Vector2((float)p.posx, (float)p.posy);
                    }
                }

                //move camera to the center of the craft
                Camera.pos = craftlist[0].pos * Drawflight.worldscale;// + Vector2.Transform((craftlist[0].size/2), Matrix.CreateRotationZ(craftlist[0].rotation));
                #endregion
            }
        }
        public static void startflight()
        {

            Flight.planet[0] = new Planet();
            Flight.planet[0].texture = Drawing.planet;
            Flight.planet[0].velocity = new Vector2(0, 0);
            Flight.planet[0].pos = new Vector2(0, 0);
            Flight.planet[0].mass = 5972200000000000000000000f;
            Flight.planet[0].radius = 6371000f;
            Flight.planet[0].hasatmosphere = true;
            Flight.planet[0].atmosphereradius = 1500000;
            Flight.planet[0].atmospheredensity = 1;

        }

        public static bool usefuel(Craft c, int id, float f)
        {
            foreach (Part p in c.partlist)
            {
                //p=engine part
                if (p.id == id)
                {
                    List<int> connectedparts = new List<int>();
                    List<int> usedparts = new List<int>();
                    foreach (int i in p.neibours)
                    {
                        connectedparts.Add(i);
                    }

                    //loop all connected parts
                    for (int i = 0; i < connectedparts.Count(); i++)
                    {
                        foreach (Part p1 in c.partlist)
                        {
                            if (p1.id == connectedparts[i])
                            {
                                if (p1.type == "fueltank")
                                {
                                    //remove fuel
                                    if (p1.fuel > f)
                                    {
                                        p1.fuel -= f;
                                        return true;
                                    }
                                    else
                                    {
                                        f -= p1.fuel;
                                        p1.fuel = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool usefuelbroken(Craft c, int id, float f)
        {
            foreach (Part p in c.partlist)
            {
                //p=engine part
                if (p.id == id)
                {
                    List<int> connectedparts = new List<int>();
                    List<int> usedparts = new List<int>();
                    foreach (int i in p.neibours)
                    {
                        connectedparts.Add(i);
                    }
                    //loop all connected parts
                    for (int i = 0; i < connectedparts.Count(); i++)
                    {
                        bool used = false;
                        foreach (int k in usedparts)
                        {
                            if (i == k) { used = true; }
                        }
                        //if part not used before
                        if (!used)
                        {
                            foreach (Part p1 in c.partlist)
                            {
                                if (p1.id == i)
                                {
                                    usedparts.Add(i);
                                    if (p1.type == "fueltank")
                                    {
                                        //add new neibours
                                        foreach (int j in p.neibours)
                                        {
                                            connectedparts.Add(j);
                                        }
                                        //remove fuel
                                        if (p1.fuel > f)
                                        {
                                            p1.fuel -= f;
                                            return true;
                                        }
                                        else
                                        {
                                            f -= p1.fuel;
                                            p1.fuel = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static Craft recalculatecollisionbox(Craft c)
        {
            float up = 0;
            float down = 0;
            float left = 0;
            float right = 0;
            foreach (Part p in c.partlist)
            {
                if (p.pos.Y < up) { up = p.pos.Y; }

                if (p.pos.Y + p.size.Y > down) { down = p.pos.Y + p.size.Y; }

                if (p.pos.X < left) { left = p.pos.X; }

                if (p.pos.X + p.size.X > right) { right = p.pos.X + p.size.X; }
            }
            c.posoffset = new Vector2(left, up);
            c.size = new Vector2(right, down);
            return c;
        }
    }
}
