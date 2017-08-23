using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bolt
{    
    class DiskRipper : IThread
    {
        public void Run(ConcurrentQueue<string> queue)
        {
            if(!IsDiskInTray())
            {
                OpenDiskTray();
                Logger.Info("Waiting for new disk");
                //Wait for disk in tray to be true
                Thread.Sleep(1000);
                while (!IsDiskInTray())
                {
                    Thread.Sleep(100);
                }
            }
            while(true)
            {
                //rip disk
                Logger.Info($"DiskRipper: Processing disk {GetDiskName()}");
                Directory.CreateDirectory($@"{Settings.OutputDirectory}{GetDiskName()}");
                Process.Start(Settings.MakeMKV_Location, $@"mkv disc:0 all --cache=1024 --minlength={Settings.MakeMKV_Min_Seconds} {Settings.OutputDirectory}{GetDiskName()}").WaitForExit();

                //Add file to a queue
                Logger.Info("Added disk to queue");
                queue.Enqueue($@"{Settings.OutputDirectory}{GetDiskName()}");

                //Open the tray
                OpenDiskTray();

                Logger.Info("Waiting for new disk");
                //Wait for disk in tray to be true
                Thread.Sleep(1000);
                while (!IsDiskInTray())
                {
                    Thread.Sleep(100);
                }
            }
        }

        //Source: https://stackoverflow.com/questions/3797141/programmatically-opening-the-cd-tray
        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        private static extern int mciSendStringA(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        private void OpenDiskTray()
        {
            string returnString = "";
            mciSendStringA("set cdaudio  door open", returnString, 0, 0);
            Logger.Info("Opening Disk Tray");
        }

        private void CloseDiskTray()
        {
            string returnString = "";
            mciSendStringA("set cdaudio  door closed", returnString, 0, 0);
            Logger.Info("Closing Disk Tray");
        }

        private bool IsDiskInTray()
        {
            //Source: https://stackoverflow.com/questions/11420365/detecting-if-disc-is-in-dvd-drive
            var drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.CDRom);
            return drives.ElementAt(0).IsReady;            
        }

        private string GetDiskName()
        {
            var drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.CDRom);
            return drives.ElementAt(0).VolumeLabel;
        }

        public void Setup()
        { 

        }

        public void TakeDown()
        {
            
        }
    }
}
