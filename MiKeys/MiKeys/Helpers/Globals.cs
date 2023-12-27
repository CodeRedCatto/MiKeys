using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using static DigitalHijinks.MiKeys.Helpers.Globals;
using Microsoft.Xna.Framework.Content;

namespace DigitalHijinks.MiKeys.Helpers
{
    public static class Globals
    {
        public static ContentManager CM { get; set; }
        public static GraphicsDeviceManager GDM { get; set; }
        public static GraphicsDevice GFX { get; set; }
        public static GameWindow GW { get; set; }
        public static SpriteBatch SBX { get; set; }
        public static SpriteFont SF { get; set; }

        public static Viewport VPort;
        public static int ScreenWidth { get { return VPort.Width; } }
        public static int ScreenHeight { get { return VPort.Height; } }
        public static int HalfScreenWidth { get { return ScreenWidth / 2; } }
        public static int HalfScreenHeight { get { return ScreenHeight / 2; } }
        public static Vector2 ScreenCenter { get { return new Vector2(ScreenWidth / 2, ScreenHeight / 2); } }

        public enum GameState
        {
            Default,
            PlayerInput
        }

        public static GameState gameState = GameState.Default;

        public static KeyboardState kbs;
        public static KeyboardState pkbs;
        public static MouseState ms;
        public static MouseState pms;
        public static Vector2 MousePosition;

        public static int ScrollWheel;
        public static int PrevScrollWheel;


        public static PlayerIndex ControllingPlayer;
        public static GamePadState GP_CP;
        public static GamePadState PGP_CP;

        public static GamePadState GP_P1;
        public static GamePadState PGP_P1;
        public static GamePadState GP_P2;
        public static GamePadState PGP_P2;
        public static GamePadState GP_P3;
        public static GamePadState PGP_P3;
        public static GamePadState GP_P4;
        public static GamePadState PGP_P4;

        public static void RefreshKeyboardAndMouse()
        {
            pkbs = kbs;
            kbs = Keyboard.GetState();
            pms = ms;
            ms = Mouse.GetState();


            PrevScrollWheel = ScrollWheel;
            ScrollWheel = ms.ScrollWheelValue;

            MousePosition = new Vector2(ms.X, ms.Y);
        }

        public static void SetMainGamepad(PlayerIndex pi)
        {
            ControllingPlayer = pi;
        }

        public static void RefreshGamepads()
        {
            PGP_P1 = GP_P1;
            GP_P1 = GamePad.GetState(PlayerIndex.One);

            PGP_P2 = GP_P2;
            GP_P2 = GamePad.GetState(PlayerIndex.Two);

            PGP_P3 = GP_P3;
            GP_P3 = GamePad.GetState(PlayerIndex.Three);

            PGP_P4 = GP_P4;
            GP_P4 = GamePad.GetState(PlayerIndex.Four);

            PGP_CP = GP_CP;
            GP_CP = ControllingPlayer switch
            {
                PlayerIndex.One => GP_P1,
                PlayerIndex.Two => GP_P2,
                PlayerIndex.Three => GP_P3,
                PlayerIndex.Four => GP_P4,
                _ => GP_P1
            };
        }

        public static bool GetKeyTap(Keys key)
        {
            return kbs.IsKeyDown(key) && pkbs.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return kbs.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return kbs.IsKeyUp(key);
        }

        public static bool LeftClicked()
        {
            return ms.LeftButton == ButtonState.Released && pms.LeftButton == ButtonState.Pressed;
        }

        public static bool LeftClickDown()
        {
            return ms.LeftButton == ButtonState.Pressed;
        }

        public static bool LeftClickUp()
        {
            return ms.LeftButton == ButtonState.Released;
        }

        public static bool RightClicked()
        {
            return ms.RightButton == ButtonState.Released && pms.RightButton == ButtonState.Pressed;
        }

        public static bool Toggle(this bool b)
        {
            return !b;
        }

        private static Texture2D _texture;
        public static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }

            return _texture;
        }

        public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rect, Color borderColor, Color bgColor, float thickness = 1f, bool fill = false)
        {
            DrawRect(spriteBatch, new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height), borderColor, bgColor, thickness, fill);
        }

        public static void DrawRect(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color borderColor, Color bgColor, float thickness = 1f, bool fill = false)
        {
            if (fill)
                DrawLine(spriteBatch, new(point1.X, (point1.Y + point2.Y) / 2), new(point2.X, (point1.Y + point2.Y) / 2), bgColor, Math.Abs((point1.Y - point2.Y)));
            DrawLine(spriteBatch, point1, new(point2.X, point1.Y), borderColor, thickness);
            DrawLine(spriteBatch, point1, new(point1.X, point2.Y), borderColor, thickness);
            DrawLine(spriteBatch, point2, new(point2.X, point1.Y), borderColor, thickness);
            DrawLine(spriteBatch, point2, new(point1.X, point2.Y), borderColor, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f, Texture2D t = null)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness, t);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f, Texture2D t = null)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(t == null ? GetTexture(spriteBatch) : t, point, null, color, angle, origin, scale, SpriteEffects.None, 1);
        }
    }
}
