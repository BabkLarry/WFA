using Package;
using Package.VZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Weighing_Management;
using 车牌识别2;
using static Package.package;

namespace WFA
{
    public partial class Form1 : MyForm
    {

        private const int MSG_PLATE_INFO = 0x901;
        Pounde bigpounde = new Pounde();
        int chepaijiancezhuangtai = 0; 


        public Form1()
        {
            InitializeComponent();
            startHCHome();
            genggaiXianshi(0);

            setWindoDate();
            comboBox1.SelectedIndex = 0;

        }


        public void Play(VZ vZ)
        {
            new Thread(() =>
            {
                Thread.Sleep(100);

                new Weighing_Management.Monitor(vZ).ShowDialog();
            }).Start();
        }

        List<VZ> vZs = new List<VZ>();

        public void startHCHome()
        {
            DataTable table = package.sql("select * from t_device_ip where ext1='1'").Tables[0];
            foreach (DataRow dr in table.Rows)
            {
                if (dr["state"].ToString().Equals("1"))
                {
                    VZ vZ = new VZ(dr["ip"].ToString(), dr["port"].ToString(), dr["user"].ToString(), dr["pwd"].ToString());
                    vZ.Open();
                    vZ.Call(this.Handle);
                    Play(vZ);
                    vZs.Add(vZ);
                }
            }
        }

        public void genggaiXianshi(int zhuangtai)
        {
            switch (zhuangtai)
            {
                case 0:
                    label2.Text = "空闲";
                    //groupBox2.Visible = false;
                    chepaijiancezhuangtai = 0;
                    break;
                case 1:
                    label2.Text = "过磅中";
                    groupBox2.Visible = true;
                    chepaijiancezhuangtai = 1;
                    break;
            }
        }

