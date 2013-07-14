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
using QuandlDotNet;

namespace QuandlDemo
{
    class QuandlDemo
    {
        static void Main(string[] args)
        {
            Quandl myQuandl = new Quandl();
            
            // Add the required settings to pull down data:
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings.Add("collapse", "weekly");
            settings.Add("trim_start", "2010-02-01");
            settings.Add("trim_end", "2010-03-28");

            // Fetch:
            List<Candle> data = myQuandl.GetData<Candle>("GOOG/NYSE_IBM", settings);

            // Debug Purposes Only
            foreach (Candle tick in data) {
                Console.WriteLine(tick.Time.ToShortDateString() + " H: " + tick.High);
            }
            //Pause
            Console.ReadKey();
        }
    }


    /// <summary>
    /// Data format for this quandl request
    /// </summary>
    class Candle
    {
        public DateTime Time = new DateTime();
        public Decimal Open = 0;
        public Decimal High = 0;
        public Decimal Low = 0;
        public Decimal Close = 0;
        public Decimal Volume = 0;

        /// <summary>
        /// Create our new generic data type:
        /// </summary>
        /// <param name="csvLine"></param>
        public Candle(string csvLine)
        {
            try
            {
                string[] values = csvLine.Split(',');
                if (values.Length == 6)
                {
                    Time = Convert.ToDateTime(values[0]);
                    Open = Convert.ToDecimal(values[1]);
                    High = Convert.ToDecimal(values[2]);
                    Low = Convert.ToDecimal(values[3]);
                    Close = Convert.ToDecimal(values[4]);
                    Volume = Convert.ToDecimal(values[5]);
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
