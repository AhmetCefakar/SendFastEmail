using System;
using System.Collections.Generic;
using System.Text;

namespace SendFastEmail.Models
{
	/// <summary>
	/// This class keeps a list of valid and non-valid email addresses
	/// Bu sınıf geriye geçerli ve geçerli olmayan email adreslerinin listesini tutar
	/// </summary>
	public class EMailValidateResult
	{
		public EMailValidateResult()
		{
			ValidEMailList = new List<string>();
			InValidEMailList = new List<string>();
		}

		public List<string> ValidEMailList;
		public List<string> InValidEMailList;
	}
}
