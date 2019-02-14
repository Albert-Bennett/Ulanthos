namespace Ulanthos.Interfaces
{
    /// <summary>
    /// Used to change the visability of an item.
    /// </summary>
    public interface IVisable
    {
        bool Visable { get; set; }
        void ToggleVisable();
    }
}
