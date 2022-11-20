using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Owin.Hosting;
using System.Net.Http;


namespace hotleManagement
{    
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>


        [STAThread]
        static void Main()
        {

            string baseAddress = "http://113.54.197.216:9000/";

            // Start OWIN host 
            WebApp.Start<Startup>(url: baseAddress);
           // HttpClient client = new HttpClient();
           // var response = client.GetAsync(baseAddress + "api/diaofang/27/29/123456").Result;
           // MessageBox.Show(response.Content.ReadAsStringAsync().Result);
            //Console.WriteLine(response);
           // Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
