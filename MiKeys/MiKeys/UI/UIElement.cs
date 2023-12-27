using Microsoft.Xna.Framework;
using DigitalHijinks.MiKeys.Helpers;
using static DigitalHijinks.MiKeys.Helpers.Globals;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalHijinks.MiKeys.UI
{
    public class UIElement
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Size = Vector2.One;
        public Rectangle Bounds = new(0, 0, 16, 16);
        public Vector2 Center = Vector2.Zero;

        public float Scale = 1f;

        public float BorderThickness = 1f;
        public Color BorderColor = Color.Black;
        public Color BackgroundColor = Color.White;

        /// <summary>
        /// Updates bounds rect and center vector.
        /// </summary>
        public void UpdateBase()
        {
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
            Bounds.Width = (int)(Size.X * Scale);
            Bounds.Height = (int)(Size.Y * Scale);

            Center.X = Bounds.X + (Bounds.Width) / 2;
            Center.Y = Bounds.Y + (Bounds.Height) / 2;
        }

        public void DrawBase(SpriteBatch SBX)
        {
            Globals.DrawRect(SBX, Bounds, BorderColor, BackgroundColor, BorderThickness, true);
            Globals.DrawLine(SBX, Position, Center, Color.Magenta, 2f);
        }
    }
}
