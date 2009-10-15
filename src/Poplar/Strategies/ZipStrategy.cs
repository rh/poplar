using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Poplar.Strategies
{
	public class ZipStrategy : FileSystemStrategy, IDisposable
	{
		private readonly ZipOutputStream output;

		public ZipStrategy(string path)
		{
			output = new ZipOutputStream(File.Create(path));
			output.SetLevel(9);
		}

		public override DirectoryInfo Process(DirectoryInfo source, DirectoryInfo destination)
		{
			return destination;
		}

		public override FileInfo Process(FileInfo source, FileInfo destination)
		{
			var path = Context.RelativePathFor(source);
			Context.Out.WriteLine("  add       {0}", path);
			var buffer = new byte[4096];
			var entry = new ZipEntry(path);
			output.PutNextEntry(entry);

			using (var stream = source.OpenRead())
			{
				int bytes;
				do
				{
					bytes = stream.Read(buffer, 0, buffer.Length);
					output.Write(buffer, 0, bytes);
				} while (bytes > 0);
			}

			return destination;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (output != null)
				{
					output.Dispose();
				}
			}
		}
	}
}