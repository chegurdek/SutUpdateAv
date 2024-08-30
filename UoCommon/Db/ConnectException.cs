using System;
using System.Collections.Generic;
using System.Text;

namespace Elsy.UoCommon.Db
{
    class ConnectException : Exception
    {
        private string message;
        public ConnectException() 
        {
            message = "Нет подключения к БД";

        }

        public ConnectException(string message) : base(message)
        {
           
        }

        public override string Message
        {
            get { return message; }
        }

     
    }
}
