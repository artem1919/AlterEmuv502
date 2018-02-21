﻿using System;
using System.IO;
using System.Diagnostics;

namespace Authorization
{
    public class Log
    {
        private object logLock = null;
        public Log()
        {
            logLock = new object();
        }

        public void WriteLog(ConsoleColor colorClass, ConsoleColor colorMessage, string message)
        {
            DateTime logTime = DateTime.Now;
            StackFrame stackFrame = new StackTrace().GetFrame(2);
            lock (logLock)
            {
                Console.Write(logTime.ToString("[dd/MM/yy HH:mm:ss:fff]") + " [");
                Console.ForegroundColor = colorClass;
                Console.Write(stackFrame.GetMethod().ReflectedType.Name + "." + stackFrame.GetMethod().Name);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("] » ");
                Console.ForegroundColor = colorMessage;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public void WriteLine(string message)
        {
            this.WriteLog(ConsoleColor.Green, ConsoleColor.DarkGreen, message);
        }

        public void WriteError(string message)
        {
            this.WriteLog(ConsoleColor.Red, ConsoleColor.DarkRed, message);
        }

        public void WriteDebug(string message)
        {
            this.WriteLog(ConsoleColor.Yellow, ConsoleColor.DarkYellow, message);
        }

        public void WriteDev(string message)
        {
            this.WriteLog(ConsoleColor.Cyan, ConsoleColor.DarkCyan, message);
        }

        private static Log instance;
        public static Log Instance { get { if (instance == null) { instance = new Log(); } return instance; } }
    }
}