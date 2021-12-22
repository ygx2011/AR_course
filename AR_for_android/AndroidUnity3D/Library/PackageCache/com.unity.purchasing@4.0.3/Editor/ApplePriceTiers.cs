namespace UnityEditor.Purchasing
{
	internal static class ApplePriceTiers
	{
		internal const int kNumTiers = 88;

		// Cache
		private static string [] s_Strings;
		private static int [] s_Dollars;

		internal static string [] Strings {
			get {
				GenerateAppleTierData ();
				return s_Strings;
			}
		}

		internal static int [] RoundedDollars {
			get {
				GenerateAppleTierData ();
				return s_Dollars;
			}
		}

		internal static double ActualDollarsForAppleTier (int tier)
		{
		    if (RoundedDollars[tier] == 0)
		        return 0;
		    
			return RoundedDollars[tier] - 0.01;
		}

		private static void GenerateAppleTierData ()
		{
			if (s_Strings == null || s_Dollars == null) {
				s_Strings = new string [kNumTiers];
				s_Dollars = new int [kNumTiers];

				var i = 0;
				s_Dollars [i] = 0;
				s_Strings [i++] = "Free";

				var dollars = 1;
				for (; i < kNumTiers; ++i) {
					if (i == 63) {
						s_Strings [i] = CreateApplePriceTierString (i, 125);
						s_Dollars [i] = 125;
					} else if (i == 69) {
						s_Strings [i] = CreateApplePriceTierString (i, 175);
						s_Dollars [i] = 175;
					} else {
						s_Strings [i] = CreateApplePriceTierString (i, dollars);
						s_Dollars [i] = dollars;

						if (i >= 82) { // 82 - 87 USD $100 increments to $1000
							dollars += 100;
						} else if (i >= 77) { // 77 - 82 USD $50 increments to $500
							dollars += 50;
						} else if (i >= 60) { // 60 - 77 $10 increments to $250, except 63 = $125 and 69 = $175
							dollars += 10;
						} else if (i >= 50) { // 50 - 59 USD $5 increments
							dollars += 5;
						} else { // 1 - 49 USD $1 increments
							dollars++;
						}
					}
				}
			}
		}

		private static string CreateApplePriceTierString (int tier, int roundedDollars)
		{
			return string.Format ("Tier {0} - USD {1:0.00}", tier, (float)roundedDollars - 0.01f);
		}
	}
}
