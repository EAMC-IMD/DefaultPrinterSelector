using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SapphTools.DefaultPrinterSelector {
    public static class Printers {

        private const int ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_INSUFFICIENT_BUFFER = 122;

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int pcchBuffer);

        public static string GetDefaultPrinter() {
            int pcchBuffer = 0;
            if (GetDefaultPrinter(null, ref pcchBuffer))
                return null;
            int lastWin32Error = Marshal.GetLastWin32Error();
            if (lastWin32Error == ERROR_INSUFFICIENT_BUFFER) {
                StringBuilder pszBuffer = new StringBuilder(pcchBuffer);
                if (GetDefaultPrinter(pszBuffer, ref pcchBuffer))
                    return pszBuffer.ToString();
                lastWin32Error = Marshal.GetLastWin32Error();
            }

            if (lastWin32Error == ERROR_FILE_NOT_FOUND)
                return null;
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static IEnumerable<string> GetPrinterList() {
            return PrinterSettings.InstalledPrinters.OfType<string>();
        }
    }
}
