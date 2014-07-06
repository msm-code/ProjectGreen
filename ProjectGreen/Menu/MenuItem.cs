using System.Drawing;
using OpenTK;
using System;
namespace ProjectGreen
{
    class MenuItem
    {
        public MenuItem(string textureId, string selectedId, Action<MenuState> action)
        {
            this.TextureId = textureId;
            this.SelectedTextureId = selectedId;
            this.Action = action;
        }

        public Action<MenuState> Action { get; private set; }
        public Box2 Bounds { get; set; }
        public string TextureId { get; private set; }
        public string SelectedTextureId { get; private set; }
        public IVisual CurrentVisual { get; set; }
    }
}
