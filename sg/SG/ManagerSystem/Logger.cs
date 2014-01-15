using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem
{
    public class Logger
    {
        static managersystemEntities ms_ent = new managersystemEntities();

        public static void Log(string message, string level)
        {
            Log l = new Log();

            l.level = level;
            l.message = message;
            l.timestamp = DateTime.Now;

            ms_ent.Logs.Add(l);
            ms_ent.SaveChanges();
        }

        public static void Info(string message)
        {
            Log(message, "INFO");
        }

        public static void Error(string message)
        {
            Log(message, "ERROR");
        }
    }
}