using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendFastEmail.Models
{
	public class SmtpConfiguration
	{
		public string Host { get; set; }

		public int Port { get; set; }

		public bool EnableSsl { get; set; }
		
		public bool UseDefaultCredentials { get; set; }

		public System.Net.Mail.SmtpDeliveryMethod DeliveryMethod { get; set; }
	}
}
