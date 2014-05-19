using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Redis;

namespace Redis.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmd = new RedisTest();
            Console.WriteLine("运行中……");
            cmd.Run();
            Console.WriteLine("运行完成！");
            Console.Read();


            //var currentKey = "urn:shippers:current";
            //var prospectiveKey = "urn:shippers:prospective";
            //var currentShippers = new List<Shipper>();
            //var prospectiveShippers = new List<Shipper>();

            //var lameShipper = new Shipper
            //{
            //    Id = 3,
            //    CompanyName = "We do everything!",
            //    DateCreated = DateTime.UtcNow,
            //    ShipperType = ShipperType.All,
            //    UniqueRef = Guid.NewGuid()
            //};
            //for (int i = 0; i < 5; i++)
            //{
            //    lameShipper.Id++;
            //    currentShippers.Add(lameShipper);
            //}
            //using (var help = new RedisHelper())
            //{
            //    help.Add("currentKey", currentShippers);
            //}

        }
    }
}
