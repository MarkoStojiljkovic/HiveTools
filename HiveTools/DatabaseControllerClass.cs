using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace HiveTools
{
    class DatabaseControllerClass
    {
        public static string FetchValuesFromDatabase(string connectionString, string command, char delimiter)
        {

            // Command to get MAC, IP and location ID
            //string command1 = "SELECT device.MAC,device.IP,location.id FROM device INNER JOIN location ON device.ID=location.DEVICE_ID ORDER BY location.id";
            //string command2 = "SELECT * FROM device";

            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(command);

            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = ConfigClass.other.dbWaitTimeout;
            OracleDataReader dr = cmd.ExecuteReader();



            StringBuilder sb = new StringBuilder();
            int fieldCnt = 0;
            while (dr.Read()) // Read one row
            {
                while (fieldCnt < dr.FieldCount)
                {
                    sb.Append(dr.GetValue(fieldCnt) + ";");
                    fieldCnt++;
                }
                sb.Remove(sb.Length - 1, 1); // Remove last ';'
                sb.Append(delimiter); // new line
                fieldCnt = 0;
            }
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1); // Remove last delimiter
            }
            conn.Close();

            return sb.ToString();
        }


        public static void SendCommandToDatabase(string connectionString, string command)
        {
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(command);

            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            
            conn.Close();
        }


        //public static void InsertMessageInDatabase(senderInfo s, string connectionString)
        //{
        //    string tempTimeStamp = "TO_TIMESTAMP('" + s.extractedTimestamp.year + "." + s.extractedTimestamp.month +
        //        "." + s.extractedTimestamp.day + " " + s.extractedTimestamp.hour +
        //        ":" + s.extractedTimestamp.minute + ":" + s.extractedTimestamp.second + "." + s.extractedTimestamp.milisecond + "','yyyy.MM.dd HH24:mi.ss.ff')";
        //    string command1 = "INSERT INTO EVENT_HISTORY (LOCATION_ID, EVENT_ID, NAME, EVENT_COMMENT, VALUE, VALID, TIME) VALUES('" + s.locationID + "','" + s.address + "','" + s.name + "','" +
        //       s.comment + "','" + s.value.ToString() + "','" + s.valid + "'," + tempTimeStamp + ")";

        //    OracleConnection conn = new OracleConnection(connectionString);
        //    conn.Open();
        //    OracleCommand cmd1 = new OracleCommand(command1);

        //    cmd1.Connection = conn;
        //    cmd1.CommandType = System.Data.CommandType.Text;
        //    OracleDataReader dr = cmd1.ExecuteReader();

        //    // Second task, delete row

        //    string command2 = "DELETE FROM EVENT_PORTRAIT WHERE LOCATION_ID = " + s.locationID + " AND EVENT_ID = " + s.address;

        //    OracleCommand cmd2 = new OracleCommand(command2);
        //    cmd2.Connection = conn;
        //    cmd2.CommandType = System.Data.CommandType.Text;
        //    dr = cmd2.ExecuteReader();

        //    // Third task, write row, agian
        //    string command3 = "INSERT INTO EVENT_PORTRAIT (LOCATION_ID, EVENT_ID, NAME, EVENT_COMMENT, VALUE, VALID, TIME) VALUES('" + s.locationID + "','" + s.address + "','" + s.name + "','" +
        //       s.comment + "','" + s.value.ToString() + "','" + s.valid + "'," + tempTimeStamp + ")";


        //    OracleCommand cmd3 = new OracleCommand(command3);
        //    cmd3.Connection = conn;
        //    cmd3.CommandType = System.Data.CommandType.Text;
        //    dr = cmd3.ExecuteReader();


        //    //string command2 = "UPDATE EVENT_PORTRAIT SET LOCATION_ID = '" + s.locationID + "', EVENT_ID = '" + s.address + "', NAME='"+ s.name + "',EVENT_COMMENT='" +
        //    //    s.comment + "',VALUE='" +s.value.ToString() +"', VALID='"+ s.valid +"', TIME='"+ tempTimeStamp +"' WHERE LOCATION_ID='" + s.locationID + "' AND EVENT_ID='" +
        //    //     s.address + "'";

        //    //OracleCommand cmd2 = new OracleCommand(command2);
        //    //cmd2.Connection = conn;
        //    //cmd2.CommandType = System.Data.CommandType.Text;
        //    //dr = cmd2.ExecuteReader();


        //    conn.Close();
        //}
    }
}
