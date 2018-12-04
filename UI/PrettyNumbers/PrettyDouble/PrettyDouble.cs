using System;

namespace Bannified {
	public static class PrettyDouble
	{
		public static readonly string[] standardSuffixesSet = { " ", "K", "M", "B", "T" };

		const string specialSuffixCharSet = " abcdefghijklmnopqrstuvwxyxABCDEFGHIJKLMNOPQRSTUVWXYZ";

		const int numberOfDecimals = 2;

		/// <summary>
		/// Returns the Pretty-formatted (max 5 digits) BigInteger.
		/// 
		/// The Pretty format is as follows:
		/// - No more than 5 digits at any time
		/// - Fractional part is always to 2 most significant numbers.
		/// - Follows the Short scale pattern, where the difference in order of magnitude
		/// between a number and another of a scale greater is 3.
		/// </summary>
		/// <returns>Pretty-formatted (max 5 digits) BigInteger.</returns>
		public static string DoubleToPrettyDouble(double value)
		{
			string fullString = value.ToString("F0");

			int suffixIndex = ((fullString.Length - 1) / 3) % standardSuffixesSet.Length;

			int specialSuffixIndex = ((fullString.Length - 1) / ((standardSuffixesSet.Length) * 3)) % specialSuffixCharSet.Length;

			string result;

			int numberOfWhole = (fullString.Length - 1) % 3 + 1;

			string number = (fullString.Length > 3) ?
				String.Join(".", fullString.Substring(0, numberOfWhole), fullString.Substring(numberOfWhole, numberOfDecimals)) :
				String.Join(".", fullString.Substring(0, numberOfWhole));

			result = number;

			if (specialSuffixIndex > 0)
			{
				if (suffixIndex == 0)
				{
					result = String.Join("", number, specialSuffixCharSet[specialSuffixIndex]); // a
				}
				else
				{
					result = String.Join("", number, specialSuffixCharSet[specialSuffixIndex], standardSuffixesSet[suffixIndex]); // aK
				}
			}
			else
			{
				result = String.Join("", number, standardSuffixesSet[suffixIndex]); // K
			}

			return result;
		}
	}
}

