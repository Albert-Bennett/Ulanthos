namespace Ulanthos.Interfaces.Game
{
    /// <summary>
    /// A delegate used to determine if an object was clicked.
    /// </summary>
    /// <param name="sender">The object who threw the event.</param>
    public delegate void OnClicked(object sender);

    /// <summary>
    /// A delegate used to determine if an object was hovered over.
    /// </summary>
    /// <param name="sender">The object who threw the event.</param>
    public delegate void OnHoveredOver(object sender);

    /// <summary>
    /// Used by objects to handle input.
    /// </summary>
    public interface IHandleInput
    {
        void HandleInput();
    }
}
