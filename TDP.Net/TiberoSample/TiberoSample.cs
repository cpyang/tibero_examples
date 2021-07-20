using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Tibero.DataAccess.Client;

namespace Tibero_Test
{
    class TiberoSample
    {
        public static void Main(string[] args)
        {
	    ///*
            string db_name = "tibero";
            string hostname = "localhost";
            string port = "8629";
            string username = "tibero";
            string password = "tmax";
	    //*/
	    
	    /*
            string db_name = "tibero";
            string hostname = "192.168.218.201";
            string port = "28629";
            string username = "tibero";
            string password = "tmax";
	    */

            //string tableDDL = "CREATE TABLE TDP_TEST (ID NUMBER(5), VALUE VARCHAR2(1000))";
            string tableDDL = "CREATE TABLE TDP_TEST (ID NUMBER(10), VALUE VARCHAR2(1000))";
            long rowCount = 10;
	    int loopCount = 10;

            if (args.Length == 2)
            {
                rowCount = long.Parse(args[0]);
                loopCount = int.Parse(args[1]);
            }

            Console.Write("Tibero");
            Console.Write("(" + rowCount + "x" + loopCount + ")");

            string pooling = "true";
            string table_name = "TDP_TEST";
            int minPool = 1;
            int maxPool = 5;
            int incrPool = 1;
            int decrPool = 1;
            int connectTimeOut = 15;
            string Validate = "true";
            string Promotable_Transaction = "promotable";//local or promotable
            string enlist = "true";
            string poolStr = pooling = "Pooling=" + pooling + "; "
                + "Min Pool Size = " + minPool + "; " + "Max Pool Size = " + maxPool + "; "
                + "Incr Pool Size = " + incrPool + "; " + "Decr Pool Size = " + decrPool + "; "
                // + "Connection Lifetime = " + lifetime + " ;"
                // + "Self Tuning = true; "
		        + "Promotable Transaction = " + Promotable_Transaction + "; "
                + "Enlist = " + enlist + "; " + "Connection Timeout = " + connectTimeOut + "; "
                + "Validate Connection = " + Validate + "; ";
            string tbconnstr = "Data Source=((INSTANCE=(HOST=" + hostname
				+ ")(PORT=" + port + ")(DB_NAME=" + db_name
                + ")));User Id=" + username + ";Password=" + password + ";" + poolStr;

            BulkInsertData bulk = new BulkInsertData();
            bulk.setConnection(tbconnstr);
            bulk.dropTable(table_name);
            bulk.createTable(tableDDL);
            DataTable dt = bulk.getSchemaInfo(table_name);
            DataRow row;
            for (long i = 0; i < rowCount; i++)
            {
                row = dt.NewRow();
                row["ID"] = i;
                row["VALUE"] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
                dt.Rows.Add(row);
            }
            Stopwatch bulkInsertDataWatch;
            bulkInsertDataWatch = new Stopwatch();
            ///*
            // Bulk Copy
            bulkInsertDataWatch.Reset();
            bulkInsertDataWatch.Start();
	    for (int i=0; i<loopCount; i++) {
		        //Console.Write(i + ": ");
            	bulk.bulkCopy(table_name, dt);
	    }
            Console.WriteLine("\tAverage=" + (bulkInsertDataWatch.ElapsedMilliseconds/loopCount));
            //*/

            /*
            // Bulk Insert 
            bulkInsertDataWatch.Reset();
            bulkInsertDataWatch.Start();
	    for (int i=0; i<loopCount; i++) {
            	bulk.bulkInsert(table_name, dt);
	    }
            Console.WriteLine("BulkInsertDataTime = " + bulkInsertDataWatch.ElapsedMilliseconds.ToString());
	    
            // Bulk Insert 
            bulkInsertDataWatch.Reset();
            bulkInsertDataWatch.Start();
	    for (int i=0; i<loopCount; i++) {
            	bulk.bulkInsert2(table_name, dt);
	    }
            Console.WriteLine("BulkInsertDataTime = " + bulkInsertDataWatch.ElapsedMilliseconds.ToString());
	    
            // Bulk Insert 
            bulkInsertDataWatch.Reset();
            bulkInsertDataWatch.Start();
            bulk.bulkInsert3(table_name, dt);
            Console.WriteLine("BulkInsertDataTime = " + bulkInsertDataWatch.ElapsedMilliseconds.ToString());
            */

            // Close Connection
            bulk.closeConnection();
        }
    }

