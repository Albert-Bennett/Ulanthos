namespace Ulanthos.Interfaces
{
    /// <summary>
    /// Used to enable objects.
    /// </summary>
    public interface IEnabled
    {
        bool Enabled { get; set; }
        void ToggleEnabled();
    }
}
