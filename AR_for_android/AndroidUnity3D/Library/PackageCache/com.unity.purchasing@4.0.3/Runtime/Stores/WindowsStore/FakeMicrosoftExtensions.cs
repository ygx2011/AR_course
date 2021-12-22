using System;
using UnityEngine.Purchasing;

namespace UnityEngine.Purchasing
{
	internal class FakeMicrosoftExtensions : IMicrosoftExtensions
	{
		public void RestoreTransactions()
		{
			return;
		}
	}
}
