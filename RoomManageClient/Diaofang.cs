using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace hotleManagement
{
    public partial class Diaofang : Form
    {
        public Diaofang()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
                var resdata = JArray.Parse(NetTool.GetUrl(NetTool.baseUrl + "/api/getEmpty/", Encoding.UTF8));
                dataGridView1.Rows.Clear();
                int i;
                for (i = 0; i < resdata.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells["roomid"].Value = resdata[i]["roomid"];
                    dataGridView1.Rows[i].Cells["address"].Value = resdata[i]["address"];
                }
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!CheckFormValue())
            {
                MessageBox.Show("信息填写不完整！！");
                return;
            }
            int nowroom = 0;
            string uniqueNum = this.uniqueNum.Text;
            int toRoomid = 0;
            try
            {
                toRoomid = int.Parse(toRoomBox.Text);
                nowroom = int.Parse(roomBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("房间号输入不正确！");
                return;

            }
            if (uniqueNum.Length != 6)
            {
                MessageBox.Show("请输入6位学号或工号！");
                return;
            }
            
            if (roomBox.Text != "" && toRoomBox.Text != "" && this.uniqueNum.Text != "")
            {
                var Url = NetTool.baseUrl+"/api/diaofang/";
            
                var url = Url + nowroom + "/" + toRoomid + "/" + uniqueNum;
                int res = int.Parse(NetTool.GetUrl(url, Encoding.UTF8));
                switch (res)
                {
                    case 1:
                        MessageBox.Show("提交成功！");
                        this.Close();
                        break;
                    case 2:
                        MessageBox.Show("学号或工号已存在！");
                        this.Close();
                        break;
                    case 3:
                        MessageBox.Show("学号或工号与房间号对应不上！");
                        this.Close();
                        break;
                    case 4:
                        MessageBox.Show("当前房间为空！");
                        this.Close();
                        break;
                    case 5:
                        MessageBox.Show("没有当前房间！");
                        this.Close();
                        break;
                    case 6:
                        MessageBox.Show("要调去的房间不存在！");
                        this.Close();
                        break;
                    default:
                        MessageBox.Show("服务器发生错误！");
                        this.Close();
                        break;
                       
                }
                

            }
        }
      
        /// <summary>
        /// 判断表格中 姓名 性别 家庭人数 职称 是否为空
        /// </summary>
        /// <returns></returns>
        private bool CheckFormValue()
        {
            if (roomBox.Text == "" || uniqueNum.Text == "")
                return false;
            return true;
        }

  
    }
}
