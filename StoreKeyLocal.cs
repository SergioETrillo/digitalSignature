using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace CryptographyInDotNet
{
    public class StoreKeyLocal
    {
        private string containerName;
        public StoreKeyLocal(string containerName)
        {
            this.containerName = containerName;
        }
        public void GenKey_SaveInContainer()
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container MyKeyContainerName.  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

            // Display the key information to the console.  
            Console.WriteLine("Key added to container: \n  {0}", rsa.ToXmlString(true));
        }

        public void GetKeyFromContainer()
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container MyKeyContainerName.  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

            // Display the key information to the console.  
            Console.WriteLine("Key retrieved from container : \n {0}", rsa.ToXmlString(true));
        }

        public RSAParameters GetPublicKey()
        {
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;

            var rsa = new RSACryptoServiceProvider(cp);

            return rsa.ExportParameters(false);
        }

        public RSAParameters GetPrivateKey()
        {
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;

            var rsa = new RSACryptoServiceProvider(cp);

            return rsa.ExportParameters(true);
        }

        public void DeleteKeyFromContainer()
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container.  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

            // Delete the key entry in the container.  
            rsa.PersistKeyInCsp = false;

            // Call Clear to release resources and delete the key from the container.  
            rsa.Clear();

            Console.WriteLine("Key deleted.");
        }
    }
}
