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
    public static class Drawflight
    {
        public static float worldscale = 10f;

        public static float apoapsis = 0;
        public static float periapsis = 0;
        public static float timetoapoapsis = 0;
        public static float timetoperiapsis = 0;
        public static float orbitalperiod = 0;
        public static float eccentricity = 0;

        public static void draw(SpriteBatch sb, GraphicsDevice gd, Matrix matrix, GameTime gameTime, List<Craft> craftlist)
        {
            gd.Clear(Color.Black);

            #region rendertarget
            foreach (Craft c in craftlist)
            {
                if (c.needsrerendering > 0)
                {
                    c.rendertarget = new RenderTarget2D(gd, (int)((c.size.X - c.posoffset.X) * 1000), (int)((c.size.Y - c.posoffset.Y) * 1000));
                    gd.SetRenderTarget(c.rendertarget);
                    gd.Clear(Color.Transparent);
                    sb.Begin();
                    foreach (Part p in c.partlist)
                    {
                        sb.Draw(Drawing.dictionary[p.type], new Rectangle((int)((p.pos.X-c.posoffset.X) * 1000), (int)((p.pos.Y-c.posoffset.Y) * 1000), (int)(p.size.X * 1000), (int)(p.size.Y * 1000)), Color.White);
                    }
                    sb.End();
                    c.needsrerendering--;
                    gd.SetRenderTarget(null);
                }
            }
            #endregion

            #region draw world
            sb.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.matrix(gd.Viewport));

            #region draw path
            if (Camera.zoom < 1.76313472)
            {
                //drawaccurateorbits(sb, gd, matrix, gameTime, craftlist);
                draworbits(sb, gd, matrix, gameTime, craftlist);
                sb.Draw(Drawing.arrow, craftlist[0].pos, null, Color.White, (float)craftlist[0].rotation, new Vector2(32, 32), (1 / Camera.zoom) * 0.25f, SpriteEffects.None, 1);
            }
            #endregion

            #region draw planets
            if (Flight.planet[0].hasatmosphere)
            {
                Rectangle arec = new Rectangle((int)((Flight.planet[0].posx-(Flight.planet[0].radius + Flight.planet[0].atmosphereradius))*worldscale), (int)((Flight.planet[0].posy-(Flight.planet[0].radius + Flight.planet[0].atmosphereradius))*worldscale), (int)((Flight.planet[0].radius + Flight.planet[0].atmosphereradius) * 2*worldscale), (int)((Flight.planet[0].radius + Flight.planet[0].atmosphereradius) * 2*worldscale));
                sb.Draw(Drawing.atmosphere, arec, Color.White);
            }
            Rectangle rec = new Rectangle((int)((Flight.planet[0].posx-Flight.planet[0].radius*1)*worldscale), (int)((Flight.planet[0].posy-Flight.planet[0].radius*1)*worldscale), (int)(Flight.planet[0].radius * 2*worldscale), (int)(Flight.planet[0].radius * 2*worldscale));
            sb.Draw(Flight.planet[0].texture, rec, Color.White);

            //sb.Draw(Drawing.white, new Rectangle((int)craftlist[0].pos.X,(int)craftlist[0].pos.Y,1,1), Color.Green);
            #endregion

            #region draw crafts
            sb.End();
            sb.Begin();

            foreach (Craft c in craftlist)
            {
                Rectangle srec = new Rectangle(0, 0, c.rendertarget.Width, c.rendertarget.Height);
                sb.Draw(c.rendertarget, Camera.screencenter+(c.pos*worldscale), srec, Color.White, c.rotation, new Vector2(0, 0), Camera.zoom/1000, SpriteEffects.None, 1);
            }
            sb.End();
            #endregion
            #endregion

            #region draw menus
            sb.Begin();
            sb.DrawString(Drawing.font, "Throttle = " + (craftlist[0].throttle * 100).ToString(), new Vector2(0, 0), Color.White);
            sb.DrawString(Drawing.font, "vel = " + Drawing.getsiprefix(craftlist[0].vel.Length()) + "M/S", new Vector2(0, 20), Color.White);

            foreach (Part p in craftlist[0].partlist)
            {
                if (p.type == "engine" & p.on)
                {
                    sb.DrawString(Drawing.font, "TWR = " + (craftlist[0].throttle * p.thrust / (craftlist[0].mass * 9.81f)), new Vector2(0, 40), Color.White);
                }
            }
            sb.DrawString(Drawing.font, "Timewarp = " + Flight.timewarp.ToString() + "* speed", new Vector2(0, 60), Color.White);

            sb.DrawString(Drawing.font, "Altitude = " + Drawing.getsiprefix((Flight.planet[0].pos - craftlist[0].pos).Length()) + "m", new Vector2(200, 0), Color.White);
            sb.DrawString(Drawing.font, "Alt surf = " + Drawing.getsiprefix((Flight.planet[0].pos - craftlist[0].pos).Length() - Flight.planet[0].radius) + "m", new Vector2(200, 20), Color.White);

            if (Camera.zoom < 1.76313472)
            {
                sb.DrawString(Drawing.font, "Apoapsis = " + Drawing.getsiprefix(apoapsis) + "m", new Vector2(500, 0), Color.White);
                sb.DrawString(Drawing.font, "Periapsis = " + Drawing.getsiprefix(periapsis) + "m", new Vector2(500, 20), Color.White);

                sb.DrawString(Drawing.font, "Time to apoapsis = " + Drawing.getsiprefix(timetoapoapsis) + "s", new Vector2(800, 0), Color.White);
                sb.DrawString(Drawing.font, "Time to periapsis = " + Drawing.getsiprefix(timetoperiapsis) + "s", new Vector2(800, 20), Color.White);

                sb.DrawString(Drawing.font, "Eccentricity = " + eccentricity.ToString(), new Vector2(1100, 0), Color.White);
            }

            #region deltavstats
            if (Editor.stage != null & craftlist.Count != 0 & true)
            {
                Craft c = new Craft();
                int laststage = 10;
                c.partlist = new List<Part>(craftlist[0].partlist);
                foreach (Part p in c.partlist)
                {
                    p.dontcalculatemymass = false;
                }
                float[] deltav = new float[10];
                float[] twr = new float[10];
                for (int s = 0; s < 10; s++)
                {
                    if (Editor.stage[s] != null)
                    {
                        foreach (int i in Editor.stage[s])
                        {
                            for (int j = 0; j < c.partlist.Count(); j++)
                            {
                                Part p = c.partlist[j];
                                if (c.partlist[j].id == i)
                                {
                                    if (c.partlist[j].type == "separator")
                                    {
                                        //separator part
                                        List<Part> lista = new List<Part>();
                                        List<int> used = new List<int>();
                                        foreach (Part pa in c.partlist)
                                        {
                                            if (pa.id != p.parent)
                                            {
                                                foreach (int n in p.neibours)
                                                {
                                                    if (n == pa.id) { lista.Add(pa); }
                                                }
                                            }
                                        }

                                        for (int k = 0; k < lista.Count(); k++)
                                        {
                                            bool partused = false;
                                            foreach (int l in used)
                                            {
                                                if (lista[k].id == l) { partused = true; }
                                            }
                                            if (!partused)
                                            {
                                                foreach (Part pa in c.partlist)
                                                {
                                                    foreach (int n in lista[k].neibours)
                                                    {
                                                        if (n == pa.id) { lista.Add(pa); used.Add(pa.id); }
                                                    }
                                                }

                                            }
                                        }
                                        foreach (Part pa in lista)
                                        {
                                            pa.dontcalculatemymass = true;
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < c.partlist.Count(); i++)
                        {
                            if (c.partlist[i].removeme) { c.partlist.Remove(c.partlist[i]); }
                        }

                        if (Editor.stage[s].Count == 0 & laststage == 10) { laststage = s; }
                        foreach (int i in Editor.stage[s])
                        {
                            for (int j = 0; j < c.partlist.Count(); j++)
                            {
                                if (c.partlist[j].id == i)
                                {
                                    if (c.partlist[j].type == "engine")
                                    {
                                        //engine part
                                        foreach (int k in c.partlist[j].neibours)
                                        {
                                            for (int l = 0; l < c.partlist.Count(); l++)
                                            {
                                                if (c.partlist[l].id == k)
                                                {
                                                    //neibour of the engine
                                                    float mass = DrawEditor.getmass(c);
                                                    deltav[s] += (float)(Math.Log(mass / (mass - c.partlist[l].fuel)) * c.partlist[j].isp * 9.81f);

                                                    //why ar u lying to me???
                                                    twr[s] = c.partlist[j].thrust / (mass * 9.81f);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        laststage = s;
                    }
                }
                //explanations here
                int ssep = 50;
                sb.Draw(Drawing.white, new Rectangle(0, (int)Camera.screencenter.Y * 2 + 10 - (laststage + 1) * ssep, 600, (laststage + 2) * ssep), Color.Gray);

                for (int i = 0; i < laststage; i++)
                {
                    sb.Draw(Drawing.white, new Rectangle(10, (int)Camera.screencenter.Y * 2 - 50 - i * ssep, 90, ssep), Color.Black);
                    sb.DrawString(Drawing.font, deltav[i].ToString(), new Vector2(10, Camera.screencenter.Y * 2 - 50 - i * ssep), Color.White);

                    sb.Draw(Drawing.white, new Rectangle(110, (int)Camera.screencenter.Y * 2 - 50 - i * ssep, 90, ssep), Color.Black);
                    sb.DrawString(Drawing.font, twr[i].ToString(), new Vector2(110, Camera.screencenter.Y * 2 - 50 - i * ssep), Color.White);
                }
                sb.DrawString(Drawing.font, "deltav", new Vector2(10, Camera.screencenter.Y * 2 + 10 - (laststage + 1) * ssep), Color.White);

                sb.DrawString(Drawing.font, "TWR", new Vector2(110, Camera.screencenter.Y * 2 + 10 - (laststage + 1) * ssep), Color.White);

                foreach (Part p in c.partlist)
                {
                    p.dontcalculatemymass = false;
                }
            }
            sb.End();
            #endregion

            #endregion
        }



        public static void drawaccurateorbits(SpriteBatch sb, GraphicsDevice gd, Matrix matrix, GameTime gameTime, List<Craft> craftlist)
        {
            periapsis = int.MaxValue;
            apoapsis = 0;

            Vector2 p = craftlist[0].pos;
            Vector2 op = craftlist[0].pos;
            Vector2 v = craftlist[0].vel;

            Vector2 v2 = (Flight.planet[0].pos - p);
            double starta = Math.Atan2(v2.Y, v2.X);
            bool b = false;
            Vector2 oop = craftlist[0].pos;
            int j = 0;
            for (int i = 0; i < 1000000; i++)
            {
                op = p;
                float r = (Flight.planet[0].pos - p).Length();
                v2 = (Flight.planet[0].pos - p);
                double a = Math.Atan2(v2.Y, v2.X);
                float G = (0.00000000006674f * Flight.planet[0].mass) / (r * r);
                v.X += (float)(G * Math.Cos(a) / 60);
                v.Y += (float)(G * Math.Sin(a) / 60);
                p += v;

                if (j > 100)
                {
                    j = 0;
                    DrawLine(sb, oop, p);
                    oop = p;
                }
                j++;

                //stop drawing after full orbit
                if (b)
                {
                    if (a > starta & a < starta + Math.PI * 0.5f)
                    {
                        DrawLine(sb, oop, p);
                        break;
                    }
                }
                else
                {
                    if (a > starta + Math.PI || a > starta - Math.PI & a < starta - Math.PI * 0.5f) { b = true; }
                }

                //find orbital parameters

                if (r < periapsis)
                {
                    periapsis = r;
                    timetoperiapsis = i * 60;
                }
                if (r > apoapsis)
                {
                    apoapsis = r;
                    timetoapoapsis = i * 60;
                }
            }
            eccentricity = (apoapsis - periapsis) / (apoapsis + periapsis);
        }

        public static void draworbits(SpriteBatch sb, GraphicsDevice gd, Matrix matrix, GameTime gameTime, List<Craft> craftlist)
        {
            periapsis = int.MaxValue;
            apoapsis = 0;

            Vector2 p = craftlist[0].pos;
            Vector2 op = craftlist[0].pos;
            Vector2 v = craftlist[0].vel;

            Vector2 ap = new Vector2(0, 0);
            Vector2 pe = new Vector2(0, 0);

            int timestep = 200;
            int t = 0;

            Vector2 v2 = (Flight.planet[0].pos - p);
            double starta = Math.Atan2(v2.Y, v2.X);
            bool b = false;
            Vector2 oop = craftlist[0].pos;
            for (int i = 0; i < 10000; i++)
            {
                //calculate gravity
                op = p;
                float r = (Flight.planet[0].pos - p).Length();
                v2 = (Flight.planet[0].pos - p);
                double a = Math.Atan2(v2.Y, v2.X);
                float G = (0.00000000006674f * (Flight.planet[0].mass) / (r * r));

                //calculate timestep
                timestep = (int)((1 / G) * 1000);
                if (timestep <= 0) { timestep = 1; }

                //next step
                v.X += (float)(G * Math.Cos(a) / 60) * timestep;
                v.Y += (float)(G * Math.Sin(a) / 60) * timestep;
                p += v * timestep / 60;
                t += timestep;

                DrawLine(sb, oop*worldscale, p*worldscale);
                oop = p;

                //stop drawing after full orbit
                if (b)
                {
                    if (a >= starta & a < starta + (Math.PI * 0.5f))
                    {
                        DrawLine(sb, oop, p);

                        sb.Draw(Drawing.apoapsis, new Rectangle((int)(ap.X*worldscale - (1 / Camera.zoom * 5)), (int)(ap.Y*worldscale - (1 / Camera.zoom * 5)), (int)((1 / Camera.zoom) * 10.0f), (int)((1 / Camera.zoom) * 10.0f)), null, Color.White);

                        sb.Draw(Drawing.periapsis, new Rectangle((int)(pe.X*worldscale - (1 / Camera.zoom * 5)), (int)(pe.Y*worldscale - (1 / Camera.zoom * 5)), (int)((1 / Camera.zoom) * 10.0f), (int)((1 / Camera.zoom) * 10.0f)), null, Color.White);

                        break;
                    }
                }
                else
                {
                    if (a > starta + Math.PI || a > starta - Math.PI & a < starta - Math.PI * 0.5f) { b = true; }
                }

                //find orbital parameters

                if (r < periapsis)
                {
                    periapsis = r;
                    timetoperiapsis = t / 60;

                    pe = p;
                }
                if (r > apoapsis)
                {
                    apoapsis = r;
                    timetoapoapsis = t / 60;

                    ap = p;
                }
            }
            eccentricity = (apoapsis - periapsis) / (apoapsis + periapsis);
        }

        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(Drawing.white,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    (int)((1 / Camera.zoom) * 1.0f)), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        public static void DrawblueLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(Drawing.white,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    (int)((1 / Camera.zoom) * 1.0f)), //width of line, change this to make thicker line
                null,
                Color.Blue, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }
}
