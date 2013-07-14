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
    public class Quandl
    {
        // Wrapper Class for Accessing the Quandl.com API
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
        /// <param name="token"></param>
        public void SetAuthToken(string token)
        {
            AuthToken = token;
        }

        /// <summary>
        /// Principle function for getting data about a given stock
        /// dataset = dataset code as per Quandl.com website
        /// format = format for data to be returned as, default = "csv". Options are "csv", "plain", "json", "xml"
        /// </summary>
        /// <param name="dataset"> dataset code as per Quandl.com website</param>
        /// <param name="settings"></param>
        /// <param name="format"></param>
        public List<T> GetData<T>(string dataset, Dictionary<string, string> settings, string format = "csv")
        {
            /* Princple function for getting data about a give stock 
             * dataset = dataset code as per Quandl.com website
             * format = format for data to be returned as, default = "csv". Options are "csv", "plain", "json", "xml"
             * 
             * settings = A dictionary of keyords describing what to data to obtain as follows:
             *  trim_start: format is "yyyy-mm-dd"
             *  trim_end: format is "yyyy-mm-dd"
             *  collapse: Options are "daily", "weekly", "monthly", "quarterly", "annual"
             *  transformation: options are "diff", "rdiff", "cumul", and "normalize"
             *  rows: Number of rows which will be returned, e.g. ("rows", "1")
             *  sort_order: options are "asc", "desc". Default: "asc"
             *  
             *  In addition any other Quandl.com parameter can be passed
             */
            
            string requestUrl = "";
            string rawData = "";
            List<T> data = new List<T>();

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

            //Convert into a list of class objects
            string[] lines = rawData.Split(new[] { '\r', '\n' });

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
}
