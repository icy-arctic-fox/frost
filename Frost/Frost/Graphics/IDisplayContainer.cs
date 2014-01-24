using System.Collections.Generic;

namespace Frost.Graphics
{
	/// <summary>
	/// A renderable object that can have child objects that are also renderable
	/// </summary>
	public interface IDisplayContainer<TDrawable> : IRenderable, ICollection<TDrawable> where TDrawable : IRenderable
	{
	}
}
