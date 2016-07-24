using LauncherServer.ConsoleServer;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using BattleNET;
using System.Linq;

namespace LauncherServer
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
        string salt, string hashAlgorithm = "SHA1",
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
        string salt, string hashAlgorithm = "SHA1",
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
    namespace ConsoleServer
    {
        public class ClientObject
        {
            public static bool debug = true; // Режим отладки
            public TcpClient client;
            string Pass = ":3:Ys49-C-+=UC%7264x"; // Пароль для соединения (Должен быть такой же как и у клиента)
            DateTime daytoday = DateTime.Today;
            public ClientObject(TcpClient tcpClient)
            {
                client = tcpClient;
            }
            static string[] Authed = new string[50];
            public static bool Isrented(string Ip)
            {
                bool iss=true;
                for (int i=0; i<=49; i++)
                {
                    if (Authed[i] != null)
                    {
                        if (Authed[i] == Ip)
                        {
                            iss = false;
                        }
                    }
                }
                return iss;
            }
            public void Process()
            {
                NetworkStream stream = null;
                int id=0;
                string ip = null;
                try
                {
                    while(Authed[id]!=null)
                    {
                        id++;
                    }
                    Authed[id] = "0.0.0.0";
                    stream = client.GetStream();
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    string Salt = daytoday.ToString(new CultureInfo("en-US")); // Динамическая соль (дата)
                    while (true)
                    {
                        // получаем сообщение
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0;
                        do
                        {
                            bytes = stream.Read(data, 0, data.Length);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (stream.DataAvailable);

                        string message = builder.ToString();
                        message = AESEncryption.Decrypt(message, Pass, Salt);
                        string[] messagemass = message.Split(new char[] { ':' });
                        ip = messagemass[0];
                        message = messagemass[1];
                        message = message.Trim(new char[] { ' ' });
                        if (debug)
                        {
                            Console.WriteLine("{0}:{1}", ip, message);
                        }
                        
                        message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
                        //Сообщение получено
                        switch (message)
                        {
                            case "DEAUTH":
                                Authed[id] = "0.0.0.0";
                                if (debug)
                                {

                                    Console.WriteLine("Деавтоаризован временно {0}", ip);
                                }
                                break;
                            case "AUTH":
                                if (debug)
                                {
                                    Console.WriteLine("Автоаризован {0}", ip);
                                }
                                Authed[id] = ip;
                                break;
                            case "EXIT":
                                break;
                            case "RENDED":
                                break;
                            default:
                                if (debug)
                                {
                                    Console.WriteLine("Неопознанно {0}", ip);
                                }
                                message = "ERROR";
                                break;
                        }
                        // отправляем обратно сообщение в верхнем регистре
                        if (message == "EXIT")
                        {
                            message = "EXITCONFIMED";
                            message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
                            message = AESEncryption.Encrypt(message, Pass, Salt);
                            data = Encoding.Unicode.GetBytes(message);
                            stream.Write(data, 0, data.Length);
                            client.Close();
                            Authed[id] = null;
                            if (debug)
                            {
                                Console.WriteLine("Деавтоаризован {0}", ip);
                            }
                            break;
                        }
                        if (message == "ERROR")
                        {
                            message = "EXITCONFIMED";
                            message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
                            message = AESEncryption.Encrypt(message, Pass, Salt);
                            data = Encoding.Unicode.GetBytes(message);
                            stream.Write(data, 0, data.Length);
                            client.Close();
                            Authed[id] = null;
                            if (debug)
                            {
                                Console.WriteLine("Деавтоаризован {0}", ip);
                            }
                            break;
                        }
                        message = "OK";
                        message = AESEncryption.Encrypt(message, Pass, Salt);
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }
                }
                catch (Exception ex)
                {
                    if (debug)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Authed[id] = null;
                    if (debug)
                    {
                        Console.WriteLine("Деавтоаризован {0} {1}", ip, id);
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                    if (client != null)
                        client.Close();
                }
            }
        }
    }
    class Program
    {
        private static void BattlEyeConnected(BattlEyeConnectEventArgs args)
        {
            //if (args.ConnectionResult == BattlEyeConnectionResult.Success) { /* Connected successfully */ }
            //if (args.ConnectionResult == BattlEyeConnectionResult.InvalidLogin) { /* Connection failed, invalid login details */ }
            //if (args.ConnectionResult == BattlEyeConnectionResult.ConnectionFailed) { /* Connection failed, host unreachable */ }

            if (args.ConnectionResult == BattlEyeConnectionResult.ConnectionFailed) { /* Connection failed, host unreachable */
                Environment.Exit(0); // Выход при неподключении
            }
        }

        private static void BattlEyeDisconnected(BattlEyeDisconnectEventArgs args)
        {
            //if (args.DisconnectionType == BattlEyeDisconnectionType.ConnectionLost) { /* Connection lost (timeout), if ReconnectOnPacketLoss is set to true it will reconnect */ }
            //if (args.DisconnectionType == BattlEyeDisconnectionType.SocketException) { /* Something went terribly wrong... */ }
            //if (args.DisconnectionType == BattlEyeDisconnectionType.Manual) { /* Disconnected by implementing application, that would be you */ }
            Environment.Exit(0); // Выход при отключении
        }
        static string[,] PlayersList = new string[50, 5];
        static string PlayersM;
        static int Players = 0;
        static string rmprobel(string ni)
        {
            bool w = true;
            while (w)
            {
                if (" ".Contains(ni[0]))
                {
                    ni = ni.Remove(0, 1);
                }
                else
                {
                    w = false;
                }
            }
            return ni;
        }
        static string readtoprobel(string ni)
        {
            int i = 0;
            bool w = true;
            string nii = ni;
            while (w)
            {
                if (true != " ".Contains(ni[0]))
                {
                    i++;
                    ni = ni.Remove(0, 1);
                }
                else
                {
                    w = false;
                }
                if (":".Contains(ni[0]))
                {
                    w = false;
                }
            }
            if (ni == null)
            {

            }
            else
            {
                nii = nii.Remove(i);
            }
            return nii;
        }
        static string rmtoprobel(string ni)
        {
            bool w = true;
            while (w)
            {
                if (true != " ".Contains(ni[0]))
                {
                    ni = ni.Remove(0, 1);
                }
                else
                {
                    w = false;
                }
            }
            return ni;
        }
        static void CleanPlayers(int p)
        {
            for (int i = 0; i <= p - 1; i++)
            {
                for (int y = 0; y <= 4; y++)
                {
                    PlayersList[i, y] = null;
                }
            }
        }
        static void FillPlayers(string ni, int p)
        {
            for (int i = 0; i <= p - 1; i++)
            {
                PlayersList[i, 0] = char.ToString(ni[0]);
                ni = ni.Remove(0, 1);
                ni = rmprobel(ni);
                PlayersList[i, 1] = readtoprobel(ni);
                ni = rmtoprobel(ni);
                ni = rmprobel(ni);
                PlayersList[i, 2] = readtoprobel(ni);
                ni = rmtoprobel(ni);
                ni = rmprobel(ni);
                PlayersList[i, 3] = readtoprobel(ni);
                ni = rmtoprobel(ni);
                ni = rmprobel(ni);
                PlayersList[i, 4] = readtoprobel(ni);
                ni = rmtoprobel(ni);
                if (i == p - 1)
                {
                    ni = null;
                }
                else
                {
                    ni = rmprobel(ni);
                }
            }
        }
        static void CheckPlayers(int p)
        {
            for (int i=0; i<=p-1; i++)
            {
                if (ClientObject.Isrented(PlayersList[i,1]))
                {
                    Console.WriteLine(string.Format("{0} {1} Не запущен!", PlayersList[i, 0], PlayersList[i,4]));
                    string msgg = "kick " + PlayersList[i, 0] + " Launcher is not running!";
                    b.SendCommand(msgg);
                }
            }
        }
        static void OnMsg ()
        {
            if (ClientObject.debug)
            {

                Console.WriteLine("Начало проверки");
            }
            try
            {
                //PlayersM = Message;//Players on server:\n[#] [IP Address]:[Port] [Ping] [GUID] [Name]\n--------------------------------------------------\n
                PlayersM = PlayersM.Remove(0, 115);//\n(2 players in total)
                Players = (int)char.GetNumericValue(PlayersM[PlayersM.Length - 19]);
                if (Players == 0) { if (ClientObject.debug) { Console.WriteLine("Нет игроков"); } }
                else
                {
                    CleanPlayers(Players);
                    PlayersM = PlayersM.Remove(PlayersM.Length - 21);//\n
                    PlayersM = PlayersM.Replace("\n", " ");//(?)
                    PlayersM = PlayersM.Replace("(?)", "");
                    PlayersM = PlayersM.Replace("(Lobby)", "");
                    PlayersM = PlayersM.Insert(PlayersM.Length, " ");
                    FillPlayers(PlayersM, Players);
                    CheckPlayers(Players);
                    if (ClientObject.debug)
                    {
                        Console.WriteLine("Проверенны игроки");
                    }
                }
            } catch(Exception)
            {
                if (ClientObject.debug)
                {
                    Console.WriteLine("Ошибка не проверенно");
                }
            }
            
        }
        public static void BattlEyeMessageReceived(BattlEyeMessageEventArgs args)
        {
            //Console.WriteLine("Получено сообщение:");
            //Console.WriteLine(args.Message);
            //Console.WriteLine("Конец сообщения.");
            if (args.Message.StartsWith("Players on server:"))
            {
                PlayersM = args.Message;
                OnMsg();
                //Thread reloadthread = new Thread(new ThreadStart(OnMsg));
                //reloadthread.Start();
            };
        }
        private static BattlEyeClient b;
        public static void bereload()
        {
            while(true)
            {
                Thread.Sleep(10000);
                b.SendCommand("Players");
                if (ClientObject.debug)
                {
                    Console.WriteLine("Отправленна перезагрузка");
                }
            }
        }
        const int port = 2673; // Порт сервера для подключений
        static TcpListener listener;
    static void Main(string[] args)
        {
            try
            {
                IPAddress host = IPAddress.Parse("127.0.0.1"); // Адрес Arma 2 OA сервера для подключения
                int sport = 2302; // Порт Arma 2 OA
                string password = "0000"; // Пароль BEServer
                BattlEyeLoginCredentials loginCredentials = new BattlEyeLoginCredentials
                {
                    Host = host,
                    Port = sport,
                    Password = password,
                };
                b = new BattlEyeClient(loginCredentials);
                b.BattlEyeMessageReceived += BattlEyeMessageReceived;
                b.BattlEyeConnected += BattlEyeConnected;
                b.BattlEyeDisconnected += BattlEyeDisconnected;
                b.ReconnectOnPacketLoss = true;
                b.Connect();
                listener = new TcpListener(IPAddress.Parse("0.0.0.0"), port); // Адрес сервера для прослушивания
                listener.Start();
                if (ClientObject.debug)
                {

                    Console.WriteLine("Ожидание подключений...");
                }
                b.SendCommand("Players");
                Thread reloadthread = new Thread(new ThreadStart(bereload));
                reloadthread.Start();
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);

                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                if (ClientObject.debug)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
       }
    }
}
