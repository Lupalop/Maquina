using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class Utils
    {
        public static string CreateLocation(string[] names, bool appendSlashAtEnd = false)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < names.Length; i++)
            {
                // If this is the last item in the array, don't add backward slash
                if (i == names.Length - 1 && !appendSlashAtEnd)
                {
                    stringBuilder.Append(names[i]);
                    continue;
                }
                stringBuilder.Append(names[i] + "\\");
            }
            return stringBuilder.ToString();
        }
    }
}
