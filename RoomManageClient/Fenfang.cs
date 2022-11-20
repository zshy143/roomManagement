using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;


namespace hotleManagement
{
    public partial class Fenfang : Form
    {
        public Fenfang()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public int GetAgeByBirthdate(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }
        /// <summary>
        /// 判断表格中 姓名 性别 家庭人数 职称 是否为空
        /// </summary>
        /// <returns></returns>
        private bool CheckFormValue() {
            if (nameBox.Text == "" || comboBox1.Text == "" || textBox2.Text == ""|| comboBox2.Text == ""||uniqueNum.Text == "")
                return false;
            return true;
        }
  
        private void button1_Click(object sender, EventArgs e)
        {
            if (!CheckFormValue())
            {
                MessageBox.Show("信息填写不完整！！");
                return;
            }
            string name = nameBox.Text;
            string birthday = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            
            string sex = comboBox1.Text;
            int familyNum = 0;
            string zhicheng = comboBox2.Text;
            string uniqueNum = this.uniqueNum.Text;
            string entryTime = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            int age = GetAgeByBirthdate(dateTimePicker1.Value);
            int worktime = GetAgeByBirthdate(dateTimePicker2.Value);
            int scores = 0;
            int ruzhu = 0;
            if (age < 17 || age > 65)
            {
                MessageBox.Show("请输入正确的出生日期，保证年龄在17-65之间！","年龄不满足");
                return;
            }
            if (uniqueNum.Length != 6)
            {
                MessageBox.Show("请输入6位学号或工号！");
                return;
            }
            try
            {
                familyNum = int.Parse(textBox2.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("家庭人数输入不正确！","填写错误");
                return;
            }
            Dictionary<string, int> zhichengToScore = new Dictionary<string, int>
            { { "教授",5}, { "副教授", 4}, { "研究员", 4 }, { "副研究员", 3 }, { "学生", 2 } };
            
            scores =scores+ age / 10 + worktime / 2 + familyNum / 2 + zhichengToScore[comboBox2.Text];


            if(scores<3)
            {
                MessageBox.Show("不满足分房条件！");
         
            }
            else if(nameBox.Text != ""&& comboBox1.Text != ""&& textBox2.Text != ""&& comboBox2.Text != "")
            {
              
                var zhukeform = new JObject();
                zhukeform.Add("name", name);
                zhukeform.Add("sex", sex);
                zhukeform.Add("age", age);
                zhukeform.Add("birth", birthday);
                zhukeform.Add("familyNum", familyNum);
                zhukeform.Add("zhicheng", zhicheng);
                zhukeform.Add("entryTime", entryTime);
                zhukeform.Add("workTime", worktime);
                zhukeform.Add("scores", scores);
                zhukeform.Add("ruzhu", ruzhu);
                zhukeform.Add("uniqueNum", uniqueNum);
                int res = int.Parse(NetTool.PostUrl(NetTool.baseUrl+"/api/save/", zhukeform.ToString()));
                if (res == 1)
                {
                    MessageBox.Show("提交成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("提交失败！");
                    this.Close();
                }

            }
        }


    }
}
