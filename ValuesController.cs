using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Web.Http;
using System.Windows.Forms;

namespace hotleManagement
{
 
    public class ValuesController : ApiController
    {
        string connectionString = "server=.;database=hotel;integrated security=SSPI;MultipleActiveResultSets=true;";

        [HttpGet]
        [Route("api/getZhuke/{uniqueNum}")]
        public ruzhu Get(string uniqueNum)//获得入住的人的信息
        {
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库                    
            //string sql1 = "select room.roomid,room.zhukeid,room.address,zhuke.name,zhuke.uniqueNum from room inner join zhuke on room.zhukeid=zhuke.id where room.empty=0";
            string sql1 = "select room.roomid,room.zhukeid,room.address,zhuke.name from room inner join zhuke on room.zhukeid=zhuke.id where room.empty=0 and zhuke.uniqueNum='"+uniqueNum+"'";

            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();
            //List<ruzhu> roominfo = new List<ruzhu>();
            ruzhu roominfo=null;
            while (reader1.Read())
            {
                int roomid = reader1.GetInt32(reader1.GetOrdinal("roomid"));
                string address = reader1.GetString(reader1.GetOrdinal("address"));
                string name = reader1.GetString(reader1.GetOrdinal("name"));
                //roominfo.Add(new ruzhu(name, uniqueNum, address, roomid));
                roominfo = new ruzhu(name, uniqueNum, address, roomid);
            }
            SqlCon.Close();
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ruzhu>>(Newtonsoft.Json.JsonConvert.SerializeObject(roominfo));
            return roominfo;
            //return Newtonsoft.Json.JsonConvert.SerializeObject(roominfo);
        }

        [HttpGet]
        [Route("api/getEmpty")]
        public List<emptyRoom> Get1()//获得空房间的信息
        {

            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库                    
            string sql1 = "select roomid,address from room where empty=1";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = sql1;
            SqlDataReader reader1 = readcommand1.ExecuteReader();
            List<emptyRoom> empty = new List<emptyRoom>();
            while (reader1.Read())
            {
                int roomid = reader1.GetInt32(reader1.GetOrdinal("roomid"));
                string address = reader1.GetString(reader1.GetOrdinal("address"));
                empty.Add(new emptyRoom(roomid,address));
            }
            SqlCon.Close();

           // return Newtonsoft.Json.JsonConvert.SerializeObject(empty);
           return empty;
        }

        [HttpGet]
        [Route("api/tuifang/{nowroom}/{uniqueNum}")]
        public int tuifang(int nowroom,string uniqueNum)
        {
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库
            string selectSql = "select room.empty,zhuke.uniqueNum from room inner join zhuke on room.zhukeid=zhuke.id where room.roomid='" + nowroom + "'";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = selectSql;
            SqlDataReader reader1 = readcommand1.ExecuteReader();//房间和人对的上不
            if(reader1.Read())
            {
                string temp = reader1.GetString(reader1.GetOrdinal("uniqueNum"));
                int empty = reader1.GetInt32(reader1.GetOrdinal("empty"));
                if (temp != uniqueNum)
                {
                    reader1.Close();
                    readcommand1.Dispose();
                    SqlCon.Close();
                    return 3;//学号或者工号与对应房间对不上
                }
                if(empty==1)
                {
                    reader1.Close();
                    readcommand1.Dispose();
                    SqlCon.Close();
                    return 2;//该房间为空，无需退房
                }

            }
            else
            {
                return 5;//没有当前房间
            }
            reader1.Close();
            readcommand1.Dispose();

            //更新表数据
            string updateSql1 = "update room set empty=1 where roomid='" + nowroom + "'";
            SqlCommand updateCom1 = SqlCon.CreateCommand();
            updateCom1.CommandText = updateSql1;
            updateCom1.ExecuteNonQuery();//更新当前房间为空
            updateCom1.Dispose();

            string deleteSql1 = "delete zhuke where uniqueNum='" + uniqueNum + "'";
            SqlCommand deleteCom1 = SqlCon.CreateCommand();
            deleteCom1.CommandText = deleteSql1;
            deleteCom1.ExecuteNonQuery();//删除完成了调房的调房表中的表项
            deleteCom1.Dispose();
            return 1;//退房成功
        }

        [HttpGet]
        [Route("api/diaofang/{nowroom}/{thenroom}/{uniqueNum}")]
        //[Route("api/diaofang")]
        public int Put(int nowroom, int thenroom,string uniqueNum)//保存调房申请
        {
           
            
            
            
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库

            string selectSql = "select zhuke.uniqueNum,zhuke.scores,room.empty from zhuke inner join room on room.zhukeid=zhuke.id where room.roomid='"+nowroom+"'";
            SqlCommand readcommand1 = SqlCon.CreateCommand();
            readcommand1.CommandText = selectSql;
            SqlDataReader reader1 = readcommand1.ExecuteReader();
            int scores=0;
            if (reader1.Read())
            {
                string temp = reader1.GetString(reader1.GetOrdinal("uniqueNum"));
                int empty = reader1.GetInt32(reader1.GetOrdinal("empty"));
                scores= reader1.GetInt32(reader1.GetOrdinal("scores"));
                if (empty == 1)
                {
                    reader1.Close();
                    readcommand1.Dispose();
                    SqlCon.Close();
                    return 4;//当前房间为空
                }
                if (temp!=uniqueNum)
                {
                    reader1.Close();
                    readcommand1.Dispose();
                    SqlCon.Close();
                    return 3;//学号或者工号与对应房间对不上
                }

            }
            else
            {
                return 5;//没有当前房间
            }

            reader1.Close();
            readcommand1.Dispose();
            //存入数据库
            string sql = "insert into diaofang(nowroom,thenroom,uniqueNum,scores) values('" + nowroom + "','" + thenroom + "','" + uniqueNum + "','"+scores+"')";
            SqlCommand sql1 = new SqlCommand(sql, SqlCon);
            if (sql1.ExecuteNonQuery() != 1)
            {
                SqlCon.Close();
                return 2;//想去的或现在的房间不存在
            }
            else
            {
                SqlCon.Close();
                return 1;//保存成功
            }

        }
        [HttpPost]
        [Route("api/save")]
        // DELETE api/values/5 
        public int Post(zhuke data)
        {
           // MessageBox.Show(data);
            SqlConnection SqlCon = new SqlConnection(connectionString); //数据库连接
            SqlCon.Open(); //打开数据库
                           //存入数据库
            string sql = "insert into zhuke(name,sex,age,birth,familyNum,zhicheng,entryTime,workTime,scores,uniqueNum) values('" + data.name + "','" + data.sex + "','" + data.age + "','" + data.birthday + "','" + data.familyNum + "','" + data.zhicheng + "','" + data.entryTime + "','" + data.worktime + "','" + data.scores + "','" + data.uniqueNum + "')";
            SqlCommand sql1 = new SqlCommand(sql, SqlCon);
            if (sql1.ExecuteNonQuery() != 1)
            {
                SqlCon.Close();
                return -1;//保存失败
            }
            else
            {
                SqlCon.Close();
                return 1;//保存成功
            }
        }
    }
}
