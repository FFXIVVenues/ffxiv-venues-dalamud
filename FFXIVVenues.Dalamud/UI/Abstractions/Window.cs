using Dalamud.Interface;
using ImGuiNET;
using System.Numerics;

namespace FFXIVVenues.Dalamud.UI.Abstractions
{
    internal abstract class Window
    {
        public bool Visible => _visible;
        protected string Title { get; set; } = "FFXIV Venues";
        protected Vector2 InitialSize { get; set; } = new Vector2(600, 450);
        protected Vector2 MinimumSize { get; set; } = new Vector2(300, 200);
        protected Vector2 MaximumSize { get; set; } = new Vector2(1000, 1000);
        protected ImGuiWindowFlags WindowFlags { get; set; } = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;

        private bool _visible = false;

        public Window(UiBuilder uiBuilder) {
            uiBuilder.Draw += this.Draw;
        }

        public void Show() =>
            this._visible = true;

        public void Hide() =>
            this._visible = false;

        public void Draw()
        {
            if (!this._visible)
                return;

            ImGui.SetNextWindowSize(this.InitialSize, ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(this.MinimumSize, this.MaximumSize);
            if (ImGui.Begin(this.Title, ref this._visible, this.WindowFlags))
            {
                this.Render();
            };
            ImGui.End();
        }

        public abstract void Render();

    }
}
