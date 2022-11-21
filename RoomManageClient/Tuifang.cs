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
            switch (res)
            {
                case 1:
                    MessageBox.Show("提交成功！");
                    this.Close();
                    break;
                case 2:
                    MessageBox.Show("房间为空无需退房！");
                    this.Close();
                    break;
                case 3:
                    MessageBox.Show("学号或工号与房间号对应不上！");
                    this.Close();
                    break;
                case 5:
                    MessageBox.Show("没有当前房间！");
                    this.Close();
                    break;
                default:
                    MessageBox.Show("服务器发生错误！");
                    this.Close();
                    break;

            }
        }
    }
}
