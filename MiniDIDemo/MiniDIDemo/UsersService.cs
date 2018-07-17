using System;

namespace MiniDIDemo
{
    public class UsersService : IUser
    {
        public ICar _carService;
        public UsersService(ICar carInterface)
        {
            _carService = carInterface;
        }


        public void MyName(string name)
        {
            Console.WriteLine("我的名字是："+ name);
        }


        public void MyCarName()
        {
            Console.WriteLine("我的汽车是：");
            _carService.ShowName();
        }

    }
}
