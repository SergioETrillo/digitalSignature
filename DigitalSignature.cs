﻿using System.Security.Cryptography;

namespace CryptographyInDotNet
{
    public class DigitalSignature
    {
        private RSAParameters _publicKey;
        private RSAParameters _privateKey;
        private StoreKeyLocal _storeKeyLocal;

        public DigitalSignature(StoreKeyLocal storeKeyLocal)
        {
            _storeKeyLocal = storeKeyLocal;
        }

        public void UseContainer()
        {
            _publicKey = _storeKeyLocal.GetPublicKey();
            _privateKey = _storeKeyLocal.GetPrivateKey();
        }

        public void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {                
                rsa.PersistKeyInCsp = false;               
                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);                
            }
        }

        public byte[] SignData(byte[] hashOfDataToSign)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_privateKey);
                
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);                
                rsaFormatter.SetHashAlgorithm("SHA256");

                return rsaFormatter.CreateSignature(hashOfDataToSign);
            }
        }

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.ImportParameters(_publicKey);

                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }   
    }
}