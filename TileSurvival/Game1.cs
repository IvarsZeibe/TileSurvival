using CollisonLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameUI;
using System;
using System.Collections.Generic;

namespace TileSurvival
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Shapes shapes;
        internal static UI ui;
        internal static Screen screen;
        internal static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        internal static List<Tile> Tiles = new List<Tile>();
        internal static List<GameObject> gameObjects = new List<GameObject>();
        Player player;
        World world;
        internal static Random rand;
        //1689357599 stone left
        internal static int seed;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1220;
            graphics.PreferredBackBufferHeight = 1200;
            graphics.ApplyChanges();
            screen = new Screen(GraphicsDevice, 1920, 1200);
            base.Initialize();

            seed = new Random().Next();
            seed = 351972793;
            rand = new Random(seed);

            ui = new UI(GraphicsDevice, Textures, Fonts);
            world = new World();
            //Tiles.Add(new Tile(new Point(0, 0)));
            //Tiles.Add(new Tile(1, 1));
            //Tiles.Add(new Tile(120, 80));
            //player = new Player(Vector2.Zero);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            shapes = new Shapes(GraphicsDevice, screen.renderTarget2D.Width *1, screen.renderTarget2D.Height*1);

            var texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });
            Textures.Add("default", texture);
            Textures.Add("circle", CreateCircle(1024, Color.White));
            Textures.Add("circleArc", CreateCircleArc(128, Color.White, 3));
            Textures.Add("player", CreatePlayerTexture(24, Color.Red));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Input.Update();
            //player.Update(gameTime);
            world.Update(gameTime);
            screen.CameraPosition = world.player.Hitbox.Position;

            ui.Update(gameTime);



            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            ui.PrepareDraw(spriteBatch);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            screen.Set();

            shapes.CameraPosition = screen.CameraPosition;
            shapes.Begin();
            //foreach (var tile in Tiles)
            //{
            //    tile.DrawShapes(shapes);
            //}
            world.DrawShapes(shapes);
            //player.DrawShapes(shapes);
            shapes.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);
            //foreach (var tile in Tiles)
            //{
            //    //tile.Draw(spriteBatch);
            //}
            world.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);
            ui.Draw(spriteBatch);
            spriteBatch.End();

            screen.Present(spriteBatch);


            base.Draw(gameTime);
        }

        Texture2D CreateCircle(int diameter, Color color)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, diameter, diameter);
            Color[] data = new Color[diameter * diameter];
            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    if (new Vector2(x - diameter / 2, y - diameter / 2).Length() > diameter / 2)
                    {
                        data[diameter * y + x] = Color.Transparent;
                    }
                    else
                        data[diameter * y + x] = color;
                }
            }
            texture.SetData(data);
            return texture;
        }
        Texture2D CreatePlayerTexture(int size, Color color, Color? eyeColor = null)
        {
            eyeColor ??= Color.Black;
            Texture2D texture = new Texture2D(GraphicsDevice, size, size);
            Color[] data = new Color[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x > size * 0.6 && x < size * 0.75 &&
                        ((y > size * 0.25 && y < size * 0.4) ||
                        (y > size * 0.6 && y < size * 0.75)))
                    {
                        data[y * size + x] = (Color)eyeColor;
                    }
                    else
                        data[y * size + x] = color;
                }
            }
            texture.SetData(data);
            return texture;
        }
        Texture2D CreateCircleArc(int diameter, Color color, float thickness = 1)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, diameter, diameter);
            Color[] data = new Color[diameter * diameter];
            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    float distanceFromCenter = new Vector2(x - diameter / 2, y - diameter / 2).Length();
                    if (distanceFromCenter > diameter / 2 || distanceFromCenter < diameter / 2 - thickness)
                    {
                        data[diameter * y + x] = Color.Transparent;
                    }
                    else
                    {
                        if (y > diameter / 4f && y < diameter / 4f * 3f && x < diameter / 2f)
                            data[diameter * y + x] = color;
                        else
                            data[diameter * y + x] = Color.Transparent;
                    }
                }
            }
            texture.SetData(data);
            return texture;
        }
        public static Vector2 GetScale(string textureName, Vector2 size)
        {
            var texture = Game1.Textures[textureName];
            Vector2 scale = size / new Vector2(texture.Width, texture.Height);
            return scale;
        }
        public static Vector2 GetScale(string textureName, float size)
        {
            var texture = Game1.Textures[textureName];
            Vector2 scale = new Vector2(size, size) / new Vector2(texture.Width, texture.Height);
            return scale;
        }
    }
}
