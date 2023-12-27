using DigitalHijinks.MiKeys.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using DigitalHijinks.MiKeys.Helpers;
using static DigitalHijinks.MiKeys.Helpers.Globals;

namespace DigitalHijinks.MiKeys.Managers
{
    public class EntryManager
    {
        public const char NULL = (char)0x00;
        public const char Backspace = (char)0x08;
        public const char Newline = (char)0x0A;
        //GameState returnState;

        Texture2D keyTex;

        private string _outputString = "";
        private int _maxLength = 0;
        private int _minLength = 0;
        private float _scale = 2f;

        public bool IsActive { get; set; } = false;

        private int _keySize = 16;

        public event EventHandler InputCaptured;

        private Rectangle InputSize;

        private bool CapsLockOn = false;
        private bool Shift = false;
        private bool NumLockOn = false;

        private bool VirtualCapsLockOn = false;
        private bool VirtualShift = false;
        private bool VirtualNumLockOn = false;

        private Vector2 _selectedPos;
        private UIKey _selectedKey;

        GameState returnState;

        public enum InputType
        {
            Keyboard,
            Mouse,
            Controller
        }

        // Placeholder for creating default layouts
        public enum KeyboardStyle
        {
            //Full,
            //TKL,
            //Compact,
            Mini,
            Keypad
        }

        private InputType _inputType = InputType.Keyboard;

        List<UIKey> UIKeys;

        private float _caretTimer = 0f;
        private bool _caretShow = false;
        private float _caretInterval = 500f;

        public EntryManager() { }

        public void Load(ContentManager CM, string KeyTexture)
        {
            keyTex = CM.Load<Texture2D>(KeyTexture);
        }

