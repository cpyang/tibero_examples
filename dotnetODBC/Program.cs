using System;
using System.Data.Odbc;
 
namespace test {
    class Program {
        static void Main(string[] args) {
	    bool tibero = true;
	    //tibero = false;
	    OdbcConnectionStringBuilder builder;

	    if (tibero) {
		builder = new OdbcConnectionStringBuilder {
		    Driver = "Tibero"
		};
		builder.Add("DSN", "tibero");
		builder.Add("HostName", "localhost");
		builder.Add("PortNumber", "8629");
		builder.Add("Database", "tibero");
	    } else {
		builder = new OdbcConnectionStringBuilder {
		    Driver = "Oracle"
		};
		builder.Add("DSN", "oracle");
		builder.Add("HostName", "localhost");
		builder.Add("PortNumber", "1521");
		builder.Add("Database", "XEPDB1");
	    }
 
	    string connection_string = builder.ConnectionString;
            using (OdbcConnection connection = new OdbcConnection(connection_string))
            {
                string sqlQuery = "SELECT 'Hello ' as hello, 'World!' as world FROM dual UNION SELECT '哈囉', '世界!' FROM dual";
		OdbcCommand command;
		OdbcDataReader reader;
                try {
	            Console.WriteLine("Connecting to \"{0}\"", connection_string);
                    connection.Open();
                    command = new OdbcCommand(sqlQuery, connection);
		    Console.WriteLine("ExecuteReader()");
                    reader = command.ExecuteReader();

                    for (int i=0; i<reader.FieldCount; i++) {
                        Console.Write("Column({0}) {1}\t", i, reader.GetName(i));
                    }
                    Console.Write("\n");
 
                    if(reader.HasRows) {
                        while(reader.Read()) {
                            Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
			}
                    }
                    reader.Close();
                    command.Dispose();
		} catch (OdbcException e) {
                    for (int i=0; i < e.Errors.Count; i++) {
                        Console.Write("Exception #{0}\nMessage:{1}\nNative:{2}\nSource:{3}\nSQL:{4}\nStackTrace:{5}\n",
                            i, e.Errors[i].Message, e.Errors[i].NativeError.ToString(),
			    e.Errors[i].Source, e.Errors[i].SQLState, e.StackTrace.ToString());
                    }
                }
            }
        }
    }
}