    class BulkInsertData
    {
        string tbconnstr = null;
        public TiberoConnection conn = null;

        public void setConnection(string tbconnstr)
        {
            this.tbconnstr = tbconnstr;
            this.conn = new TiberoConnection(tbconnstr);
        }

        public void closeConnection()
        {
            this.conn.Close();
            this.conn.Dispose();
        }

        public void createTable(String tableDDL)
        {
            TiberoCommand cmd = null;
            TiberoConnection conn = this.conn;
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = tableDDL;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            finally
            {
                conn.Close();
                //conn.Dispose();
            }
        }

        public void dropTable(String table_name)
        {
            TiberoCommand cmd = null;
            TiberoConnection conn = this.conn;
            string tableDDL = "DROP TABLE " + table_name;
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = tableDDL;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            finally
            {
                conn.Close();
                //conn.Dispose();
            }
        }

        public DataTable getSchemaInfo(string table_name)
        {
            TiberoConnection conn = this.conn;
            TiberoCommand cmd;
            DataTable schemaTable = null;
            DataTable typeMappingTable = new DataTable();
            try
            {
                conn.Open();
                cmd = new TiberoCommand("select * from " + table_name, conn);
                TiberoDataReader reader = cmd.ExecuteReader();
                schemaTable = reader.GetSchemaTable();
                int rowCnt = schemaTable.Rows.Count;

                DataRow row;
                DataColumn fNameColumn = new DataColumn();
                for (int i = 0; i < rowCnt; i++)
                {
                    row = schemaTable.Rows[i];
                    //Console.WriteLine("Column: " + row["COLUMNNAME"] + " (" + row["DATATYPE"] + ")");
                    fNameColumn = new DataColumn();
                    fNameColumn.DataType = System.Type.GetType(row["DATATYPE"].ToString()); ;
                    fNameColumn.ColumnName = row["COLUMNNAME"].ToString();
                    typeMappingTable.Columns.Add(fNameColumn);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            finally
            {
                conn.Close();
                //conn.Dispose();
            }
            return typeMappingTable;
        }

        public void bulkCopy(string table_name, DataTable table)
        {
            //Console.WriteLine("BulkCopy");
            TiberoConnection conn = this.conn;
            //TiberoTransaction tr = null;

            try
            {
                conn.Open();
                //tr = conn.BeginTransaction();
		        TiberoBulkCopyOptions option = TiberoBulkCopyOptions.UseInternalTransaction;
                TiberoBulkCopy bulkCopy = new TiberoBulkCopy(conn,option);
		        bulkCopy.BulkCopyTimeout = 0;
                //bulkCopy.BatchSize = table.Rows.Count;
                bulkCopy.DestinationTableName = table_name;
                bulkCopy.WriteToServer(table);
                //tr.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                //Console.ReadLine();
            }
            finally 
            {
                //tr.Commit();
                conn.Close();
            }
        }

        public void bulkInsert(string table_name, DataTable table)
        {
            Console.WriteLine("Bulk Insert with auto parameter handling");
            TiberoConnection conn = this.conn;
            TiberoTransaction tr = null;

            try
            {
                conn.Open();
                tr = conn.BeginTransaction();
                TiberoCommand command = conn.CreateCommand();      
                command.CommandText = "INSERT INTO TDP_TEST (ID, VALUE) VALUES (:ID, :VALUE)";
                command.ArrayBindCount = table.Rows.Count;
                command.BindByName = true;

                /*
                foreach(DataRow row in table.Rows)
                {
                    Console.WriteLine(row["ID"].GetType());
                    Console.WriteLine(row["VALUE"].GetType());
                }
                */

                foreach (DataColumn col in table.Columns)
                {
                    /*
                    Console.WriteLine(col.ColumnName);
                    Console.WriteLine(col.DataType);
                    Console.WriteLine(col.GetType());
                    */

                    switch (col.DataType.ToString())
                    {
                        case "System.Int32":
                            command.Parameters.Add(col.ColumnName, TiberoDbType.Int32
                                , table.AsEnumerable().Select(c => c.Field<int>(col.ColumnName)).ToArray(), ParameterDirection.Input);
                            break;

                        case "System.Int64":
                        case "System.Decimal":
                            //command.Parameters.Add(col.ColumnName, TiberoDbType.Long
                            //    , table.AsEnumerable().Select(c => c.Field<long>(col.ColumnName)).ToArray(), ParameterDirection.Input);
                            //command.Parameters.Add(col.ColumnName, TiberoDbType.Decimal
                            //    , table.AsEnumerable().Select(c => c.Field<Decimal>(col.ColumnName)).ToArray(), ParameterDirection.Input);
                            var param = new TiberoParameter(col.ColumnName, TiberoDbType.Decimal);
                            decimal[] values = new decimal[table.Rows.Count];
                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                values[i] = Convert.ToDecimal(table.Rows[i][col.ColumnName]);
                            }
                            param.Value = values;
                            command.Parameters.Add(param);
                            break;

                        case "System.String":
                            command.Parameters.Add(col.ColumnName, TiberoDbType.Varchar2
                                , table.AsEnumerable().Select(c => c.Field<string>(col.ColumnName)).ToArray(), ParameterDirection.Input);
                            break;

                        default:
                            Console.WriteLine("XXX Unhandled: " + col.DataType);
                            break; 
                    }
                }
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                //Console.ReadLine();
            }
            finally 
            {
                tr.Commit();
                conn.Close();
            }
        }

        public void bulkInsert2(string table_name, DataTable table)
        {
            TiberoConnection conn = this.conn;
            TiberoTransaction tr = null;
            Console.WriteLine("Bulk Insert");

            try
            {
                conn.Open();
                tr = conn.BeginTransaction();

                /*
                foreach (DataColumn col in table.Columns)
                {
                    Console.WriteLine("Column " + col.ColumnName + "(" + col.DataType + ")");
                }
                */

                var pID = new TiberoParameter("ID", TiberoDbType.Decimal);
                var pVALUE = new TiberoParameter("VALUE", TiberoDbType.Varchar2);

                decimal[] ids = new decimal[table.Rows.Count];
                string[] values = new string[table.Rows.Count];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    ids[i] = Convert.ToDecimal(table.Rows[i]["ID"]);
                    values[i] = Convert.ToString(table.Rows[i]["VALUE"]);
                }
                pID.Value = ids;
                pVALUE.Value = values;

                TiberoCommand command = conn.CreateCommand();
                command.CommandText = "INSERT INTO TDP_TEST (ID, VALUE) VALUES (:ID, :VALUE)";
                command.ArrayBindCount = ids.Length;
                command.BindByName = true;
                command.Parameters.Add(pID);
                command.Parameters.Add(pVALUE);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                //Console.ReadLine();
            }
            finally
            {
                tr.Commit();
                conn.Close();
            }
        }

