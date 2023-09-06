using System;

namespace ProjectManagement.Infrastructure.Classes
{
    public class LevenshteinDistance
    {
        public static int Distance(string? str1, string? str2)
        {
            if (str1 != null && str2 != null)
            {
                int[,] d = new int[str1.Length + 1, str2.Length + 1];

                for (int i = 0; i <= str1.Length; i++)
                {
                    d[i, 0] = i;
                }

                for (int j = 0; j <= str2.Length; j++)
                {
                    d[0, j] = j;
                }

                for (int j = 1; j <= str2.Length; j++)
                {
                    for (int i = 1; i <= str1.Length; i++)
                    {
                        int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;
                        d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                    }
                }

                return d[str1.Length, str2.Length];
            }
            else
            {
                return int.MaxValue;
            }
        }
    }
}
