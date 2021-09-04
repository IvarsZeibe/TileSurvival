using CollisonLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TileSurvival
{
    class Player : GameObject
    {
        Color color = Color.Red;
        const int WIDTH = 24;
        float layerDepth = 0.5f;
        float speed = 300;
        bool inWater = false;
        public Player(Vector2 position)
        {
            //Hitbox = Hitbox.CreateRectanglePolygon(position, WIDTH, WIDTH);
            Hitbox = Hitbox.CreateCircle(position, WIDTH / 2);
        }
        const float FOOTPRINT_INTERVAL= 150f;
        float distanceTraveledSinceFootprint = 0f;
        Vector2 lookDirection = new Vector2(0, 1);
        float sinceTileBreak = 0f;
        const float TILE_BREAKING_COOLDOWN = 1f;
        float rotation = 0;
        public override void Update(GameTime gameTime)
        {
            Vector2 direction = Vector2.Zero;
            if (Input.Actions.Contains("moveLeft"))
                direction += new Vector2(-1, 0);
            if (Input.Actions.Contains("moveRight"))
                direction += new Vector2(1, 0);
            if (Input.Actions.Contains("moveUp"))
                direction += new Vector2(0, -1);
            if (Input.Actions.Contains("moveDown"))
                direction += new Vector2(0, 1);
            if (direction != Vector2.Zero)
                direction.Normalize();

            SetLookDirection(direction);
            //if(direction != Vector2.Zero)
            //    lookDirection = direction;
            sinceTileBreak += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Input.Actions.Contains("breakTile") && sinceTileBreak > TILE_BREAKING_COOLDOWN)
            {
                sinceTileBreak = 0f;
                var ownTile = World.GetTile(Hitbox.Position);
                if (ownTile.canBreak)
                    ownTile.Break();
                else
                {
                    World.GetTile(new Point(ownTile.TilePosition.X + (int)lookDirection.X, ownTile.TilePosition.Y + (int)lookDirection.Y)).Break();
                }
                //World.GetTile(World.GetTileCoord(Hitbox.Position).)
                //World.GetChunk(Hitbox.Position).GetTile((World.GetTileCoord(Hitbox.Position).ToVector2() + lookDirection).ToPoint()).Break();
            }

            Vector2 movement = direction * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            //Hitbox.Move(direction * (float)gameTime.ElapsedGameTime.TotalSeconds * speed, Game1.Tiles.Select(tile => tile.Hitbox).ToList());
            Hitbox.Teleport(movement);
            var intersectedChunks = World.Chunks.Values.Where(chunk => chunk.IsLoaded && chunk.Hitbox.IsCollidingWith(this.Hitbox, out var temp)).ToList();
            Hitbox.Teleport(-movement);


            if (intersectedChunks.Any(chunk => chunk.biome == Biome.Ocean))
            {
                inWater = true;
            }
            else
                inWater = false;
            if(inWater)
                movement *= 0.5f;

            var hitboxes = intersectedChunks.SelectMany(chunk => chunk.gameObjects.Select(gameObject => gameObject.Hitbox));
            Hitbox.Move(movement, hitboxes.ToList());

            distanceTraveledSinceFootprint += movement.Length();
            if(distanceTraveledSinceFootprint > FOOTPRINT_INTERVAL)
            {
                World.GetChunk(Hitbox.Position).AddGameObject(new Footprint(Hitbox.Position, MathF.Atan2(direction.Y, direction.X), inWater));
                distanceTraveledSinceFootprint = 0f;
            }

            LoadChunks();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Game1.Textures["player"], 
                Game1.screen.GetScreenPosition(Hitbox.Position),
                null,
                color,
                rotation,
                new Vector2(Game1.Textures["player"].Width, Game1.Textures["player"].Height) * 0.5f,
                Game1.GetScale("player", WIDTH),
                SpriteEffects.None,
                layerDepth);
        }
        void LoadChunks()
        {
            var centerChunkCoord = World.GetChunkCoord(Hitbox.Position);
            int renderDistance = 3;
            for(int x = -renderDistance; x < renderDistance + 1; x++)
            {
                for (int y = -renderDistance; y < renderDistance + 1; y++)
                {
                    World.ForceLoadChunk(new Point(centerChunkCoord.X + x, centerChunkCoord.Y + y));
                }
            }

        }
        void SetLookDirection(Vector2 direction)
        {
            if (direction == Vector2.Zero)
                return;
            if (MathF.Round(direction.Length()) != 1)
                throw new Exception("Direction is not normalized");
            rotation = MathF.Atan2(direction.Y, direction.X);
            if (rotation >= MathF.PI * -0.75f && rotation <= MathF.PI * -0.25f)
                lookDirection = new Vector2(0, -1);
            else if (rotation >= MathF.PI * -0.25f && rotation < MathF.PI * 0.25f)
                lookDirection = new Vector2(1, 0);
            else if (rotation >= MathF.PI * 0.25f && rotation < MathF.PI * 0.75f)
                lookDirection = new Vector2(0, 1);
            else
                lookDirection = new Vector2(-1, 0);
            //-1.57 == up
            //0 = right
            //3.14 == left
            //1.57 == down
        }
    }
}
