using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.IO;


namespace SutUpdateAv.Models
{
    public class UserSetting
    {
        string initialCatalog;
        string dataSource;
        string userID;
        string password;
        string provider;


        static string sutConnectionString;
        static string sutUser;
        static string sutServer;
        static string sutDb;
        static string sutProvider;



        public UserSetting(string dataSource, string initialCatalog, string userID, string password, string provider)
        {
            this.dataSource = dataSource;
            this.initialCatalog = initialCatalog;
            this.password = password;
            this.userID = userID;
            this.provider = provider;




            SqlConnectionStringBuilder sqlConnBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = initialCatalog,
                DataSource = dataSource,
                UserID = userID,
                Password = password,
                TrustServerCertificate = true,
              //   MultiSubnetFailover = true,
                 Encrypt = false

            };


            if (provider == "NPGSQL")
            {
                sutConnectionString = $"server={dataSource};user id={userID};password={password};database={initialCatalog};";
            }
            else
            {
                sutConnectionString = sqlConnBuilder.ToString();
            }

       



            sutUser = userID;
            sutDb = initialCatalog;
            sutServer = dataSource;
            sutProvider = provider;
        }

        [JsonConstructor]
        public UserSetting(string dataSource, string initialCatalog, string userID, string provider = null)
        {
            this.dataSource = dataSource;
            this.initialCatalog = initialCatalog;
            this.userID = userID;
            this.provider = provider;


        }


        public static string SutUser
        {
            get { return sutUser; }
        }

        public static string SutDb
        {
            get { return sutDb; }
        }
        public static string SutServer
        {
            get { return sutServer; }
        }


        public static string SutProvider
        {
            get { return sutProvider; }
        }

        public static string UserSettingSutUpdaterFile
        {
            get
            {
                // return Path.Combine(Environment.GetFolderPath(
                //   Environment.SpecialFolder.ApplicationData), "UserSettingSutUpdater.json");

                //return Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, @"UserSettingSutUpdater.json");


                return Path.Combine(Environment.CurrentDirectory, @"UserSettingSutUpdater.json");
            }
        }




        public static string SutConnectionString
        {
            get
            {
                return sutConnectionString;
            }

        }


        public string InitialCatalog
        {
            get
            {
                return initialCatalog;
            }
            set
            {
                initialCatalog = value;
            }

        }

        public string DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;
            }


        }

        public string UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
            }


        }


        public string Provider
        {
            get
            {
                return provider;
            }
            set
            {
                provider = value;
            }


        }

        public static string Title
        {
            get
            {
                return $", версия: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()} ; сервер: {SutServer}  база данных: {SutDb}  пользователь: {SutUser}";
            }
          
        }
        

        //[JsonIgnore]
        //public static string Password 
        //{
        //    get; set; 
        //}
    }
}
