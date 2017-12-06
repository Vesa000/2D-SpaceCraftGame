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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public List<Craft> craftlist = new List<Craft>();
        public Game1()
        {
            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            Drawing.planet = Content.Load<Texture2D>("Textures/planet");
            Drawing.atmosphere = Content.Load<Texture2D>("Textures/atmosphere");
            Drawing.white = Content.Load<Texture2D>("Textures/white");
            Drawing.arrow = Content.Load<Texture2D>("Textures/arrow");
            Drawing.flame = Content.Load<Texture2D>("Textures/flame");

            Drawing.commandpod = Content.Load<Texture2D>("Textures/commandpod");
            Drawing.fueltank = Content.Load<Texture2D>("Textures/fueltank");
            Drawing.engine = Content.Load<Texture2D>("Textures/engine");
            Drawing.separator = Content.Load<Texture2D>("Textures/separator");

            Drawing.plusbutton = Content.Load<Texture2D>("Textures/plusbutton");
            Drawing.minusbutton = Content.Load<Texture2D>("Textures/minusbutton");
            Drawing.redxbutton = Content.Load<Texture2D>("Textures/redxbutton");
            Drawing.menufont = Content.Load<SpriteFont>("fonts/font1");
            Drawing.font = Content.Load<SpriteFont>("fonts/Segoe_UI_15_Bold");

            Drawing.apoapsis = Content.Load<Texture2D>("Textures/apoapsis");
            Drawing.periapsis = Content.Load<Texture2D>("Textures/periapsis");

            Drawing.dictionary.Add("commandpod", Drawing.commandpod);
            Drawing.dictionary.Add("fueltank", Drawing.fueltank);
            Drawing.dictionary.Add("engine", Drawing.engine);
            Drawing.dictionary.Add("separator", Drawing.separator);
            Drawing.dictionary.Add("none", Drawing.flame);

            Editor.fueltypes.Add("Methalox");
            Editor.fueltypes.Add("Kerlox");

            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height- 80;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!Play.gameon) { this.Exit(); }
            Play.game(craftlist);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Drawing.drawgame(spriteBatch, GraphicsDevice, Camera.matrix(GraphicsDevice.Viewport), gameTime, craftlist);
            base.Draw(gameTime);
        }
    }
}
