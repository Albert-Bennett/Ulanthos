using Ulanthos.Interfaces;

namespace Ulanthos.Framework
{
    /// <summary>
    /// Defines an abstract class which is much like the GameComponent class.
    /// </summary>
    public abstract class UlanComponent : Selectable, IName, IEnabled
    {
        string name;

        bool initilized = false;

        /// <summary>
        /// The name of this ExedusComponent.
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Weather or not this ExedusComponent has been initialized.
        /// </summary>
        public bool Initialized { get { return initilized; } }

        /// <summary>
        /// Creates a new ExedusComponent.
        /// </summary>
        /// <param name="name">The name of this ExedusComponent.</param>
        public UlanComponent(string name)
        {
            this.name = name;
            Enabled = true;
            ComponentManager.Add(this);
        }

        /// <summary>
        /// Initializes this ExedusComponent.
        /// </summary>
        public virtual void Initialize()
        {
            if (!initilized)
                initilized = true;
        }

        public virtual void Load() { }
        public virtual void Update() { }
        public virtual void Unload() { }
    }
}
