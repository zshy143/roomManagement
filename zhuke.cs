using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotleManagement
{
    public class zhuke
    {

        public int id { get; set; }
        public string name { get; set; }
        public DateTime birthday { get; set; }
        public string sex { get; set; }
        public int familyNum { get; set; }
        public string zhicheng { get; set; }
        public DateTime entryTime { get; set; }
        public int age { get; set; }
        public int worktime { get; set; }
        public int scores { get; set; }
        public string uniqueNum { get; set; }
        public zhuke(int id,string na,DateTime bir,string se,int fa,string zhi,DateTime en,int ag,int wor,int sco,string uni)
        {
            this.id = id;
            name = na;
            birthday = bir;
            sex = se;
            familyNum = fa;
            zhicheng = zhi;
            entryTime = en;
            age = ag;
            worktime = wor;
            scores = sco;
            uniqueNum = uni;
        }
    }
}
