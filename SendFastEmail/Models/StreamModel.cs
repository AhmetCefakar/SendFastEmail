using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendFastEmail.Models
{
	/// <summary>
	/// Bu sınıf mail'e streamModel olarak dosya eklenmek istendiğinde kullanılacak.
	/// 'Name' alanı dosya uzantısını da içermelidir
	/// </summary>
	public class StreamModel
	{
		public string Name { get; set; }
		public byte[] ByteList { get; set; }  // MemoryStream'e çevrilecek olan byte dizisi.  MemoryStream { get; set; }
	}
}
