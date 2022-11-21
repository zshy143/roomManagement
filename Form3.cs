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
    public partial class Form3 : Form
    {
        int roomid = -1;//房间号
        int empty=0;
        string name = "";//住客名字
        string address = "";//房间地址
        bool columnAdded = false;
        static string connectionString = "server=.;database=hotel;integrated security=SSPI;MultipleActiveResultSets=true;";
        public Form3()
        {
            InitializeComponent();
        }
        private void AddColumn()//添加表的列
        {
            if (columnAdded == false)
            {
                dataGridView1.Columns.Add("roomid", "房号");//添加列
                dataGridView1.Columns.Add("address", "房间地址");
                dataGridView1.Columns.Add("empty", "是否有人居住");
                dataGridView1.Columns[0].Width = 35;//空房号这一栏的宽度
                dataGridView1.Columns[2].Width = 150;
                dataGridView1.DataSource = null;//解除数据源绑定
                columnAdded = true;
            }
            else
            {
                dataGridView1.DataSource = null;//解除数据源绑定
                dataGridView1.Rows.Clear();
            }
        }
        private void button2_Click(object sender, EventArgs e)//删除房间
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入要删除的房间的房间号", "无效房间号");
                return;
            }
            roomid = int.Parse(textBox2.Text);
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库

            string selectSql = "select empty from room where roomid='" + roomid + "'";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = selectSql;
            SqlDataReader reader1 = readcommand1.ExecuteReader();//读房间是否为空
            if(reader1.Read())
            {
                empty = reader1.GetInt32(reader1.GetOrdinal("empty"));
                if(empty!=1)//房间不空
                {
                    reader1.Close();
                    readcommand1.Dispose();
                    SqlCon.Close();
                    MessageBox.Show("该房间现在有人居住，无法删除。", "房间不空");
                    return;
                }
            }
            reader1.Close();
            readcommand1.Dispose();

            //存入数据库
            string sql = "delete room where roomid='" + roomid + "'";
            SqlCommand sql1 = new SqlCommand(sql, SqlCon);
            if (sql1.ExecuteNonQuery() != 1)
            {
                MessageBox.Show("请检查该房间是否已经存在", "删除失败");
                sql1.Dispose();
                SqlCon.Close();
                return;
            }

            MessageBox.Show("删除房间成功!", "删除成功");
            sql1.Dispose();
            SqlCon.Close();
            textBox2.Text = "";
            checkRoom();
            return;
        
    }

        private void button1_Click(object sender, EventArgs e)//添加房间
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请输入要添加的房间的房间地址", "无效地址");
                return;
            }
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库
                           //存入数据库
            address = textBox1.Text;
            string sql = "insert into room(empty,address) values(1,'" + address + "')";
            SqlCommand sql1 = new SqlCommand(sql, SqlCon);

            try
            {
                sql1.ExecuteNonQuery();
            }catch
            {
                MessageBox.Show("请检查该房间是否已经存在", "保存失败");
                sql1.Dispose();
                SqlCon.Close();
                return;
            }
           /* if (sql1.ExecuteNonQuery() != 1)
            {
                MessageBox.Show("请检查该房间是否已经存在", "保存失败");
                sql1.Dispose();
                SqlCon.Close();
                return;
            }*/

            MessageBox.Show("添加房间成功!", "添加成功");
            sql1.Dispose();
            SqlCon.Close();
            textBox1.Text = "";
            checkRoom();
            return;
        }
        private void checkRoom()//查询所有房间
        {
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库                    
            string sql1 = "select roomid,zhukeid,address,empty from room";
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
                    dataGridView1.Rows[index].Cells[0].Value = reader1.GetInt32(reader1.GetOrdinal("roomid")).ToString();
                    dataGridView1.Rows[index].Cells[1].Value = reader1.GetString(reader1.GetOrdinal("address"));
                    empty = reader1.GetInt32(reader1.GetOrdinal("empty"));
                    if (empty == 1)
                    {
                        dataGridView1.Rows[index].Cells[2].Value = "该房间为空";
                    }
                    else
                    {
                        dataGridView1.Rows[index].Cells[2].Value = "该房间已被分出去";
                    }
                } while (reader1.Read());

            }
        }
        private void button3_Click(object sender, EventArgs e)//按键查询所有房间
        {
            checkRoom();

        }
    }
}
