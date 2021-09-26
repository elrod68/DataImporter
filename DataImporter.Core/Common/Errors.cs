using System;
using NLog;

namespace DataImporter.Core.Common
{
    public static class ErrorsAndLog
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //display s in console. if addDateTime=true (default) precede with date/time
        public static void ConsoleWriteLine(string l, 
                                            Boolean addDateTime = true)
        {

            string s;
            if (addDateTime) s = string.Format("{0}:{1}", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), l);
            else s = string.Format("{0}", l);
            
            LogMessage(s);
        }

        public static void HandleGenericError(Exception ex)
        {
            try
            {
                logger.Error(ex.Message+Environment.NewLine+ex.StackTrace,"Error Generated");
            }
            catch (Exception)
            {
                logger.Error(ex.Message, "Error Generated");
            }
        }

        public static void LogMessage(string msg)
        {
            try
            {
                logger.Log(LogLevel.Info,msg);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "Error Generated");
            }
        }
    }
}