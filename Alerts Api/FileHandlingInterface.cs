using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Alerts_Api
{
    public class FileHandlingInterface
    {
        public static string AskUserForFileLocWithPrompt(string fileFiter = "CSV Files (*.csv)|*.csv")
        {
            string newFileLoc = "";
            do
            {
                Thread t = new Thread((ThreadStart)(() => {
                    OpenFileDialog saveFileDialog1 = new OpenFileDialog();

                    saveFileDialog1.Filter = fileFiter;

                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        newFileLoc = saveFileDialog1.FileName;
                    }
                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();
            } while (string.IsNullOrWhiteSpace(newFileLoc) && UserInputInterface.AskUserConsentPrompt("Would you like to try and select a file again?", "Error Unreadable file"));

            return newFileLoc;
        }
        public static string AskUserForDirectoryWithPrompt()
        {
            string newFileDirectory = "";
            do
            {
                Thread t = new Thread((ThreadStart)(() => {
                    FolderBrowserDialog saveFileDialog1 = new FolderBrowserDialog();

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        newFileDirectory = saveFileDialog1.SelectedPath;
                    }
                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();
            } while (string.IsNullOrWhiteSpace(newFileDirectory) && UserInputInterface.AskUserConsent("Would you like to try again? (y/n): "));

            return newFileDirectory;
        }
        public static string AskUserForNewFileLocWithPrompt(string defaultName, string fileFilter = "CSV Files (*.csv)|*.csv")
        {
            if (!fileFilter.Contains("."))
            {
                throw new ArgumentException("File filer must be in this format:\n Name (*.file_type)|*.file_type");
            }
            string newFileLoc = "";
            do
            {
                Thread t = new Thread((ThreadStart)(() => {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    saveFileDialog1.Filter = fileFilter;
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.Title = "Save file as";
                    saveFileDialog1.FileName = defaultName;
                    saveFileDialog1.RestoreDirectory = true;

                    var initalLoc = ApplicationGetLastOpenSavePath();
                    if (!String.IsNullOrEmpty(initalLoc) && File.Exists(initalLoc + "\\" +defaultName))
                    {
                        int i = 1;
                        do
                        {
                            var ext = Path.GetExtension(defaultName);
                            var name = Path.GetFileNameWithoutExtension(defaultName);
                            name += $" ({i}){ext}";
                            i++;
                            saveFileDialog1.FileName = name;
                        } while (File.Exists(saveFileDialog1.FileName));
                    }
                    
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        newFileLoc = saveFileDialog1.FileName;

                    }
                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();
            } while (string.IsNullOrWhiteSpace(newFileLoc) && UserInputInterface.AskUserConsentPrompt("Would you like to try and select a file again?", "Error Unreadable file"));
            //Checks for user file types
            if (!newFileLoc.Contains('.'))
            {
                var item = fileFilter.Split("|");
                var file_type = item[1].Split(".")[1];
                newFileLoc += file_type;
            }

            return newFileLoc;
        }

        #region Dll/Last Saved Path
        
        private static string ApplicationGetLastOpenSavePath()
        {
            string lastVisitedPath = string.Empty;
            var lastVisitedKey = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\LastVisitedPidlMRU", false);
            string[] values = lastVisitedKey.GetValueNames();
            var executableName = System.AppDomain.CurrentDomain.FriendlyName + ".exe";
            foreach (string value in values)
            {
                if (value == "MRUListEx") continue;
                var keyValue = (byte[])lastVisitedKey.GetValue(value);

                string appName = Encoding.Unicode.GetString(keyValue, 0, executableName.Length * 2);
                
                if (!appName.ToLower().Equals(executableName.ToLower())) continue;

                int offset = executableName.Length * 2 + "\0\0".Length;
                lastVisitedPath = GetPathFromIDList(keyValue, offset);
                break;
            }
            return lastVisitedPath;
        }
        private static string GetPathFromIDList(byte[] idList, int offset)
        {
            int buffer = 520;  // 520 = MAXPATH * 2
            var sb = new StringBuilder(buffer);

            IntPtr ptr = Marshal.AllocHGlobal(idList.Length);
            Marshal.Copy(idList, offset, ptr, idList.Length - offset);

            // or -> bool result = SHGetPathFromIDListW(ptr, sb);
            bool result = SHGetPathFromIDListEx(ptr, sb, buffer, GPFIDL_FLAGS.GPFIDL_UNCPRINTER);
            Marshal.FreeHGlobal(ptr);
            return result ? sb.ToString() : string.Empty;
        }

        [DllImport("shell32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool SHGetPathFromIDListW(
            IntPtr pidl,
            [MarshalAs(UnmanagedType.LPTStr)]
    StringBuilder pszPath);
        [DllImport("shell32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool SHGetPathFromIDListA(
            IntPtr pidl,
            [MarshalAs(UnmanagedType.LPTStr)]
    StringBuilder pszPath);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool SHGetPathFromIDListEx(
            IntPtr pidl,
            [MarshalAs(UnmanagedType.LPTStr)]
    [In,Out] StringBuilder pszPath,
            int cchPath,
            GPFIDL_FLAGS uOpts);

        internal enum GPFIDL_FLAGS : uint
        {
            GPFIDL_DEFAULT = 0x0000,
            GPFIDL_ALTNAME = 0x0001,
            GPFIDL_UNCPRINTER = 0x0002
        }

        #endregion Dll/Last Saved Path
    }
}
