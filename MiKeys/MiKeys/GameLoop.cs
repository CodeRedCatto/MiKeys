using DigitalHijinks.MiKeys.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;
using static DigitalHijinks.MiKeys.Helpers.Globals;

namespace DigitalHijinks.MiKeys
{
    public class GameLoop : Game
    {
        EntryManager EM;
        string resultString = "";


        public GameLoop()
        {
            GDM = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public void InitialSetup()
        {

            CM = Content;
            GW = Window;
            GDM = GDM;
            GFX = GraphicsDevice;
            VPort = GraphicsDevice.Viewport;

            IsMouseVisible = true;

            GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
            VPort.Width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 128;
            VPort.Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 256;
            GDM.PreferredBackBufferWidth = ScreenWidth;
            GDM.PreferredBackBufferHeight = ScreenHeight;

            GraphicsDevice.Viewport = VPort;
            GDM.ApplyChanges();
        }

        protected override void Initialize()
        {
            InitialSetup();

            EM = new EntryManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SBX = new SpriteBatch(GraphicsDevice);
            SF = Content.Load<SpriteFont>("FSEX302");
            EM.Load(Content, "MiKey_KeyExample");
        }

        void InputCaptured(object sender, EventArgs e)
        {
            resultString = sender as string;
            EM.InputCaptured -= InputCaptured;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            RefreshKeyboardAndMouse();
            RefreshGamepads();

            if (EM.IsActive)
            {
                EM.Update(gameTime);
            }
            else
            {
                if (GetKeyTap(Keys.Space))
                {
                    EM.SetupInput(gameState, 0, 32, 15);
                    EM.InputCaptured += InputCaptured;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float resultScale = 3f;

            SBX.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, null);
            SBX.DrawString(SF, resultString, new Vector2(HalfScreenWidth - (resultString.Length * (8 * resultScale) / 2), HalfScreenHeight - 32), Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            SBX.End();

            EM.DrawInput();

            base.Draw(gameTime);
        }
    }
}