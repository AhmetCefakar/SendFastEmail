using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendFastEmail.Models
{
	/// <summary>
	/// This class will be used when you want to add files to stream as streamModel
	/// Bu sınıf mail'e streamModel olarak dosya eklenmek istendiğinde kullanılacak
	/// </summary>
	public class StreamModel
	{
		public string Name { get; set; } // The name of the file, including the file extension (Dosya uzantısını da içeren dosyanın adı)
		public byte[] ByteList { get; set; }  // MemoryStream'e çevrilecek olan byte dizisi.  MemoryStream { get; set; }
	}
}
