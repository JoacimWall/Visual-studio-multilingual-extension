using System;
using MultilingualExtensionSampleConsoleApp.Resources;
namespace MultilingualExtensionSampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(AppResources.Hello);
            Console.WriteLine(AppResources.Press_Any_Key_To_Exit);
            Console.ReadKey();
        }
    }
}
