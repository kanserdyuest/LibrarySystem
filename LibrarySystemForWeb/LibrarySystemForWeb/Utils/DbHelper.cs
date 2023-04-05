using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace ConsoleAppEntity.utils
{

    public static class DbHelper
    {
        // 1.创建连接对象 连接字符串 ==> 打开连接 ==> 捕捉可能发生的异常

        // 2.创建指令对象 指定指令对象的连接对象 ==> 指定要执行的sql语句 ==> 选择三种执行方法其中一种

        // 3. 处理结果，关闭连接

        public static string ConnetionString { get; private set; } = "server=localhost; port=3306; user=root; password=123456; database=asp_net_db;SslMode=none";

        public static MySqlConnection Con
        {
            get
            {
                var con = new MySqlConnection(ConnetionString);
                try
                {
                    con.Open();
                    return con;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public static MySqlCommand Cmd
        {
            get
            {
                var cmd = new MySqlCommand()
                {
                    Connection = Con
                };

                return cmd;
            }
        }

        // 执行增删改
        public static bool Update(string sql)
        {
            var cmd = Cmd;
            cmd.CommandText = sql;
            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                Con.Clone();
            }

        }

        // 查询结果集中第一行第一列的结果
        public static object SelectForScalar(string sql)
        {
            var cmd = Cmd;
            cmd.CommandText = sql;
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                Con.Clone();
            }
        }

        // 获取结果集返回reader
        public static MySqlDataReader SelectForReader(string sql)
        {
            var cmd = Cmd;
            cmd.CommandText = sql;
            try
            {
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (MySqlException)
            {
                cmd.Connection.Clone();
                throw;
            }
        }


    }
}