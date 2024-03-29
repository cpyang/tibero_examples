import java.sql.*;
import java.util.Properties;
import java.sql.DriverManager;
import java.sql.Connection;
import java.sql.SQLException;
import com.tmax.tibero.jdbc.*;
import com.tmax.tibero.jdbc.util.*;
import javax.sql.rowset.*;


public class Hello {

	public static void main(String[] argv) {
		PreparedStatement statement;
		String x, y;

		System.out.println("-------- Tibero JDBC Connection Testing ------");
                /*
		try {
			Class.forName("com.tmax.tibero.jdbc.TbDriver");
		} catch (ClassNotFoundException e) {
			System.out.println("Where is your Tibero JDBC Driver?");
			e.printStackTrace();
			return;
		}

		System.out.println("Tibero JDBC Driver Registered!");
		*/

		Connection connection = null;

		try {
			//String url = "jdbc:tibero:thin:@localhost:8629:tibero";
			String url = "jdbc:tibero:thin:@(DESCRIPTION=(FAILOVER=OFF)(LOAD_BALANCE=OFF)(ADDRESS_LIST=(ADDRESS=(HOST=localhost)(PORT=8629)))(DATABASE_NAME=tibero))";
			Driver driver = DriverManager.getDriver(url);
			Properties prop = new Properties();
			//prop.put("characterset","ZHT16MSWIN950");
			prop.put("characterset","UTF8");
			//prop.put("defaultNChar","false");
			prop.put("program_name","HELLO");
			prop.put("user","tibero");
			prop.put("password", "tmax");
			System.out.println(driver.toString());
			connection = DriverManager.getConnection(url,prop);
			//connection = (TbConnection) DriverManager.getConnection(url,prop);
			//TbDatabaseMetaData metadata = (TbDatabaseMetaData) connection.getMetaData();
			DatabaseMetaData metadata = connection.getMetaData();
			System.out.println("URL: " + metadata.getURL());
		} catch (SQLException e) {
			System.out.println("Connection Failed! Check output console");
			e.printStackTrace();
			return;
		}

    		try {
        		DatabaseMetaData dbmd = connection.getMetaData();
			System.out.println("dbmd:driver version = " + dbmd.getDriverVersion());
        		System.out.println("dbmd:driver name = " + dbmd.getDriverName());
        		System.out.println("db name = " + dbmd.getDatabaseProductName());
        		System.out.println("db ver = " + dbmd.getDatabaseProductVersion());
        		System.out.println("jdbc version = " + dbmd.getJDBCMajorVersion() + "." +  dbmd.getJDBCMinorVersion()) ;

/*
			rs = dbmd.getClientInfoProperties();
			while (rs.next()) {
				for (int i = 1; i < rs.getMetaData().getColumnCount() + 1; i++) {
					System.out.print(" " + rs.getMetaData().getColumnName(i) + "=" + rs.getObject(i));
				}
				System.out.println("");
			}
*/
    		} catch (SQLException e) {
			e.printStackTrace();
		}

		if (connection != null) {
			System.out.println("You made it, take control your database now!");
		} else {
			System.out.println("Failed to make connection!");
		}

		try {
			statement = connection.prepareStatement("DELETE from t");
			statement.executeQuery();
		} catch (SQLException e) {
			System.out.println("Query Failed! Check output console");
			e.printStackTrace();
			return;
		}

		try {
			//statement = connection.prepareStatement("INSERT INTO test VALUES ('晜堃喀許')");
			statement = connection.prepareStatement("TRUNCATE TABLE t");
			statement.executeQuery();
			statement = connection.prepareStatement("INSERT INTO t VALUES (1,'游錫堃','游錫堃')");
			statement.executeQuery();
			statement = connection.prepareStatement("INSERT INTO t VALUES (1,'許茹芸淚海慶功宴吃蓋飯','許茹芸淚海慶功宴吃蓋飯')");
			statement.executeQuery();
			statement = connection.prepareStatement("INSERT INTO t VALUES (3,N'游錫堃',N'游錫堃')");
			statement.executeQuery();
			statement = connection.prepareStatement("INSERT INTO t VALUES (3,N'許茹芸淚海慶功宴吃蓋飯',N'許茹芸淚海慶功宴吃蓋飯')");
			statement.executeQuery();
		} catch (SQLException e) {
			System.out.println("Query Failed! Check output console");
			e.printStackTrace();
			return;
		}

    		//String query = "select 'Hello World!'as HELLO from dual";
    		//String query = "select val as HELLO from test";
    		String query = "select t.nvar as HELLO from t";

	        try (Statement stmt = connection.createStatement()) {
	             	ResultSet rs = stmt.executeQuery(query);
			while (rs.next()) {
				String hello = rs.getString("HELLO");
				System.out.println(hello);
			}
	    	} catch (SQLException e) {
			System.out.println("Query Failed! Check output console");
			e.printStackTrace();
			return;
		}

		RowSetFactory myRowSetFactory = null;
		JdbcRowSet jdbcRs = null;
		ResultSet rs = null;
		Statement stmt = null;

		try { 
		      	myRowSetFactory = RowSetProvider.newFactory();
		      	jdbcRs = myRowSetFactory.createJdbcRowSet();

		      	jdbcRs.setUrl("jdbc:tibero:thin:@localhost:8629:tibero");
		      	jdbcRs.setUsername("tibero");
		      	jdbcRs.setPassword("tmax");

		      	jdbcRs.setCommand("select 'Hello World! 哈囉 世界！' from dual");
		      	jdbcRs.execute();

			while (jdbcRs.next()) {  
                        	System.out.println(jdbcRs.getString(1));  
			}
                 
		}  catch (SQLException e) {
			System.out.println("Query Failed! Check output console");
			e.printStackTrace();
			return;
		}

	}
}
