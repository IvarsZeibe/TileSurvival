using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TileSurvival
{
    class Footprint : GameObject
    {
        const float LIFESPAN = 3f;
        float timeAlive = 0f;
        Vector2 position;
        float layerDepth = 0.7f;
        Vector2 size = new Vector2(10, 10);
        bool inWater = false;
        string texture = "circle";
        Color color = new Color(0, 0, 0, 50);
        float rotation = 0f;
        public Footprint(Vector2 position, float rotation = 0f, bool inWater = false)
        {
            this.position = position;
            this.rotation = rotation + MathF.PI;
            if (inWater)
            {
                this.inWater = inWater;
                size = new Vector2(40, 40);
                texture = "circleArc";
                color = new Color(255, 255, 255, 200);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (inWater)
            {
                size += new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds * 5, (float)gameTime.ElapsedGameTime.TotalSeconds) * 5;
                //if(World.GetChunk(position).biome != Biome.Ocean)
                //{
                //    IsAlive = false;
                //}
            }
            timeAlive += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeAlive > LIFESPAN)
            {
                IsAlive = false;
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(Game1.Textures[texture].Width, Game1.Textures[texture].Height) * .5f;
            spriteBatch.Draw(Game1.Textures[texture], Game1.screen.GetScreenPosition(position), null, color, rotation, origin, Game1.GetScale(texture, size), SpriteEffects.None, layerDepth);

        }
    }
}
