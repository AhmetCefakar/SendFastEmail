using SendFastEmail.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace SendFastEmail
{
	public static class EMailValidator
	{
		/// <summary>
		/// This method checks the e-mail address given and returns a bool value.
		/// </summary>
		/// <param name="eMailAddress"></param>
		/// <returns>Boolean value</returns>
		public static bool IsValid(string eMailAddress)
		{
			try
			{
				MailAddress m = new MailAddress(eMailAddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		/// <summary>
		/// This method returns a list of valid and non-valid email addresses
		/// </summary>
		/// <param name="eMailAddressList"></param>
		/// <returns>This return object have valid email adresses list and invalid email adresses list</returns>
		public static EMailValidateResult IsValid(List<string> eMailAddressList)
		{
			EMailValidateResult result = new EMailValidateResult();

			// Doğru ve yanlış mail adreslerinin listelere kelenmesi işlemleri
			for (int i = 0; i < eMailAddressList.Count; i++)
			{
				if (IsValid(eMailAddressList[i]))
				{
					result.ValidEMailList.Add(eMailAddressList[i]);
				}
				else
				{
					result.InValidEMailList.Add(eMailAddressList[i]);
				}
			}
			return result;
		}

	}
}
