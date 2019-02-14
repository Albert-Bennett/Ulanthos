using Ulanthos.Interfaces;

namespace Ulanthos
{
    /// <summary>
    /// An abstract class to help manage the selecting of items.
    /// </summary>
    public abstract class Selectable : IVisable, IEnabled
    {
        protected bool enabled = true;
        protected bool visible = true;

        /// <summary>
        /// Weather the Selectable object is enabled or not.
        /// </summary>
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        /// <summary>
        /// Weather the Selectable object is visable or not.
        /// </summary>
        public bool Visable { get { return visible; } set { visible = value; } }

        /// <summary>
        /// Toggles this Selectable object's enabled property.
        /// </summary>
        public void ToggleEnabled()
        {
            if (enabled)
                enabled = false;
            else
                enabled = true;
        }

        /// <summary>
        /// Toggles this Selectable object's visability property.
        /// </summary>
        public void ToggleVisable()
        {
            if (visible)
                visible = false;
            else
                visible = true;
        }
    }
}
