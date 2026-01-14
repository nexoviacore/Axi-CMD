using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;


public class Helper
{
    public Ihelper SetDatabase(string DataBasetype, string connectionString)
    {
        Object dbtype = new object();

      if (DataBasetype.ToLower() == "oracle")
        {
            dbtype = (Ihelper)new Oraclehelper(connectionString);
        }
        else if (DataBasetype.ToLower() == "mysql" || DataBasetype.ToLower() == "mariadb")
        {
            dbtype = (Ihelper)new Mysqlhelper(connectionString);
        }
        else if (DataBasetype.ToLower() == "ms sql")
        {
            dbtype = (Ihelper)new Sqlhelper(connectionString);

        }
        else if (DataBasetype.ToLower() == "postgresql" || DataBasetype.ToLower() == "postgre")
        {


            dbtype = (Ihelper)new PosrgreSQL(connectionString);
        }
        else
        {
            dbtype = (Ihelper)new Mysqlhelper(connectionString);
        }
        return (Ihelper)dbtype;

    }
}