        public void bulkInsert3(string table_name, DataTable table)
        {
            TiberoConnection conn = this.conn;
            TiberoTransaction tr = null;
            Console.WriteLine("Bulk Insert");

            try
            {
                conn.Open();
                tr = conn.BeginTransaction();

                /*
                foreach (DataColumn col in table.Columns)
                {
                    Console.WriteLine("Column " + col.ColumnName + "(" + col.DataType + ")");
                }
                */

                var pID = new TiberoParameter("ID", TiberoDbType.Int64);
                var pVALUE = new TiberoParameter("VALUE", TiberoDbType.Varchar2);

                long[] ids = new long[table.Rows.Count];
                string[] values = new string[table.Rows.Count];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    ids[i] = Convert.ToInt64(table.Rows[i]["ID"]);
                    values[i] = Convert.ToString(table.Rows[i]["VALUE"]);
                }
                pID.Value = ids;
                pVALUE.Value = values;

                TiberoCommand command = conn.CreateCommand();
                command.CommandText = "INSERT INTO TDP_TEST (ID, VALUE) VALUES (:ID, :VALUE)";
                command.ArrayBindCount = ids.Length;
                command.BindByName = true;
                command.Parameters.Add(pID);
                command.Parameters.Add(pVALUE);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                //Console.ReadLine();
            }
            finally
            {
                tr.Commit();
                conn.Close();
            }
        }
    }
}

/* vim: set tabstop=4:softtabstop=4:shiftwidth=4:expandtab */ 
