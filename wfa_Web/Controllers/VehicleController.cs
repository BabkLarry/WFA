using Package;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using static Package.package;

namespace wfa_Web.Controllers
{
    public class VehicleController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Car car)
        {
            Console.WriteLine(car.ToString());

            car.accompanying_img = "";

            car.driverslicense_img = "";

            //先查询 在提交
            Car oldCar = Car.SeleceCar(car.car_no);
            if (!oldCar.id.Equals(""))
            {
             return package.toJson("{\"mode\": \"success\"}");
            }

            Car.insterCar(car);

            return  package.toJson("{\"mode\": \"success\"}");

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {

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
                    $"VALUES ( '{car.car_no}', '{car.registration}', '{car.code}', '{car.engine_number}', '{car.emission_standard}', '{car.driverslicense_img}', '0');";

                package.sql(sqlString, sql_connection.data);
            }

            public static bool equesCar(Car car, Car otherCar)
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
                    DataRow poundeRow = package.sql(sqlstring, sql_connection.data).Tables[0].Rows[0];
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
                    return new Car()
                    {
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
}