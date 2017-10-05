using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Policy;
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

        public static byte[] Encode(string file)
        {
            return Encoding.UTF8.GetBytes(LoadJson(file));
        }

        public static byte[] Hash(byte[] data)
        {
            return SHA256.Create().ComputeHash(data);
        }

        public static byte[] SignData(byte[] hashedData, DigitalSignature digitalSignature)
        {
            digitalSignature.UseContainer();

            return digitalSignature.SignData(hashedData);
        }

        static void Main()
        {
            // setup keys
            var storeKey = new StoreKeyLocal("SergioBag");
            var digitalSignature = new DigitalSignature(storeKey);

            var hashDocument = Hash(Encode("license.lic"));
            var tamperedHashedDocument = Hash(Encode("tampered.lic"));

            var signature = SignData(hashDocument, digitalSignature);


            var verified = digitalSignature.VerifySignature(hashDocument, signature);
            var verifiedTampered = digitalSignature.VerifySignature(tamperedHashedDocument, signature);
            
            Console.WriteLine("Digital Signature Demonstration in .NET");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();            
            Console.WriteLine();
            Console.WriteLine("   Original Text = " + 
                Encoding.Default.GetString(Encode("license.lic")));

            Console.WriteLine();
            Console.WriteLine("   Digital Signature = " + 
                Convert.ToBase64String(signature));
            Console.WriteLine();
            Console.WriteLine("   Hash original Doc= " +
                              Convert.ToBase64String(hashDocument));
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
