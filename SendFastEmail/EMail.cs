using System;
using System.IO;
using System.Net;
using System.Net.Mail;

using SendFastEmail.Models;
using SendFastEmail.Enums;

namespace SendFastEmail
{
	public static class EMail
	{
		/// <summary>
		/// Aldığı modele göre mail yollama işlemini yapan metod, Bu metod ile dosya yolu ile ya da fileStream olarak dosya eklenebiliyor
		/// </summary>
		/// <param name="mailModel">Gönderilecek Olan Maille İlgili Verileri Tutan Model</param>
		/// <returns> 'MailSendResult' modeli tipinden mail gönderme işlemi sonucunu</returns>
		public static MailSendResult Send(MailContent mailModel)
		{
			MailSendResult sendResult = null;

			try
			{
				// Yollanacak olan mail'in ayarlanması
				// To  : Mail'in kime yollanacağı listesi
				// CC  : Gönderilen Mail'den bilgisi olması istene kişilerin eklendiği liste. Mail'i alan kişi ya da kişiler bilgi için eklenen kişileri görebilir. 
				// BCC : Gönderilen Mail'den bilgisi olması istene kişilerin eklendiği liste. Mail'i alan kişi kişiler bilgi bilgi için eklenen kişileri göremez ve ayrıca To'daki kişiler cevap yazınca Bcc'deki kişelere gitmez
				using (MailMessage mailMessage = new MailMessage())
				{
					mailMessage.From = mailModel.From;
					mailModel.ToList.ForEach(q => mailMessage.To.Add(q)); // Bu şekilde birden fazla mail adresi eklene biliyor
					mailModel.CCList.ForEach(q => mailMessage.CC.Add(q));
					mailModel.BccList.ForEach(q => mailMessage.Bcc.Add(q));
					mailMessage.Subject = mailModel.Subject;
					mailMessage.Body = mailModel.Body;
					mailMessage.IsBodyHtml = mailModel.IsBodyHtml;

					#region Seçilen dosyaların mail'e eklenmesi
					// Dosya yollarından ekleme işlemi
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

					// MemoryStream'den dosya ekleme işlemi
					if (mailModel.StreamModels != null)
					{
						foreach (StreamModel streamModel in mailModel.StreamModels)
						{
							mailMessage.Attachments.Add(new Attachment(new MemoryStream(streamModel.ByteList), streamModel.Name));
						}
					}
					#endregion

					#region Smtp ayarlarının tanımlanması. İstenirse Veritabanından çekilip smtp ayarları tanımlanabilir
					SmtpClient smtp = new SmtpClient
					{
						Host = mailModel.SmtpConfiguration.Host,
						Port = mailModel.SmtpConfiguration.Port,
						EnableSsl = mailModel.SmtpConfiguration.EnableSsl,
						UseDefaultCredentials = mailModel.SmtpConfiguration.UseDefaultCredentials,
						DeliveryMethod = mailModel.SmtpConfiguration.DeliveryMethod
					};
					#endregion

					smtp.Credentials = new NetworkCredential(mailModel.Email.Trim(), mailModel.Password.Trim());
					smtp.Send(mailMessage);

					// Bu kısma geliniyorsa mail gönderirken hata alınmamıştır
					sendResult = new MailSendResult
					{
						Description = "Email Sent",
						ErrorMessage = null,
						Result = MailResult.OK
					};
				}
			}
			catch (Exception ex)
			{
				sendResult = new MailSendResult
				{
					Result = MailResult.Error,
					Description = "An Error Occurred  While Email Send!",
					ErrorMessage = ex.Message,
					Exception = ex
				};

			}
			return sendResult;
		}

		/// <summary>
		/// If you use this method, you should give true MailMessage and SmtpClient model
		/// </summary>
		/// <param name="mailModel">Email Settings</param>
		/// <param name="smtp">Smtp Settings</param>
		/// <returns></returns>
		public static MailSendResult Send(MailMessage mailModel, SmtpClient smtp)
		{
			MailSendResult sendResult = null;

			try
			{
				smtp.Send(mailModel);

				// Bu kısma geliniyorsa mail gönderirken hata alınmamıştır
				sendResult = new MailSendResult
				{
					Description = "Email Sent",
					ErrorMessage = null,
					Result = Enums.MailResult.OK
				};
			}
			catch (Exception ex)
			{
				sendResult = new MailSendResult
				{
					Result = Enums.MailResult.Error,
					Description = "An Error Occurred While Email Send!",
					ErrorMessage = ex.Message,
					Exception = ex
				};
			}
			return sendResult;
		}
		
	}
}
