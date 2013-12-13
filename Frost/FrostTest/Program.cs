using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FrostTest
{
	class Program
	{
		static void Main (string[] args)
		{
			var window = new Frost.Display.Window();
			var sm = new Frost.Modules.StateManager(window, null, null);
			sm.Run();
		}
	}
}
