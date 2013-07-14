/*
 * Created by Ryan Hill, Copyright July 2013
 * 
 *  This file is part of QuandlDotNet package.
 * 
 *  QuandlDotNet is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  QuandlDotNet is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with QuandlDotNet.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace QuandlDotNet
{
    /// <summary>
    /// Wrapper Class for Quandl.com API
    /// Currently only supports access to the Google (GOOG) or Yahoo (YAHOO) financial databases
    /// </summary>
    public class Quandl
    {
        private const string QUANDL_API_URL = "http://www.quandl.com/api/v1/";
        private string AuthToken;
        private string OutputFormat;

        /// <summary>
        /// Quandl Object Constructor
        /// </summary>
        /// <param name="authenticationToken">string auth token - if authentication token not specified on construction then access is limited to 10 per day</param>
        public Quandl(string authenticationToken = "")
        {
            AuthToken = authenticationToken;
        }

        /// <summary>
        /// Set the authorization token for the API calls.
        /// </summary>
        public void SetAuthToken(string token)
        {
            AuthToken = token;
        }


        /// <summary>
        /// Fetch the raw string data from Quandl.
        /// Note that the GetRawData function should be used if a custom data formating class is required
        /// Otherwise use the GetData function
        /// </summary>
        /// <param name="dataset"> dataset code as per Quandl.com website</param>
        /// <param name="settings"> as per the the Quandl.com website </param>
        /// <param name="format"> format for data to be returned as, default = "csv". Options are "csv", "plain", "json", "xml" </param>
        /// <returns></returns>
        public string GetRawData(string dataset, Dictionary<string, string> settings, string format = "csv")
        {
            string requestUrl = "";
            string rawData = "";

            //Set the output format:
            OutputFormat = format;

            if (AuthToken == "")
            {
                requestUrl = QUANDL_API_URL + String.Format("datasets/{0}.{1}?", dataset, format);
                foreach (KeyValuePair<string, string> kvp in settings)
                {
                    requestUrl = requestUrl + String.Format("{0}={1}&", kvp.Key, kvp.Value);
                }
            }
            else
            {
                requestUrl = QUANDL_API_URL + String.Format("datasets/{0}.{1}?auth_token={2}", dataset, format, AuthToken);
                foreach (KeyValuePair<string, string> kvp in settings)
                {
                    requestUrl = requestUrl + String.Format("&{0}={1}", kvp.Key, kvp.Value);
                }
            }

            try
            {
                //Prevent 404 Errors:
                WebClient client = new WebClient();
                rawData = client.DownloadString(requestUrl);
            }
            catch (Exception err)
            {
                throw new Exception("Sorry there was an error and we could not connect to Quandl: " + err.Message);
            }

            return rawData;
        }

        /// <summary>
        /// Principle function for getting data about a given stock
        /// </summary>
        /// <typeparam name="T"> Currently only CsvFormat type is supported </typeparam>
        /// <param name="dataset"> dataset code as per Quandl.com website </param>
        /// <param name="settings"> as per the the Quandl.com website </param>
        /// <returns></returns>
        public List<T> GetData<T>(string dataset, Dictionary<string, string> settings)
        {
            //Initialize our generic holder:
            List<T> data = new List<T>();

            //Determine formatting type
            //Will eventually support HTML, Json and XML type... maybe... someday...
            string format;
            if (typeof(T) == typeof(CsvFinancialFormat))
            {
                format = "csv";
            }
            else
            {
                throw new Exception("Invalid format type. Please use CsvFormat");
            }

            //Download the required strings:
            string rawData = GetRawData(dataset, settings, format);

            //Convert into a list of class objects
            string[] lines = rawData.Split(new[] { '\r', '\n' });
            Console.WriteLine(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Trim().Length > 0)
                {
                    data.Add((T)Activator.CreateInstance(typeof(T), line));
                }
            }

            return data;
        }
    }
    /// <summary>
    /// Data format for Csv financial quandl request
    /// </summary>
    public class CsvFinancialFormat
    {
        public DateTime Time = new DateTime();
        public Decimal Open = 0;
        public Decimal High = 0;
        public Decimal Low = 0;
        public Decimal Close = 0;
        public Decimal Volume = 0;
        public string InputString;
         

        /// <summary>
        /// Create our new generic data type:
        /// </summary>
        /// <param name="csvLine"></param>
        public CsvFinancialFormat(string csvLine)
        {
            InputString = csvLine;
            try
            {
                string[] values = csvLine.Split(',');
                if (values.Length >= 6)
                {
                    Time = Convert.ToDateTime(values[0]);
                    try
                    {
                        Volume = Convert.ToDecimal(values[5]);
                         // Catch formatting issues with regaurds to days in which no trades occur
                        if (Volume > 0)
                        {
                            Open = Convert.ToDecimal(values[1]);
                            High = Convert.ToDecimal(values[2]);
                            Low = Convert.ToDecimal(values[3]);
                            Close = Convert.ToDecimal(values[4]);
                        }
                        else
                        {
                            // No trades occured, make all open/high/low == close
                            Open = Convert.ToDecimal(values[4]);
                            High = Convert.ToDecimal(values[4]);
                            Low = Convert.ToDecimal(values[4]);
                            Close = Convert.ToDecimal(values[4]);
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("Missing Data:" + csvLine);
                    }
                }
            }
            catch (Exception err)
            {
                //Write the titles out:
                Console.WriteLine("Er:" + csvLine);
            }
        }
       
    }
}