        /// <summary>
        /// Sets up the input box
        /// </summary>
        /// <param name="min">The minimum string length</param>
        /// <param name="max">The maximum string length</param>
        /// <param name="width">The width of the keyboard in key units</param>
        public void SetupInput(GameState gs, int min, int max, int width)
        {
            _outputString = "";
            returnState = gs;
            _minLength = min;
            _maxLength = max;

            _keySize = (int)(16 * _scale);

            InputSize = new Rectangle(0, 0, (int)(_maxLength * 8 * _scale), 64);

            UIKeys = new List<UIKey>
            {
                // Row 1
                new(Keys.OemTilde, '`', '~', 1f),
                new(Keys.D1, '1', '!', 1f),
                new(Keys.D2, '2', '@', 1f),
                new(Keys.D3, '3', '#', 1f),
                new(Keys.D4, '4', '$', 1f),
                new(Keys.D5, '5', '%', 1f),
                new(Keys.D6, '6', '^', 1f),
                new(Keys.D7, '7', '&', 1f),
                new(Keys.D8, '8', '*', 1f),
                new(Keys.D9, '9', '(', 1f),
                new(Keys.D0, '0', ')', 1f),
                new(Keys.OemMinus, '-', '_', 1f),
                new(Keys.OemPlus, '=', '+', 1f),
                new(Keys.Back, 2f, "Back"), // Special back

                //Row 2
                new(Keys.None, 1.5f),
                new(Keys.Q, 'q', 'Q', 1f),
                new(Keys.W, 'w', 'W', 1f),
                new(Keys.E, 'e', 'E', 1f),
                new(Keys.R, 'r', 'R', 1f),
                new(Keys.T, 't', 'T', 1f),
                new(Keys.Y, 'y', 'Y', 1f),
                new(Keys.U, 'u', 'U', 1f),
                new(Keys.I, 'i', 'I', 1f),
                new(Keys.O, 'o', 'O', 1f),
                new(Keys.P, 'p', 'P', 1f),
                new(Keys.OemOpenBrackets, '[', '{', 1f),
                new(Keys.OemCloseBrackets, ']', '}', 1f),
                new(Keys.OemPipe, '\\', '|', 1.5f),

                //Row 3
                new(Keys.CapsLock, 1.75f, "Caps"), // Special Caps lock
                new(Keys.A, 'a', 'A', 1f),
                new(Keys.S, 's', 'S', 1f),
                new(Keys.D, 'd', 'D', 1f),
                new(Keys.F, 'f', 'F', 1f),
                new(Keys.G, 'g', 'G', 1f),
                new(Keys.H, 'h', 'H', 1f),
                new(Keys.J, 'j', 'J', 1f),
                new(Keys.K, 'k', 'K', 1f),
                new(Keys.L, 'l', 'L', 1f),
                new(Keys.OemSemicolon, ';', ':', 1f),
                new(Keys.OemQuotes, '\'', '"', 1f),
                new(Keys.Enter, 2.25f, "Enter"), // Special Finish

                //Row 4
                new(Keys.LeftShift, 2f, "Shift"), // Special L Shift
                new(Keys.Z, 'z', 'Z', 1f),
                new(Keys.X, 'x', 'X', 1f),
                new(Keys.C, 'c', 'C', 1f),
                new(Keys.V, 'v', 'V', 1f),
                new(Keys.B, 'b', 'B', 1f),
                new(Keys.N, 'n', 'N', 1f),
                new(Keys.M, 'm', 'M', 1f),
                new(Keys.OemComma, ',', '<', 1f),
                new(Keys.OemPeriod, '.', '>', 1f),
                new(Keys.OemQuestion, '/', '?', 1f),
                new(Keys.RightShift, 3f, "Shift"), // Special R Shift

                //Row 5
                new(Keys.None, 1.25f), // Special spacer
                new(Keys.None, 1.25f), // Special spacer
                new(Keys.None, 1.25f), // Special spacer
                new(Keys.Space, ' ', ' ', 7f, "Space"),
                new(Keys.None, 1.25f), // Special spacer
                new(Keys.None, 1.5f), // Special spacer
                new(Keys.None, 1.5f) // Special spacer
            };

            //UIKeys.Add(new(Keys.NumPad0, 'a', 'A', 1f));
            //UIKeys.Add(new(Keys.NumPad1, 'a', 'A', 1f));
            //UIKeys.Add(new(Keys.NumPad2, 'a', 'A', 1f));
            //UIKeys.Add(new(Keys.NumPad3, 'a', 'A', 1f));
            //UIKeys.Add(new(Keys.NumPad4, 'a', 'A', 1f));
            //UIKeys.Add(new(Keys.NumPad5, 'a', 'A', 1f));
            //UIKeys.Add(new(Keys.NumPad6, 'a', 'A', 1f));
            //UIKeys.Add(new(Keys.NumPad7, 'a', 'A', 1f));
            //UIKeys.Add(new(Keys.NumPad8, 'a', 'A', 1f));
            //UIKeys.Add(new(Keys.NumPad9, 'a', 'A', 1f));

            Vector2 pos = new Vector2((ScreenWidth / 2) - ((32 * _scale * width) / 2), (ScreenHeight / 2) - (32 * _scale));

            _selectedPos = pos + new Vector2(16 * _scale, 16 * _scale);
            _selectedKey = UIKeys.First();
            float cumul = 0f;
            foreach (UIKey uik in UIKeys)
            {
                uik.Initialize(keyTex, _scale);

                uik.Position = pos;
                cumul += uik.BlockSize;
                pos.X += uik.BlockSize * 32 * _scale;

                if (cumul >= width)
                {
                    cumul = 0f;
                    pos.X = (ScreenWidth / 2) - ((32 * _scale * width) / 2);
                    pos.Y += 32 * _scale;

                }
            }


            IsActive = true;
        }

        public void Finish()
        {
            IsActive = false;
            gameState = returnState;
            InputCaptured?.Invoke(_outputString, EventArgs.Empty);
        }

