using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TileSurvival
{
    class AirTile : Tile
    {
        public Biome biome { get; }
        public AirTile(int x, int y)
        {
            TilePosition = new Point(x, y);

        }
    }
}
