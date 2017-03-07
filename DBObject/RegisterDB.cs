﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBObject
{
    public class RegisterDB
    {
        private string myConnectionString;
        int length;
        public RegisterDB()
        {
            myConnectionString = "Server=192.168.1.9; Port = 3306; Database = radius; Uid=root;Pwd=av3c5Ys";
        }
        public int CreateNewUser(string userName, string password, string Email)
        {
            
            int retCode = 0;
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            myConnection.Open();

            MySqlCommand myCommand = myConnection.CreateCommand();
            MySqlTransaction myTrans;

            // Start a local transaction
            myTrans = myConnection.BeginTransaction();
            // Must assign both transaction object and connection
            // to Command object for a pending local transaction
            myCommand.Connection = myConnection;
            myCommand.Transaction = myTrans;
            try
            {
                myCommand.CommandText = "insert into radcheck (username,attribute,op,value) VALUES('" + userName + "','user-password',':=','" + password + "')";
                myCommand.ExecuteNonQuery();
                myCommand.CommandText = "insert into userinfo (username, email) VALUES('" + userName + "','" + Email + "')";
                myCommand.ExecuteNonQuery();
                myTrans.Commit();
                // Console.WriteLine("Both records are written to database.");
            }
            catch (Exception e)
            {
                retCode = -1;
                try
                {
                    myTrans.Rollback();
                }
                catch (Exception ex)
                {
                    if (myTrans.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                        " was encountered while attempting to roll back the transaction.");
                    }
                }

                Console.WriteLine("An exception of type " + e.GetType() +
                " was encountered while inserting the data.");
                Console.WriteLine("Neither record was written to database.");
            }
            finally
            {
                myConnection.Close();
            }
            return retCode;
        }

        //public int LoginUser(string username, string password)
        //{
        //    try
        //    {

        //        var args = new string[4];
        //        args[0] = "192.168.1.3";
        //        args[1] = "testing123";
        //        args[2] = username;
        //        args[3] = password;

        //        length = args.Length;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return length;
        //}
    }
}
