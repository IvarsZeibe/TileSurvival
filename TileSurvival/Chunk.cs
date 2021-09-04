using CollisonLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TileSurvival
{
    public enum Biome
    {
        None,
        Plains,
        Forest,
        Desert,
        Mesa,
        Ocean,
        Snow,
        Mountains
    }
    class Chunk
    {
        public List<GameObject> gameObjects = new List<GameObject>();
        public List<Tile> tiles { get => gameObjects.OfType<Tile>().ToList(); }

        public Hitbox Hitbox;
        Point Coord;
        const int tileCount = 16;
        float layerDepth = 0.9f;
        Vector2 size = new Vector2(512, 512);
        public bool IsLoaded = false;
        Color groundColor;
        public Biome biome;
        Random rand;
        public Chunk(Point coord)
        {
            Coord = coord;
            Hitbox = Hitbox.CreateRectanglePolygon(coord.ToVector2() * size.X ,  size.X, size.Y);
            rand = new Random(Game1.seed * Coord.GetHashCode());
            biome = BiomeGenerator.GenerateBiome(coord);
            switch (biome)
            {
                case Biome.Plains:
                    groundColor = new Color(100, 230, 100);
                    break;
                case Biome.Forest:
                    groundColor = Color.Green;
                    break;
                case Biome.Desert:
                    groundColor = Color.Yellow;
                    break;
                case Biome.Mesa:
                    groundColor = Color.Orange;
                    break;
                case Biome.Ocean:
                    groundColor = Color.Blue;
                    break;
                case Biome.Snow:
                    groundColor = Color.White;
                    break;
                case Biome.Mountains:
                    groundColor = Color.Gray;
                    break;
            }
            for(int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    if (biome == Biome.Mountains) {
                        if (rand.Next(4) == 0)
                        {
                            AddGameObject(new StoneTile(x + Coord.X * 16 - 8, y + Coord.Y * 16 - 8));
                            continue;
                        }
                    }
                    if (biome == Biome.Forest)
                    {
                        if (rand.Next(7) == 0)
                        {
                            AddGameObject(new TreeTile(x + Coord.X * 16 - 8, y + Coord.Y * 16 - 8));
                            continue;
                        }
                    }

                    AddGameObject(new AirTile(x + Coord.X * 16 - 8, y + Coord.Y * 16 - 8));
                }
            }
            //if(biome == Biome.Mountains)
            //{
            //    for (int x = 0; x < 16; x++)
            //    {
            //        for (int y = 0; y < 16; y++)
            //        {
            //            AddGameObject(new Tile(x + Coord.X * 16 - 8, y + Coord.Y * 16 - 8));
            //        }
            //    }

            //}
        }
        public virtual void Update(GameTime gameTime)
        {
            gameObjects = gameObjects.Where(gameObject => gameObject.IsAlive).ToList();
            foreach (var gameObject in gameObjects)
                gameObject.Update(gameTime);
            
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in gameObjects)
                gameObject.Draw(spriteBatch);
        }
        public virtual void DrawShapes(Shapes shapes)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[Hitbox.Vertices.Count];
            List<float> colorVariation = BiomeGenerator.GradientForPolygon(Hitbox.Vertices);
            for (int i = 0; i < vertices.Length; i++)
            {
                var color = groundColor * Math.Clamp((colorVariation[i] + 1f), 0.9f, 1.2f);
                color.A = 255;
                vertices[i] = new VertexPositionColor(new Vector3(Hitbox.Vertices[i], layerDepth), color);
            }
            shapes.DrawPolygon(vertices);
            //var worldPosition = Coord.ToVector2() * size;
            //var color = groundColor * Math.Clamp((BiomeGenerator.GradientForPolygon(new List<Vector2>() {Coord.ToVector2() * size - size * 0.5f })[0] + 1), 0.8f, 1.2f);
            //shapes.DrawRectangle(worldPosition.X, worldPosition.Y, size.X, size.Y, layerDepth, color);
            foreach (var gameObject in gameObjects)
                gameObject.DrawShapes(shapes);
        }
        public void AddGameObject(GameObject gameObject)
        {
            if (gameObject is Tile)
            {
                var newTile = gameObject as Tile;
                if (newTile.TilePosition.X - Coord.X * 16 > tileCount || newTile.TilePosition.Y - Coord.Y * 16 > tileCount)
                    return;
                if (!gameObjects.Where(tile => tile is Tile).Any(tile => (tile as Tile).TilePosition == newTile.TilePosition && tile.IsAlive))
                {
                    gameObjects.Add(newTile);
                }
                else
                {

                }
            }
            else
                gameObjects.Add(gameObject);
        }
        ///Version 3
        //Biome GenerateBiome(Point coord)
        //{
        //    float heat = Heat(coord.ToVector2());
        //    if (heat < -0.3)
        //        return Biome.Mesa;
        //    else if (heat < -0.1)
        //        return Biome.Desert;
        //    else if (heat < 0.05)
        //        return Biome.Plains;
        //    else
        //        return Biome.Forest;
        //}
        //float Heat(Vector2 pos)
        //{
        //    return PerlinNoise.Noise(pos / 64.0f) * 1.0f +
        //        PerlinNoise.Noise(pos / 32f) * 0.5f +
        //        PerlinNoise.Noise(pos / 16f) * 0.25f;
        //}

        ///Version 2
        //Biome GenerateBiome(Point coord)
        //{
        //    if (IsSeedTile(coord))
        //    {
        //        int hashCode = coord.GetHashCode() * coord.GetHashCode() * Game1.seed;
        //        //switch (Math.Abs(hashCode % (hashCode / 10)))
        //        //switch(new Random(hashCode * Game1.seed).Next(10))
        //        //int sum = 0;
        //        //while (hashCode != 0)
        //        //{
        //        //    sum += hashCode % 10;
        //        //    hashCode /= 10;
        //        //}
        //        //hashCode = sum * Game1.seed;
        //        switch (Math.Abs(hashCode % (hashCode / 10)))
        //        {
        //            case 0:
        //            case 1:
        //                return Biome.Desert;
        //            case 2:
        //            case 3:
        //                return Biome.Forest;
        //            case 4:
        //            case 5:
        //                return Biome.Ocean;
        //            case 6:
        //            case 7:
        //                return Biome.Mesa;
        //            default:
        //                return Biome.Plains;
        //        }

        //    }
        //    else
        //    {
        //        Point seedTile = SearchSeedTile(coord);
        //        return GenerateBiome(seedTile);
        //    }

        //}
        //Point SearchSeedTile(Point origin)
        //{
        //    int radius = 1;
        //    while (true)
        //    {
        //        for (int i = -radius; i < radius; i++)
        //        {
        //            Point seedTile = new Point(i, -radius) + origin;
        //            if (IsSeedTile(seedTile))
        //                return seedTile;
        //        }
        //        for (int i = -radius; i < radius; i++)
        //        {
        //            Point seedTile = new Point(radius, i) + origin;
        //            if (IsSeedTile(seedTile))
        //                return seedTile;
        //        }
        //        for (int i = -radius; i < radius; i++)
        //        {
        //            Point seedTile = new Point(-i, radius) + origin;
        //            if (IsSeedTile(seedTile))
        //                return seedTile;
        //        }
        //        for (int i = -radius; i < radius; i++)
        //        {
        //            Point seedTile = new Point(-radius, -i) + origin;
        //            if (IsSeedTile(seedTile))
        //                return seedTile;
        //        }
        //        radius++;
        //    }
        //}
        //bool IsSeedTile(Point coord)
        //{
        //    int hashCode = coord.GetHashCode() * Game1.seed;
        //    //int sum = 0;
        //    //while (hashCode != 0)
        //    //{
        //    //    sum += hashCode % 10;
        //    //    hashCode /= 10;
        //    //}
        //    //hashCode = sum * Game1.seed;
        //    return Math.Abs(new Random(hashCode * Game1.seed).Next(100)) < 2;
        //    //return Math.Abs(hashCode % (hashCode / 1000)) < 80;
        //}

        /// Version1
        //Biome GenerateBiome(Point coord, int recursion = 0)
        //{
        //    var color = CloneBiome(coord, recursion);
        //    if (color != null)
        //        return (Biome)color;

        //    int hashCode = coord.GetHashCode() * Game1.seed;
        //    int biome = Math.Abs(hashCode % (hashCode / 10));
        //    switch (biome)
        //    {
        //        case 0:
        //        case 1:
        //            return Biome.Desert;
        //        case 2:
        //        case 3:
        //            return Biome.Ocean;
        //        case 4:
        //        case 5:
        //            return Biome.Forest;
        //        case 6:
        //        case 7:
        //            return Biome.Mesa;
        //        default:
        //            return Biome.Plains;
        //    }
        //}
        //Biome? CloneBiome(Point coord, int recursion)
        //{
        //    if (recursion > 100)
        //        return null;
        //    int hashCode = coord.GetHashCode() * Game1.seed;
        //    int cloneFrom = Math.Abs((hashCode / 10) % (hashCode / 100));
        //    switch (cloneFrom)
        //    {
        //        case 0:
        //        case 1:
        //        case 2:
        //        case 3:
        //            return GenerateBiome(new Point(coord.X - 1, coord.Y), recursion);
        //        case 4:
        //        case 5:
        //        case 6:
        //        case 7:
        //            return GenerateBiome(new Point(coord.X, coord.Y - 1), recursion);
        //        default:
        //            return null;
        //    }
        //}
    }
}
