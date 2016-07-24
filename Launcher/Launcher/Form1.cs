using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System.Globalization;
using System.Net.Sockets;

namespace Launcher
{
    
    public partial class Form1 : Form
    {
        // ОСНОВНЫЕ НАСТРОЙКИ
        const string name = "ForgottenName"; // Название вашего лаунчера
        const string secure = "http"; // https для безопасного (нужен SSL) или http для небезопасного
        const string siteaddr = "localhost"; // сайт с WWW файлами
        const string serverip = "127.0.0.1\n"; // адрес для подключения
        const string md5client = "B2B1D38C92493B4D1B8B23B0AC103BE1"; // MD5 для Dayz***.exe (Arma2OA.exe)
        const string serverport = "2302\n"; // Порт для поключения
        const int port = 2673; // Порт лаунч сервера
        const string address = "127.0.0.1"; // Адрес лаунч сервера
        const string servernames = "Something Name of Server"; // Название сервера в лаунчере
        const string Pass = ":3:Ys49-C-+=UC%7264x"; // Пароль для подключения (такой же как и у сервера)
        const string ver = "0.0.0.5"; // Версия лаунчера (такая же как и в ver.php)
        const string site = "https://vk.com/"; // Ваша группа в вк
        string mod = "@DayZ_Epoch"; // Моды для загрузки
        // КОНЕЦ ОСНОВНЫЕ НАСТРОЙКИ
        string ip;
        string message;
        const string ping = "RENDED"; 
        const string auth = "AUTH";
        const string reply = "OK";
        const string deauth = "DEAUTH";
        const string eexit = "EXIT";
        const string exitconf = "EXITCONFIMED";
        static DateTime daytoday = DateTime.Today;
        string Salt = daytoday.ToString(new CultureInfo("en-US"));
        TcpClient client = null;
        NetworkStream stream;
        public Form1()
        {
            InitializeComponent();
        }

