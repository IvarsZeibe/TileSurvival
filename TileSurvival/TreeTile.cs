using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using CollisonLibrary;

namespace TileSurvival
{
    class TreeTile : Tile
    {
        Vector2 hitboxSize;
        public override bool canBreak { get; } = true;
        public TreeTile(int x, int y)
        {
            color = Color.DarkGreen;
            TilePosition = new Point(x, y);
            layerDepth = 0.4f;
            hitboxSize = new Vector2(24f, 24f);
            Hitbox = Hitbox.CreateCircle(TilePosition.ToVector2() * WIDTH + new Vector2(WIDTH, WIDTH) * 0.5f, hitboxSize.X * 0.5f);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 size = new Vector2(WIDTH, WIDTH);
            spriteBatch.Draw(Game1.Textures["circle"], Game1.screen.GetScreenPosition(Hitbox.Position - size * 0.5f), null, color, 0f, Vector2.Zero, Game1.GetScale("circle", size), SpriteEffects.None, layerDepth);

        }
    }
}
