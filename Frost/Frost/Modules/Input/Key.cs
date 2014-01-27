using KC = SFML.Window.Keyboard.Key;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Values for various keyboard keys
	/// </summary>
	public enum Key
	{
		#region Alphabet

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

		#region Numbers above the letters

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

		Escape = 36,
		LControl = 37,
		LShift = 38,
		LAlt = 39,
		LSystem = 40,
		RControl = 41,
		RShift = 42,
		RAlt = 43,
		RSystem = 44,
		Menu = 45,
		LBracket = 46,
		RBracket = 47,
		SemiColon = 48,
		Comma = 49,
		Period = 50,
		Quote = 51,
		Slash = 52,
		BackSlash = 53,
		Tilde = 54,
		Equal = 55,
		Dash = 56,
		Space = 57,
		Return = 58,
		Back = 59,
		Tab = 60,
		PageUp = 61,
		PageDown = 62,
		End = 63,
		Home = 64,
		Insert = 65,
		Delete = 66,
		Add = 67,
		Subtract = 68,
		Multiply = 69,
		Divide = 70,
		Pause = 100,
	}
}
