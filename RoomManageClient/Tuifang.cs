using System;
using System.Text;
using System.Windows.Forms;

namespace hotleManagement
{
    public partial class Tuifang : Form
    {
        public Tuifang()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int nowroom = 0;
            string uniqueNum = textBox2.Text;
            try
            {
                nowroom = int.Parse(textBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("房间号输入有问题！");
                return;
            }
            if (uniqueNum == "" || uniqueNum.Length != 6)
            {
                MessageBox.Show("请输入6位学号或工号！");
                return;
            }
            var Url = NetTool.baseUrl+"/api/tuifang/";
            var url = Url+nowroom+"/"+uniqueNum;
            int res = int.Parse(NetTool.GetUrl(url, Encoding.UTF8));
            if (res == 1)
            {
                MessageBox.Show("退房成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("退房失败！");
                this.Close();
            }
        }
    }
}
