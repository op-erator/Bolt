using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bolt
{
    public delegate void NextProcessDelegate(FileInfo file);
    public class Converter : IThread
    {
        public void Run(ConcurrentQueue<string> queue)
        {
            while(true)
            {
                string folderName;
                if(queue.TryDequeue(out folderName))
                {
                    Logger.Info("Converter: Got folder name");
                    foreach(string file in Directory.GetFiles(folderName, "*.mkv"))
                    {                    
                        string convertedFileName = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + "_conv.mp4";
                        Logger.Info($"Converter: Converting file {file} to {convertedFileName}");
                        Process.Start(Settings.HandBrake_Location, $@"-C 6 -i ""{file}"" -o ""{convertedFileName}""").WaitForExit();
                        if(Settings.HandBrake_DeleteOriginalFile)
                        {
                            string newFileName = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".mp4";
                            Logger.Info($"Converter: Renaming file {convertedFileName} to {newFileName}");
                            File.Move(convertedFileName, newFileName);
                            File.Delete(file);
                        }
                    }                    
                }
                Thread.Sleep(1000);
            }
        }

        public void Setup()
        {
            throw new NotImplementedException();
        }

        public void TakeDown()
        {
            throw new NotImplementedException();
        }
    }
}
