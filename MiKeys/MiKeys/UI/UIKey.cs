using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalHijinks.MiKeys.UI
{
    public class UIKey : UIElement
    {
        public enum UIKeyType
        {
            Single,
            Double,
            Special
        }

        Texture2D Texture { get; set; }
        public float BlockSize { get; set; }

        public Keys Key { get; set; }

        public char Lower { get; set; }
        public char Upper { get; set; }

        public bool Highlight = false;

        public string Label { get; set; }


        /// <summary>
        /// Constructs a UIKey with a label to be used for special interactions. Will not insert characters. To create a 'spacer' key, pass in Keys.None.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <param name="blockSize">The width of the key in 'blocks'.</param>
        /// <param name="label">The label for the key.</param>
        public UIKey(Keys key, float blockSize = 1f, string label = "")
        {

            BlockSize = blockSize;
            Key = key;

            Lower = (char)0;
            Upper = (char)0;
            Label = label;

            Size = new Vector2(BlockSize * 32, 32);

            BorderColor = Color.Chartreuse;
        }

        /// <summary>
        /// Constructs a Normal UIKey with a default character and a shifted character. To ignore shifts (e.g., the spacebar) simply pass the same character to upper and lower.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <param name="lower">The default character to use.</param>
        /// <param name="upper">The shifted character to use.</param>
        /// <param name="blockSize">The width of the key in 'blocks'.</param>
        /// <param name="label">The label for the key.</param>
        public UIKey(Keys key, char lower, char upper, float blockSize = 1f, string label = "")
        {
            Key = key;
            Lower = lower;
            Upper = upper;
            BlockSize = blockSize;
            Label = label;

            Size = new Vector2(blockSize * 32, 32);

            BorderColor = Color.Chartreuse;
        }

        public void Initialize(Texture2D texture = null, float scale = 1f)
        {
            Texture = texture;
            Scale = scale;
        }

        public void Update()
        {
            UpdateBase();
            Highlight = false;

        }

        public void Draw(SpriteBatch SBX, SpriteFont SF, bool swapChars)
        {
            if (Texture == null)
            {
                BackgroundColor = Key == Keys.None ? Color.Black : Highlight ? Color.DarkSlateGray : Color.Black;
                DrawBase(SBX);
            }
            else
            {
                int h = Highlight ? 32 : 0;

                if (Key == Keys.None)
                    h = 64;

                int chunk = (int)(8 * Scale);

                Rectangle mi = new(Bounds.X + chunk, Bounds.Y + chunk, Bounds.Width - (chunk * 2), Bounds.Height - (chunk * 2));
                SBX.Draw(Texture, mi, new Rectangle(h + 8, 8, 16, 16), Color.White, 0f, new Vector2(0, 0), 0, 0f);

                Rectangle tm = new(Bounds.X + chunk, Bounds.Y, Bounds.Width - (chunk * 2), chunk);
                SBX.Draw(Texture, tm, new Rectangle(h + 8, 0, 16, 8), Color.White, 0f, new Vector2(0, 0), 0, 0f);

                Rectangle bm = new Rectangle(Bounds.X + chunk, Bounds.Y + Bounds.Height - chunk, Bounds.Width - (chunk * 2), chunk);
                SBX.Draw(Texture, bm, new Rectangle(h + 8, 24, 16, 8), Color.White, 0f, new Vector2(0, 0), 0, 0f);

                Rectangle lm = new Rectangle(Bounds.X, Bounds.Y + chunk, chunk, Bounds.Height - (chunk * 2));
                SBX.Draw(Texture, lm, new Rectangle(h, 8, 8, 16), Color.White, 0f, new Vector2(0, 0), 0, 0f);

                Rectangle rm = new Rectangle(Bounds.X + Bounds.Width - chunk, Bounds.Y + chunk, chunk, Bounds.Height - (chunk * 2));
                SBX.Draw(Texture, rm, new Rectangle(h + 24, 8, 8, 16), Color.White, 0f, new Vector2(0, 0), 0, 0f);

                Rectangle tl = new Rectangle(Bounds.X, Bounds.Y, chunk, chunk);
                SBX.Draw(Texture, tl, new Rectangle(h, 0, 8, 8), Color.White, 0f, new Vector2(0, 0), 0, 0f);

                Rectangle tr = new Rectangle(Bounds.X + Bounds.Width - chunk, Bounds.Y, chunk, chunk);
                SBX.Draw(Texture, tr, new Rectangle(h + 24, 0, 8, 8), Color.White, 0f, new Vector2(0, 0), 0, 0f);

                Rectangle bl = new Rectangle(Bounds.X, Bounds.Y + Bounds.Height - chunk, chunk, chunk);
                SBX.Draw(Texture, bl, new Rectangle(h, 24, 8, 8), Color.White, 0f, new Vector2(0, 0), 0, 0f);

                Rectangle br = new Rectangle(Bounds.X + Bounds.Width - chunk, Bounds.Y + Bounds.Height - chunk, chunk, chunk);
                SBX.Draw(Texture, br, new Rectangle(h + 24, 24, 8, 8), Color.White, 0f, new Vector2(0, 0), 0, 0f);
            }

            if (!string.IsNullOrEmpty(Label))
            {
                SBX.DrawString(SF, Label, Position + new Vector2((32 * Scale * BlockSize) / 2, 16 * Scale) - new Vector2((Label.Length * 8 * Scale) / 2, 8 * Scale), Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 1);
            }
            else
            {
                SBX.DrawString(SF, Upper == '\0' ? " " : swapChars ? Lower.ToString() : Upper.ToString(), Position + new Vector2(6 * Scale, 4 * Scale), Color.White, 0, Vector2.Zero, Scale / 2, SpriteEffects.None, 1);
                SBX.DrawString(SF, Lower == '\0' ? " " : swapChars ? Upper.ToString() : Lower.ToString(), Position + new Vector2((32 * Scale * BlockSize) / 2, 16 * Scale) - new Vector2((6 * Scale) / 2, 8 * Scale), Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 1);
            }
        }
    }
}
