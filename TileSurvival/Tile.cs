using CollisonLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TileSurvival
{
    abstract class Tile : GameObject
    {
        public Point TilePosition;
        public Color color = Color.Transparent;
        protected const int WIDTH = 32;
        protected float layerDepth = 0.8f;
        public virtual bool canBreak { get; } = false;
        //public Tile(Point position, bool hasCollision)
        //{
        //    TilePosition = position;
        //    WorldPosition = TilePosition.ToVector2() * WIDTH;
        //    if(hasCollision)
        //        Hitbox = Hitbox.CreateRectanglePolygon(WorldPosition + new Vector2(WIDTH, WIDTH) * 0.5f, WIDTH, WIDTH);
        //}
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch) 
        {
            //spriteBatch.Draw(Game1.Textures["default"], Game1.screen.GetScreenPosition(WorldPosition) - new Vector2(WIDTH, WIDTH) * 0.5f, null, color, 0f, Vector2.Zero, Game1.GetScale("default", new Vector2(WIDTH, WIDTH)), SpriteEffects.None, layerDepth);
        }
        public override void DrawShapes(Shapes shapes) 
        {
            //shapes.DrawPolygon(Hitbox, color, layerDepth);
        }
        public void Break()
        {
            if (canBreak)
            {
                IsAlive = false;
                World.GetChunk(TilePosition).AddGameObject(new AirTile(TilePosition.X, TilePosition.Y));
            }
        }
    }
}
