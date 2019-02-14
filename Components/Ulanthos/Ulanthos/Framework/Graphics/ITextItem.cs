using Ulanthos.Framework.Graphics;
using Ulanthos.Math;

namespace Ulanthos.Interfaces.Graphics
{
    /// <summary>
    /// An object used to define a text item.
    /// </summary>
    public interface ITextItem : IColour
    {
        Vector2 Position { get; set; }
        Font Font { get; set; }
    }
}
