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
    public static class DrawEditor
    {
        public static void draw(SpriteBatch sb, GraphicsDevice gd, Matrix matrix, GameTime gameTime, List<Craft> craftlist)
        {
            gd.Clear(Color.CornflowerBlue);
            sb.Begin();
            sb.Draw(Drawing.white, new Rectangle(10, 10, 400, 30), Color.Green);
            sb.DrawString(Drawing.menufont, "Commandpod", new Vector2(50, 10), Color.White);
            sb.Draw(Drawing.white, new Rectangle(10, 50, 400, 30), Color.Green);
            sb.DrawString(Drawing.menufont, "Fueltank", new Vector2(50, 50), Color.White);
            sb.Draw(Drawing.white, new Rectangle(10, 90, 400, 30), Color.Green);
            sb.DrawString(Drawing.menufont, "Engine", new Vector2(50, 90), Color.White);
            sb.Draw(Drawing.white, new Rectangle(10, 130, 400, 30), Color.Green);
            sb.DrawString(Drawing.menufont, "Separator", new Vector2(50, 130), Color.White);


            sb.Draw(Drawing.white, new Rectangle(500, 10, 400, 30), Color.Red);
            sb.DrawString(Drawing.menufont, "Delete craft", new Vector2(550, 10), Color.White);
            sb.Draw(Drawing.white, new Rectangle(1000, 10, 400, 30), Color.Green);
            sb.DrawString(Drawing.menufont, "Fly", new Vector2(1050, 10), Color.White);

            #region part editor
            if (Editor.parteditor & craftlist.Count != 0)
            {
                foreach (Part p in craftlist[0].partlist)
                {
                    if (p.id == Editor.parteditornumber)
                    {
                        //V2 is startpoint of the parteditor window
                        Vector2 v2 = new Vector2(0, Camera.screencenter.Y * 2 - 500);

                        //draw line from part to parteditorwindow
                        //Drawflight.DrawLine(sb,v2+new Vector2(300,0), new Vector2((p.pos.X)* Camera.zoom + Camera.pos.X, ((p.pos.Y) * Camera.zoom + Camera.pos.Y)));

                        //draw part editor borders
                        sb.Draw(Drawing.white, new Rectangle((int)v2.X, (int)v2.Y, 300, 500), Color.Blue);
                        
                        //part type
                        sb.Draw(Drawing.white, new Rectangle((int)v2.X + 10, (int)v2.Y + 10, 240, 30), Color.White);
                        sb.DrawString(Drawing.menufont, p.type, new Vector2(v2.X + 10, v2.Y + 10), Color.Black);

                        //delete part button
                        sb.Draw(Drawing.redxbutton, new Rectangle((int)v2.X + 260, (int)v2.Y + 10, 30, 30), Color.White);
                        
                        //part width
                        sb.Draw(Drawing.minusbutton, new Rectangle((int)v2.X + 10, (int)v2.Y + 50, 30, 30), Color.White);
                        sb.Draw(Drawing.plusbutton, new Rectangle((int)v2.X + 260, (int)v2.Y + 50, 30, 30), Color.White);
                        sb.Draw(Drawing.white, new Rectangle((int)v2.X + 50, (int)v2.Y + 50, 200, 30), Color.White);
                        sb.DrawString(Drawing.font, "Width: " + (p.size.X / 1000).ToString() + " M", new Vector2(v2.X + 50, v2.Y + 50), Color.Black);

                        //part height
                        sb.Draw(Drawing.minusbutton, new Rectangle((int)v2.X + 10, (int)v2.Y + 100, 30, 30), Color.White);
                        sb.Draw(Drawing.plusbutton, new Rectangle((int)v2.X + 260, (int)v2.Y + 100, 30, 30), Color.White);
                        sb.Draw(Drawing.white, new Rectangle((int)v2.X + 50, (int)v2.Y + 100, 200, 30), Color.White);
                        sb.DrawString(Drawing.font, "Height: " + (p.size.Y / 1000).ToString() + " M", new Vector2(v2.X + 50, v2.Y + 100), Color.Black);

                        //part mass
                        sb.Draw(Drawing.white, new Rectangle((int)v2.X + 10, (int)v2.Y + 150, 280, 30), Color.White);
                        sb.DrawString(Drawing.font, "Mass: " + Drawing.getsiprefix(p.mass * 1000) + "g", new Vector2(v2.X + 50, v2.Y + 150), Color.Black);

                        #region commandpod
                        if (p.type == "commandpod")
                        {
                        }
                        #endregion
                        #region fueltank
                        if (p.type == "fueltank")
                        {
                            //wet mass
                            sb.Draw(Drawing.white, new Rectangle((int)v2.X + 10, (int)v2.Y + 200, 280, 30), Color.White);
                            sb.DrawString(Drawing.font, "Wet mass: " + Drawing.getsiprefix((p.mass + p.fuel) * 1000) + "g", new Vector2(v2.X + 50, v2.Y + 200), Color.Black);
                            //fueltype
                            sb.Draw(Drawing.minusbutton, new Rectangle((int)v2.X + 10, (int)v2.Y + 250, 30, 30), Color.White);
                            sb.Draw(Drawing.plusbutton, new Rectangle((int)v2.X + 260, (int)v2.Y + 250, 30, 30), Color.White);
                            sb.Draw(Drawing.white, new Rectangle((int)v2.X + 50, (int)v2.Y + 250, 200, 30), Color.White);
                            sb.DrawString(Drawing.font, "fueltype: " + p.fueltype, new Vector2(v2.X + 50, v2.Y + 250), Color.Black);
                        }
                        #endregion
                        #region engine
                        if (p.type == "engine")
                        {
                            sb.Draw(Drawing.white, new Rectangle((int)v2.X + 10, (int)v2.Y + 200, 280, 30), Color.White);
                            sb.DrawString(Drawing.font, "ISP: " + p.isp + "S", new Vector2(v2.X + 50, v2.Y + 200), Color.Black);

                            sb.Draw(Drawing.minusbutton, new Rectangle((int)v2.X + 10, (int)v2.Y + 250, 30, 30), Color.White);
                            sb.Draw(Drawing.plusbutton, new Rectangle((int)v2.X + 260, (int)v2.Y + 250, 30, 30), Color.White);
                            sb.Draw(Drawing.white, new Rectangle((int)v2.X + 50, (int)v2.Y + 250, 200, 30), Color.White);
                            sb.DrawString(Drawing.font, "Fuel type: " + p.fueltype, new Vector2(v2.X + 50, v2.Y + 250), Color.Black);

                            sb.Draw(Drawing.minusbutton, new Rectangle((int)v2.X + 10, (int)v2.Y + 300, 30, 30), Color.White);
                            sb.Draw(Drawing.plusbutton, new Rectangle((int)v2.X + 260, (int)v2.Y + 300, 30, 30), Color.White);
                            sb.Draw(Drawing.white, new Rectangle((int)v2.X + 50, (int)v2.Y + 300, 200, 30), Color.White);
                            sb.DrawString(Drawing.font, "thrust: " + Drawing.getsiprefix(p.thrust) + "N", new Vector2(v2.X + 50, v2.Y + 300), Color.Black);
                        }
                        #endregion
                        #region separator
                        if (p.type == "separator")
                        {
                        }
                        #endregion
                    }
                }
            }
            #endregion

            #region stages
            if (Editor.stage != null & craftlist.Count != 0)
            {
                for (int s = 0; s < 10; s++)
                {
                    if (Editor.stage[s] != null)
                    {
                        sb.Draw(Drawing.white, Editor.stagerec[s], Color.Red);
                        sb.DrawString(Drawing.menufont, "S" + s.ToString(), new Vector2(Editor.stagerec[s].X + 5, Editor.stagerec[s].Y + (Editor.stagerec[s].Height / 2) - 20), Color.White);

                        foreach (int i in Editor.stage[s])
                        {
                            foreach (Part p in craftlist[0].partlist)
                            {
                                if (p.id == i)
                                {
                                    if (p.id!=Editor.stageonhand)
                                    {
                                        sb.Draw(Drawing.white, new Rectangle((int)Editor.stageiconpositionditictionary[i].X, (int)Editor.stageiconpositionditictionary[i].Y, 40, 40), Color.White);
                                        sb.Draw(Drawing.dictionary[p.type], new Rectangle((int)Editor.stageiconpositionditictionary[i].X, (int)Editor.stageiconpositionditictionary[i].Y, 40, 40), Color.White);
                                    }
                                }
                            }
                        }
                    }
                }
                if (Editor.stageonhand != 0)
                {
                    foreach (Part p in craftlist[0].partlist)
                    {
                        if (p.id == Editor.stageonhand)
                        {
                            sb.Draw(Drawing.white, new Rectangle((int)Play.mousestate.X - 20, Play.mousestate.Y - 20, 40, 40), Color.White);
                            sb.Draw(Drawing.dictionary[p.type], new Rectangle((int)Play.mousestate.X - 20, Play.mousestate.Y - 20, 40, 40), Color.White);
                        }
                    }
                }
            }
            #endregion

            #region deltavstats
            if (Editor.stage != null & craftlist.Count != 0&true)
            {
                Craft c = new Craft();
                int laststage = 10;
                c.partlist = new List<Part>(craftlist[0].partlist);
                foreach(Part p in c.partlist)
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

                        if (Editor.stage[s].Count == 0&laststage==10) { laststage = s; }
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
                                                    float mass = getmass(c);
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
                sb.Draw(Drawing.white, new Rectangle(400,(int)Camera.screencenter.Y*2+10-(laststage+1)*ssep, 600, (laststage+2)*ssep), Color.Black);

                for(int i=0;i<laststage;i++)
                {
                    sb.Draw(Drawing.white, new Rectangle(400, (int)Camera.screencenter.Y * 2 - 50 - i * ssep, 90, ssep), Color.Gray);
                    sb.DrawString(Drawing.font, deltav[i].ToString(), new Vector2(400, Camera.screencenter.Y * 2 - 50 - i * ssep), Color.White);

                    sb.Draw(Drawing.white, new Rectangle(500, (int)Camera.screencenter.Y * 2 - 50 - i * ssep, 90, ssep), Color.Gray);
                    sb.DrawString(Drawing.font, twr[i].ToString(), new Vector2(500, Camera.screencenter.Y * 2 - 50 - i * ssep), Color.White);
                }
                sb.DrawString(Drawing.font, "deltav", new Vector2(400, Camera.screencenter.Y * 2+10 - (laststage+1) * ssep), Color.White);

                sb.DrawString(Drawing.font, "TWR", new Vector2(500, Camera.screencenter.Y * 2+10  - (laststage+1) * ssep), Color.White);

                foreach(Part p in c.partlist)
                {
                    p.dontcalculatemymass = false;
                }
            }
            sb.End();
            #endregion
            sb.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.matrix(gd.Viewport));

            Rectangle rec = new Rectangle((int)(((Play.mousestate.X - Camera.screencenter.X) / Camera.zoom) + Camera.pos.X), (int)(((Play.mousestate.Y - Camera.screencenter.Y) / Camera.zoom) + Camera.pos.Y), (int)Editor.handpart.size.X, (int)Editor.handpart.size.Y);
            if (Editor.handpart.type != "none") { sb.Draw(Drawing.dictionary[Editor.handpart.type], rec, Color.White); }

            if (craftlist.Count != 0)
            {
                #region drawparts


                foreach (Part p in craftlist[0].partlist)
                {
                    sb.Draw(Drawing.dictionary[p.type], new Rectangle((int)p.pos.X * 1, (int)p.pos.Y, (int)p.size.X, (int)p.size.Y), Color.White);
                    if (Editor.stageonhand != 0 & p.id == Editor.stageonhand || Editor.stageitemhovered != 0 & p.id == Editor.stageitemhovered)
                    {
                        Color c = Color.Green;
                        c.A = 100;
                        sb.Draw(Drawing.dictionary[p.type], new Rectangle((int)p.pos.X, (int)p.pos.Y, (int)p.size.X, (int)p.size.Y), c);
                    }
                    #region snap part
                    if (Editor.handpart.type != "none")
                    {
                        //draw part that will snap
                        Vector2 v2 = new Vector2(((Play.mousestate.X - Camera.screencenter.X) / Camera.zoom) + Camera.pos.X, (((Play.mousestate.Y - Camera.screencenter.Y) / Camera.zoom) + Camera.pos.Y));
                        //snap to bottom
                        if (Play.isinside(v2, new Vector2(p.pos.X + (p.size.X / 2) - Editor.handpart.size.X / 2, (p.pos.Y + p.size.Y)), new Vector2(Editor.handpart.size.X, Editor.handpart.size.Y)))
                        {
                            sb.Draw(Drawing.dictionary[Editor.handpart.type], new Rectangle((int)(p.pos.X + (p.size.X / 2) - (Editor.handpart.size.X / 2)), (int)((p.pos.Y + p.size.Y)), (int)Editor.handpart.size.X, (int)Editor.handpart.size.Y), Color.White);
                        }
                        //snap to top
                        if (Play.isinside(v2, new Vector2((p.pos.X + (p.size.X / 2) - (Editor.handpart.size.X / 2)), (p.pos.Y - Editor.handpart.size.Y)), new Vector2(Editor.handpart.size.X, Editor.handpart.size.Y)))
                        {
                            sb.Draw(Drawing.dictionary[Editor.handpart.type], new Rectangle((int)(p.pos.X + (p.size.X / 2) - (Editor.handpart.size.X / 2)), (int)(p.pos.Y - Editor.handpart.size.Y), (int)Editor.handpart.size.X, (int)Editor.handpart.size.Y), Color.White);
                        }
                        //snap to left
                        if (Play.isinside(v2, new Vector2((p.pos.X - Editor.handpart.size.X), (p.pos.Y + (p.size.Y / 2) - (Editor.handpart.size.Y / 2))), new Vector2(Editor.handpart.size.X, Editor.handpart.size.Y)))
                        {
                            sb.Draw(Drawing.dictionary[Editor.handpart.type], new Rectangle((int)(p.pos.X - Editor.handpart.size.X), (int)(p.pos.Y + (p.size.Y / 2) - (Editor.handpart.size.Y / 2)), (int)Editor.handpart.size.X, (int)Editor.handpart.size.Y), Color.White);
                        }
                        //snap to right
                        if (Play.isinside(v2, new Vector2((p.pos.X + p.size.X), (p.pos.Y + (p.size.Y / 2) - (Editor.handpart.size.Y / 2))), new Vector2(Editor.handpart.size.X, Editor.handpart.size.Y)))
                        {
                            sb.Draw(Drawing.dictionary[Editor.handpart.type], new Rectangle((int)(p.pos.X + p.size.X), (int)(p.pos.Y + (p.size.Y / 2) - (Editor.handpart.size.Y / 2)), (int)Editor.handpart.size.X, (int)Editor.handpart.size.Y), Color.White);
                        }
                    }
                }
                #endregion
            }
            sb.End();
            #endregion
        }
        public static float getmass(Craft c)
        {
            float m = 0f;
            foreach (Part p in c.partlist)
            {
                if (!p.dontcalculatemymass)
                {
                    m += p.mass;
                    m += p.fuel;
                }
            }
            return m;
        }

    }
}
