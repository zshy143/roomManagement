using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;



namespace hotleManagement
{
    public partial class Form1 : Form
    {
        static string connectionString = "server=.;database=hotel;integrated security=SSPI;MultipleActiveResultSets=true;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public int GetAgeByBirthdate(DateTime birthdate)//根据生日或入职时间获得年龄或工作时间
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        private void button1_Click(object sender, EventArgs e)//读入数据，并计算分数，年龄等，高于一定分数则存入数据库等待分房
        {
            string name = nameBox.Text;
            DateTime birthday = dateTimePicker1.Value;
            string sex = comboBox1.Text;
            int familyNum = int.Parse(textBox2.Text);
            string zhicheng = comboBox2.Text;
            string uniqueNum = textBox1.Text;
            DateTime entryTime = dateTimePicker2.Value;
            int age = GetAgeByBirthdate(dateTimePicker1.Value);
            
            if (age < 17 || age > 65)
            {
                MessageBox.Show("请输入正确的出生日期,保证年龄在17～65之间！","年龄不满足");
                return;
            }

            if(uniqueNum.Length!=6)
            {
                MessageBox.Show("学号或工号格式不对，请输入6位数字！", "学号或工号错误");
                return;
            }
            int worktime = GetAgeByBirthdate(dateTimePicker2.Value);
            int scores = 0;
            scores += age / 10;
            scores += worktime / 2;
            scores += familyNum/ 2;

            if (zhicheng == "教授") scores += 5;
            else if (zhicheng == "副教授") scores += 4;
            else if (zhicheng == "研究员") scores += 4;
            else if (zhicheng == "副研究员") scores += 3;
            else if (zhicheng == "学生") scores += 2;

            if(scores<3)
            {
                MessageBox.Show("不满住条件，拒绝为其分房!", "拒绝分房");
                return;
            }

            if(name!=""&&sex!=""&& textBox2.Text != ""&&zhicheng!=""&& textBox1.Text!="")
            {

                SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
                SqlCon.Open(); //打开数据库
                //存入数据库
                string sql = "insert into zhuke(name,sex,age,birth,familyNum,zhicheng,entryTime,workTime,scores,uniqueNum) values('"+name+"','"+sex+"','"+age+"','"+ dateTimePicker1.Value + "','"+familyNum+"','"+zhicheng+"','"+ dateTimePicker2.Value+ "','"+worktime+"','"+scores+"','"+uniqueNum+"')";
                SqlCommand sql1 = new SqlCommand(sql, SqlCon);
                if(sql1.ExecuteNonQuery()!=1)
                {
                    MessageBox.Show("请检查学号等信息是否正确或是否有信息未填", "保存失败");
                    sql1.Dispose();
                    SqlCon.Close();
                    return;
                }

                MessageBox.Show("保存成功!","保存");
                sql1.Dispose();
                SqlCon.Close();

                nameBox.Text="";
                dateTimePicker1.Value=DateTime.Now;         
                textBox2.Text="";
                dateTimePicker2.Value = DateTime.Now;

            }
            else
            {
                MessageBox.Show("请将所有信息填完整.", "提示");
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)//打开退房和调房申请页面
        {
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void button2_Click(object sender, EventArgs e)//分房，读出空房roomid和未入住的zhukeid，更新对应信息
        {
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库
            // string sql = "select id,name,sex,age,birth,familyNum,zhicheng,entryTime,workTime,scores from zhuke order by scores,age desc";
            string sql1 = "select id from zhuke where ruzhu=0 order by scores desc,age desc";
            SqlCommand readcommand1 =SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();//读租客信息
            string sql2 = "select roomid from room where empty=1 order by roomid";
            SqlCommand readcommand2 = SqlCon.CreateCommand();
            readcommand2.CommandText = sql2;
            SqlDataReader reader2 = readcommand2.ExecuteReader();//读空房间的roomid
            int fenFangWanCheng = 1;//判断分房是否正常完成
            while(reader1.Read())
            {
                int id = reader1.GetInt32(reader1.GetOrdinal("id"));//读未入住租客的id
                int roomid = 0;
                if(reader2.Read())
                {
                    roomid = reader2.GetInt32(reader2.GetOrdinal("roomid"));//读空房间的roomid
                    string updateSql = "update room set zhukeid = '" +id+ "', empty=0 where roomid = '" +roomid+ "'";//更新zhukeid和非空
                    SqlCommand updateCom = SqlCon.CreateCommand();
                    updateCom.CommandText = updateSql;
                    if(updateCom.ExecuteNonQuery()!=1)//更新房间信息中的empty和zhukeid，说明该房间已入住和住的人的id
                    {
                        reader1.Close();
                        reader2.Close();
                        readcommand1.Dispose();
                        readcommand2.Dispose();
                        break;
                    }
                    string updateSql2 = "update zhuke set ruzhu = 1 where id = '" + id + "'";
                    SqlCommand updateCom2 = SqlCon.CreateCommand();
                    updateCom2.CommandText = updateSql2;
                    if (updateCom2.ExecuteNonQuery() != 1)//更新住客表的ruzhu，说明该人已入住
                    {
                        reader1.Close();
                        reader2.Close();
                        readcommand1.Dispose();
                        readcommand2.Dispose();
                        break;
                    }

                }
                else//房间已满，显示无法分房
                {
                    fenFangWanCheng = 0;
                    reader1.Close();
                    reader2.Close();
                    readcommand1.Dispose();
                    readcommand2.Dispose();
                    //
                    MessageBox.Show("房间已满，无法继续分房！", "房间已满");
                    break;
                }

            }
            if(fenFangWanCheng==1)
            {
                MessageBox.Show("分房完成！", "分房完成");
            }
            SqlCon.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}
