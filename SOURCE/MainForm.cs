using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SapphTools.DefaultPrinterSelector {
    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            Regex ignoreRegex = new Regex(Properties.Resources.IgnoreKeys);
            string defaultPrinter = Printers.GetDefaultPrinter();
            var printerList =
                from string printer in Printers.GetPrinterList()
                let matches = ignoreRegex.Matches(printer)
                where matches.Count == 0
                select printer;
            PrinterListBox.Items.AddRange(printerList.ToArray<object>());
            if (printerList.Contains(defaultPrinter))
                PrinterListBox.SelectedItem = defaultPrinter;
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e) {
            Printers.SetDefaultPrinter(PrinterListBox.SelectedItem.ToString());
            if (Printers.GetDefaultPrinter() == PrinterListBox.SelectedItem.ToString())
                MessageBox.Show("Default printer updated successfully!");
            Close();
        }
    }
}
