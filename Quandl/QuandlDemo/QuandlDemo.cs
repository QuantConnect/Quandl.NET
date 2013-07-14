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
using QuandlDemo;

namespace QuandlDemo
{
    class QuandlDemo
    {
        static void Main(string[] args)
        {
            Quandl myQuandl = new Quandl();
            Dictionary<string, string> kwargsDemo = new Dictionary<string,string>();
            kwargsDemo.Add("collapse", "weekly");
            kwargsDemo.Add("trim_start", "2010-02-01");
            kwargsDemo.Add("trim_end", "2010-03-28");
            myQuandl.GetFromQuandl("GOOG/NYSE_IBM", kwargsDemo);
            myQuandl.WriteToDataFile("testData");
        }
    }
}
