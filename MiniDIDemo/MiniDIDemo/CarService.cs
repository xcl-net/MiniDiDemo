using System;

namespace MiniDIDemo
{
    public class CarService : ICar
    {
        public void ShowName()
        {
            Console.WriteLine("德国奥迪");
        }
    }
}
