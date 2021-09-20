using System;
using System.Windows.Forms;

namespace WinverUWP.Broker
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new BrokerApplicationContext());
        }
    }
}
