using System;
using System.IO;
using System.Net;
using System.Net.Mail;

using SendFastEmail.Models;
using SendFastEmail.Enums;
using System.Collections.Generic;

namespace SendFastEmail
{
	public static class EMail
	{
		/// <summary>
		/// The method of sending the mail according to the model, the file can be added to this method by file or fileStream
		/// Aldığı modele göre mail yollama işlemini yapan metod, Bu metod'a dosya yolu ile ya da fileStream olarak dosya eklenebiliyor
		/// </summary>
		/// <param name="mailModel">Model Holding the Mail-Related Data</param>
		/// <returns> 'MailSendResult' sending a mail from a model type</returns>
		public static MailSendResult Send(MailContent mailModel)
		{
			#region Setting the mail to be sent (Yollanacak olan mailin ayarlanması)
			// To  : Mail'in kime yollanacağı listesi
			// CC  : Gönderilen Mail'den bilgisi olması istene kişilerin eklendiği liste. Mail'i alan kişi ya da kişiler bilgi için eklenen kişileri görebilir. 
			// BCC : Gönderilen Mail'den bilgisi olması istene kişilerin eklendiği liste. Mail'i alan kişi kişiler bilgi bilgi için eklenen kişileri göremez ve ayrıca To'daki kişiler cevap yazınca Bcc'deki kişelere gitmez
			MailMessage mailMessage = new MailMessage
			{
				From = mailModel.From,
				Subject = mailModel.Subject,
				Body = mailModel.Body,
				IsBodyHtml = mailModel.IsBodyHtml
			};
			mailModel.ToList.ForEach(q => mailMessage.To.Add(q)); // Multiple mail addresses can be added this way (Bu şekilde birden fazla mail adresi eklenebiliyor)
			mailModel.CCList.ForEach(q => mailMessage.CC.Add(q));
			mailModel.BccList.ForEach(q => mailMessage.Bcc.Add(q));

			#region Adding selected files to mail(Seçilen dosyaların mail'e eklenmesi)
			// The process of adding file paths (Dosya yollarından ekleme işlemi)
			if (mailModel.FilePaths != null)
			{
				foreach (string filePath in mailModel.FilePaths)
				{
					if (File.Exists(filePath))
					{
						string fileName = Path.GetFileName(filePath);
						mailMessage.Attachments.Add(new Attachment(filePath));
					}
				}
			}

			//Adding files from MemoryStream (MemoryStream'den dosya ekleme işlemi)
			if (mailModel.StreamModels != null)
			{
				foreach (StreamModel streamModel in mailModel.StreamModels)
				{
					mailMessage.Attachments.Add(new Attachment(new MemoryStream(streamModel.ByteList), streamModel.Name));
				}
			}
			#endregion

			#region Defining smtp settings (Smtp ayarlarının tanımlanması)
			SmtpClient smtp = new SmtpClient
			{
				Host = mailModel.SmtpConfiguration.Host,
				Port = mailModel.SmtpConfiguration.Port,
				EnableSsl = mailModel.SmtpConfiguration.EnableSsl,
				UseDefaultCredentials = mailModel.SmtpConfiguration.UseDefaultCredentials,
				DeliveryMethod = mailModel.SmtpConfiguration.DeliveryMethod,
				Credentials = new NetworkCredential(mailModel.Email.Trim(), mailModel.Password.Trim())
			};
			#endregion
			#endregion
			
			return Send(mailMessage, smtp);
		}

		/// <summary>
		/// If you use this method, you should give true MailMessage and SmtpClient model
		/// Main Send Method
		/// </summary>
		/// <param name="mailModel">Email Settings</param>
		/// <param name="smtp">Smtp Settings</param>
		/// <returns>Returns, Mail Send Status</returns>
		public static MailSendResult Send(MailMessage mailModel, SmtpClient smtp)
		{
			MailSendResult sendResult = null;

			try
			{
				smtp.Send(mailModel);

				// If this section does not receive an error when sending mail
				// Bu kısma geliniyorsa mail gönderirken hata alınmamıştır
				sendResult = new MailSendResult
				{
					Description = "Email Sent",
					ErrorMessage = null,
					Result = MailResult.OK
				};
			}
			catch (Exception ex)
			{
				sendResult = new MailSendResult
				{
					Result = MailResult.Error,
					Description = "An Error Occurred While Email Send!",
					ErrorMessage = ex.Message,
					Exception = ex
				};
			}

			mailModel.Dispose();
			smtp.Dispose();
			return sendResult;
		}

