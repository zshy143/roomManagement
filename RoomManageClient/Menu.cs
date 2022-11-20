using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace hotleManagement
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            GetSeverip();
        }

        private void GetSeverip()
        {
            try
            {
                StreamReader reader = File.OpenText("config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObj = (JObject)JToken.ReadFrom(jsonTextReader);
                NetTool.baseUrl = jsonObj["ip"].ToString();
                reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("初始化服务器ip错误");
                Environment.Exit(0);
            }
        }

        private void fenfang_Click(object sender, EventArgs e)
        {
            Fenfang form = new Fenfang();
            form.ShowDialog();
        }

        private void tuifang_Click(object sender, EventArgs e)
        {
            Tuifang form = new Tuifang();
            form.ShowDialog();
        }

        private void diaofang_Click(object sender, EventArgs e)
        {
            Diaofang form = new Diaofang();
            form.ShowDialog();
        }

        private void roomInfo_Click(object sender, EventArgs e)
        {
            CheckRoom form = new CheckRoom();
            form.ShowDialog();
        }
    }
}