        public void Update(GameTime gt)
        {
            if (!IsActive) return;

            if (GetKeyTap(Keys.Escape))
            {
                _outputString = "";
                Finish();
            }

            _caretTimer += (float)gt.ElapsedGameTime.TotalMilliseconds;

            //Check to see if the CARET timer is greater than the chosen interval
            if (_caretTimer > _caretInterval)
            {
                _caretShow = Globals.Toggle(_caretShow);
                //Reset the timer
                _caretTimer = 0f;
            }


            // Update keys and reset highlights
            Keys[] pressed = kbs.GetPressedKeys();

            if (pressed.Contains(Keys.LeftShift) || pressed.Contains(Keys.RightShift))
                Shift = true;
            else
                Shift = false;

            foreach (UIKey u in UIKeys)
            {
                // Update the key
                u.Update();

                // Check for highlighting
                if (pressed.Contains(u.Key))
                {
                    u.Highlight = true;
                }
            }

            // Check for keyboard taps
            if (GetKeys(out char[] key))
            {
                // Set Type
                _inputType = InputType.Keyboard;

                // Use keyboard states
                CapsLockOn = Keyboard.GetState().CapsLock;
                NumLockOn = Keyboard.GetState().NumLock;

                // Apply keys
                for (int i = 0; i < key.Length; i++)
                {
                    switch (key[i])
                    {
                        case Newline:
                            if (_outputString.Length >= _minLength)
                            {
                                Finish();
                                i = key.Length;
                            }
                            // TODO: else {highlight field and min chars}
                            break;
                        case Backspace:
                            if (_outputString.Length > 0)
                                _outputString = _outputString[..^1];
                            break;
                        case NULL:
                            break;
                        default:
                            if (_outputString.Length < _maxLength)
                                _outputString += key[i];
                            break;
                    }
                }
            }

            // Check for mouse clicks
            foreach (UIKey u in UIKeys)
            {
                if (u.Bounds.Contains(ms.Position))
                {
                    u.Highlight = true;

                    if (LeftClicked())
                    {
                        _inputType = InputType.Mouse;
                        switch (u.Key)
                        {
                            case Keys.Enter:
                                if (_outputString.Length >= _minLength)
                                {
                                    Finish();
                                }
                                // TODO: else {highlight field and min chars}
                                break;
                            case Keys.Back:
                                if (_outputString.Length > 0)
                                    _outputString = _outputString[..^1];
                                break;
                            case Keys.CapsLock:
                                VirtualCapsLockOn = Globals.Toggle(VirtualCapsLockOn);
                                break;
                            case Keys.LeftShift:
                            case Keys.RightShift:
                                VirtualShift = Globals.Toggle(VirtualShift);
                                break;
                            case Keys.None:
                                break;
                            default:
                                if (_outputString.Length < _maxLength)
                                    _outputString += VirtualCapsLockOn || VirtualShift ? u.Upper : u.Lower;
                                VirtualShift = false;

                                break;
                        }
                    }
                }
            }


            if (GP_CP.IsButtonDown(Buttons.DPadRight) && PGP_CP.IsButtonUp(Buttons.DPadRight))
            {
                _inputType = InputType.Controller;
                UIKey nextKey = _selectedKey;

                List<UIKey> keysToRight = UIKeys.Where(x => x.Bounds.X > _selectedKey.Bounds.X && x.Bounds.Y == _selectedKey.Bounds.Y && x.Key != Keys.None).ToList();

                if (keysToRight.Count != 0)
                {
                    nextKey = keysToRight[0];

                    foreach (UIKey u in keysToRight)
                    {
                        if (u.Bounds.X < nextKey.Bounds.X)
                        {
                            nextKey = u;
                        }
                    }

                    _selectedKey = nextKey;
                    _selectedPos = _selectedKey.Position + new Vector2(16 * _scale, 16 * _scale);
                }
            }

            if (GP_CP.IsButtonDown(Buttons.DPadLeft) && PGP_CP.IsButtonUp(Buttons.DPadLeft))
            {
                _inputType = InputType.Controller;
                UIKey nextKey = _selectedKey;

                List<UIKey> keysToLeft = UIKeys.Where(x => x.Bounds.X < _selectedKey.Bounds.X && x.Bounds.Y == _selectedKey.Bounds.Y && x.Key != Keys.None).ToList();

                if (keysToLeft.Count != 0)
                {
                    nextKey = keysToLeft[0];

                    foreach (UIKey u in keysToLeft)
                    {
                        if (u.Bounds.X > nextKey.Bounds.X)
                        {
                            nextKey = u;
                        }
                    }

                    _selectedKey = nextKey;
                    _selectedPos = _selectedKey.Position + new Vector2(16 * _scale, 16 * _scale);
                }
            }

            if (GP_CP.IsButtonDown(Buttons.DPadDown) && PGP_CP.IsButtonUp(Buttons.DPadDown))
            {
                _inputType = InputType.Controller;
                UIKey nextKey = _selectedKey;

                List<UIKey> keysToDown = UIKeys.Where(x => x.Bounds.Y > _selectedKey.Bounds.Y && x.Key != Keys.None).ToList();

                if (keysToDown.Count != 0)
                {
                    nextKey = keysToDown[0];

                    foreach (UIKey u in keysToDown)
                    {
                        if (Vector2.DistanceSquared(_selectedKey.Center, u.Center) <= Vector2.DistanceSquared(_selectedKey.Center, nextKey.Center))
                        {
                            nextKey = u;
                        }
                    }

                    _selectedKey = nextKey;
                    _selectedPos = _selectedKey.Position + new Vector2(16 * _scale, 16 * _scale);
                }
            }

            if (GP_CP.IsButtonDown(Buttons.DPadUp) && PGP_CP.IsButtonUp(Buttons.DPadUp))
            {
                _inputType = InputType.Controller;
                UIKey nextKey = _selectedKey;

                List<UIKey> keysToUp = UIKeys.Where(x => x.Bounds.Y < _selectedKey.Bounds.Y && x.Key != Keys.None).ToList();

                if (keysToUp.Count != 0)
                {
                    nextKey = keysToUp[0];

                    foreach (UIKey u in keysToUp)
                    {
                        if (Vector2.DistanceSquared(_selectedKey.Center, u.Center) < Vector2.DistanceSquared(_selectedKey.Center, nextKey.Center))
                        {
                            nextKey = u;
                        }
                    }

                    _selectedKey = nextKey;
                    _selectedPos = _selectedKey.Position + new Vector2(16 * _scale, 16 * _scale);
                }
            }

            if (GP_CP.IsButtonDown(Buttons.LeftStick) && PGP_CP.IsButtonUp(Buttons.LeftStick))
            {
                _inputType = InputType.Controller;

                if (VirtualShift)
                {
                    VirtualShift = false;
                    VirtualCapsLockOn = true;
                }
                else if (VirtualCapsLockOn)
                {
                    VirtualCapsLockOn = false;
                }
                else
                {
                    VirtualShift = true;
                }

            }

            if (GP_CP.IsButtonDown(Buttons.Back) && PGP_CP.IsButtonUp(Buttons.Back))
            {
                _inputType = InputType.Controller;
                _outputString = "";
                Finish();
            }

            if (GP_CP.IsButtonDown(Buttons.Start) && PGP_CP.IsButtonUp(Buttons.Start))
            {
                _inputType = InputType.Controller;

                if (_outputString.Length >= _minLength)
                {
                    Finish();
                }
            }

            if (GP_CP.IsButtonDown(Buttons.X) && PGP_CP.IsButtonUp(Buttons.X))
            {
                _inputType = InputType.Controller;
                if (_outputString.Length > 0)
                    _outputString = _outputString[..^1];
            }

            if (GP_CP.IsButtonDown(Buttons.Y) && PGP_CP.IsButtonUp(Buttons.Y))
            {
                _inputType = InputType.Controller;
                if (_outputString.Length < _maxLength)
                    _outputString += ' ';
            }

            if (GP_CP.IsButtonDown(Buttons.A) && PGP_CP.IsButtonUp(Buttons.A))
            {
                _inputType = InputType.Controller;

                switch (_selectedKey.Key)
                {
                    case Keys.Enter:
                        if (_outputString.Length >= _minLength)
                        {
                            Finish();
                        }
                        // TODO: else {highlight field and min chars}
                        break;
                    case Keys.Back:
                        if (_outputString.Length > 0)
                            _outputString = _outputString[..^1];
                        break;
                    case Keys.CapsLock:
                        VirtualCapsLockOn = Globals.Toggle(VirtualCapsLockOn);
                        break;
                    case Keys.LeftShift:
                    case Keys.RightShift:
                        VirtualShift = Globals.Toggle(VirtualShift);
                        break;
                    case Keys.None:
                        break;
                    default:
                        if (_outputString.Length < _maxLength)
                            _outputString += VirtualCapsLockOn || VirtualShift ? _selectedKey.Upper : _selectedKey.Lower;
                        VirtualShift = false;

                        break;
                }
            }

            if (_inputType == InputType.Controller)
            {
                _selectedKey.Highlight = true;
            }

            // Apply final states
            foreach (UIKey u in UIKeys)
            {
                switch (_inputType)
                {
                    case InputType.Keyboard:
                        if (u.Key == Keys.CapsLock && CapsLockOn)
                            u.Highlight = true;
                        if ((u.Key == Keys.LeftShift && Shift) || (u.Key == Keys.RightShift && Shift))
                            u.Highlight = true;
                        break;
                    case InputType.Controller:
                    case InputType.Mouse:
                        if (u.Key == Keys.CapsLock && VirtualCapsLockOn)
                            u.Highlight = true;
                        if ((u.Key == Keys.LeftShift && VirtualShift) || (u.Key == Keys.RightShift && VirtualShift))
                            u.Highlight = true;
                        break;
                }
            }
        }

