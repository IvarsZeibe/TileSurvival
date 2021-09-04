using CollisonLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TileSurvival
{
    abstract class GameObject
    {
        public Hitbox Hitbox = Hitbox.None;
        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
        public virtual void DrawShapes(Shapes shapes)
        {
        }
        public bool IsAlive { get; set; } = true;
    }
}
