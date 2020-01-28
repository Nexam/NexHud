
id : What it does in NexHUD. Do not change it
key : The key you want (see key list)
modifiers : array of modifier to use (Control, Alt, etc...). You can erase this line if you don't want modifiers.

=== SPECIAL: MENU ====
For the menu there is two mode available: "press" and "hold"
default mode is "press" so if will be used if "menuMode" is not defined

"press" mode:
this will show the menu when pressing the key(s).
In this case you don't need to bind "back" and it will be ignored
example:
{
	id : "menu",
	menuMode : "press",
	key : "home",
	modifiers : ["LControl","LAlt"]
},

"hold" mode:
this will show the menu as long as you hold the key(s)
for conveniance you can just only "modifiers" keys
In this case you need to bind "back".
example:
{
	id : "menu",
	menuMode : "hold",
	modifiers : ["LControl","LAlt"]
},
{
	id : "back",
	key : "home",
	modifiers : ["LControl","LAlt"]
},

==== KEY LIST ====

Name (Key)			Description
A					The A key.

AltLeft				The left alt key.

AltRight			The right alt key.

B					The B key.

Back				The backspace key (equivalent to BackSpace).

BackSlash			The backslash key.

BackSpace			The backspace key.

BracketLeft			The left bracket key.

BracketRight		The right bracket key.

C					The C key.

CapsLock			The caps lock key.

Clear				The clear key (Keypad5 with NumLock disabled, on typical keyboards).

Comma				The comma key.

ControlLeft			The left control key.

ControlRight		The right control key.

D					The D key.

Delete				The delete key.

Down				The down arrow key.

E					The E key.

End					The end key.

Enter				The enter key.

Escape				The escape key.

F					The F key.

F1					The F1 key.

F10					The F10 key.

F11					The F11 key.

F12					The F12 key.

F13					The F13 key.

F14					The F14 key.

F15					The F15 key.

F16					The F16 key.

F17					The F17 key.

F18					The F18 key.

F19					The F19 key.

F2					The F2 key.

F20					The F20 key.

F21					The F21 key.

F22					The F22 key.

F23					The F23 key.

F24					The F24 key.

F25					The F25 key.

F26					The F26 key.

F27					The F27 key.

F28					The F28 key.

F29					The F29 key.

F3					The F3 key.

F30					The F30 key.

F31					The F31 key.

F32					The F32 key.

F33					The F33 key.

F34					The F34 key.

F35					The F35 key.

F4					The F4 key.

F5					The F5 key.

F6					The F6 key.

F7					The F7 key.

F8					The F8 key.

F9					The F9 key.

G					The G key.

Grave				The grave key (equivaent to Tilde).

H					The H key.

Home				The home key.

I					The I key.

Insert				The insert key.

J					The J key.

K					The K key.

Keypad0				The keypad 0 key.

Keypad1				The keypad 1 key.

Keypad2				The keypad 2 key.

Keypad3				The keypad 3 key.

Keypad4				The keypad 4 key.

Keypad5				The keypad 5 key.

Keypad6				The keypad 6 key.

Keypad7				The keypad 7 key.

Keypad8				The keypad 8 key.

Keypad9				The keypad 9 key.

KeypadAdd			The keypad add key.

KeypadDecimal		The keypad decimal key.

KeypadDivide		The keypad divide key.

KeypadEnter			The keypad enter key.

KeypadMinus			The keypad minus key (equivalent to KeypadSubtract).

KeypadMultiply		The keypad multiply key.

KeypadPeriod		The keypad period key (equivalent to KeypadDecimal).

KeypadPlus			The keypad plus key (equivalent to KeypadAdd).

KeypadSubtract		The keypad subtract key.

L					The L key.

LAlt				The left alt key (equivalent to AltLeft.

LastKey				Indicates the last available keyboard key.

LBracket			The left bracket key (equivalent to BracketLeft).

LControl			The left control key (equivalent to ControlLeft).

Left				The left arrow key.

LShift				The left shift key (equivalent to ShiftLeft).

LWin				The left win key (equivalent to WinLeft).

M					The M key.

Menu				The menu key.

Minus				The minus key.

N					The N key.

NonUSBackSlash		The secondary backslash key.

Number0				The number 0 key.

Number1				The number 1 key.

Number2				The number 2 key.

Number3				The number 3 key.

Number4				The number 4 key.

Number5				The number 5 key.

Number6				The number 6 key.

Number7				The number 7 key.

Number8				The number 8 key.

Number9				The number 9 key.

NumLock				The num lock key.

O					The O key.

P					The P key.

PageDown			The page down key.

PageUp				The page up key.

Pause				The pause key.

Period				The period key.

Plus				The plus key.

PrintScreen			The print screen key.

Q					The Q key.

Quote				The quote key.

R					The R key.

RAlt				The right alt key (equivalent to AltRight).

RBracket			The right bracket key (equivalent to BracketRight).

RControl			The right control key (equivalent to ControlRight).

Right				The right arrow key.

RShift				The right shift key (equivalent to ShiftRight).

RWin				The right win key (equivalent to WinRight).

S					The S key.

ScrollLock			The scroll lock key.

Semicolon			The semicolon key.

ShiftLeft			The left shift key.

ShiftRight			The right shift key.

Slash				The slash key.

Sleep				The sleep key.

Space				The space key.

T					The T key.

Tab					The tab key.

Tilde				The tilde key.

U					The U key.

Unknown				A key outside the known keys.

Up					The up arrow key.

V					The V key.

W					The W key.

WinLeft				The left win key.

WinRight			The right win key.

X					The X key.

Y					The Y key.

Z					The Z key.