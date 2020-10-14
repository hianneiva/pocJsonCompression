using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace JsonCompression
{
    public class Program
    {
        private static readonly string FILE_PATH = "..\\..\\..\\payload.json";

        private static void Main(string[] args)
        {
            string jsonPayload = File.ReadAllText(FILE_PATH);

            byte[] payloadAsBytes = Encoding.UTF8.GetBytes(jsonPayload);
            Console.WriteLine($"Payload size: {payloadAsBytes.Length} bytes.\nCompressing to GZip...");

            using MemoryStream outputStream = new MemoryStream();
            using GZipStream compressionStream = new GZipStream(outputStream, CompressionMode.Compress);
            compressionStream.Write(payloadAsBytes, 0, payloadAsBytes.Length);
            compressionStream.Close();

            byte[] compressedPayload = outputStream.ToArray();

            Console.WriteLine($"Compression to GZip successfully.\nCompressed payload size: {compressedPayload.Length} bytes.\nEncoding to BASE64...");
            string encodedPayload = Convert.ToBase64String(compressedPayload);
            Console.WriteLine($"Encoded to BASE64 successfully.\nEncoded payload size: {Encoding.UTF8.GetByteCount(encodedPayload)} bytes.");

            double cToO = (double)compressedPayload.Length / payloadAsBytes.Length;
            Console.WriteLine($"\nRatio:\nCompressed to Original: { cToO.ToString("F2") }%;");
            double eToC = (double)Encoding.UTF8.GetByteCount(encodedPayload) / compressedPayload.Length;
            Console.WriteLine($"Encoded to Compressed: { eToC.ToString("F2") }%;");
            double eToO = (double)Encoding.UTF8.GetByteCount(encodedPayload) / payloadAsBytes.Length;
            Console.WriteLine($"Encoded to Original: { eToO.ToString("F2") }%;");

            byte[] decodedPayload = Convert.FromBase64String(encodedPayload);

            MemoryStream compressedStream = new MemoryStream(decodedPayload);
            GZipStream decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            MemoryStream resultStream = new MemoryStream();

            byte[] buffer = new byte[4096];
            int read;

            while ((read = decompressionStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                resultStream.Write(buffer, 0, read);
            }

            decompressionStream.Close();
            byte[] decompressedPayload = resultStream.ToArray();

            string decodedDecompressedPayload = Encoding.UTF8.GetString(decompressedPayload);

            Console.WriteLine($"Decodeding and Decompression of the Encoded/Compressed payload was successful? {decompressedPayload.Length == jsonPayload.Length}.");
        }
    }
}