        protected override void DefWndProc(ref Message m)
        {
            IntPtr intptr;
            VzClientSDK.VZ_LPR_MSG_PLATE_INFO plateInfo;
            VzClientSDK.VZ_LPR_DEVICE_INFO deviceInfo;
            int handle = 0;
            Console.WriteLine(m.Msg);
            switch (m.Msg)
            {
                case MSG_PLATE_INFO:
                    intptr = (IntPtr)m.WParam.ToInt32();
                    handle = m.LParam.ToInt32();
                    if (intptr != null)
                    {
                        // VzClientSDK.VzLPRClient_SetIOOutputAuto(m_hLPRClient, 0, 500);
                        //根据句柄获取设备IP
                        byte[] strDecIP = new byte[32];
                        int max_count = 32;
                        int ret = VzClientSDK.VzLPRClient_GetDeviceIP(handle, ref strDecIP[0], max_count);
                        string strIP = System.Text.Encoding.Default.GetString(strDecIP);
                        strIP = strIP.TrimEnd('\0');

                        plateInfo = (VzClientSDK.VZ_LPR_MSG_PLATE_INFO)Marshal.PtrToStructure(intptr, typeof(VzClientSDK.VZ_LPR_MSG_PLATE_INFO));

                        label1.Text = plateInfo.plate;
                        //识别车牌号


                        // 判断是否在识别 如果已在识别 则 跳过
                        if (chepaijiancezhuangtai != 0)
                        {
                            return;
                        }

                        genggaiXianshi(1);
                        //查询车辆信息 
                        var stringsql = $"SELECT * FROM t_balance WHERE car_no='{plateInfo.plate}' AND balance_status = '1'";
                        int numberChenge = 0;

                        // picture/202211/202211030000011.jpg  ./picture/202211/202211030000012

                        //// 显示图片
                        //if (plateInfo.img_path != "")
                        //{
                        //    pictureBox5.Image = Image.FromFile(plateInfo.img_path);
                        //}

                        while (numberChenge <= 50)
                        {
                            label18.Text = numberChenge.ToString();
                             Console.WriteLine($"第{numberChenge}次搜索@!!!!!!!!!!!!");

                            numberChenge++;
                            Thread.Sleep(500);
                            try
                            {
                                DataRow carData = package.sql(stringsql, sql_connection.data).Tables[0].Rows[0];
                                // 判断是否第一次第二次 时间
                                //拉取最近榜单号  判断过磅时间 < 5分钟 则为最新
                                DateTime guobangshijian = new DateTime(1998, 9, 7);
                                var isFirst = true;
                                if (!carData["balance_leather"].ToString().Equals("")) //判断是否第二次过磅
                                {
                                    guobangshijian = Convert.ToDateTime(carData["balance_leather"].ToString());
                                    isFirst = false;
                                } 
                                else if (!carData["balance_total"].ToString().Equals("")) //第一次过磅时间
                                {
                                    guobangshijian = Convert.ToDateTime(carData["balance_total"].ToString());
                                }

                                if (guobangshijian == null || (DateTime.Now - guobangshijian).Minutes >= 3) // 判断为空 || 时间间隔> 5分钟 进如下一次循环
                                {
                                    continue;
                                }



                                //榜单号存在 显示到面板
                                //查询旧单号 放置到新单号中

                                Pounde pounde = new Pounde() {
                                    tbaid = carData["balance_id"].ToString(),
                                    total_img = carData["picture_total"].ToString(),
                                    leather_img = carData["picture_leather"].ToString(),
                                    goods = carData["goods_name"].ToString(),
                                    p_car = Car.SeleceCar(carData["car_no"].ToString())
                                };


                                if (!carData["total_weight"].ToString().Equals(""))
                                {
                                    pounde.total = Convert.ToDouble(carData["total_weight"].ToString());
                                }

                                if (!carData["leather_weight"].ToString().Equals(""))
                                {
                                    pounde.leather = Convert.ToDouble(carData["leather_weight"].ToString());
                                }

                                if (!carData["balance_total"].ToString().Equals(""))
                                {
                                    pounde.total_time = Convert.ToDateTime(carData["balance_total"].ToString());
                                }

                                if (!carData["balance_leather"].ToString().Equals(""))
                                {
                                    pounde.leather_time = Convert.ToDateTime(carData["balance_leather"].ToString());
                                }
                                bigpounde = pounde;


                                setWindoDate();

                                break;

                            }
                            catch //报错直接进入下一次循环  可能性 未查询到榜单号
                            {
                                continue;
                            }
                        }
                        Marshal.FreeHGlobal(intptr);
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        public string getImagePiter(string dateImage)
        {
            dateImage = System.IO.Directory.GetCurrentDirectory() + dateImage.Replace(".", "/Image") + ".jpg";
            return dateImage;
        }


        public void setWindoDate()
        {
            label1.Text = bigpounde.p_car.car_no;
            label13.Text = bigpounde.goods;
            label11.Text = bigpounde.total.ToString();
            label9.Text = bigpounde.leather.ToString();

            if(bigpounde.total_time.Equals(new DateTime()))
            {
                label5.Text = "暂无";
            }
            else
            {
                label5.Text = bigpounde.total_time.ToString();
            }

            if (bigpounde.leather_time.Equals(new DateTime()))
            {
                label7.Text = "暂无";
            }
            else
            {
                label7.Text = bigpounde.leather_time.ToString();
            }

            if (!bigpounde.total_img.Equals(""))
            {
                pictureBox1.Image = Image.FromFile(getImagePiter(bigpounde.total_img));

            }
            if (!bigpounde.leather_img.Equals(""))
            {
                pictureBox5.Image = Image.FromFile(getImagePiter(bigpounde.leather_img));

            }
            if (!bigpounde.p_car.driverslicense_img.Equals(""))
            {
                pictureBox2.Image = Image.FromFile(getImagePiter(bigpounde.p_car.driverslicense_img));

            }
            

            textBox1.Text = bigpounde.p_car.code;
            textBox2.Text = bigpounde.p_car.engine_number;

            int SIndex = 0;

            if (!bigpounde.p_car.emission_standard.Equals(""))
            {
                if (bigpounde.p_car.emission_standard.Equals("D"))
                {
                    SIndex = 7;
                }
                else
                {
                    SIndex = Convert.ToInt32(bigpounde.p_car.emission_standard);
                }
            }

            if(!bigpounde.p_car.registration.Equals(new DateTime()))
            {
                dateTimePicker1.Value = bigpounde.p_car.registration;

            }




            comboBox1.SelectedIndex = SIndex;


        }

        public Pounde getWebPounde()
        {
            Pounde pounde = new Pounde();

            pounde.p_car.car_no = label1.Text;
            pounde.goods = label13.Text;
            pounde.total = Convert.ToDouble(label11.Text);
            pounde.leather = Convert.ToDouble(label9.Text);

            if (!label5.Text.Equals("暂无"))
            {
                pounde.total_time = Convert.ToDateTime(label5.Text);
            }

            if (!label7.Text.Equals("暂无"))
            {
                pounde.leather_time = Convert.ToDateTime(label7.Text);
            }

            pounde.total_img = bigpounde.total_img;
            pounde.leather_img = bigpounde.leather_img;

            pounde.p_car.registration = dateTimePicker1.Value;
            pounde.p_car.code = textBox1.Text;
            pounde.p_car.engine_number = textBox2.Text;


            string emission_standard = "";
            if(comboBox1.SelectedIndex == 7)
            {
                emission_standard = "D";
            }
            else
            {
                emission_standard = comboBox1.SelectedIndex.ToString();
            }
            pounde.p_car.emission_standard = emission_standard;
            return pounde;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 保存

            //先查询是否存在 


            Pounde  yangPounde =  Pounde.selecePounde(bigpounde.tbaid);

            Pounde savePounde = getWebPounde();
            savePounde.tbaid = bigpounde.tbaid;


            Car oldCar = Car.SeleceCar(savePounde.p_car.car_no);
            // 新车
            if (oldCar.Id.Equals(""))
            {
                //保存新的
                if (MessageBox.Show("车辆是否存储?", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Car.insterCar(savePounde.p_car);
                };

            }
            else if (!Car.equesCar(savePounde.p_car, oldCar))
            {
                //不相等
                if (MessageBox.Show("是否更新车辆数据?", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Car.updateCar(savePounde.p_car);
                }
            }


            if (yangPounde.Id.Equals(""))
            {
                // 新磅单号 
                Pounde.insterPounde(savePounde);
            }
            else
            {
                // 旧磅单号
                Pounde.updatePounde(savePounde);
            }
            MessageBox.Show("保存成功@!!!");


            bigpounde = new Pounde();
            setWindoDate();
            genggaiXianshi(0);
        }

     
    }



    // 单号
    public class Pounde
    {

        public string tbaid { get; set; } = "";                 //上级订单号

        public string Odd { get; set; } = "";                   //订单号

        public string Id { get; set; } = "";                    //id

        public DateTime total_time { get; set; }                //皮重时间

        public DateTime leather_time { get; set; }              //毛重时间

        public string total_img { get; set; } = "";             //皮重图片

        public string leather_img { get; set; } = "";           //毛重图片

        public string goods { get; set; } = "";                 //货物名称

        public double total { get; set; } = 0;                 //毛重

        public double leather { get; set; } = 0;               //皮重

        public Car p_car { get; set; } = new Car();                   //驾驶证信息



        public static Pounde selecePounde(string tbaid)
        {
            try
            {
                var stringsql = $"SELECT * FROM t_pounde WHERE tbaid='{tbaid}' AND state = '1'";
                DataRow poundeRow = package.sql(stringsql,sql_connection.data).Tables[0].Rows[0];

                Pounde pounde = new Pounde()
                {
                    Id = poundeRow["id"].ToString(),
                    tbaid = poundeRow["tbaid"].ToString(),
                    Odd = poundeRow["Odd"].ToString(),
                    total_img = poundeRow["total_img"].ToString(),
                    leather_img = poundeRow["leather_img"].ToString(),
                    goods = poundeRow["goods"].ToString(),
                    total = Convert.ToDouble(poundeRow["total"].ToString()),
                    leather = Convert.ToDouble(poundeRow["leather"].ToString())
                    
                };
                pounde.p_car =  Car.SeleceCar(poundeRow["car_on"].ToString());

                if (!poundeRow["leather_time"].ToString().Equals(""))
                {
                    pounde.leather_time = Convert.ToDateTime(poundeRow["leather_time"].ToString());
                }

                if (!poundeRow["total_time"].ToString().Equals(""))
                {
                    pounde.total_time = Convert.ToDateTime(poundeRow["total_time"].ToString());
                }

                return pounde;
            }
            catch
            {
                return new Pounde();
            }
        }

        public static void insterPounde(Pounde pounde)
        {
            string sqlString = "INSERT INTO `balance_ceshi`.`t_pounde` ( `car_on`, `total_time`, `leather_time`, `total_img`, `leather_img`, `accompanying_img`, `driverslicense_img`, `goods`, `total`, `leather`, `car_registration`, `car_code`, `car_engine_number`, `car_emission_standard`, `tbaid`,`state`)" +
                " VALUES" +
                $" ( '{pounde.p_car.car_no}', '{pounde.total_time}', '{pounde.leather_time}', '{pounde.total_img}', '{pounde.leather_img}', '{pounde.p_car.accompanying_img}', '{pounde.p_car.driverslicense_img}', '{pounde.goods}', '{pounde.total}', '{pounde.leather}', '{pounde.p_car.registration}', '{pounde.p_car.code}', '{pounde.p_car.engine_number}', '{pounde.p_car.emission_standard}',  '{pounde.tbaid}', '1')";

            package.sql(sqlString,sql_connection.data);
        }

        public static void updatePounde(Pounde pounde)
        {
            string sqlstring = $"UPDATE `balance_ceshi`.`t_pounde` SET `car_on` = '{pounde.p_car.car_no}', `total_time` = '{pounde.total_time}', `leather_time` = '{pounde.leather_time}', `total_img` = '{pounde.total_img}', `leather_img` = '{pounde.leather_img}', `accompanying_img` = '{pounde.p_car.accompanying_img}', `driverslicense_img` = '{pounde.p_car.driverslicense_img}', `goods` = '{pounde.goods}', `total` = '{pounde.total}', `leather` = '{pounde.leather}', `car_registration` = '{pounde.p_car.registration}', `car_code` = '{pounde.p_car.code}', `car_engine_number` = '{pounde.p_car.engine_number}', `car_emission_standard` = '{pounde.p_car.emission_standard}' WHERE `tbaid` = '{pounde.tbaid}'";

            package.sql(sqlstring, sql_connection.data);

        }


    }


    public class Car
    {

        public string Id { get; set; } = "";                    //id

        public string car_no { get; set; } = "";                //车牌号

        public DateTime registration { get; set; }              //注册时间

        public string code { get; set; } = "";                  //车牌识别代号

        public string engine_number { get; set; } = "";         //发动机号码

        public string emission_standard { get; set; } = "";     //排放标准  0:国0  1:国1  2:国2  3:国3  4:国4  5:国5  6:国6  D:电动

        public string driverslicense_img { get; set; } = "";    //驾驶证图片

        public string accompanying_img { get; set; } = "";      //随货清单图片

        public static void insterCar(Car car)
        {
            string sqlString = "INSERT INTO `balance_ceshi`.`t_card_date` ( `cao_no`, `registration`, `code`, `engine_number`, `emission_standard`, `driverslicense_img`, `state`) " +
                $"VALUES ( '{car.car_no}', '{car.registration}', '{car.code}', '{car.engine_number}', '{car.emission_standard}', '{car.driverslicense_img}', '1');";

            package.sql(sqlString, sql_connection.data);
        }

        public static bool equesCar(Car car,Car otherCar)
        {
            if (car.car_no.Equals(otherCar.car_no) && car.registration == otherCar.registration && car.code.Equals(otherCar.code) && car.engine_number.Equals(otherCar.engine_number) && car.emission_standard.Equals(otherCar.emission_standard) && car.driverslicense_img.Equals(otherCar.driverslicense_img) && car.accompanying_img.Equals(otherCar.accompanying_img))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Car SeleceCar(string car_no)
        {
            var sqlstring = $"SELECT * FROM t_card_date WHERE cao_no = '{car_no}' AND state = '1'";

            try
            {
                DataRow poundeRow = package.sql(sqlstring,sql_connection.data).Tables[0].Rows[0];
                Car car = new Car()
                {
                    Id = poundeRow["id"].ToString(),

                    car_no = car_no,

                    registration = Convert.ToDateTime(poundeRow["registration"].ToString()),

                    code = poundeRow["code"].ToString(),

                    engine_number = poundeRow["engine_number"].ToString(),

                    emission_standard = poundeRow["emission_standard"].ToString(),

                    driverslicense_img = poundeRow["driverslicense_img"].ToString(),
                    accompanying_img = poundeRow["accompanying_img"].ToString(),

                };
                return car;
            }
            catch
            {
                return new Car() {
                    car_no = car_no
                };
            }


        }

        public static void updateCar(Car car)
        {
            string sqlString = $"UPDATE `balance_ceshi`.`t_card_date` SET  `registration` = '{car.registration}', `code` = '{car.code}', `engine_number` = '{car.engine_number}', `emission_standard` = '{car.emission_standard}', `driverslicense_img` = '{car.driverslicense_img}' WHERE cao_no = '{car.car_no}';";
            package.sql(sqlString, sql_connection.data);
        }



    }












}