        public void DrawInput()
        {
            if (!IsActive) return;

            SBX.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            SamplerState.PointWrap,
            null,
            null,
            null,
            null);

            Vector2 inputOrigin = new(InputSize.Width / 2, (int)(16 * _scale / 2));

            Globals.DrawRect(SBX, ScreenCenter - inputOrigin - new Vector2(6 * _scale, 65 * _scale), ScreenCenter + inputOrigin + new Vector2(0, -(61 * _scale)), Color.White, Color.Black, _scale, true);

            SBX.DrawString(SF, _outputString, ScreenCenter - inputOrigin - new Vector2(2 * _scale, (16 * 4) * _scale), Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);

            if (_caretShow && _outputString.Length < _maxLength)
                SBX.DrawString(SF, "|", ScreenCenter - inputOrigin - new Vector2((2 * _scale), (16 * 4) * _scale) + new Vector2(2 * _scale + (_outputString.Length * 8 * _scale), 0), Color.WhiteSmoke, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);

            foreach (UIKey vk in UIKeys)
            {
                switch (_inputType)
                {
                    case InputType.Keyboard:
                        vk.Draw(SBX, SF, CapsLockOn || Shift);
                        break;
                    case InputType.Controller:
                    case InputType.Mouse:
                        vk.Draw(SBX, SF, VirtualCapsLockOn || VirtualShift);
                        break;
                }
            }

