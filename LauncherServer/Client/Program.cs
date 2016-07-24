using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Client
{
    /*usage 
string Orig = "GOGOGOFFF";
            string Pass = "parolAAAA";
            string Encrypt = AESEncryption.Encrypt(Orig, Pass);
            string Decrypt = AESEncryption.Decrypt(Encrypt, Pass);
            Console.WriteLine(Orig);
            Console.WriteLine(Encrypt);
            Console.WriteLine(Decrypt);*/
    class AESEncryption
    {
        ///  /// Encrypts a string /// 
        ///Text to be encrypted
        ///Password to encrypt with
        ///Salt to encrypt with
        ///Can be either SHA1 or MD5
        ///Number of iterations to do
        ///Needs to be 16 ASCII characters long
        ///Can be 128, 192, or 256
        /// An encrypted string
        public static string Encrypt(string plainText, string password,
        string salt = "Kosher", string hashAlgorithm = "SHA1",
        int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY",
        int keySize = 256)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes derivedPassword = new PasswordDeriveBytes
             (password, saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor
            (keyBytes, initialVectorBytes))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream
                             (memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }

        ///  /// Decrypts a string /// 
        ///Text to be decrypted
        ///Password to decrypt with
        ///Salt to decrypt with
        ///Can be either SHA1 or MD5
        ///Number of iterations to do
        ///Needs to be 16 ASCII characters long
        ///Can be 128, 192, or 256
        /// A decrypted string
        public static string Decrypt(string cipherText, string password,
        string salt = "Kosher", string hashAlgorithm = "SHA1",
        int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY",
        int keySize = 256)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes derivedPassword = new PasswordDeriveBytes
            (password, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;

            using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor
                     (keyBytes, initialVectorBytes))
            {
                using (MemoryStream memStream = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream
                    = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }

    }
    class Program
    {
        const int port = 2673; // Порт для ожидания подключений (не забудте пробросить его!)
        const string address = "127.0.0.1"; // Аддрес для подключения
        static void Main(string[] args)
        {
            string Pass = ":3:Ys49-C-+=UC%7264x"; // Пароль для связи (Должен бать такой же как и у сервера)
            DateTime daytoday = DateTime.Today;
            string Salt = daytoday.ToString(new CultureInfo("en-US")); // Динамическая соль (дата)
            string userName = "0.0.0.0"; // Пользователь по умолчанию
            Console.Write("Автомат или вручную? да/нет");
            string yes = Console.ReadLine();
            if(yes == "да")
            {
                WebClient clientt = new WebClient();
                Stream streamm = clientt.OpenRead("https://localhost/ip.php"); // HTTP если нет сертификата

                StreamReader sr = new StreamReader(streamm);
                userName = sr.ReadLine();

                streamm.Close();
            }
            else if (yes == "нет")
            {
                userName = Console.ReadLine();
            } else
            {
                Console.Write("да/нет");
                System.Threading.Thread.Sleep(2000);
                Environment.Exit(0);
            }
            
            
            Console.Write("IP:{0}", userName);
            Console.ReadLine();
            TcpClient client = null;
            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    Console.Write(userName + ": ");
                    // ввод сообщения
                    string message = Console.ReadLine();
                    message = String.Format("{0}: {1}", userName, message);
                    message = AESEncryption.Encrypt(message, Pass, Salt);
                    // преобразуем сообщение в массив байтов
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    // отправка сообщения
                    stream.Write(data, 0, data.Length);

                    // получаем ответ
                    data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    message = builder.ToString();
                    message = AESEncryption.Decrypt(message, Pass, Salt);
                    Console.WriteLine("Сервер: {0}", message);
                    if (message == "EXITCONFIMED")
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
