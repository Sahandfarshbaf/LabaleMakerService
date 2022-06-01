using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LabaleMakerService.Tools
{
    public class Encrypter
    {
        private byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            var ms = new MemoryStream();
            var alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;

            var cs = new CryptoStream(ms,
                alg.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(clearData, 0, clearData.Length);
            cs.Close();

            return ms.ToArray();
        }
        public string Encrypt(string clearText)
        {
            try
            {
                var Password = "mv,hkims,vnia";
                var clearBytes = Encoding.Unicode.GetBytes(clearText);

                var pdb = new PasswordDeriveBytes(Password,
                    new byte[] {(byte)'A', (byte)'-', (byte)'H', (byte)'.', (byte)'M',
                        (byte)'.', (byte)'O', (byte)'j', (byte)'v', (byte)'a', (byte)'r'});

                var encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
