using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Launcher_Installer.Properties;

namespace Launcher_Installer
{
    public partial class Form1 : Form
    {
        const string name = "ForgottenName"; // Название сервера. Должно совпадать с названием в лаунчере.

        const string md5client = "B2B1D38C92493B4D1B8B23B0AC103BE1";
            // Сумма MD5 для Resources/Dayz.exe (Исполняемый файл из папки с игрой (Arma2OA.exe))

        const string md5launch = "B2B46ECBFB552AD72E5BF2D1B524D108";
            // Сумма MD5 для Resources/Launcher.exe (Скомпилированный лаунчер)

        const string md5config = "FBB6946E62C4D4CE96017BD917145E13";
            // Сумма MD5 для Resources/Launcher.exe.config (conf фаил из папки с лаунчером)

        string path = "Выберите путь.";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = path;
            checkBox1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show(FBD.SelectedPath);
                path = FBD.SelectedPath;
                textBox1.Text = path;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            path = textBox1.Text;
        }

        private static bool IsAdmin()
        {
            var id = WindowsIdentity.GetCurrent();
            var p = new WindowsPrincipal(id);

            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private string ComputeMD5Checksum(string path)
        {
            using (var fs = File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                var fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int) fs.Length);
                var checkSum = md5.ComputeHash(fileData);
                var result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                return result;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (IsAdmin())
            {
                if (File.Exists(path + "/Expansion/beta/arma2oa.exe"))
                {
                    if (File.Exists(path + "/Expansion/beta/Dayz" + name + ".exe"))
                    {
                        try
                        {
                            File.Delete(path + "/Expansion/beta/Dayz" + name + ".exe");
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Не удалось удалить фаил Dayz" + name + ".exe",
                                "Запустите программу с правами Администратора!",
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            Close();
                        }
                    }
                    if (File.Exists(path + "/" + name + ".exe"))
                    {
                        try
                        {
                            File.Delete(path + "/" + name + ".exe");
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Не удалось удалить фаил " + name + ".exe",
                                "Запустите программу с правами Администратора!",
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            Close();
                        }
                    }
                    if (File.Exists(path + "/" + name + ".exe.config"))
                    {
                        try
                        {
                            File.Delete(path + "/" + name + ".exe.config");
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Не удалось удалить фаил " + name + ".exe.config",
                                "Запустите программу с правами Администратора!",
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            Close();
                        }
                    }
                    //___________________________НАЧАЛО ЗАПИСИ
                    try
                    {
                        File.WriteAllBytes(path + "/" + name + ".exe", Resources.Launcher);
                        File.WriteAllBytes(path + "/Expansion/beta/Dayz" + name + ".exe", Resources.DayzLauncher);
                        File.WriteAllText(path + "/" + name + ".exe.config", Resources.Launcher_exe);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Не удалось записать фаилы", "Запустите программу с правами Администратора!",
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        Close();
                    }

                    try
                    {
                        var md5 = ComputeMD5Checksum(path + "/Expansion/beta/Dayz" + name + ".exe");
                        var md55 = ComputeMD5Checksum(path + "/" + name + ".exe");
                        var md555 = ComputeMD5Checksum(path + "/" + name + ".exe.config");
                        if (md5 == md5client && md55 == md5launch && md555 == md5config)
                        {
                            MessageBox.Show("Приятной игры", "Установка завершена",
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            if (checkBox1.Checked == true)
                            {
                                try
                                {
                                    var pat5h1 = path + "/" + name + ".exe";
                                    var ssadar = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" +
                                                 name + ".lnk";
                                    ShortCut.Create(pat5h1, ssadar, "", "Запустить " + name);
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Не создать ярлык", "Запустите программу с правами Администратора!",
                                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Проверка не пройдена", "Установщик повреждён",
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            Close();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Проверка не пройдена", "Запустите программу с правами Администратора!",
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        Close();
                    }
                    //___________________________КОНЕЦ ЗАПИСИ
                }
                else
                {
                    MessageBox.Show("/Expansion/beta/arma2oa.exe не найден.",
                        "Убедитесь что вы выбрали правильный путь.",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            else
            {
                MessageBox.Show("Нет прав Администратора!", "Запустите программу с правами Администратора!",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                Close();
            }
        }
    }

    static class ShellLink
    {
        internal static IShellLinkW CreateShellLink()
        {
            return (IShellLinkW) (new shl_link());
        }

        [ComImport,
         Guid("000214F9-0000-0000-C000-000000000046"),
         InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IShellLinkW
        {
            [PreserveSig]
            int GetPath(
                [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
                int cch, ref IntPtr pfd, uint fFlags);

            [PreserveSig]
            int GetIDList(out IntPtr ppidl);

            [PreserveSig]
            int SetIDList(IntPtr pidl);

            [PreserveSig]
            int GetDescription(
                [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cch);

            [PreserveSig]
            int SetDescription(
                [MarshalAs(UnmanagedType.LPWStr)] string pszName);

            [PreserveSig]
            int GetWorkingDirectory(
                [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cch);

            [PreserveSig]
            int SetWorkingDirectory(
                [MarshalAs(UnmanagedType.LPWStr)] string pszDir);

            [PreserveSig]
            int GetArguments(
                [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cch);

            [PreserveSig]
            int SetArguments(
                [MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

            [PreserveSig]
            int GetHotkey(out ushort pwHotkey);

            [PreserveSig]
            int SetHotkey(ushort wHotkey);

            [PreserveSig]
            int GetShowCmd(out int piShowCmd);

            [PreserveSig]
            int SetShowCmd(int iShowCmd);

            [PreserveSig]
            int GetIconLocation(
                [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cch, out int piIcon);

            [PreserveSig]
            int SetIconLocation(
                [MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

            [PreserveSig]
            int SetRelativePath(
                [MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

            [PreserveSig]
            int Resolve(IntPtr hwnd, uint fFlags);

            [PreserveSig]
            int SetPath(
                [MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        [ComImport,
         Guid("00021401-0000-0000-C000-000000000046"),
         ClassInterface(ClassInterfaceType.None)]
        private class shl_link
        {
        }
    }

    public static class ShortCut
    {
        public static void Create(
            string PathToFile, string PathToLink,
            string Arguments, string Description)
        {
            var shlLink = ShellLink.CreateShellLink();

            Marshal.ThrowExceptionForHR(shlLink.SetDescription(Description));
            Marshal.ThrowExceptionForHR(shlLink.SetPath(PathToFile));
            Marshal.ThrowExceptionForHR(shlLink.SetArguments(Arguments));

            ((IPersistFile) shlLink).Save(PathToLink, false);
        }
    }
}