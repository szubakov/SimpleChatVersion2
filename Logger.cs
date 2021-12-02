using System.IO;
using System;

namespace SimpleChat
{
    public class Logger
    {
        public string filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "errors.log");

        public void Log(string message)
        {
            if(!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.Write(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss  "));
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
        }
    }
}
