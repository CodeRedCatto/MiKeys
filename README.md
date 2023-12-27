![MiKeys Logo](MiKeysLogo.png)

An open-source, virtual keyboard library for MonoGame.

Welcome to MiKeys! The focus of this project is to provide a simple interface for collecting user input. The library is designed to be flexible, accessible and easily customizable.

### Supported Inputs
- Keyboard
- Mouse 
- GamePad
- ~~Eye-Tracking~~ (If you can find an open-source project to integrate for this, @Aristurtle will love you)

### Default Layouts
- Mini
- Keypad
- ~~Full~~ (Planned)
- ~~TKL~~ (Planned)
- ~~Compact~~ (Planned)

## Setup
The project should just run out of the box. 
To run the example, wait for the project to load, then hit Space to open the virtual keyboard. When finished, hit Enter to close the virtual keyboard and view the typed message.

## Integration
The main integrations points are:

Add and initialize the EntryManager:
``` C#
EntryManager EM;
EM = new EntryManager();
```
Call ```Load()``` on EntryManager:
``` C#
EM.Load(Content, "MiKey_KeyExample");
```
Create a return string container and a callback event to assign the EntryManager's ```InputCaptured``` event:
``` C#
string resultString = "";
void InputCaptured(object sender, EventArgs e)
{
  resultString = sender as string;
  EM.InputCaptured -= InputCaptured;
}
```
Define a setup call to build a virtual keyboard layout and assign the callback:
``` C#
RefreshKeyboardAndMouse();
RefreshGamepads();

if (EM.IsActive)
{
  EM.Update(gameTime);
} else {
  if (GetKeyTap(Keys.Space)) {
    EM.SetupInput(gameState, 0, 32, 15);
    EM.InputCaptured += InputCaptured;
  }
}
```

## Creating a key graphic
The key uses a simple tileset for idle, highlighted and disabled keys. A template and example file can be found in the Content directory. The template file shows how the key is pieced together.

![A .png showing the layout of the key graphic](MiKeys/MiKeys/Content/MiKey_KeyTemplate.png)

![A .png showing an example key graphic](MiKeys/MiKeys/Content/MiKey_KeyExample.png)

## Fonts
Currently MiKeys only supports monospaced font. The font included in the repo is FixedSys Excelsior, a public domain font. The original .ttx and updated .ttf can be found here https://github.com/kika/fixedsys/ and here https://github.com/delinx/Fixedsys-Core

## Credit
The code is released under the MIT License, so go crazy. If you make anything cool, show me! Credit is appreciated :) 
