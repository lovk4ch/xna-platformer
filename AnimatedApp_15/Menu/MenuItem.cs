using System;

namespace AnimatedApp_15.MenuSystem
{
    class MenuItem
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public event EventHandler Click;
        public MenuItem(string Name)
        {
            this.Name = Name;
            Active = true;
        }
        public void OnClick()
        {
            if (Click != null)
                Click(this, null);
        }
    }
}