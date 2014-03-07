using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frost.IO.Resources;

namespace Frost.ResourcePackagerGui
{
	public class ResourceSelectedEventArgs : EventArgs
	{
		private readonly ResourcePackageEntry _entry;

		public ResourcePackageEntry Entry
		{
			get { return _entry; }
		}

		public ResourceSelectedEventArgs (ResourcePackageEntry entry)
		{
			_entry = entry;
		}
	}
}
