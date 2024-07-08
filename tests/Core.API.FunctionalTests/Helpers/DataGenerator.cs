using System.Text;

namespace Core.API.FunctionalTests.Helpers
{
    public static class DataGenerator
    {
        public static string GenerateNewPlate()
        {
            return GenerateRandomPatternString("LLLNNNN");
        }
        public static string GenerateOldPlate()
        {
            return GenerateRandomPatternString("LLLNJNN");
        }

        public static string GenerateTestModel()
        {
            return $"Model-{GenerateRandomPatternString("LLLLL")}";
        }
        public static string GenerateRandomPatternString(string pattern)
        {
            var random = new Random();
            const string charsAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string charsNumbers = "0123456789";
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < pattern.Length; i++)
            {
                var p = pattern[i];
                switch (p)
                {
                    case 'L':
                        stringBuilder.Append(charsAlphabet[random.Next(0, charsAlphabet.Length)]);
                        break;
                    case 'N':
                        stringBuilder.Append(charsNumbers[random.Next(0, charsNumbers.Length)]);
                        break;
                    case ' ':
                        stringBuilder.Append(' ');
                        break;
                    case '-':
                        stringBuilder.Append('-');
                        break;
                    default:
                        if (int.TryParse(p.ToString(), out var number))
                        {
                            stringBuilder.Append(charsNumbers[random.Next(0, int.Min(charsNumbers.Length, number))]);
                        }
                        else if (charsAlphabet.Contains(p))
                        {
                            var index = charsAlphabet.IndexOf(p);
                            stringBuilder.Append(charsAlphabet[random.Next(0, int.Min(charsAlphabet.Length, index))]);
                        }
                        throw new ArgumentException("Invalid pattern");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
