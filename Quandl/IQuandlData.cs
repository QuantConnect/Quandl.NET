using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuandlDotNet
{
    public interface IQuandlData
    {
        /// <summary>
        /// Initializer for the QuandlData Interface.
        /// </summary>
        /// <param name="line"></param>
        void IQuandlData(string line);

    }
}
