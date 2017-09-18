///
/// Created by Owen T. Parkins
///

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Bolt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Settings.Version);
            Settings.LoadSettings();
            
            ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
            if (Settings.MakeMKV_Enabled)
            {
                DiskRipper diskRipper = new DiskRipper();
                Thread ripperThread = new Thread(new ThreadStart(() => diskRipper.Run(queue)));
                ripperThread.Start();
            }
                
            if(Settings.HandBrake_Enabled)
            {
                Converter converter = new Converter();
                Thread converterThread = new Thread(new ThreadStart(() => converter.Run(queue)));
                converterThread.Start();
            }            

            //Need to have a basic shell
            while(true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}