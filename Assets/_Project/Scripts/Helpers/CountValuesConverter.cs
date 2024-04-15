namespace _Project.Scripts.Helpers
{
    public class CountValuesConverter
    {
        public static string From1000toK(decimal servCount)
        {
            decimal value = 0;
            if (servCount >= 1000 && servCount < 1000000)
            {
                value = servCount / 1000;
                return $"{value}K";
            }

            if (servCount >= 1000000 && servCount < 1000000000)
            {
                value = servCount / 1000000;
                return $"{value}M";
            }
            if (servCount >= 1000000000 && servCount < 1000000000000)
            {
                value = servCount / 1000000000;
                return $"{value}B";
            }
            return $"{servCount}";
        }

    }
}