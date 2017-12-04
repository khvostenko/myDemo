using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Logger.Implementation
{
    class JSONLogger:BaseLogger
    {
        protected override void WriteLog(string message, string pathToDirectory)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(string));
            if (!Directory.Exists(pathToDirectory))
            {
                var directory = Directory.CreateDirectory(pathToDirectory);
            }
            using (FileStream fs = new FileStream(pathToDirectory + '/' + "message.json", FileMode.OpenOrCreate)) 
            {
                jsonFormatter.WriteObject(fs, message);
            }
        }
    }
}
