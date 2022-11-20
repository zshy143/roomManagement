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
using System.Reflection;
using System.Collections;
namespace hotleManagement
{
    public partial class Form4 : Form
    {


        int zhukeid = 0;
        int roomid;
        string address;
        string uniqueNum;
        static bool columnAdded = false;
        static bool hasEmpty = true;//是否还有空房间
        static string connectionString = "server=.;database=hotel;integrated security=SSPI;;MultipleActiveResultSets=true;";
        public Form4()
        {
            InitializeComponent();
        }
        private void AddColumn()//添加表的列
        {
            if (columnAdded == false)
            {
                dataGridView1.Columns.Add("emptyRoomid", "空房号");//添加两列
                dataGridView1.Columns.Add("address", "房间地址");
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
        private void button1_Click(object sender, EventArgs e)//退房之前查询
        {
            if(textBox1.Text=="")
            {
                return;
            }
            roomid = int.Parse(textBox1.Text);

            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库

            string sql1 = "select zhukeid,empty,address from room where roomid = '"+roomid+"'";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();//读租客id
            if(reader1.Read())
            {

                
               int empty= reader1.GetInt32(reader1.GetOrdinal("empty"));
                if (empty==1)//这个房间没有被租出去
                {
                    reader1.Close();
                    readcommand1.Dispose();
                    return;
                }
                zhukeid = reader1.GetInt32(reader1.GetOrdinal("zhukeid"));
                address = reader1.GetString(reader1.GetOrdinal("address"));
                reader1.Close();
            }
            readcommand1.Dispose();

            string sql2 = "select name,age,zhicheng,sex,uniqueNum from zhuke where id = '" + zhukeid + "'";
            SqlCommand readcommand2 = SqlCon.CreateCommand();
            readcommand2.CommandText = sql2;
            SqlDataReader reader2 = readcommand2.ExecuteReader();
            if(reader2.Read())
            {
                string name = reader2.GetString(reader2.GetOrdinal("name"));
                string sex = reader2.GetString(reader2.GetOrdinal("sex"));
                string zhicheng = reader2.GetString(reader2.GetOrdinal("zhicheng"));
                string age = reader2.GetInt32(reader2.GetOrdinal("age")).ToString();
                uniqueNum = reader2.GetString(reader2.GetOrdinal("uniqueNum"));
                textBox2.Text = name;
                textBox3.Text = age;
                textBox4.Text = zhicheng;
                textBox5.Text = sex;
                textBox8.Text = uniqueNum;
                textBox7.Text = address;
            }
            reader2.Close();
            readcommand2.Dispose();

            SqlCon.Close();
        }

        private void button2_Click(object sender, EventArgs e)//确认退房
        {
            if (textBox1.Text == "")
            {
                return;
            }
            roomid = int.Parse(textBox1.Text);
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库

            string sql1 = "select zhukeid,empty from room where roomid = '" + roomid + "'";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();//读租客id
            if (reader1.Read())
            {

                int empty = reader1.GetInt32(reader1.GetOrdinal("empty"));
                if (empty == 1)//这个房间没有被租出去
                {
                    MessageBox.Show("该房间为空,无需退房。", "空房间");
                    reader1.Close();
                    readcommand1.Dispose();
                    return;
                }
                zhukeid = reader1.GetInt32(reader1.GetOrdinal("zhukeid"));
                reader1.Close();
            }
            readcommand1.Dispose();
            string Sql2 = "delete from zhuke where id = '" + zhukeid + "'";//删除住客
            SqlCommand deleteCom = SqlCon.CreateCommand();
            deleteCom.CommandText = Sql2;
            deleteCom.ExecuteNonQuery();

            string updateSql = "update room set empty=1 where roomid = '" + roomid + "'";//更新为空
            SqlCommand updateCom = SqlCon.CreateCommand();
            updateCom.CommandText = updateSql;
            updateCom.ExecuteNonQuery();

            MessageBox.Show("退房完成。", "退房完成");
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            SqlCon.Close();
            checkEmptyRoom();

        }

        private void checkEmptyRoom()//查询空房间
        {
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库

            string sql1 = "select roomid,address from room where empty =1";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();//读空房号和其地址

            AddColumn();
            int index;
            if (reader1.Read())//读出来有数据说明有空房
            {
                hasEmpty = true;
                do
                {
                    index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = reader1.GetInt32(reader1.GetOrdinal("roomid"));
                    dataGridView1.Rows[index].Cells[1].Value = reader1.GetString(reader1.GetOrdinal("address"));
                } while (reader1.Read());
            }
            else
            {
                hasEmpty = false;
            }
            dataGridView1.AutoGenerateColumns = true;
            reader1.Close();
            readcommand1.Dispose();
            SqlCon.Close();
        }

        private void button3_Click(object sender, EventArgs e)//按键查询空房间
        {
            
            checkEmptyRoom();
            if(hasEmpty==false)
            {
                MessageBox.Show("房间已满，无空房间。", "无空房间");
            }
        }

        private void button4_Click(object sender, EventArgs e)//调房
        {
            if(textBox1.Text==""||textBox6.Text=="")
            {
                MessageBox.Show("当前房间号或要调去的房间号为空。", "房间号为空");
                return;
            }
            int nowRoomid = int.Parse(textBox1.Text);//现在的
            int thenRoomid = int.Parse(textBox6.Text);//调去的房间号

            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库
            string sql1 = "select empty from room where roomid ='"+thenRoomid+"'";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();
            if(reader1.Read())
            {
                int empty = reader1.GetInt32(reader1.GetOrdinal("empty"));
                if(empty==0)//要调去的房间非空，不能调
                {
                    MessageBox.Show("要调去的房间号已有人。", "房间不空");
                    reader1.Close();
                    readcommand1.Dispose();
                    SqlCon.Close();
                    return;
                }
                else//可以调
                {                    
                    string sql2 = "select zhukeid,empty from room where roomid = '"+nowRoomid+"'";
                    SqlCommand readcommand2 = SqlCon.CreateCommand();
                    readcommand2.CommandText = sql2;
                    SqlDataReader reader2 = readcommand2.ExecuteReader();
                    if(reader2.Read())
                    {

                        empty = reader2.GetInt32(reader2.GetOrdinal("empty"));
                        if (empty == 1)//当前房间为空，不能调
                        {
                            MessageBox.Show("当前的房间号为空。", "房间空");
                            reader2.Close();
                            readcommand2.Dispose();
                            SqlCon.Close();
                            return;
                        }



                        int zhukeid = reader2.GetInt32(reader2.GetOrdinal("zhukeid"));
                        //更新room表的要调去的房间的zhukeid和empty
                        string updateSql1 = "update room set zhukeid='" + zhukeid + "',empty=0 where roomid ='" + thenRoomid + "'";
                        //将之前的房间的empty更新为1
                        string updateSql2 = "update room set empty=1 where roomid='" + nowRoomid + "'";
                        SqlCommand updateCom1 = SqlCon.CreateCommand();
                        updateCom1.CommandText = updateSql1;
                        updateCom1.ExecuteNonQuery();
                        updateCom1.Dispose();
                        SqlCommand updateCom2 = SqlCon.CreateCommand();
                        updateCom2.CommandText = updateSql2;
                        updateCom2.ExecuteNonQuery();
                        updateCom2.Dispose();
                    }

                }
            }
            SqlCon.Close();

            MessageBox.Show("调房成功。", "调房成功");
            textBox1.Text = textBox6.Text;
            textBox6.Text = "";
            checkEmptyRoom();
        }





        public DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        private void button5_Click(object sender, EventArgs e)//将调房表里的调房申请完成
        {
            string uniqueNum;
            int nowroom=0, thenroom=0,zhukeid=0;
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库
            string sql1 = "select * from diaofang order by scores desc";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();
            if(reader1.Read())
            {
                do
                {
                    nowroom = reader1.GetInt32(reader1.GetOrdinal("nowroom"));
                    thenroom = reader1.GetInt32(reader1.GetOrdinal("thenroom"));
                    uniqueNum = reader1.GetString(reader1.GetOrdinal("uniqueNum"));
                    string selectSql = "select uniqueNum,id from zhuke inner join room on room.zhukeid=zhuke.id where room.roomid='" + nowroom + "'";
                    SqlCommand readcommand2 = SqlCon.CreateCommand();
                    readcommand2.CommandText = selectSql;
                    SqlDataReader reader2 = readcommand2.ExecuteReader();//检查当前房间是否是这个人住的
                    if (reader2.Read())
                    {
                        string temp = reader2.GetString(reader2.GetOrdinal("uniqueNum"));
                        if (temp != uniqueNum)
                        {
                            reader2.Close();
                            readcommand2.Dispose();
                            continue;
                        }
                        else
                        {
                            zhukeid = reader2.GetInt32(reader2.GetOrdinal("id"));
                            reader2.Close();
                            readcommand2.Dispose();
                        }
                    }
                    //检查要去的房间是否为空
                    string selectSql2 = "select empty from room where roomid='" + thenroom + "'";
                    SqlCommand readcommand3 = SqlCon.CreateCommand();
                    readcommand3.CommandText = selectSql2;
                    SqlDataReader reader3 = readcommand3.ExecuteReader();
                    if(reader3.Read())
                    {
                        int empty = reader3.GetInt32(reader3.GetOrdinal("empty"));
                        if (empty!=1)//不为空
                        {
                            reader3.Close();
                            readcommand3.Dispose();
                            continue;
                        }
                        else
                        {
                            reader3.Close();
                            readcommand3.Dispose();
                        }
                    }

                    string updateSql1 = "update room set empty=1 where zhukeid='" + zhukeid + "'";
                    SqlCommand updateCom1 = SqlCon.CreateCommand();
                    updateCom1.CommandText = updateSql1;
                    updateCom1.ExecuteNonQuery();//更新当前房间为空
                    updateCom1.Dispose();
                    string updateSql2 = "update room set empty=0,zhukeid='" + zhukeid + "' where roomid='" + thenroom + "'";
                    SqlCommand updateCom2 = SqlCon.CreateCommand();
                    updateCom2.CommandText = updateSql2;
                    updateCom2.ExecuteNonQuery();//更新要去的房间不空和对应住客id
                    updateCom2.Dispose();

                    string deleteSql1 = "delete diaofang where uniqueNum='" + uniqueNum + "'";
                    SqlCommand deleteCom1 = SqlCon.CreateCommand();
                    deleteCom1.CommandText = deleteSql1;
                    deleteCom1.ExecuteNonQuery();//删除完成了调房的调房表中的表项
                    deleteCom1.Dispose();

                } while (reader1.Read());
            }
        }
    }



}
