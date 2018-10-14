using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendFastEmail.Enums;


namespace SendFastEmail.Models
{
	/// <summary>
	/// Mail yollama sırasında alınan sınucun döndürülmesi için kullanılan model
	/// </summary>
	public class MailSendResult
	{
		public MailResult Result { get; set; }
		public string Description { get; set; }
		public string ErrorMessage { get; set; }
		public Exception Exception { get; set; }
	}
}
