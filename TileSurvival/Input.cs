using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileSurvival
{
    class Input
    {
        public static List<string> Actions = new List<string>();
        static KeyboardState kstate = new KeyboardState();
        static KeyboardState kstateOld;
        static MouseState mstate = new MouseState();
        static MouseState mstateOld;
        public static void Update()
        {
            kstateOld = kstate;
            kstate = Keyboard.GetState();
            mstateOld = mstate;
            mstate = Mouse.GetState();

            Actions.Clear();

            if (!Game1.ui.IsWriting)
            {
                if (kstate.IsKeyDown(Keys.W))
                {
                    Actions.Add("moveUp");
                }
                if (kstate.IsKeyDown(Keys.S))
                {
                    Actions.Add("moveDown");
                }
                if (kstate.IsKeyDown(Keys.A))
                {
                    Actions.Add("moveLeft");
                }
                if (kstate.IsKeyDown(Keys.D))
                {
                    Actions.Add("moveRight");
                }
                if (kstate.IsKeyDown(Keys.D))
                {
                    Actions.Add("moveRight");
                }
                if (kstate.IsKeyDown(Keys.Space))
                {
                    Actions.Add("breakTile");
                }
            }

        }
        static bool IsNewKey(Keys key)
        {
            return kstate.IsKeyDown(key) && kstateOld.IsKeyDown(key);
        }
    }
}