        private void VK_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(site);
            }
            catch (Exception) { }
        }

        private string ComputeMD5Checksum(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                return result;
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            try
            {
                client.Close();
                Close();
            }
            catch (Exception) { }
        }

        private void start_Click(object sender, EventArgs e)
        {
            try
            {


                var runningProcs = from proc in Process.GetProcesses(".") orderby proc.Id select proc;
                if (runningProcs.Count(p => p.ProcessName.Contains("Dayz"+name)) > 0)
                {
                    Button button = (Button)sender;
                    button.Enabled = false;
                    MessageBox.Show("Ошибка: Вы пытаетесь запустить приложение повторно!", "Error: You try to start the application repeatedly!",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    string startgame = StartParams.Text;
                    string Arguments = " -beta=Expansion\\beta\\Expansion" + " -mod=" + mod + " -connect=" + serverip + " -port=" + serverport + " " + startgame; //
                    string pathFile = "Expansion/beta/arma2oa.exe";
                    string pathRun = "Expansion/beta/Dayz"+name+".exe";
                    if (File.Exists(pathFile))
                    {
                        if (File.Exists(pathRun))
                        {
                            message = auth;
                            message = string.Format("{0}: {1}", ip, message);
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
                            if (message == "OK")
                            {
                                try
                                {
                                    string md5 = ComputeMD5Checksum("Expansion\\beta\\Dayz"+name+".exe");
                                    if (md5 == md5client)
                                    {

                                        Process.Start("Expansion\\beta\\Dayz"+name+".exe", Arguments);
                                        this.WindowState = FormWindowState.Minimized;
                                        int isarmarun = 0, kill = 0;
                                        while (isarmarun != 1)
                                        {
                                            System.Threading.Thread.Sleep(5000);
                                            md5 = ComputeMD5Checksum("Expansion\\beta\\Dayz"+name+".exe");
                                            if (md5 != md5client) kill = 1;
                                            var runningProcswhile = from proc in Process.GetProcesses(".") orderby proc.Id select proc;
                                            if (runningProcswhile.Count(p => p.ProcessName.Contains("Wireshark")) > 0) { kill = 1; }
                                            if (runningProcswhile.Count(p => p.ProcessName.Contains("skriptexecuter2")) > 0) { kill = 1; }
                                            if (runningProcswhile.Count(p => p.ProcessName.Contains("WpePro")) > 0) { kill = 1; }
                                            if (runningProcswhile.Count(p => p.ProcessName.Contains("ArmA2OA")) > 0) { kill = 1; }
                                            if (runningProcswhile.Count(p => p.ProcessName.Contains("Dayz"+name)) > 1) { kill = 1; }
                                            message = ping;
                                            message = String.Format("{0}: {1}", ip, message);
                                            message = AESEncryption.Encrypt(message, Pass, Salt);
                                            // преобразуем сообщение в массив байтов
                                            data = Encoding.Unicode.GetBytes(message);
                                            // отправка сообщения
                                            stream.Write(data, 0, data.Length);

                                            // получаем ответ
                                            data = new byte[64]; // буфер для получаемых данных
                                            builder = new StringBuilder();
                                            bytes = 0;
                                            do
                                            {
                                                bytes = stream.Read(data, 0, data.Length);
                                                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                                            }
                                            while (stream.DataAvailable);

                                            message = builder.ToString();
                                            message = AESEncryption.Decrypt(message, Pass, Salt);
                                            if (message != "OK")
                                            {
                                                kill = 1;
                                            }
                                            string target_name = "Dayz"+name;
                                            Process[] local_procs = Process.GetProcesses();
                                            try
                                            {
                                                Process target_proc = local_procs.First(p => p.ProcessName == target_name);
                                                if (kill == 1) target_proc.Kill();
                                            }
                                            catch (InvalidOperationException)
                                            {
                                                isarmarun = 1;
                                                this.WindowState = FormWindowState.Normal;
                                            }
                                        }
                                        if (message != "OK")
                                        {
                                            MessageBox.Show("Ошибка: Не получен ответ от сервера.", "Ошибка",
                                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                        }
                                        message = deauth;
                                        message = String.Format("{0}: {1}", ip, message);
                                        message = AESEncryption.Encrypt(message, Pass, Salt);
                                        // преобразуем сообщение в массив байтов
                                        data = Encoding.Unicode.GetBytes(message);
                                        // отправка сообщения
                                        stream.Write(data, 0, data.Length);

                                        // получаем ответ
                                        data = new byte[64]; // буфер для получаемых данных
                                        builder = new StringBuilder();
                                        bytes = 0;
                                        do
                                        {
                                            bytes = stream.Read(data, 0, data.Length);
                                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                                        }
                                        while (stream.DataAvailable);

                                        message = builder.ToString();
                                        message = AESEncryption.Decrypt(message, Pass, Salt);
                                        if (message != "OK")
                                        {
                                            MessageBox.Show("Ошибка: Не получен ответ от сервера.", "Ошибка",
                                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            File.Delete("Expansion\\beta\\Dayz"+name+".exe");
                                            WebClient webClient = new WebClient();
                                            webClient.DownloadFile(secure+"://"+siteaddr+"/Dayz"+name+".exe", @"Expansion\\beta\\Dayz"+name+".exe");
                                            MessageBox.Show("Ошибка: Клиент не присутствовал и был закачен. Запустите игру заново.", "Ошибка",
                                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                        }
                                        catch (Exception) { }
                                    }


                                }
                                catch (Exception) { }
                            }
                            else
                            {
                                MessageBox.Show("Ошибка: Ошибка аунтификации.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            }
                        }
                        else
                        {
                            try
                            {
                                WebClient webClient = new WebClient();
                                webClient.DownloadFile(secure+"://"+siteaddr+"/Dayz"+name+".exe", @"Expansion\\beta\\Dayz"+name+".exe");
                                MessageBox.Show("Ошибка: Клиент не присутствовал и был закачен. Запустите игру заново.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            }
                            catch (Exception) { }
                        }
                    }
                    else
                    {
                        MessageBox.Show("/Expansion/beta/arma2oa.exe не найден. Убедитесь что лаунчер лежит в папке с игрой.", "The file isn't found",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }

                }
            }
            catch (Exception) { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            {
                try
                {
                    StartParams.Text = Properties.Settings.Default.StartParams;
                    labelServer.Text = servernames;
                }
                catch (Exception) { }
                try
                {
                    var runningProcs = from proc in Process.GetProcesses(".") orderby proc.Id select proc; // Проверяем что запущен только один лаунчер
                    if (runningProcs.Count(p => p.ProcessName.Contains(name)) > 1) {
                        if (runningProcs.Count(p => p.ProcessName.Contains("Launcher.vshost")) > 1)
                        {

                        } else
                        {
                            Close();
                        }
                            
                    }// если лаунчер запущен отменяем запуск дубликата
                    if (runningProcs.Count(p => p.ProcessName.Contains("Wireshark")) > 0) { Close(); }// если Wireshark запущен отменяем запуск
                    if (runningProcs.Count(p => p.ProcessName.Contains("skriptexecuter2")) > 0) { Close(); }
                }
                catch (Exception) { }
                try
                {
                    if (File.Exists("Update.exe"))
                    {
                        try
                        {
                            File.Delete("Update.exe");
                        }
                        catch (Exception) { }
                    }
                    string line = ver;
                    WebClient clientt = new WebClient();
                    Stream streamm = clientt.OpenRead(secure+"://"+siteaddr+"/ver.php");
                    
                    StreamReader sr = new StreamReader(streamm);
                    line = sr.ReadLine();
                    streamm.Close();
                    streamm = clientt.OpenRead(secure+"://"+siteaddr+"/ip.php");
                    sr = new StreamReader(streamm);
                    ip = sr.ReadLine();
                    streamm.Close();
                    int result = string.Compare(line, ver);
                        if (result != 0)
                        {
                            try
                            {
                                WebClient webUpdate = new WebClient();
                            webUpdate.DownloadFile(secure+"://"+siteaddr+"/Update.exe", @"Update.exe");
                                Process.Start("Update.exe");
                            }
                            catch (Exception) { }
                            Close();
                        }
                }
                catch (Exception) {
                    MessageBox.Show("Не удалось подключится к серверу.", "Проверьте подключение к интернету.",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    Close();
                }
                try
                {
                    client = new TcpClient(address, port);
                    stream = client.GetStream();
                    message = ping;
                    message = string.Format("{0}: {1}", ip, message);
                    message = AESEncryption.Encrypt(message, Pass, Salt);
                    byte[] data = Encoding.Unicode.GetBytes(message);
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
                    if (message == reply)
                    {
                        
                    }
                    else
                    {
                        MessageBox.Show("Некорректный ответ сервера", "Проверьте подключение к интернету.",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        Close();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось подключится к серверу. Проверьте подключение к интернету или время на компьютере.", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    Close();
                }
            }

        }

        private void Settings_Click(object sender, EventArgs e)
        {
            SettingsBox.Visible = true;
        }

        private void ExitSettingsCancel_Click(object sender, EventArgs e)
        {
            SettingsBox.Visible = false;
        }

        private void ExitSettings_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.StartParams = StartParams.Text;
                Properties.Settings.Default.Save();
                SettingsBox.Visible = false;
            }
            catch (Exception) { }
        }
    }
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
}
