namespace Bogus.Extensions.Belgium
{
    /// <summary>
    /// API extensions specific for a geographical location.
    /// </summary>
    public static class ExtensionsForBelgium
    {
        private const int FollowNumberMin = 1;
        private const int FollowNumberMax = 998;

        /// <summary>
        /// Returns a valid Belgian National Register Number based on date of birth and gender.
        /// </summary>
        /// <param name="p">Person with date of birth and gender</param>
        /// <param name="formatted"></param>
        /// <returns></returns>
        public static string BNN(this Person p, bool formatted = true)
        {
            var nationalRegisterNumber = string.Empty;

            const string Key = $"{nameof(ExtensionsForBelgium)}BNN";
            if (p.context.ContainsKey(Key))
            {
                nationalRegisterNumber = p.context[Key] as string;
            }
            else
            {
                var followNumber = GenerateFollowNumber(p);

                var birthDatePart = birthDate.ToString("yyMMdd");
                var followNumberPart = followNumber.ToString().PadLeft(3, '0');

                long dividend;
                if (birthDate.Year > 1999)
                    dividend = long.Parse($"2{birthDatePart}{followNumberPart}");
                else
                    dividend = long.Parse($"{birthDatePart}{followNumberPart}");

                var remainder = dividend % Divisor;
                var controlNumber = Divisor - remainder;
                var controlNumberPart = controlNumber.ToString().PadLeft(2, '0');

                nationalRegisterNumber = $"{birthDatePart}{followNumberPart}{controlNumberPart}";
            }

            if (formatted)
                return Format(nationalRegisterNumber);

            return nationalRegisterNumber;
        }

        private static int GenerateFollowNumber(Person p)
        {
            var followNumber = p.Random.Int(FollowNumberMin, FollowNumberMax);

            if (p.Gender == DataSets.Name.Gender.Female)
            {
                if (followNumber % 2 != 0)
                    followNumber++;
            }
            else
            {
                if (followNumber % 2 == 0)
                    followNumber--;
            }
            return followNumber;
        }

        private static string Format(string nationalRegisterNumber) => nationalRegisterNumber.ToString("00.00.00-000.00");
    }
}
