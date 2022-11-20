using System;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace hotleManagement
{
    public partial class CheckRoom : Form
    {
        public CheckRoom()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string uniqueNum = textBox1.Text;
            if (uniqueNum == "" || uniqueNum.Length != 6)
            {
                MessageBox.Show("请输入6位学号或工号！");
                return;
            }
            var Url = NetTool.baseUrl+"/api/getZhuke/";
            var url = Url + uniqueNum;
            try
            {
                JObject res = JObject.Parse(NetTool.GetUrl(url, Encoding.UTF8));
                textBox2.Text = res.GetValue("roomid").ToString();
                //textBox2.Text = res["roomid"];
                textBox3.Text = res.GetValue("address").ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("没有房间信息！");
            }
            
        }
    }
}
