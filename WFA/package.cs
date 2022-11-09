using Microsoft.CSharp;
using MySql.Data.MySqlClient;
using ReaderB;
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
using System.Net.Sockets;
using System.Reflection;
using System.Speech.Synthesis;
using System.Text;
using FastReport;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace Package
{
    public class package
    {
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
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings[((int)sqlconnection).Equals(0) ? "sql_conf" : (((int)sqlconnection).Equals(1) ? "sql_data" : "sql_balance")].ToString());
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
        #region 文字朗读封装
        //文字朗读
        static SpeechSynthesizer speech = new SpeechSynthesizer();
        /// <summary>
        /// read(要朗读的文字,速度,音量)
        /// </summary>
        /// <param name="read"></param>
        /// <param name="rate"></param>
        /// <param name="volume"></param>
        public static void read(string read, int rate, int volume, bool yibu = true)
        {
            try
            {
                speech.Rate = rate;//速度
                speech.Volume = volume;//音量,0-100
                speech.SpeakAsyncCancelAll();
                if (yibu)
                {
                    speech.SpeakAsync(read);//异步播放，但是要等到前面的发音完成后才会播放该发音
                }
                else
                {
                    speech.Speak(read);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region 动态代码
        /// <summary>
        /// 动态代码
        /// </summary>
        /// <param name="path">路径\文件名</param>
        /// <param name="num">参数</param>
        /// <returns></returns>
        public static object code(string path, object num)
        {
            object ret = null;
            //获取源码文件
            StreamReader sr = new StreamReader(path);
            string cs_num = sr.ReadToEnd();
            //添加dll引用
            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.AddRange(cs_num.Replace(@"//·", "`").Split('`')[1].Split('·'));
            //不生成exe
            objCompilerParameters.GenerateExecutable = false;
            //生成输出
            objCompilerParameters.GenerateInMemory = true;
            //代码
            CompilerResults cr = new CSharpCodeProvider().CreateCompiler().CompileAssemblyFromSource(objCompilerParameters, cs_num.Replace(@"//·", "`").Split('`')[2]);

            if (cr.Errors.HasErrors)
            {
                //编译出错
                ret = "Exce";
                foreach (CompilerError error in cr.Errors)
                {
                    ret += "·" + error.ErrorText;
                }
            }
            else
            {
                try
                {
                    // 通过反射，调用函数
                    Assembly objAssembly = cr.CompiledAssembly;
                    object objHelloWorld = objAssembly.CreateInstance("Code.Code");
                    MethodInfo objMI = objHelloWorld.GetType().GetMethod("code");
                    try
                    {
                        List<object> nums = new List<object>();
                        nums.Add(num);

                        ret = objMI.Invoke(objHelloWorld, nums.ToArray());
                    }
                    catch (Exception ex)
                    {
                        //运行错误
                        ret = "Exce·" + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    //调用错误
                    ret = "Exce·" + ex.Message;
                }
            }
            return ret;
        }
        #endregion
        #region 瀚岳IC读卡器
        /// <summary>
        /// 连接瀚岳设备
        /// HY_Open(设备ip, 设备端口)
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static int HY_Open(string ip, string port)
        {
            try
            {
                byte fComAdr = 0xff;
                int id = -1;
                ip = ip.Trim();
                if (StaticClassReaderB.OpenNetPort(Convert.ToInt32(port), ip, ref fComAdr, ref id).Equals(0))
                {
                    log("连接瀚岳设备" + ip + "成功，缓存id:" + id);
                    return id;
                }
                else
                {
                    StaticClassReaderB.CloseNetPort(id);
                    log("连接瀚岳设备" + ip + "失败");
                    return id;
                }
            }
            catch (Exception ex)
            {
                log("连接瀚岳设备" + ip + "失败，错误:" + ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// 读取用户区一次
        /// HY_Read(设备ip)
        /// </summary>
        /// <param name="id"></param>
        public static string HY_Read(int id)
        {
            try
            {
                byte fComAdr = 0xff;
                byte[] EPC = new byte[0];

                int CardNum = 0;
                //返回数据长度
                int Totallen = 0;
                byte[] EPCs = new byte[5000];
                StaticClassReaderB.Inventory_G2(ref fComAdr, 0, 0, 0, EPCs, ref Totallen, ref CardNum, id);
                EPC = new byte[Totallen - 1];
                Array.Copy(EPCs, 1, EPC, 0, 12);
                byte[] CardData = new byte[320];
                byte[] pwd = { 0x00, 0x00, 0x00, 0x00 };
                //执行编号
                int ferrorcode = -1;
                //读取长度
                byte lang = 0x05;
                if (StaticClassReaderB.ReadCard_G2(ref fComAdr, EPC, 2, 0, lang, pwd, 0, 0, 0, CardData, 12, ref ferrorcode, id) == 0)
                {
                    byte[] daw = new byte[lang * 2];
                    Array.Copy(CardData, daw, lang * 2);
                    StringBuilder sb = new StringBuilder(EPC.Length * 3);
                    foreach (byte b in daw)
                        sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                    return sb.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        public void HY_Write(int id, string card_no)
        {
            try
            {
                byte fComAdr = 0xff;
                byte[] EPC = new byte[0];

                int CardNum = 0;
                //返回数据长度
                int Totallen = 0;
                byte[] EPCs = new byte[5000];
               var returnNumber =  StaticClassReaderB.Inventory_G2(ref fComAdr, 0, 0, 0, EPCs, ref Totallen, ref CardNum, id);
                EPC = new byte[Totallen - 1];
                Array.Copy(EPCs, 1, EPC, 0, 12);

                byte[] CardData = new byte[320];
                byte[] pwd = { 0x00, 0x00, 0x00, 0x00 };
                //执行编号
                int ferrorcode = -1;
                //写入数据
                byte[] buffer = new byte[card_no.Length / 2];
                for (int i = 0; i < card_no.Length; i += 2)
                    buffer[i / 2] = (byte)Convert.ToByte(card_no.Substring(i, 2), 16);
                StaticClassReaderB.WriteCard_G2(ref fComAdr, EPC, 3/*0保留区，1EPC，2TCD区，3用户区*/, 0, 10, buffer, pwd, 0, 0, 0, 0, 12, ref ferrorcode, id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        /// <summary>
        /// 断开瀚岳设备
        /// HY_Close(设备ip, 设备端口)
        /// </summary>
        /// <param name="id"></param>
        public static bool HY_Close(int id)
        {

            if (id < 0)
            {
                return true;
            }

            try
            {
                
                StaticClassReaderB.CloseNetPort(id);
                log("断开瀚岳设备，缓存id:" + id + "成功");
                return true;
            }
            catch (Exception ex)
            {
                log("断开瀚岳设备，缓存id:" + id + "失败，错误:" + ex.Message);
                return false;
            }
        }
        #endregion
        #region 同步数据
        // 缓冲器
        static byte[] result = new byte[1024 * 1024];
        // IP终端
        static IPEndPoint ipEndPoint = null;
        // 客户端Socket 
        static Socket ClientSocket = null;
        //获取数据长度 
        static int receiveLength = 0;
        //获取服务器发来的数据 
        static string strData = "";
        static string company_id = package.sql("SELECT company_id FROM t_system").Tables[0].Rows[0][0].ToString();
        static string dataname = sql("SELECT data_name FROM t_system where company_id='"+company_id+"'").Tables[0].Rows[0][0].ToString();
        //sql语句
        static string sql_delete = "delete from t_balance where company_id = '" + company_id + "' and balance_id in ";
        

        public static bool socket_tcp()
        {
            //公司id
            //拼接语句
            DataSet balance = sql("SELECT * FROM t_balance WHERE upload in ('-2','1')");
            sql_delete += "(";
            foreach (DataRow row in balance.Tables[0].Rows)
            {
                sql_delete += "'" + row["balance_id"].ToString() + "',";
            }
            //去掉末端"，"
            sql_delete = sql_delete.Substring(0, sql_delete.Length - 1) + ")";
            
            //发送指令
            try
            {
                new Thread(() =>
                {
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            ipEndPoint = new IPEndPoint(IPAddress.Parse(getxml("system.xml", "Product/server", "ip")), int.Parse(getxml("system.xml", "Product/server", "port")));//初始化IP终端 
                            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//初始化服务端Socket 
                            ClientSocket.Connect(ipEndPoint);//端口绑定
                            ClientSocket.Send(Encoding.UTF8.GetBytes(company_id + @"·1·0·" + sql_delete + "$"));
                            //获取数据长度 
                            receiveLength = ClientSocket.Receive(result);
                            //获取服务器发来的数据 
                            strData = Encoding.UTF8.GetString(result, 0, receiveLength);
                            if (strData.Equals("") || receiveLength == 0 || strData.Split('$')[0].Split('·')[0].Equals("-1"))
                            {
                                //接收到空数据结束接收
                                return;
                            }
                            //if (strData.Split('$')[0].Split('·')[0].Equals("0"))
                            //{
                            //    strData = "";

                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return;
                        }
                    });
                    thread.Start();
                    Thread.Sleep(30000);
                    thread.Abort();
                    ClientSocket.Close();
                }).Start();
                return true;
            }
            catch (Exception)
            {
                try
                {
                    ClientSocket.Close();
                }
                catch (Exception)
                {
                }
                return false;
            }
        }
        public static void socket_tcp_int(string b_id)
        {
            string sql_inset = "INSERT INTO " + dataname + ".t_balance(`id`, `balance_id`, `car_no`, `driver`,`goods_name`,`specification`,`takegoods`,`carcontact`, `total_weight`, `leather_weight`, `actual_weight`,`extra`, `impurity`, `water`, `package_weight`, `sweight`,`supplier`,`receiver`, `pintube`, `order`, `oil`,`price`,`amount`, `customeraddress`,`delivery`,`balance_total`, `balance_leather`, `operator`, `balance_status`,`balance_class`,`picture_total`,`picture_leather`,`upload`,`ext1`, `ext2`, `ext3`, `ext4`) values  ";
            DataSet balance = package.sql("SELECT * FROM t_balance WHERE upload in ('1') and balance_status='1' and balance_id='"+b_id+"'");
            foreach (DataRow row in balance.Tables[0].Rows)
            {
                //首端ID添加时为null
                sql_inset += "(null,";
                for (int i = 1; i < row.ItemArray.Length - 1; i++)
                {
                    sql_inset += (row.ItemArray[i].ToString().Equals("") ? "null" : "'" + row.ItemArray[i].ToString() + "'") + ",";
                }
                //末端根据判断添加ext4和公司id
                sql_inset += "" + (row.ItemArray[row.ItemArray.Length - 1].ToString().Equals("1") ? "'0'" : (row.ItemArray[row.ItemArray.Length - 1].ToString().Equals("'1'") ? "-2" : "null")) + "),";
            }
            //去掉末端"，"
            sql_inset = sql_inset.Substring(0, sql_inset.Length - 1);
            new Thread(() => {
                try
                {
                    // 缓冲器
                    ipEndPoint = new IPEndPoint(IPAddress.Parse(getxml("system.xml", "Product/server", "ip")), int.Parse(getxml("system.xml", "Product/server", "port")));//初始化IP终端 
                    ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//初始化服务端Socket 
                    ClientSocket.Connect(ipEndPoint);//端口绑定
                    ClientSocket.Send(Encoding.UTF8.GetBytes(company_id + "·1·0·" + sql_inset + "$"));
                    //获取数据长度 
                    receiveLength = ClientSocket.Receive(result);
                    //获取服务器发来的数据 
                    strData = Encoding.UTF8.GetString(result, 0, receiveLength);
                    if (strData.Equals("") || receiveLength == 0 || strData.Split('$')[0].Split('·')[0].Equals("-1"))
                    {
                        //接收到空数据结束接收
                        return;
                    }
                    if (strData.Split('$')[0].Split('·')[0].Equals("0"))
                    {
                        sql("UPDATE t_balance SET upload = '0' WHERE upload = '1'");
                        sql("UPDATE t_balance SET upload = '-1' WHERE upload = '-2'");
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }).Start();
        }
        #endregion










        
    }
}
