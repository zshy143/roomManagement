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
    public partial class Form2 : Form
    {
        string uniqueNum="";//学号或工号
        int roomid=-1;//房间号
        string name="";//住客名字
        string address="";//房间地址
        bool columnAdded = false;
        static string connectionString = "server=.;database=hotel;integrated security=SSPI;MultipleActiveResultSets=true;";
        public Form2()
        {
            InitializeComponent();
        }
        public void reset()
        {
            name=address=uniqueNum = "";
            roomid = -1;
        }
        private void AddColumn()//添加表的列
        {
            if (columnAdded == false)
            {
                dataGridView1.Columns.Add("roomid", "房号");//添加列
                dataGridView1.Columns.Add("address", "房间地址");
                dataGridView1.Columns.Add("uniqueNum", "学号或工号");
                dataGridView1.Columns.Add("name", "姓名");
                dataGridView1.Columns[0].Width = 30;//空房号这一栏的宽度
                dataGridView1.DataSource = null;//解除数据源绑定
                columnAdded = true;
            }
            else
            {
                dataGridView1.DataSource = null;//解除数据源绑定
                dataGridView1.Rows.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)//按学号或工号查询
        {
            if(textBox1.Text=="")
            {
                return;
            }
            this.reset();
            uniqueNum = textBox1.Text;
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库
                           // string sql1 = "select room.roomid,room.zhukeid,room.address,zhuke.name from room inner join zhuke on room.zhukeid=zhuke.id where room.empty=0";
            string sql1 = "select room.roomid,room.zhukeid,room.address,zhuke.name from room inner join zhuke on room.zhukeid=zhuke.id where room.empty=0 and uniqueNum='" + uniqueNum + "'";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();//读租客和房间信息

            AddColumn();
            int index;        

            if (reader1.Read())
            {
                    index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = reader1.GetInt32(reader1.GetOrdinal("roomid"));
                    dataGridView1.Rows[index].Cells[1].Value = reader1.GetString(reader1.GetOrdinal("address"));
                    dataGridView1.Rows[index].Cells[2].Value = uniqueNum;
                    dataGridView1.Rows[index].Cells[3].Value = reader1.GetString(reader1.GetOrdinal("name"));
            }
            else//没有空房
            {
                MessageBox.Show("查无此人或未为其分房。", "查无此人");

            }
            reader1.Close();
            readcommand1.Dispose();
            SqlCon.Close();
            dataGridView1.AutoGenerateColumns = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                return;
            }
            this.reset();
            roomid = int.Parse(textBox2.Text);
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库
                           // string sql1 = "select room.roomid,room.zhukeid,room.address,zhuke.name from room inner join zhuke on room.zhukeid=zhuke.id where room.empty=0";
            string sql1 = "select room.roomid,room.zhukeid,room.address,zhuke.name,zhuke.uniqueNum from room inner join zhuke on room.zhukeid=zhuke.id where room.empty=0 and roomid='" + roomid + "'";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();//读租客和房间信息
            AddColumn();
            int index;

            if (reader1.Read())
            {
                index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = roomid;
                dataGridView1.Rows[index].Cells[1].Value = reader1.GetString(reader1.GetOrdinal("address"));
                dataGridView1.Rows[index].Cells[2].Value = reader1.GetString(reader1.GetOrdinal("uniqueNum"));
                dataGridView1.Rows[index].Cells[3].Value = reader1.GetString(reader1.GetOrdinal("name"));
            }
            else//没有
            {
                MessageBox.Show("该房间并为分出去或并无该房间。", "空房");

            }
            reader1.Close();
            readcommand1.Dispose();
            SqlCon.Close();
            dataGridView1.AutoGenerateColumns = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库                    
            string sql1 = "select room.roomid,room.zhukeid,room.address,zhuke.name,zhuke.uniqueNum from room inner join zhuke on room.zhukeid=zhuke.id where room.empty=0";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();//读租客和房间信息
            AddColumn();
            int index;

            if (reader1.Read())
            {
                do
                {
                    index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = reader1.GetInt32(reader1.GetOrdinal("roomid"));
                    dataGridView1.Rows[index].Cells[1].Value = reader1.GetString(reader1.GetOrdinal("address"));
                    dataGridView1.Rows[index].Cells[2].Value = reader1.GetString(reader1.GetOrdinal("uniqueNum"));
                    dataGridView1.Rows[index].Cells[3].Value = reader1.GetString(reader1.GetOrdinal("name"));
                } while (reader1.Read());
               
            }
            else//没有
            {
                MessageBox.Show("该房间并为分出去或并无该房间。", "空房");

            }
            reader1.Close();
            readcommand1.Dispose();
            SqlCon.Close();
            dataGridView1.AutoGenerateColumns = true;
        }


    }
}