		/// <summary>
		/// Use this method, If you want to use string paramaters for send a email
		/// </summary>
		/// <param name="body">Mail Body</param>
		/// <param name="subject">Mail Subject</param>
		/// <param name="to">Single to Mail Adderss</param>
		/// <param name="email">Your Email Address</param>
		/// <param name="password">Your Email Password</param>
		/// <param name="host">Smtp Host Name</param>
		/// <param name="port">Smtp Port Number</param>
		/// <param name="filePaths">Attachment Files Paths List</param>
		/// <param name="streamByteList">Attachment Files Stream List</param>
		/// <param name="ccList">CC Mail Address</param>
		/// <param name="bccList">BCC Mail Address</param>
		/// <param name="isHtml">Mail Content is Html</param>
		/// <param name="enableSsl">Ssl Using</param>
		/// <param name="useDefaultCredentials"></param>
		/// <param name="smtpDeliveryMethod"></param>
		/// <returns>Returns, Mail Send Status</returns>
		public static MailSendResult Send(
			string body,
			string subject,
			string to,
			string email,
			string password,
			string host,
			int port,
			List<string> filePaths,
			List<byte[]> streamByteList,
			List<string> ccList,
			List<string> bccList,
			bool isHtml = true,
			bool enableSsl = true,
			bool useDefaultCredentials = true,
			SmtpDeliveryMethod smtpDeliveryMethod = SmtpDeliveryMethod.Network)
		{
			return Send(body, subject, new List<string> { to }, email, password, host, port, filePaths, streamByteList, ccList, bccList, isHtml, enableSsl, useDefaultCredentials, smtpDeliveryMethod);
		}


		/// <summary>
		/// Use this method, If you want to use string paramaters for send a email
		/// </summary>
		/// <param name="body">Mail Body</param>
		/// <param name="subject">Mail Subject</param>
		/// <param name="toList">Single to Mail Adderss</param>
		/// <param name="email">Your Email Address</param>
		/// <param name="password">Your Email Password</param>
		/// <param name="host">Smtp Host Name</param>
		/// <param name="port">Smtp Port Number</param>
		/// <param name="filePaths">Attachment Files Paths List</param>
		/// <param name="streamByteList">Attachment Files Stream List</param>
		/// <param name="ccList">CC Mail Address</param>
		/// <param name="bccList">BCC Mail Address</param>
		/// <param name="isHtml">Mail Content is Html</param>
		/// <param name="enableSsl">Ssl Using</param>
		/// <param name="useDefaultCredentials"></param>
		/// <param name="smtpDeliveryMethod"></param>
		/// <returns>Returns, Mail Send Status</returns>
		public static MailSendResult Send(
			string body,
			string subject,
			List<string> toList,
			string email,
			string password,
			string host,
			int port,
			List<string> filePaths,
			List<byte[]> streamByteList,
			List<string> ccList,
			List<string> bccList,
			bool isHtml = true,
			bool enableSsl = true,
			bool useDefaultCredentials = true,
			SmtpDeliveryMethod smtpDeliveryMethod = SmtpDeliveryMethod.Network)
		{
			#region Creating 'MailContent' Model
			MailContent mailContent = new MailContent
			{
				From = new MailAddress(email),
				Subject = subject,
				Body = body,
				IsBodyHtml = isHtml,
				Email = email,
				Password = password,
				SmtpConfiguration = new SmtpConfiguration
				{
					Host = host,
					Port = port,
					EnableSsl = enableSsl,
					UseDefaultCredentials = useDefaultCredentials,
					DeliveryMethod = smtpDeliveryMethod
				},
				FilePaths = filePaths
			};

			for (int i = 0; i < streamByteList.Count; i++)
			{
				mailContent.StreamModels.Add(new StreamModel
				{
					Name = $"Attachment_File_{i + 1}",
					ByteList = streamByteList[i],
				});
			}

			toList.ForEach(q => mailContent.ToList.Add(new MailAddress(q)));
			ccList.ForEach(q => mailContent.CCList.Add(new MailAddress(q)));
			bccList.ForEach(q => mailContent.BccList.Add(new MailAddress(q)));
			#endregion

			return Send(mailContent);
		}

	}
}
