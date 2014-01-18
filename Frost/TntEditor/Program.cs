using System;
using System.Windows.Forms;

namespace Frost.TntEditor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main (string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var mainForm = new MainForm();
			if(args.Length > 0)
				mainForm.LoadFile(args[0]);
			Application.Run(mainForm);
		}
	}
}
