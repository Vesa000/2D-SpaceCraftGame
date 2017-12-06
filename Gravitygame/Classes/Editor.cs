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
    public static class Editor
    {
        //public static string partinhand = "none";
        public static Part handpart = new Part();

        public static bool parteditor = false;
        public static int parteditornumber = 0;

        public static List<int>[] stage = new List<int>[10];
        public static Dictionary<int, Vector2> stageiconpositionditictionary = new Dictionary<int, Vector2>();
        public static Rectangle[] stagerec = new Rectangle[10];
        public static int stageonhand = 0;
        public static int stageitemhovered = 0;

        public static List<string> fueltypes = new List<string>();

        public static void edit(List<Craft> craftlist)
        {
            #region camera
            if (Play.mousestate.MiddleButton == ButtonState.Pressed)
            {
                Camera.pos -= (Play.mousestate.Position.ToVector2() - Play.oldmousestate.Position.ToVector2()) / Camera.zoom;
            }
            Camera.zoom *= 1 + (Play.mousestate.ScrollWheelValue - Play.oldmousestate.ScrollWheelValue) * 0.001f;
            #endregion

            #region stages
            if (craftlist.Count != 0)
            {
                for (int i = 0; i < stage.Length; i++)
                {
                    stage[i] = new List<int>();
                }

                foreach (Part p in craftlist[0].partlist)
                {
                    if (p.type == "engine" || p.type == "separator")
                    {
                        stage[p.stage].Add(p.id);
                    }
                }
            }
            Vector2 pos = new Vector2(Camera.screencenter.X * 2 - 60, Camera.screencenter.Y * 2 - 60);
            stageiconpositionditictionary.Clear();

            for (int i = 0; i < 10; i++)
            {
                if (stage[i] != null)
                {
                    int startheight = (int)pos.Y;
                    foreach (int j in stage[i])
                    {
                        stageiconpositionditictionary.Add(j, pos);
                        pos.Y -= 50;
                    }
                    //empty stage
                    if (pos.Y == startheight)
                    {
                        stagerec[i] = new Rectangle((int)pos.X - 70, (int)pos.Y + 40, 120, (int)(30));
                    }
                    else
                    {
                        stagerec[i] = new Rectangle((int)pos.X - 70, (int)pos.Y + 40, 120, (int)(startheight - pos.Y + 20));
                    }
                }
                pos.Y -= 50;
            }
            stageitemhovered = 0;
            if (stage != null & craftlist.Count != 0)
            {
                foreach (Part p in craftlist[0].partlist)
                {
                    if (p.type == "engine" || p.type == "separator")
                    {
                        if (Play.isinside(new Vector2(Play.mousestate.X, Play.mousestate.Y), new Vector2(stageiconpositionditictionary[p.id].X, stageiconpositionditictionary[p.id].Y), new Vector2(40, 40)))
                        {
                            stageitemhovered = p.id;
                        }
                    }
                }
            }
            #endregion

            #region leftbuttonpressed
            //leftbuttonpressed
            if (Play.mousestate.LeftButton == ButtonState.Pressed & Play.oldmousestate.LeftButton == ButtonState.Released)
            {
                #region dragstages
                if (stage != null & craftlist.Count != 0)
                {
                    if (stageonhand == 0)
                    {
                        foreach (Part p in craftlist[0].partlist)
                        {
                            if (p.type == "engine" || p.type == "separator")
                            {
                                if (Play.isinside(new Vector2(Play.mousestate.X, Play.mousestate.Y), new Vector2(stageiconpositionditictionary[p.id].X, stageiconpositionditictionary[p.id].Y), new Vector2(40, 40)))
                                {
                                    stageonhand = p.id;
                                }
                            }
                        }
                    }
                    //stage on hand
                    else
                    {
                        for (int i = 0; i < stage.Length; i++)
                        {
                            if (Play.isinside(new Vector2(Play.mousestate.X, Play.mousestate.Y), new Vector2(stagerec[i].X, stagerec[i].Y), new Vector2(stagerec[i].Width, stagerec[i].Height)))
                            {
                                foreach (Part p in craftlist[0].partlist)
                                {
                                    if (p.id == stageonhand)
                                    {
                                        p.stage = i;
                                        stageonhand = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region parteditor
                Vector2 mousepos = Play.mousestate.Position.ToVector2();
                if (parteditor & Play.isinside(mousepos, new Vector2(0, Camera.screencenter.Y * 2 - 500), new Vector2(300, 500)))
                {
                    foreach (Part p in craftlist[0].partlist)
                    {
                        if (p.id == Editor.parteditornumber)
                        {
                            #region delete part
                            Vector2 v2 = new Vector2(0, Camera.screencenter.Y * 2 - 500);
                            if (Play.isinside(mousepos, new Vector2(v2.X + 260, v2.Y + 10), new Vector2(30, 30)))
                            {
                                p.removeme = true;
                            }
                            #endregion

                            #region change part size
                            Vector2 sizedelta = new Vector2(0, 0);
                            //if smaller width button pressed
                            if (Play.isinside(mousepos, new Vector2(v2.X + 10, v2.Y + 50), new Vector2(30, 30)))
                            {
                                sizedelta.X -= 100;
                            }
                            //if larger width button pressed
                            if (Play.isinside(mousepos, new Vector2(v2.X + 260, v2.Y + 50), new Vector2(30, 30)))
                            {
                                sizedelta.X += 100;
                            }
                            //if smaller height button pressed
                            if (Play.isinside(mousepos, new Vector2(v2.X + 10, v2.Y + 100), new Vector2(30, 30)))
                            {
                                sizedelta.Y -= 100;
                            }
                            //if larher height button pressed
                            if (Play.isinside(mousepos, new Vector2(v2.X + 260, v2.Y + 100), new Vector2(30, 30)))
                            {
                                sizedelta.Y += 100;
                            }
                            if (Play.state.IsKeyDown(Keys.LeftShift)) { sizedelta *= 0.1f; }
                            if (Play.state.IsKeyDown(Keys.LeftControl)) { sizedelta *= 10; }

                            if (p.parentdirection == "center")
                            {
                                p.pos.X -= sizedelta.X / 2;
                                p.size.X += sizedelta.X;
                                p.pos.Y -= sizedelta.Y / 2;
                                p.size.Y += sizedelta.Y;
                            }
                            if (p.parentdirection == "up")
                            {
                                p.pos.X -= sizedelta.X / 2;
                                p.size.X += sizedelta.X;
                                p.size.Y += sizedelta.Y;
                            }
                            if (p.parentdirection == "down")
                            {
                                p.pos.X -= sizedelta.X / 2;
                                p.size.X += sizedelta.X;
                                p.pos.Y -= sizedelta.Y / 2;
                                p.size.Y += sizedelta.Y / 2;
                            }
                            if (p.parentdirection == "left")
                            {
                                p.pos.Y -= sizedelta.Y / 2;
                                p.size.Y += sizedelta.Y;
                                p.size.X += sizedelta.X;
                            }
                            if (p.parentdirection == "right")
                            {
                                p.pos.Y -= sizedelta.Y / 2;
                                p.size.Y += sizedelta.Y;
                                p.pos.X -= sizedelta.X;
                                p.size.X += sizedelta.X;
                            }
                            #endregion

                            #region commandpod
                            if (p.type == "commandpod")
                            {
                                p.mass = 10000;
                            }
                            #endregion

                            #region fueltank
                            if (p.type == "fueltank")
                            {
                                //change fueltype backward
                                if (Play.isinside(mousepos, new Vector2(v2.X + 10, v2.Y + 250), new Vector2(30, 30)))
                                {
                                    p.fueltype = changefueltype(p.fueltype, -1);
                                }

                                //change fueltype forward
                                if (Play.isinside(mousepos, new Vector2(v2.X + 260, v2.Y + 250), new Vector2(30, 30)))
                                {
                                    p.fueltype = changefueltype(p.fueltype, 1);
                                }
                                double volume = Math.PI * p.size.X / 1000 * p.size.X / 1000 * p.size.Y / 1000 * 1000;
                                volume *= 0.8443f;
                                p.mass = (float)volume * 0.1f;
                                if (p.fueltype == "Methalox")
                                {
                                    //fuel density*volume of tank
                                    p.fuel = (float)(0.8376 * volume);
                                }
                                else if (p.fueltype == "Kerlox")
                                {
                                    //fuel density*volume of tank
                                    p.fuel = (float)(1.0112 * volume);
                                }
                            }
                            #endregion

                            #region engine
                            if (p.type == "engine")
                            {
                                if (Play.isinside(mousepos, new Vector2(v2.X + 10, v2.Y + 250), new Vector2(30, 30)))
                                {
                                    p.fueltype = changefueltype(p.fueltype, -1);
                                }

                                if (Play.isinside(mousepos, new Vector2(v2.X + 260, v2.Y + 250), new Vector2(30, 30)))
                                {
                                    p.fueltype = changefueltype(p.fueltype, 1);
                                }

                                float thrustdelta = 0;
                                //less thrust
                                if (Play.isinside(mousepos, new Vector2(v2.X + 260, v2.Y + 300), new Vector2(30, 30)))
                                {
                                    thrustdelta += 1;
                                }

                                //MOAR POWER
                                if (Play.isinside(mousepos, new Vector2(v2.X + 10, v2.Y + 300), new Vector2(30, 30)))
                                {
                                    thrustdelta -= 1;
                                }
                                thrustdelta *= 10000;
                                if (Play.state.IsKeyDown(Keys.LeftShift)) { thrustdelta *= 0.1f; }
                                if (Play.state.IsKeyDown(Keys.LeftControl)) { thrustdelta *= 10; }
                                p.thrust += thrustdelta;

                                p.mass = (p.thrust / (90*9.81f));

                                if (p.fueltype == "Methalox") { p.isp = 361; }
                                else if (p.fueltype == "Kerlox") { p.isp = 305; }
                            }
                            #endregion

                            #region separator
                            if (p.type == "separator")
                            {

                            }
                            #endregion
                        }
                    }
                    for (int i = 0; i < craftlist[0].partlist.Count(); i++)
                    {
                        if (craftlist[0].partlist[i].removeme)
                        {
                            //remove deleted part
                            int id = craftlist[0].partlist[i].id;
                            craftlist[0].partlist.Remove(craftlist[0].partlist[i]);

                            for (int j = 0; j < craftlist[0].partlist.Count(); j++)
                            {
                                for (int k = 0; k < craftlist[0].partlist[j].neibours.Count(); k++)
                                {
                                    //and from other parts neibours
                                    if (craftlist[0].partlist[j].neibours[k] == id) { craftlist[0].partlist[j].neibours.Remove(craftlist[0].partlist[j].neibours[k]); }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region menu buttons
                else if (Play.isinside(new Vector2(Play.mousestate.X, Play.mousestate.Y), new Vector2(10, 10), new Vector2(400, 30)))
                {
                    handpart.type = "commandpod";
                }
                else if (Play.isinside(new Vector2(Play.mousestate.X, Play.mousestate.Y), new Vector2(10, 50), new Vector2(400, 30)))
                {
                    handpart.type = "fueltank";
                }
                else if (Play.isinside(new Vector2(Play.mousestate.X, Play.mousestate.Y), new Vector2(10, 90), new Vector2(400, 30)))
                {
                    handpart.type = "engine";
                }
                else if (Play.isinside(new Vector2(Play.mousestate.X, Play.mousestate.Y), new Vector2(10, 130), new Vector2(400, 30)))
                {
                    handpart.type = "separator";
                }

                else if (Play.isinside(new Vector2(Play.mousestate.X, Play.mousestate.Y), new Vector2(500, 10), new Vector2(400, 30)))
                {
                    craftlist.Clear();
                }
                else if (Play.isinside(new Vector2(Play.mousestate.X, Play.mousestate.Y), new Vector2(1000, 10), new Vector2(400, 30)))
                {
                    craftlist[0] = changecraftpositions(craftlist[0]);
                    craftlist[0] = Flight.recalculatecollisionbox(craftlist[0]);
                    Flight.startflight();
                    Play.editor = false;
                    Play.flight = true;
                }
                #endregion

                #region place part
                else if (handpart.type != "none")
                {
                    #region add first part
                    //add firstpart
                    if (craftlist.Count == 0)
                    {
                        Craft craft = new Craft();
                        Part part = new Part();
                        part.id = craft.partnumber;
                        craft.partnumber++;
                        part.pos = new Vector2((((Play.mousestate.X - Camera.screencenter.X) / Camera.zoom) + Camera.pos.X) * 1, ((((Play.mousestate.Y - Camera.screencenter.Y) / Camera.zoom) + Camera.pos.Y) * 1));
                        part.type = handpart.type;
                        craft.partlist.Add(part);
                        craftlist.Add(craft);
                    }
                    #endregion

                    #region add part to craft
                    //add part
                    else
                    {
                        bool snapped = false;
                        for (int i = 0; i < craftlist[0].partlist.Count() & !snapped; i++)
                        {
                            Part p = craftlist[0].partlist[i];
                            Vector2 v2 = new Vector2(((Play.mousestate.X - Camera.screencenter.X) / Camera.zoom) + Camera.pos.X, (((Play.mousestate.Y - Camera.screencenter.Y) / Camera.zoom) + Camera.pos.Y));
                            //snap below
                            if (Play.isinside(v2, new Vector2(p.pos.X + (p.size.X / 2) - Editor.handpart.size.X / 2, (p.pos.Y + p.size.Y)), new Vector2(Editor.handpart.size.X, Editor.handpart.size.Y)))
                            {
                                handpart.pos = new Vector2(p.pos.X + (p.size.X / 2) - (Editor.handpart.size.X / 2), p.pos.Y + p.size.Y);
                                handpart.parentdirection = "up";
                                snapped = true;
                            }
                            //snap above
                            if (Play.isinside(v2, new Vector2((p.pos.X + (p.size.X / 2) - (Editor.handpart.size.X / 2)), (p.pos.Y - Editor.handpart.size.Y)), new Vector2(Editor.handpart.size.X, Editor.handpart.size.Y)))
                            {
                                handpart.pos = new Vector2(p.pos.X + (p.size.X / 2) - (Editor.handpart.size.X / 2), p.pos.Y - Editor.handpart.size.Y);
                                handpart.parentdirection = "down";
                                snapped = true;
                            }
                            //snap left
                            if (Play.isinside(v2, new Vector2((p.pos.X - Editor.handpart.size.X), (p.pos.Y + (p.size.Y / 2) - (Editor.handpart.size.Y / 2))), new Vector2(Editor.handpart.size.X, Editor.handpart.size.Y)))
                            {
                                handpart.pos = new Vector2(p.pos.X - Editor.handpart.size.X, p.pos.Y + (p.size.Y / 2) - (Editor.handpart.size.Y / 2));
                                handpart.parentdirection = "right";
                                snapped = true;
                            }
                            //snap right
                            if (Play.isinside(v2, new Vector2((p.pos.X + p.size.X), (p.pos.Y + (p.size.Y / 2) - (Editor.handpart.size.Y / 2))), new Vector2(Editor.handpart.size.X, Editor.handpart.size.Y)))
                            {
                                handpart.pos = new Vector2(p.pos.X + p.size.X, p.pos.Y + (p.size.Y / 2) - (Editor.handpart.size.Y / 2));
                                handpart.parentdirection = "left";
                                snapped = true;
                            }

                            if (snapped)
                            {
                                Part prt = new Part();
                                prt.pos = handpart.pos;
                                prt.parentdirection = handpart.parentdirection;
                                prt.type = handpart.type;
                                prt.id = craftlist[0].partnumber;

                                prt.parent = p.id;
                                prt.neibours.Add(p.id);
                                p.neibours.Add(prt.id);

                                craftlist[0].partnumber++;
                                craftlist[0].partlist.Add(prt);
                            }
                        }
                    }
                    #endregion
                }
                #region remove part
                else
                {

                }
                #endregion

                #endregion
            }
            #endregion

            #region rightbuttonpressed
            //rightbuttonpressed
            else if (Play.mousestate.RightButton == ButtonState.Pressed & Play.oldmousestate.RightButton == ButtonState.Released)
            {
                handpart.type = "none";
                stageonhand = 0;

                for (int i = 0; i < craftlist[0].partlist.Count(); i++)
                {
                    Part p = craftlist[0].partlist[i];
                    Vector2 v2 = new Vector2(((Play.mousestate.X - Camera.screencenter.X) / Camera.zoom) + Camera.pos.X, (((Play.mousestate.Y - Camera.screencenter.Y) / Camera.zoom) + Camera.pos.Y));
                    if (Play.isinside(v2, new Vector2(p.pos.X, p.pos.Y), new Vector2(p.size.X, p.size.Y)))
                    {
                        parteditor = true;
                        parteditornumber = p.id;
                    }
                }
            }
            #endregion

        }
        public static string changefueltype(string s, int i)
        {
            if (s == "nofuel") { s = fueltypes[0]; }
            else
            {
                for (int j = 0; j < fueltypes.Count; j++)
                {
                    if (s == fueltypes[j])
                    {
                        i += j;
                        if (i >= fueltypes.Count()) { i = 0; }
                        if (i < 0) { i = fueltypes.Count() - 1; }
                        s = fueltypes[i];
                        break;
                    }
                }
            }
            return s;
        }

        public static Craft changecraftpositions(Craft c)
        {
            Vector2 ppos = c.partlist[0].pos;
            for (int i = 0; i < c.partlist.Count(); i++)
            {
                c.partlist[i].pos -= ppos;
                c.partlist[i].pos /= 1000;
                c.partlist[i].size /= 1000;
                c.needsrerendering=2;
            }

            return c;
        }
    }
}