            SBX.End();
        }

        public bool GetKeys(out char[] key)
        {
            Keys[] keys = kbs.GetPressedKeys();
            bool shift = CapsLockOn || kbs.IsKeyDown(Keys.LeftShift) || kbs.IsKeyDown(Keys.RightShift);

            key = new char[keys.Length];

            if (keys.Length > 0)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (pkbs.IsKeyDown(keys[i]))
                        continue;

                    key[i] = KeyToChar(keys[i], shift);
                }
                return true;
            }

            key = new char[1] { NULL };
            return false;
        }

        public static char KeyToChar(Keys key, bool shift)
        {
            return key switch
            {
                //Alphabet keys
                Keys.A => shift ? 'A' : 'a',
                Keys.B => shift ? 'B' : 'b',
                Keys.C => shift ? 'C' : 'c',
                Keys.D => shift ? 'D' : 'd',
                Keys.E => shift ? 'E' : 'e',
                Keys.F => shift ? 'F' : 'f',
                Keys.G => shift ? 'G' : 'g',
                Keys.H => shift ? 'H' : 'h',
                Keys.I => shift ? 'I' : 'i',
                Keys.J => shift ? 'J' : 'j',
                Keys.K => shift ? 'K' : 'k',
                Keys.L => shift ? 'L' : 'l',
                Keys.M => shift ? 'M' : 'm',
                Keys.N => shift ? 'N' : 'n',
                Keys.O => shift ? 'O' : 'o',
                Keys.P => shift ? 'P' : 'p',
                Keys.Q => shift ? 'Q' : 'q',
                Keys.R => shift ? 'R' : 'r',
                Keys.S => shift ? 'S' : 's',
                Keys.T => shift ? 'T' : 't',
                Keys.U => shift ? 'U' : 'u',
                Keys.V => shift ? 'V' : 'v',
                Keys.W => shift ? 'W' : 'w',
                Keys.X => shift ? 'X' : 'x',
                Keys.Y => shift ? 'Y' : 'y',
                Keys.Z => shift ? 'Z' : 'z',
                //Decimal keys
                Keys.D0 => shift ? ')' : '0',
                Keys.D1 => shift ? '!' : '1',
                Keys.D2 => shift ? '@' : '2',
                Keys.D3 => shift ? '#' : '3',
                Keys.D4 => shift ? '$' : '4',
                Keys.D5 => shift ? '%' : '5',
                Keys.D6 => shift ? '^' : '6',
                Keys.D7 => shift ? '&' : '7',
                Keys.D8 => shift ? '*' : '8',
                Keys.D9 => shift ? '(' : '9',
                //Numpad keys
                Keys.NumPad0 => '0',
                Keys.NumPad1 => '1',
                Keys.NumPad2 => '2',
                Keys.NumPad3 => '3',
                Keys.NumPad4 => '4',
                Keys.NumPad5 => '5',
                Keys.NumPad6 => '6',
                Keys.NumPad7 => '7',
                Keys.NumPad8 => '8',
                Keys.NumPad9 => '9',
                //Special keys
                Keys.OemTilde => shift ? '~' : '`',
                Keys.OemSemicolon => shift ? ':' : ';',
                Keys.OemQuotes => shift ? '"' : '\'',
                Keys.OemQuestion => shift ? '?' : '/',
                Keys.OemPlus => shift ? '+' : '=',
                Keys.OemPipe => shift ? '|' : '\\',
                Keys.OemPeriod => shift ? '>' : '.',
                Keys.OemOpenBrackets => shift ? '{' : '[',
                Keys.OemCloseBrackets => shift ? '}' : ']',
                Keys.OemMinus => shift ? '_' : '-',
                Keys.OemComma => shift ? '<' : ',',
                Keys.Space => ' ',
                Keys.Back => Backspace,
                Keys.Enter => Newline,
                _ => NULL,
            };
        }
    }
}
