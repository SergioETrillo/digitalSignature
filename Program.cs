using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyInDotNet
{
    class Program
    {
        public static string LoadJson(string file)
        {
            using (StreamReader r = new StreamReader(@"D:\Temp\" + file))
            {
                string payload = r.ReadToEnd();

                return payload;
            }
        }

        static void Main()
        {
            string jsonFile = LoadJson("license.lic");
            string tamperedFile = LoadJson("tampered.lic");

            var docTampered = Encoding.UTF8.GetBytes(tamperedFile);
            var document = Encoding.UTF8.GetBytes(jsonFile);

            byte[] hashedDocument;
            byte[] tamperedHashedDocument;

            using (var sha256 = SHA256.Create())
            {
                hashedDocument = sha256.ComputeHash(document);
            }

            using (var sha256 = SHA256.Create())
            {
                tamperedHashedDocument = sha256.ComputeHash(docTampered);
            }

            // setup keys
            var storeKey = new StoreKeyLocal("SergioBag");
            var digitalSignature = new DigitalSignature(storeKey);


            digitalSignature.UseContainer();

            var signature = digitalSignature.SignData(hashedDocument);
            var verified = digitalSignature.VerifySignature(hashedDocument, signature);
            var verifiedTampered = digitalSignature.VerifySignature(tamperedHashedDocument, signature);
            
            Console.WriteLine("Digital Signature Demonstration in .NET");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();            
            Console.WriteLine();
            Console.WriteLine("   Original Text = " + 
                Encoding.Default.GetString(document));

            Console.WriteLine();
            Console.WriteLine("   Digital Signature = " + 
                Convert.ToBase64String(signature));
            Console.WriteLine();
            Console.WriteLine("   Hash original Doc= " +
                              Convert.ToBase64String(hashedDocument));
            Console.WriteLine("   Hash tampered Doc= " +
                              Convert.ToBase64String(tamperedHashedDocument));
            Console.WriteLine();

            Console.WriteLine(verified
                ? "The digital signature has been correctly verified."
                : "The digital signature has NOT been correctly verified.");

            Console.WriteLine("Tampered document");
            Console.WriteLine(verifiedTampered
                ? "The digital signature has been correctly verified."
                : "The digital signature has NOT been correctly verified.");
            Console.ReadLine();
        }
    }
}
