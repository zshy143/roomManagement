using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotleManagement
{
    [Serializable]
    public class ruzhu//存入住的人的相关信息
    {
        string name;
        string uniqueNum;
        string address;//房间地址
        int roomid;//房间号
        public ruzhu(string n, string u, string a, int r)
        {
            name = n;
            uniqueNum = u;
            address = a;
            roomid = r;
        }
    }
}

