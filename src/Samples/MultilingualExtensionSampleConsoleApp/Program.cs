using System;
using System.Globalization;
using System.Threading;
using MultilingualExtensionSampleConsoleApp.Resources;
namespace MultilingualExtensionSampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            AppResources.Culture = Thread.CurrentThread.CurrentUICulture;
            Console.WriteLine(AppResources.Hello);
            Console.WriteLine(AppResources.Press_Any_Key_To_Exit);
            Console.WriteLine(AppResources.Read_Instructions);
            Console.ReadKey();
        }
    }
}
