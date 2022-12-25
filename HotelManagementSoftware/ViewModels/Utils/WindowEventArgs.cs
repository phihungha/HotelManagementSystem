using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
