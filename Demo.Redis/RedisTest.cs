using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.Services.Models;

namespace Demo.Redis
{
    public class RedisTest
    {
        public void Run()
        {
            var key = "lst:Shipper";
            using (var client = RedisManager.GetClient())
            {
                var user = client.As<User>();

                if (user.GetAll().Count > 0)
                    user.DeleteAll();

                var qiujialong = new User
                {
                    Id = user.GetNextSequence(),
                    Name = "qiujialong",
                    Job = new Job { Position = ".NET" }
                };
                var chenxingxing = new User
                {
                    Id = user.GetNextSequence(),
                    Name = "chenxingxing",
                    Job = new Job { Position = ".NET" }
                };
                var luwei = new User
                {
                    Id = user.GetNextSequence(),
                    Name = "luwei",
                    Job = new Job { Position = ".NET" }
                };
                var zhourui = new User
                {
                    Id = user.GetNextSequence(),
                    Name = "zhourui",
                    Job = new Job { Position = "Java" }
                };

                var userToStore = new List<User> { qiujialong, chenxingxing, luwei, zhourui };
                user.StoreAll(userToStore);

                Thread.Sleep(3000);


                string msg = "目前共有：" + user.GetAll().Count.ToString() + "人！"; 
                Console.WriteLine(msg);
            }
        }
    }
}
