using CollisonLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TileSurvival
{
    class StoneTile : Tile
    {
        public override bool canBreak { get; } = true;
        public StoneTile(int x, int y)
        {
            TilePosition = new Point(x, y);
            Hitbox = Hitbox.CreateRectanglePolygon(TilePosition.ToVector2() * WIDTH + new Vector2(WIDTH, WIDTH) * 0.5f, WIDTH, WIDTH);
            color = Color.DarkGray;
        }
        public override void DrawShapes(Shapes shapes)
        {
            shapes.DrawPolygon(Hitbox, color, layerDepth);
        }
    }
}
