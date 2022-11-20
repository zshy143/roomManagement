using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotleManagement
{
    [Serializable]
    public class emptyRoom
    {
        int roomid;
        string address;
        public emptyRoom(int i, string a)
        {
            roomid = i;
            address = a;
        }
    }
}
