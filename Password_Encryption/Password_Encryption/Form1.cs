using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Password_Encryption
{
    public partial class Form1 : Form
    {
        public const string initVector = "tu89geji340t89u2";
        public const int keysize = 256; // This constant is used to determine the keysize of the encryption algorithm.

        public Form1()
        {
            InitializeComponent();
        }

        private void Encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    //Here key is of 128 bit  
                    //Key should be either of 128 bit or of 192 bit  
                    richTextBox1.Text = EncryptText(textBox1.Text, "sblw-2hn4-sqoy19");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Encrypt Button Click" + ex.Message);
            }
        }

        private void Decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox1.Text != string.Empty)
                {
                    //Key shpuld be same for encryption and decryption  
                    MessageBox.Show("Plain Text :- " + DecryptText(richTextBox1.Text, "sblw-2hn4-sqoy19"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Decrypt Button Click" + ex.Message);
            }
        }

        public static string EncryptText(string input, string key)
        {
            try
            {
                string passPhrase = key;/////encryption Key text
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(input);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);////To encrypt
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Encrypt" + ex.Message);
                return null;
            }
        }

        public static string DecryptText(string input, string key)
        {
            try
            {
                string passPhrase = key;/////encryption Key text same 
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(input);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Decrypt" + ex.Message);
                return null;
            }
        }

    }
}
