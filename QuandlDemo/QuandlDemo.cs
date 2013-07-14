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
            settings.Add("trim_end", "2010-04-28");
            settings.Add("transformation", "normalize");
            settings.Add("sort_order", "asc");

            // Fetch:
            List<CsvFinancialFormat> data = myQuandl.GetData<CsvFinancialFormat>("YAHOO/MX_IBM", settings); //"GOOG/NYSE_IBM"

            // Debug Purposes Only
            foreach (CsvFinancialFormat tick in data)
            {
                //Console.WriteLine(tick.Time.ToShortDateString() + " H: " + tick.High);
                Console.WriteLine(tick.InputString);
            }
            //Pause
            Console.ReadKey();
        }
    }



}
