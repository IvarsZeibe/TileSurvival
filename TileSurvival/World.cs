using CollisonLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TileSurvival
{
    class World
    {
        static public Dictionary<Point, Chunk> Chunks { get; private set; } = new Dictionary<Point, Chunk>();
        static public List<Point> ChunksLoadedNextUpdate = new List<Point>();
        public Player player;
        public World()
        {
            player = new Player(Vector2.Zero);

        }
        public virtual void Update(GameTime gameTime)
        {
            foreach (var chunk in Chunks)
            {
                chunk.Value.IsLoaded = false;
            }
            foreach (var chunkCoord in ChunksLoadedNextUpdate)
                LoadChunk(chunkCoord);
            ChunksLoadedNextUpdate.Clear();
            player.Update(gameTime);
            foreach (var chunk in Chunks.Values)
            {
                if (chunk.IsLoaded)
                    chunk.Update(gameTime);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);
            foreach (var chunk in Chunks.Values)
            {
                if (chunk.IsLoaded)
                    chunk.Draw(spriteBatch);
            }
        }
        public virtual void DrawShapes(Shapes shapes)
        {
            player.DrawShapes(shapes);
            foreach (var chunk in Chunks.Values)
            {
                if(chunk.IsLoaded)
                    chunk.DrawShapes(shapes);
            }
        }
        static public Point GetTileCoord(Vector2 position)
        {
            return new Point((int)Math.Floor(position.X / 32), (int)Math.Floor(position.Y / 32));
        }
        static public Tile GetTile(Point coord)
        {
            var chunk = GetChunk(coord);
            foreach (var tile in chunk.tiles)
            {
                if (tile.TilePosition == coord)
                    return tile;
            }
            throw new Exception("no tile found");
        }
        static public Tile GetTile(Vector2 position)
        {
            var coord = GetTileCoord(position);
            var chunk = GetChunk(position);
            foreach(var tile in chunk.tiles)
            {
                if (tile.TilePosition == coord)
                    return tile;
            }
            throw new Exception("no tile found");
        }
        static public Point GetChunkCoord(Vector2 position)
        {
            return new Point((int)Math.Floor((position.X + 256) / 512), (int)Math.Floor((position.Y + 256) / 512));
        }
        static public Point GetChunkCoord(Point tilePosition)
        {
            return new Point((int)Math.Floor((tilePosition.X + 8) / 16f), (int)Math.Floor((tilePosition.Y + 8) / 16f));
        }
        static public Chunk GetChunk(Point tileCoord)
        {
            Point coord = GetChunkCoord(tileCoord);
            if (!Chunks.ContainsKey(coord))
                Chunks.Add(coord, new Chunk(coord));
            return Chunks[coord];
        }
        static public Chunk GetChunk(Vector2 position)
        {
            var coord = GetChunkCoord(position);
            if (!Chunks.ContainsKey(coord))
                Chunks.Add(coord, new Chunk(coord));
            return Chunks[coord];
        }
        static public void LoadChunk(Point coord)
        {
            if (!Chunks.ContainsKey(coord))
            {
                Chunks.Add(coord, new Chunk(coord));
            }
            Chunks[coord].IsLoaded = true;
        }
        static public void ForceLoadChunk(Point coord)
        {
            ChunksLoadedNextUpdate.Add(coord);
        }
        static public void Spawn(Vector2 positon)
        {
        }
    }
}
