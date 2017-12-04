using Logger;
using System;
using System.Collections.Generic;
using System.Text;
using Logger.enums;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Diagnostics;

namespace Logger.Implementation
{
    public class BaseLogger: ILogger
    {
       
        public void Log(string logString, Level logLevel, DateTime dateTime, WriteFormat format)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("App1.config");
            string[] readFromXML = new string[3];
            int i = 0;
            foreach (XmlElement element in xml.GetElementsByTagName("add"))
            {
                if (element.Attributes["key"] == null)
                    continue;
                readFromXML[i] = element.Attributes["value"].Value;
                i++;

                
            }

            Level logg = new Level();

            switch(readFromXML[1])
            {
                case "warning":
                    logg = Level.warning;
                    break;
                case "debug":
                    logg = Level.debug;
                    break;
                case "error":
                    logg = Level.error;
                    break;
                case "info":
                    logg = Level.info;
                    break;
                default:
                    break;
            }

            string message = "";

            switch (logg)
            {
                case Level.warning:
                    message = LogWarn(logString);
                    Console.WriteLine(LogWarn(logString));
                    break;
                case Level.debug:
                    message = LogDebug(logString);
                    Console.WriteLine(LogDebug(logString));
                    break;
                case Level.error:
                    message = LogError(logString);
                    Console.WriteLine(LogError(logString));
                    break;
                case Level.info:
                    message = LogInfo(logString);
                    Console.WriteLine(LogInfo(logString));
                    break;
                default:
                    Console.WriteLine("change config file");
                    break;
            }

            WriteFormat wf = new WriteFormat();
            XMLLogger xmlLogger = new XMLLogger();
            JSONLogger jsonLogger = new JSONLogger();

            switch (readFromXML[2])
            {
                case "plain":
                    wf = WriteFormat.plain;
                    break;
                case "xml":
                    wf = WriteFormat.xml;
                    break;
                case "json":
                    wf = WriteFormat.json;
                    break;
                default:
                    break;


            }


            switch (wf)
            {
                case WriteFormat.json:
                    jsonLogger.WriteLog(message,readFromXML[0]);
                    break;
                case WriteFormat.plain:
                    WriteLog(message, readFromXML[0]);
                    break;
                case WriteFormat.xml:
                    xmlLogger.WriteLog(message, readFromXML[0]);
                    break;
                default:
                    break;
            }


        }



        protected string LogWarn(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Warning!");
            StackTrace stackTrace = new StackTrace(true);

            for (int i = 0; i < stackTrace.FrameCount; i++)
            {

                StackFrame sf = stackTrace.GetFrame(i);
                sb.Append("\n"+sf.GetMethod().ToString());
                sb.Append("\n" + sf.GetFileLineNumber().ToString());
               
            }
            return sb.ToString();
            
        }

        protected string LogError(string message)
        {
            //StackTrace
           
            StringBuilder sb = new StringBuilder();

            sb.Append("Error!");

            StackTrace stackTrace = new StackTrace(true);

            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                
                StackFrame sf = stackTrace.GetFrame(i);
                sb.Append("\n" + sf.GetMethod().ToString());
                sb.Append("\n" + sf.GetFileLineNumber().ToString());
             

            }
            return sb.ToString();
        }

        protected string LogInfo(string message)
        {
            TraceSource traceSource = new TraceSource(message);
            StringBuilder sb = new StringBuilder();
       
            sb.Append("\n" + traceSource.Name);
            sb.Append("\n" + traceSource.Switch.Level);
            sb.Append("\n" + traceSource.Switch.DisplayName);



            return sb.ToString();
        }

        protected string LogDebug(string message)
        {
            
            return "Debug";
        }

        protected virtual void WriteLog(string message, string pathToDirectory)
        {
          
            if (!Directory.Exists(pathToDirectory))
            {
                var directory = Directory.CreateDirectory(pathToDirectory);
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(pathToDirectory + '/' + "plain.txt", false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(message);
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

           
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //free unmanaged resources
            }
        }
    }
}
