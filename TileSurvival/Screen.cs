using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TileSurvival
{
    class Screen : MonogameUI.Screen
    {
        public Vector2 CameraPosition = Vector2.Zero;
        public Screen(GraphicsDevice graphicsDevice, int width, int height) : base(graphicsDevice, width, height)
        {
        }
        public Vector2 GetScreenPosition(Vector2 worldPosition)
        {
            return worldPosition - CameraPosition + new Vector2(renderTarget2D.Width, renderTarget2D.Height) * 0.5f;
        }
    }
}
