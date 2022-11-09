using Microsoft.CSharp;
using MySql.Data.MySqlClient;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace Package
{
    public class package
    {
        public static HttpResponseMessage toJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        public enum sql_connection
        {
            conf = 0,
            data = 1,
            balance = 2
        }
        /// <summary>
        /// 执行sql语句并返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet sql(string sql, sql_connection sqlconnection = sql_connection.conf)
        {
            DataSet DataSet = new DataSet();
            DataTable DataTable = new DataTable();
            MySqlConnection con = new MySqlConnection("server=60.205.204.236;user id=root; password=qwer6961; database=balance_ceshi; pooling=false;port=3306;SslMode=Preferred;Charset=utf8");
            con.Open();
            MySqlDataAdapter sqlData = new MySqlDataAdapter(sql, con);
            sqlData.Fill(DataTable);
            DataSet.Tables.Add(DataTable);
            con.Close();
            
            return DataSet;

        }

        public string sqlconnection = ConfigurationManager.ConnectionStrings["sql_data"].ToString();

        public long insterSql(string sqlString)
        {
            MySqlConnection con = new MySqlConnection(sqlconnection);
            con.Open();
            MySqlCommand mycmd = new MySqlCommand();
            mycmd.Connection = con;
            mycmd.CommandType = CommandType.Text;
            mycmd.CommandText = sqlString;
            mycmd.ExecuteNonQuery();
            var id = mycmd.LastInsertedId;
            con.Close();
            return id;
        }

        public static void sql(Func<string> insert, sql_connection balance)
        {
            throw new NotImplementedException();
        }
        #region XML
        /// <summary>
        /// 修改xml节点属性值
        /// getxml(xml路径,节点路径,节点属性名,值)
        /// </summary>
        public static void setxml(string xml, string node, string name, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xml);
            var root = xmlDoc.DocumentElement;//取到根结点
            XmlElement node1 = (XmlElement)xmlDoc.SelectSingleNode(node);
            node1.SetAttribute(name, value);
            xmlDoc.Save(xml);
        }

        /// <summary>
        /// 读取xml节点属性值
        /// setxml(xml路径,节点路径,节点属性名)
        /// </summary>
        public static string getxml(string xml, string node, string name)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xml);
            var root = xmlDoc.DocumentElement;//取到根结点
            XmlElement node1 = (XmlElement)xmlDoc.SelectSingleNode(node);
            return node1.GetAttribute(name).ToString();
        }
        #endregion
        #region 日志
        /// <summary>
        /// 添加一条记录到日志文件
        /// 储存目录
        /// C:\FeiRanLog\yyyy-mm-dd.log
        /// </summary>
        /// <param name="num"></param>
        public static void log(string num)
        {
            try
            {
                // 判断文件路径是否存在，不存在则创建文件夹
                if (!Directory.Exists(@"C:\FeiRanLog\"))
                {
                    // 不存在就创建目录
                    Directory.CreateDirectory(@"C:\FeiRanLog\");
                }
                //尝试打开文件，不存在创建
                FileStream fs = new FileStream(@"C:\FeiRanLog\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.OpenOrCreate);
                fs.Close();
                //写入,true是在文件流后追加
                StreamWriter sw = new StreamWriter(@"C:\FeiRanLog\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", true);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + num);
                sw.Close();
            }
            catch (Exception )
            {
                //MessageBox.Show("写入日志文件失败，错误:" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// 清理30天前的日志文件
        /// </summary>
        /// <param name="i"></param>
        public static void log()
        {
            try
            {
                // 判断文件路径是否存在，不存在则创建文件夹
                if (!Directory.Exists(@"C:\FeiRanLog\"))
                {
                    // 不存在就创建目录
                    Directory.CreateDirectory(@"C:\FeiRanLog\");
                }
                List<string> names = Directory.EnumerateFiles(@"C:\FeiRanLog\").ToList();
                foreach (string name in names)
                {
                    //判断文件是否是30天前
                    if (DateTime.Parse(name.Substring(name.Length - 14, name.Substring(name.Length - 14).Length - 4)) < DateTime.Now.AddDays(-30))
                    {
                        //删除文件
                        File.Delete(@name);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
        /// <summary>
        /// 获取CMO口
        /// </summary>
        /// <returns></returns>
        public static string[] ActivePorts()
        {
            ArrayList activePorts = new ArrayList();
            foreach (string pname in SerialPort.GetPortNames())
            {
                activePorts.Add(Convert.ToInt32(pname.Substring(3)));
            }
            activePorts.Sort();
            string[] mystr = new string[activePorts.Count];
            int i = 0;
            foreach (int num in activePorts)
            {
                mystr[i++] = "COM" + num.ToString();
            }
            return mystr;
        }
        #region 串口
        /// <summary>
        /// 控制继电器
        /// </summary>
        /// <param name="order">继电器控制指令</param>
        /// <param name="read">是否接受返回值</param>
        public static byte[] rod(byte[] order, bool read = false)
        {
            byte[] read_bytes = null;
            SerialPort port = new SerialPort();
            //初始化串口属性
            port.BaudRate = 9600;//波特率
            port.Parity = Parity.None;//校验法：无
            port.DataBits = 8;//数据位：8
            port.StopBits = StopBits.One;//停止位：1
            try
            {
                port.PortName = sql("SELECT com FROM t_device_com").Tables[0].Rows[0][0].ToString();
                port.Open();//打开串口
                port.Write(order, 0, order.Length);//发送数据
                Thread.Sleep(500);
                if (read)
                {
                    if (port.ReadBufferSize != 0)
                    {
                        read_bytes = new byte[1024];
                        //port.Read(read_bytes, 0, port.ReadBufferSize);
                        port.Read(read_bytes, 0, 8);
                    }
                }
                port.Close();//关闭串口
            }
            catch (Exception )
            {
            }
            return read_bytes;
        }
        /// <summary>
        /// 控制继电器开关0全部、1-4控制单个
        /// </summary>
        /// <param name="rod_no">开关编号</param>
        /// <param name="on_off">true开/false关</param>
        public static void rod(int rod_no = 0, bool on_off = true)
        {
            if (on_off)
            {
                //开
                switch (rod_no)
                {
                    case 0:
                        rod(new byte[] { 0xFE, 0x0F, 0x00, 0x00, 0x00, 0x04, 0x01, 0xFF, 0x31, 0xD2 }, false);
                        break;
                    case 1:
                        rod(new byte[] { 0xFE, 0x05, 0x00, 0x00, 0xFF, 0x00, 0x98, 0x35 }, false);
                        break;
                    case 2:
                        rod(new byte[] { 0xFE, 0x05, 0x00, 0x01, 0xFF, 0x00, 0xC9, 0xF5 }, false);
                        break;
                    case 3:
                        rod(new byte[] { 0xFE, 0x05, 0x00, 0x02, 0xFF, 0x00, 0x39, 0xF5 }, false);
                        break;
                    case 4:
                        rod(new byte[] { 0xFE, 0x05, 0x00, 0x03, 0xFF, 0x00, 0x68, 0x35 }, false);
                        break;
                }
            }
            //package.Rodr(new byte[] { 0xFE, 0x05, 0x00, 0x01, 0xFF, 0x00, 0xC9, 0xF5 });
            //Thread.Sleep(1000);

            //package.Rodr(new byte[] { 0xFE, 0x05, 0x00, 0x01, 0x00, 0x00, 0x88, 0x05 });
            //package.Rodr(new byte[] { 0xFE, 0x05, 0x00, 0x00, 0xFF, 0x00, 0x98, 0x35 });
            //Thread.Sleep(1000);
            //package.Rodr(new byte[] { 0xFE, 0x05, 0x00, 0x00, 0x00, 0x00, 0xD9, 0xC5 });
            else
            {
                //关
                switch (rod_no)
                {
                    case 0:
                        rod(new byte[] { 0xFE, 0x0F, 0x00, 0x00, 0x00, 0x04, 0x01, 0x00, 0x71, 0x92 }, false);
                        break;
                    case 1:
                        rod(new byte[] { 0xFE, 0x05, 0x00, 0x00, 0x00, 0x00, 0xD9, 0xC5 }, false);
                        break;
                    case 2:
                        rod(new byte[] { 0xFE, 0x05, 0x00, 0x01, 0x00, 0x00, 0x88, 0x05 }, false);
                        break;
                    case 3:
                        rod(new byte[] { 0xFE, 0x05, 0x00, 0x02, 0x00, 0x00, 0x78, 0x05 }, false);
                        break;
                    case 4:
                        rod(new byte[] { 0xFE, 0x05, 0x00, 0x03, 0x00, 0x00, 0x29, 0xC5 }, false);
                        break;
                }
            }
        }
        #endregion
      










        
    }
}
