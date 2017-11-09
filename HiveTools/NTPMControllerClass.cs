using HiveTools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace HiveTools
{
    class NTPMControllerClass
    {
        /// <summary>
        /// Fetch IP and PORT number from database by DEVICE_ID
        /// </summary>
        /// <param name="delim"> This char will be used for data separation </param>
        /// <returns></returns>
        public static string FindIPAndPortFromId(DataClass dataPtr, char delim)
        {
            string commmand = "SELECT IP,PORT FROM DEVICE WHERE ID=" + dataPtr.deviceID.ToString();

            string result = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), commmand, delim);
            
            return result;
        }

        /// <summary>
        /// Get key value pairs that are needed when requesting custom.xml file
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetPairsForCustomXmlPostMethod(string start, string stop, string user, string pass)
        {
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("start", start),
                new KeyValuePair<string, string>("stop", stop),
                new KeyValuePair<string, string>("type", "by_15min"),
                new KeyValuePair<string, string>("tags", "Allt"),
                new KeyValuePair<string, string>("user", user),
                new KeyValuePair<string, string>("pass", pass),
            };
            return pairs;
        }

        // NEW CODE
        /// <summary>
        /// Get report XML and measurement XML, parse them in Xdocuments, and create CSV
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        //async public Task<string> TaskCreateCSVString(string start, string stop)
        //{
        //    _start = start;
        //    _stop = stop;


        //    List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("start", _start),
        //        new KeyValuePair<string, string>("stop", _stop),
        //        new KeyValuePair<string, string>("type", "by_15min"),
        //        new KeyValuePair<string, string>("tags", "Allt"),
        //        new KeyValuePair<string, string>("user", _user),
        //        new KeyValuePair<string, string>("pass", _pass),
        //    };
        //    // Get XML as strings, parse them and fill XDocuments
        //    string reportRes =await GetReport(_ip);
        //    _reportDoc = XDocument.Parse(reportRes);
        //    FormCustomConsole.WriteLine("-------Start of XML documents:");
        //    FormCustomConsole.WriteLine(_reportDoc.ToString());
        //    string measurementRes = await HTTPClientClass.PostRequest("http://"+_ip+"/custom.xml", pairs);
        //    _measurementDoc = XDocument.Parse(measurementRes);
        //    FormCustomConsole.WriteLine(_measurementDoc.ToString());
        //    FormCustomConsole.WriteLine("-------End of XML documents:");

        //    return CreateCSVFromXML(start, stop);
        //}

       
        async public static Task<string> GetReport(string ip)
        {
            return await HTTPClientClass.GetRequest("http://" + ip + "/report.xml");
        }
        
        //string CreateCSVFromXML(string start, string stop)
        //{
        //    string delim = ",";
        //    string[] arguments = { "IPaL", "IPaA", "IPaH", "IPbL", "IPbA", "IPbH", "IPcL", "IPcA", "IPcH", "IPtL", "IPtA", "IPtH", "VPaL", "VPaA", "VPaH", "VPbL", "VPbA", "VPbH", "VPcL", "VPcA", "VPcH", "VabL", "VabA", "VabH", "VbcL", "VbcA", "VbcH", "VcaL", "VcaA", "VcaH", "PPaL", "PPaA", "PPaH", "PPbL", "PPbA", "PPbH", "PPcL", "PPcA", "PPcH", "PPtL", "PPtA", "PPtH", "PQaL", "PQaA", "PQaH", "PQbL", "PQbA", "PQbH", "PQcL", "PQcA", "PQcH", "PQtL", "PQtA", "PQtH", "PSaL", "PSaA", "PSaH", "PSbL", "PSbA", "PSbH", "PScL", "PScA", "PScH", "PStL", "PStA", "PStH", "EPaP", "EPbP", "EPcP", "EPtP", "EPaC", "EPbC", "EPcC", "EPtC", "EQaP", "EQbP", "EQcP", "EQtP", "EQaC", "EQbC", "EQcC", "EQtC", "TPE0", "TPE1", "TPE2", "TPE3", "TQE0", "TQE1", "TQE2", "TQE3", "DemA", "DemB", "DemC", "DemT", "HIaL", "HIaA", "HIaH", "HIbL", "HIbA", "HIbH", "HIcL", "HIcA", "HIcH", "HUaL", "HUaA", "HUaH", "HUbL", "HUbA", "HUbH", "HUcL", "HUcA", "HUcH", "TemL", "TemA", "TemH", "FreL", "FreA", "FreH", "PFaL", "PFaA", "PFaH", "PFbL", "PFbA", "PFbH", "PFcL", "PFcA", "PFcH", "PFtL", "PFtA", "PFtH" };

        //    StringBuilder sb = new StringBuilder();

        //    // Form file header

        //    XElement dataReport = _reportDoc.Root.Element("cfg").Element("tcpip"); // This element will hold values for CSV header


        //    string hostname = dataReport.Element("hostname").Value.Trim(); // NTPM have bug with name, it needs trimming
        //    string mac = dataReport.Element("mac").Value;
            

        //    sb.Append("HOSTNAME:"+ hostname +"\r\n");
        //    sb.Append("MAC:"+ mac +"\r\n");
        //    sb.Append("START_TIME:" + start + "\r\n");
        //    sb.Append("STOP_TIME:" + stop + "\r\n");
        //    sb.Append("TYPE: by_15min\r\n");
        //    sb.Append("DELIMITER:COMMA\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("Time,"); // This is first row which is not measurment

        //    foreach (var item in arguments)
        //    {
        //        sb.Append(item + delim);
        //    }
        //    sb.Remove(sb.Length - 1, 1); // Remove last delimiter
        //    sb.Append("\r\n");

        //    // Get measurement values
        //    XElement dataMeasure = _measurementDoc.Root.Element("data"); // Data element

        //    //string mac = dataMeasure.Attribute("mac").Value;
        //    string type = dataMeasure.Attribute("type").Value;
        //    string sCout = dataMeasure.Attribute("count").Value;

        //    // Now form CSV
        //    int numOfPoints = Convert.ToInt32(sCout);

        //    for (int i = 0; i < numOfPoints; i++)
        //    {
        //        // Get "point" XElement for current seq attribute
        //        XElement pointXE = dataMeasure.Elements("point").Where(x => x.Attribute("seq").Value == i.ToString()).FirstOrDefault();
        //        // Get time and status from point XElement
        //        string time = pointXE.Attribute("time").Value;
        //        string status = pointXE.Attribute("status").Value;

        //        FormCustomConsole.WriteLine("Status tag: " + status);

        //        sb.Append(time + delim);

        //        switch (status)
        //        {
        //            case "valid":
        //                foreach (var item in arguments)
        //                {
        //                    string temp = pointXE.Element(item).Value + delim;
        //                    sb.Append(temp);
        //                }
        //                sb.Remove(sb.Length - 1, 1);
        //                sb.Append("\r\n");
        //                break;
        //            case "null":
        //                foreach (var item in arguments) // I just want to iterate number of arguments times and append "null"
        //                {
        //                    sb.Append("null" + delim);
        //                }
        //                sb.Remove(sb.Length - 1, 1);
        //                sb.Append("\r\n");
        //                break;
        //            default:
        //                break; // Just skip if invalid
        //        }

                
        //    }
        //    sb.Remove(sb.Length - 2, 2); // Remove last newline

        //    return sb.ToString();
        //}

        /// <summary>
        /// Create header for CSV document (which will be used for Custom.xml)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="reportDoc"></param>
        /// <returns></returns>
        public static string CreateCSVHeader(string start, string stop, XDocument reportDoc)
        {
            string delim = ",";
            string[] arguments = { "IPaL", "IPaA", "IPaH", "IPbL", "IPbA", "IPbH", "IPcL", "IPcA", "IPcH", "IPtL", "IPtA", "IPtH", "VPaL", "VPaA", "VPaH", "VPbL", "VPbA", "VPbH", "VPcL", "VPcA", "VPcH", "VabL", "VabA", "VabH", "VbcL", "VbcA", "VbcH", "VcaL", "VcaA", "VcaH", "PPaL", "PPaA", "PPaH", "PPbL", "PPbA", "PPbH", "PPcL", "PPcA", "PPcH", "PPtL", "PPtA", "PPtH", "PQaL", "PQaA", "PQaH", "PQbL", "PQbA", "PQbH", "PQcL", "PQcA", "PQcH", "PQtL", "PQtA", "PQtH", "PSaL", "PSaA", "PSaH", "PSbL", "PSbA", "PSbH", "PScL", "PScA", "PScH", "PStL", "PStA", "PStH", "EPaP", "EPbP", "EPcP", "EPtP", "EPaC", "EPbC", "EPcC", "EPtC", "EQaP", "EQbP", "EQcP", "EQtP", "EQaC", "EQbC", "EQcC", "EQtC", "TPE0", "TPE1", "TPE2", "TPE3", "TQE0", "TQE1", "TQE2", "TQE3", "DemA", "DemB", "DemC", "DemT", "HIaL", "HIaA", "HIaH", "HIbL", "HIbA", "HIbH", "HIcL", "HIcA", "HIcH", "HUaL", "HUaA", "HUaH", "HUbL", "HUbA", "HUbH", "HUcL", "HUcA", "HUcH", "TemL", "TemA", "TemH", "FreL", "FreA", "FreH", "PFaL", "PFaA", "PFaH", "PFbL", "PFbA", "PFbH", "PFcL", "PFcA", "PFcH", "PFtL", "PFtA", "PFtH" };

            StringBuilder sb = new StringBuilder();

            // Form file header

            XElement dataReport = reportDoc.Root.Element("cfg").Element("tcpip"); // This element will hold values for CSV header


            string hostname = dataReport.Element("hostname").Value.Trim(); // NTPM have bug with name, it needs trimming
            string mac = dataReport.Element("mac").Value;


            sb.Append("HOSTNAME:" + hostname + "\r\n");
            sb.Append("MAC:" + mac + "\r\n");
            sb.Append("START_TIME:" + start + "\r\n");
            sb.Append("STOP_TIME:" + stop + "\r\n");
            sb.Append("TYPE: by_15min\r\n");
            sb.Append("DELIMITER:COMMA\r\n");
            sb.Append("\r\n");
            sb.Append("Time,"); // This is first row which is not measurment

            foreach (var item in arguments)
            {
                sb.Append(item + delim);
            }
            sb.Remove(sb.Length - 1, 1); // Remove last delimiter
            sb.Append("\r\n");

            return sb.ToString();
        }


        public static string CreateCSVMeasurementRow(XDocument measurementDoc)
        {
            string delim = ",";
            string[] arguments = { "IPaL", "IPaA", "IPaH", "IPbL", "IPbA", "IPbH", "IPcL", "IPcA", "IPcH", "IPtL", "IPtA", "IPtH", "VPaL", "VPaA", "VPaH", "VPbL", "VPbA", "VPbH", "VPcL", "VPcA", "VPcH", "VabL", "VabA", "VabH", "VbcL", "VbcA", "VbcH", "VcaL", "VcaA", "VcaH", "PPaL", "PPaA", "PPaH", "PPbL", "PPbA", "PPbH", "PPcL", "PPcA", "PPcH", "PPtL", "PPtA", "PPtH", "PQaL", "PQaA", "PQaH", "PQbL", "PQbA", "PQbH", "PQcL", "PQcA", "PQcH", "PQtL", "PQtA", "PQtH", "PSaL", "PSaA", "PSaH", "PSbL", "PSbA", "PSbH", "PScL", "PScA", "PScH", "PStL", "PStA", "PStH", "EPaP", "EPbP", "EPcP", "EPtP", "EPaC", "EPbC", "EPcC", "EPtC", "EQaP", "EQbP", "EQcP", "EQtP", "EQaC", "EQbC", "EQcC", "EQtC", "TPE0", "TPE1", "TPE2", "TPE3", "TQE0", "TQE1", "TQE2", "TQE3", "DemA", "DemB", "DemC", "DemT", "HIaL", "HIaA", "HIaH", "HIbL", "HIbA", "HIbH", "HIcL", "HIcA", "HIcH", "HUaL", "HUaA", "HUaH", "HUbL", "HUbA", "HUbH", "HUcL", "HUcA", "HUcH", "TemL", "TemA", "TemH", "FreL", "FreA", "FreH", "PFaL", "PFaA", "PFaH", "PFbL", "PFbA", "PFbH", "PFcL", "PFcA", "PFcH", "PFtL", "PFtA", "PFtH" };

            StringBuilder sb = new StringBuilder();

            // Get measurement values
            XElement dataMeasure = measurementDoc.Root.Element("data"); // Data element

            //string mac = dataMeasure.Attribute("mac").Value;
            string type = dataMeasure.Attribute("type").Value;
            string sCout = dataMeasure.Attribute("count").Value;

            // Now form CSV
            int numOfPoints = Convert.ToInt32(sCout);

            for (int i = 0; i < numOfPoints; i++)
            {
                // Get "point" XElement for current seq attribute
                XElement pointXE = dataMeasure.Elements("point").Where(x => x.Attribute("seq").Value == i.ToString()).FirstOrDefault();
                // Get time and status from point XElement
                string time = pointXE.Attribute("time").Value;
                string status = pointXE.Attribute("status").Value;

                FormCustomConsole.WriteLine("Status tag: " + status);

                sb.Append(time + delim);

                switch (status)
                {
                    case "valid":
                    case "live":
                        foreach (var item in arguments)
                        {
                            string temp = pointXE.Element(item).Value + delim;
                            sb.Append(temp);
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("\r\n");
                        break;
                    case "null":
                        foreach (var item in arguments) // I just want to iterate number of arguments times and append "null"
                        {
                            sb.Append("null" + delim);
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("\r\n");
                        break;
                    default:
                        throw new Exception("Status tag not recognised");
                }


            }
            sb.Remove(sb.Length - 2, 2); // Remove last newline

            return sb.ToString();
        }

        /// <summary>
        /// Check for error attribute
        /// </summary>
        /// <param name="doc"></param>
        public static void CheckErrorTag(XDocument doc)
        {
            string res = doc.Root.Element("data").Attribute("error").Value;
            switch (res)
            {
                case @"wrong username/password":
                    throw new Exception("Wrong username or password!");
                case "none":
                    break;
                default:
                    throw new Exception("Error tag not recognised!");
            }
        }
    }
}
