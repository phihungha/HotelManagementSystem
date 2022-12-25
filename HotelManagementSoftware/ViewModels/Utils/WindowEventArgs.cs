using System;

namespace HotelManagementSoftware.ViewModels.Utils
{
    public class WindowEventArgs : EventArgs
    {

        public int Id { get; private set; }
        public WindowEventArgs(int id)
        {
            this.Id = id;
        }
    }
}
