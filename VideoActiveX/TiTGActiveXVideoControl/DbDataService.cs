﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
//using PublicWCFServices;

namespace DataServiceClient
{
    class DbDataService : IDataService
    {
        string dbPictureTable = GrabImageClient.Helper.getAppSetting("dbPictureTable");
        string dbFingerTable = GrabImageClient.Helper.getAppSetting("dbFingerTable");

        string dbIdColumn = GrabImageClient.Helper.getAppSetting("dbIdColumn");
        string dbPictureColumn = GrabImageClient.Helper.getAppSetting("dbPictureColumn");
        string dbFingerColumn = GrabImageClient.Helper.getAppSetting("dbFingerColumn");

        public byte[] GetImage(IMAGE_TYPE imageType, int id)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            byte[] buffer = null;

            try
            {
                string connectionString = GrabImageClient.Helper.getConnectionString();
                conn = new SqlConnection(connectionString);

                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;

                if (imageType == IMAGE_TYPE.picture)
                    cmd.CommandText = "SELECT " + dbPictureColumn + " FROM " + dbPictureTable + " WHERE " + dbIdColumn + " = @id";
                else
                    cmd.CommandText = "SELECT " + dbFingerColumn + " FROM " + dbFingerTable + " WHERE " + dbIdColumn + " = @id";

                //cmd.Parameters.Add(new SqlCeParameter("@id", SqlDbType.Int));   // doesn't work
                cmd.Parameters.AddWithValue("@id", id);

                reader = cmd.ExecuteReader();
                //reader.Read();

                //SqlBinary binary;
                //SqlBytes bytes;

                //                if (reader.HasRows)   //Does not work for CE
                if (reader.Read())
                {
                    //if (!reader.IsDBNull(0))
                    //    id = reader.GetInt32(0);
                    if (!reader.IsDBNull(0))
                    {
                        //binary = reader.GetSqlBinary(1);
                        if (imageType == IMAGE_TYPE.picture)
                            buffer = (byte[])reader[dbPictureColumn]; //(byte[])reader["AppImage"];
                        else
                            buffer = (byte[])reader[dbFingerColumn]; //(byte[])reader["AppImage"];

                        //int maxSize = 200000;
                        //buffer = new byte[maxSize];
                        //reader.GetBytes(1, 0L, buffer, 0, maxSize);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    if (reader != null)
                        reader.Close();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    if (conn != null)
                        conn = null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return buffer;

        }

        public void SendImage(IMAGE_TYPE imageType, int id, ref byte[] buffer)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            //object incrementId = null;
            try
            {
                //System.Windows.Forms.MessageBox.Show("connect str1: " + getConnectionString());
                //conn = new SqlConnection(getConnectionString());
                string connectionString = GrabImageClient.Helper.getConnectionString();
                conn = new SqlConnection(connectionString);

                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;

                string dbImageTable, dbImageColumn;
                if (imageType == IMAGE_TYPE.picture)
                {
                    dbImageTable = dbPictureTable;
                    dbImageColumn = dbPictureColumn;
                }
                else
                {
                    dbImageTable = dbFingerTable;
                    dbImageColumn = dbFingerColumn;
                }

                cmd.CommandText = String.Format(@"
                            begin tran
                                update {0} with (serializable) SET {1} = @picture where {2} = @id
                                if @@rowcount = 0 
                                begin
                                    insert into {0} ({2}, {1}) values (@id, @picture) 
                                end
                            commit tran ", dbImageTable, dbImageColumn, dbIdColumn);

                cmd.Parameters.Add("@picture", SqlDbType.VarBinary);
                cmd.Parameters["@picture"].Value = buffer;

                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    if (conn != null)
                        conn = null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
/*
        private String getConnectionString(string name = "ConnectionString")
        {
            var configurationService = new TiTGActiveXVideoControl.ConfigurationService.ConfigurationServiceClient();
            var connectionString = configurationService.getConnectionString(name);

            //string connectionString = null;
            //var connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
            //if (connectionStringSettings != null)
            //    connectionString = connectionStringSettings.ConnectionString;

            //// If we didn't find setting, try to load it from current dll's config file
            //if (string.IsNullOrEmpty(connectionString))
            //{
            //    var assemly = System.Reflection.Assembly.GetExecutingAssembly();
            //    var configuration = ConfigurationManager.OpenExeConfiguration(assemly.Location);
            //    var value = configuration.ConnectionStrings.ConnectionStrings[name];
            //    if (value != null)
            //    {
            //        connectionString = value.ConnectionString;
            //    }
            //}

            return connectionString;
        }
*/
    }
}
