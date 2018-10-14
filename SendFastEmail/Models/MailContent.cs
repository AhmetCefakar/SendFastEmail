using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SendFastEmail.Models
{
	/// <summary>
	/// Bu sınıf gönderilecek olan mailin modelini tutmaktadır
	/// </summary>
	public class MailContent
	{
		public MailContent()
		{
			FilePaths = new List<string>();
			StreamModels = new List<StreamModel>();
			ToList = new List<MailAddress>();
			CCList = new List<MailAddress>();
			BccList = new List<MailAddress>();
		}

		// Mail ile ilgili parametreler
		public MailAddress From { get; set; }
		public List<MailAddress> ToList { get; set; }
		public List<MailAddress> CCList { get; set; }
		public List<MailAddress> BccList { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public bool IsBodyHtml { get; set; }

		public List<string> FilePaths { get; set; } // Mail'e eklenecek olan dosyaların yollarını tutan liste
		public List<StreamModel> StreamModels { get; set; } // Mail'e eklenecek olan memoryStream'leri tutan liste

		public SmtpConfiguration SmtpConfiguration { get; set; } // Smtp ile ilgili ayarlar bu sınıfın içerisinde tutuluyor

		// Mail address-Password
		public string Email { get; set; }
		public string Password { get; set; }

	}
}
