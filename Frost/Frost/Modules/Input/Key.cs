using KC = SFML.Window.Keyboard.Key;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Values for various keyboard keys
	/// </summary>
	public enum Key
	{
		#region Letters

		/// <summary>
		/// The letter A
		/// </summary>
		A = KC.A,

		/// <summary>
		/// The letter B
		/// </summary>
		B = KC.B,

		/// <summary>
		/// The letter C
		/// </summary>
		C = KC.C,

		/// <summary>
		/// The letter D
		/// </summary>
		D = KC.D,

		/// <summary>
		/// The letter E
		/// </summary>
		E = KC.E,

		/// <summary>
		/// The letter F
		/// </summary>
		F = KC.F,

		/// <summary>
		/// The letter G
		/// </summary>
		G = KC.G,

		/// <summary>
		/// The letter H
		/// </summary>
		H = KC.H,

		/// <summary>
		/// The letter I
		/// </summary>
		I = KC.I,

		/// <summary>
		/// The letter J
		/// </summary>
		J = KC.J,

		/// <summary>
		/// The letter K
		/// </summary>
		K = KC.K,

		/// <summary>
		/// The letter L
		/// </summary>
		L = KC.L,

		/// <summary>
		/// The letter M
		/// </summary>
		M = KC.M,

		/// <summary>
		/// The letter N
		/// </summary>
		N = KC.N,

		/// <summary>
		/// The letter O
		/// </summary>
		O = KC.O,

		/// <summary>
		/// The letter P
		/// </summary>
		P = KC.P,

		/// <summary>
		/// The letter Q
		/// </summary>
		Q = KC.Q,

		/// <summary>
		/// The letter R
		/// </summary>
		R = KC.R,

		/// <summary>
		/// The letter S
		/// </summary>
		S = KC.S,

		/// <summary>
		/// The letter T
		/// </summary>
		T = KC.T,

		/// <summary>
		/// The letter U
		/// </summary>
		U = KC.U,

		/// <summary>
		/// The letter V
		/// </summary>
		V = KC.V,

		/// <summary>
		/// The letter W
		/// </summary>
		W = KC.W,

		/// <summary>
		/// The letter X
		/// </summary>
		X = KC.X,

		/// <summary>
		/// The letter Y
		/// </summary>
		Y = KC.Y,

		/// <summary>
		/// The letter Z
		/// </summary>
		Z = KC.Z,
		#endregion

		#region Numbers

		/// <summary>
		/// The number 0
		/// </summary>
		Num0 = KC.Num0,

		/// <summary>
		/// The number 1
		/// </summary>
		Num1 = KC.Num1,

		/// <summary>
		/// The number 2
		/// </summary>
		Num2 = KC.Num2,

		/// <summary>
		/// The number 3
		/// </summary>
		Num3 = KC.Num3,

		/// <summary>
		/// The number 4
		/// </summary>
		Num4 = KC.Num4,

		/// <summary>
		/// The number 5
		/// </summary>
		Num5 = KC.Num5,

		/// <summary>
		/// The number 6
		/// </summary>
		Num6 = KC.Num6,

		/// <summary>
		/// The number 7
		/// </summary>
		Num7 = KC.Num7,

		/// <summary>
		/// The number 8
		/// </summary>
		Num8 = KC.Num8,

		/// <summary>
		/// The number 9
		/// </summary>
		Num9 = KC.Num9,
		#endregion

		#region Punctuation

		/// <summary>
		/// Left square bracket symbol
		/// </summary>
		LBracket = KC.LBracket,

		/// <summary>
		/// Right square bracket symbol
		/// </summary>
		RBracket = KC.RBracket,

		/// <summary>
		/// Semi-colon symbol
		/// </summary>
		SemiColon = KC.SemiColon,

		/// <summary>
		/// Comma symbol
		/// </summary>
		Comma = KC.Comma,

		/// <summary>
		/// Period symbol
		/// </summary>
		Period = KC.Period,

		/// <summary>
		/// Single quote symbol
		/// </summary>
		Quote = KC.Quote,

		/// <summary>
		/// Forward slash symbol
		/// </summary>
		Slash = KC.Slash,

		/// <summary>
		/// Backward slash symbol
		/// </summary>
		BackSlash = KC.BackSlash,

		/// <summary>
		/// Tilde symbol
		/// </summary>
		Tilde = KC.Tilde,

		/// <summary>
		/// Equals sign
		/// </summary>
		Equal = KC.Equal,

		/// <summary>
		/// Hyphen symbol
		/// </summary>
		Dash = KC.Dash,
		#endregion

		#region Number pad

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad0 = KC.Numpad0,

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad1 = KC.Numpad1,

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad2 = KC.Numpad2,

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad3 = KC.Numpad3,

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad4 = KC.Numpad4,

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad5 = KC.Numpad5,

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad6 = KC.Numpad6,

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad7 = KC.Numpad7,

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad8 = KC.Numpad8,

		/// <summary>
		/// The number 0 on the number pad
		/// </summary>
		NumPad9 = KC.Numpad9,

		/// <summary>
		/// Addition key on the number pad
		/// </summary>
		Add = KC.Add,

		/// <summary>
		/// Subtraction key on the number pad
		/// </summary>
		Subtract = KC.Subtract,

		/// <summary>
		/// Multiplication key on the number pad
		/// </summary>
		Multiply = KC.Multiply,

		/// <summary>
		/// Division key on the number pad
		/// </summary>
		Divide = KC.Divide,
		#endregion

		#region Function (F) keys

		/// <summary>
		/// The F1 key
		/// </summary>
		F1 = KC.F1,

		/// <summary>
		/// The F2 key
		/// </summary>
		F2 = KC.F2,

		/// <summary>
		/// The F3 key
		/// </summary>
		F3 = KC.F3,

		/// <summary>
		/// The F4 key
		/// </summary>
		F4 = KC.F4,

		/// <summary>
		/// The F5 key
		/// </summary>
		F5 = KC.F5,

		/// <summary>
		/// The F6 key
		/// </summary>
		F6 = KC.F6,

		/// <summary>
		/// The F7 key
		/// </summary>
		F7 = KC.F7,

		/// <summary>
		/// The F8 key
		/// </summary>
		F8 = KC.F8,

		/// <summary>
		/// The F9 key
		/// </summary>
		F9 = KC.F9,

		/// <summary>
		/// The F10 key
		/// </summary>
		F10 = KC.F10,

		/// <summary>
		/// The F11 key
		/// </summary>
		F11 = KC.F11,

		/// <summary>
		/// The F12 key
		/// </summary>
		F12 = KC.F12,

		/// <summary>
		/// The F13 key
		/// </summary>
		F13 = KC.F13,

		/// <summary>
		/// The F14 key
		/// </summary>
		F14 = KC.F14,

		/// <summary>
		/// The F15 key
		/// </summary>
		F15 = KC.F15,
		#endregion

		#region Arrows

		/// <summary>
		/// The left arrow key
		/// </summary>
		Left = KC.Left,

		/// <summary>
		/// The right arrow key
		/// </summary>
		Right = KC.Right,

		/// <summary>
		/// The up arrow key
		/// </summary>
		Up = KC.Up,

		/// <summary>
		/// The down arrow key
		/// </summary>
		Down = KC.Down,
		#endregion

		#region Control keys

		/// <summary>
		/// Escape key
		/// </summary>
		Escape = KC.Escape,

		/// <summary>
		/// Left control key
		/// </summary>
		LControl = KC.LControl,

		/// <summary>
		/// Left shift key
		/// </summary>
		LShift = KC.LShift,

		/// <summary>
		/// Left alt key
		/// </summary>
		LAlt = KC.LAlt,

		/// <summary>
		/// Left system (Windows) key
		/// </summary>
		LSystem = KC.LSystem,

		/// <summary>
		/// Right control key
		/// </summary>
		RControl = KC.RControl,

		/// <summary>
		/// Right shift key
		/// </summary>
		RShift = KC.RShift,

		/// <summary>
		/// Right alt key
		/// </summary>
		RAlt = KC.RAlt,

		/// <summary>
		/// Right system (Windows) key
		/// </summary>
		RSystem = KC.RSystem,

		/// <summary>
		/// Menu key
		/// </summary>
		Menu = KC.Menu,

		/// <summary>
		/// Pause key
		/// </summary>
		Pause = KC.Pause,
		#endregion

		#region Spacing keys

		/// <summary>
		/// Space bar
		/// </summary>
		Space = KC.Space,

		/// <summary>
		/// Enter/Return key
		/// </summary>
		Enter = KC.Return,

		/// <summary>
		/// Backspace key
		/// </summary>
		Backspace = KC.Back,

		/// <summary>
		/// Tab indent key
		/// </summary>
		Tab = KC.Tab,
		#endregion

		#region Navigation keys

		/// <summary>
		/// Page up key
		/// </summary>
		PageUp = KC.PageUp,

		/// <summary>
		/// Page down key
		/// </summary>
		PageDown = KC.PageDown,

		/// <summary>
		/// End key
		/// </summary>
		End = KC.End,

		/// <summary>
		/// Home key
		/// </summary>
		Home = KC.Home,

		/// <summary>
		/// Insert key
		/// </summary>
		Insert = KC.Insert,

		/// <summary>
		/// Delete key
		/// </summary>
		Delete = KC.Delete,
		#endregion
	}
}
