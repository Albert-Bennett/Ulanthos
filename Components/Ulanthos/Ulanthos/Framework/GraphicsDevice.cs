using System;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Ulanthos.Framework.Graphics;
using Ulanthos.Interfaces;

namespace Ulanthos.Framework
{
    /// <summary>
    /// Creates a representation of the machine's graphic's device.
    /// </summary>
    public class GraphicsDevice : IDispose
    {
        Device device = null;
        Game game = null;

        /// <summary>
        /// Creates a new Graphic's device.
        /// </summary>
        /// <param name="game">The Game that this is attached to.</param>
        public GraphicsDevice(Game game)
        {
            this.game = game;

            Initialize();
        }

        /// <summary>
        /// Initializes the GraphicsDevice.
        /// </summary>
        public void Initialize()
        {
            bool adapterFound = false;

            foreach (AdapterInformation adapter in Manager.Adapters)
                foreach (DisplayMode display in adapter.SupportedDisplayModes)
                {
                    if (display.Width != 640 || display.Height != 480)
                        continue;

                    if (display.RefreshRate != 75)
                        continue;

                    adapterFound = true;
                    break;
                }

            if (!adapterFound)
                throw new ArgumentException("The graphics card on your machine is not supported.");

            if (!Manager.CheckDeviceType(Manager.Adapters.Default.Adapter,
                DeviceType.Hardware, Format.X8R8G8B8, Format.X8R8G8B8, false))
                throw new ArgumentException("The graphics card on your machine is not supported.");

            if (!Manager.CheckDeviceFormat(Manager.Adapters.Default.Adapter,
                DeviceType.Hardware, Manager.Adapters.Default.CurrentDisplayMode.Format,
                Usage.DepthStencil, ResourceType.Surface, DepthFormat.D16))
                throw new ArgumentException("The graphics card on your machine is not supported.");

            Caps caps = Manager.GetDeviceCaps(Manager.Adapters.Default.Adapter,
                DeviceType.Hardware);

            CreateFlags flags;

            if (caps.DeviceCaps.SupportsHardwareTransformAndLight)
                flags = CreateFlags.HardwareVertexProcessing;
            else
                flags = CreateFlags.SoftwareVertexProcessing;

            PresentParameters pp = new PresentParameters();

            pp.Windowed = false;
            pp.EnableAutoDepthStencil = true;
            pp.AutoDepthStencilFormat = DepthFormat.D16;
            pp.SwapEffect = SwapEffect.Discard;
            pp.BackBufferWidth = 640;
            pp.BackBufferHeight = 480;
            pp.BackBufferFormat = Format.X8R8G8B8;
            pp.PresentationInterval = PresentInterval.Immediate;

            device = new Device(0, DeviceType.Hardware, game, flags, pp);

            device.DeviceReset += new EventHandler(OnDeviceReset);
            OnDeviceReset(device, null);
        }

        public void OnDeviceReset(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Clears the various buffers for the GraphicsDevice.
        /// </summary>
        /// <param name="colour">The colour to clear the GraphicsDevice to.</param>
        public void Clear(Colour colour)
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer,
                Color.FromArgb(colour.A, colour.R, colour.G, colour.B), 1.0f, 0);
        }

        /// <summary>
        /// Shows the rendered scene.
        /// </summary>
        public void Present()
        {
            device.Present();
        }

        /// <summary>
        /// Disposes of the GraphicsDevice.
        /// </summary>
        public void Dispose()
        {
            device.Dispose();
        }
    }
}
