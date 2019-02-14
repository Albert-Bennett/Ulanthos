namespace Ulanthos.Media.Audio
{
    /// <summary>
    /// The properties of an Jsong object.
    /// </summary>
    public struct JProp
    {
        /// <summary>
        /// The volume of the JSong.
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// The tempo of the JSong.
        /// </summary>
        public float Tempo { get; set; }

        /// <summary>
        /// The pitch of the JSong.
        /// </summary>
        public float Pitch { get; set; }
    }
}
