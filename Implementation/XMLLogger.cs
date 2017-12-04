using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Logger.Implementation
{
    
    
    class XMLLogger:BaseLogger
    {
       
        protected override void WriteLog(string message, string pathToDirectory)
        {
            
            XmlSerializer formatter = new XmlSerializer(typeof(string));
            if (!Directory.Exists(pathToDirectory))
            {
                var directory = Directory.CreateDirectory(pathToDirectory);
            }

            using (FileStream fs = new FileStream(pathToDirectory+'/'+"message.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, message);

            }
        }
    }
}
