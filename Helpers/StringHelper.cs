using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StersTransport.Helpers
{
    public class StringHelper
    {
        public string generatestringFromList(List<string>datalist)
        {
            string result = string.Empty;
            for (int c = 0; c < datalist.Count; c++)
            
            {
                if (c == 0)
                {
                    result += datalist[c];
                }
                else
                {
                    result += Environment.NewLine;
                    result += datalist[c];
                }
            }
            return result;
        }
    }
}
