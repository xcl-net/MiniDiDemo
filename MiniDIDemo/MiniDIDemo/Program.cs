using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniDIDemo
{
    /// <summary>
    /// 本项目演示DI注入原理
    /// </summary>
    class Program
    {
        
        //public static IUser _user; //不用di容器调用，会出现未将对象引用设置到实例的异常，容器就是为了初始化实例；
        public static ICar _car;
        //public Program(IUser user)
        //{
        //    _user = user;
        //}

        public Program(ICar car)
        {
            _car = car;
        }
        static void Main(string[] args)
        {
            //1. 创建容器；
            SampleDI container = new SampleDI();
            //2. 注册服务；
            container.Register<IUser, UsersService>();
            container.Register<ICar, CarService>();

            //3. 从容器中获取对象；

            UsersService user = container.GetService<UsersService>();
            user.MyName("peter");
            user.MyCarName();


            Console.WriteLine("接口调用完毕！");
            Console.ReadKey();
        }
    }
}
