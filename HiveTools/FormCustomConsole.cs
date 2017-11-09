using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HiveTools
{
    public partial class FormCustomConsole : Form
    {
        delegate void SetTextCallback(string text);
        public static bool isActive = false;
        public static FormCustomConsole FormCustomConsolePtr = null;

        private const int texboxMaxChars = 60000; // If texbox has this number of chars, delete oldest ones
        private const int texboxDeleteOffset = 1000; // It will delete this number of chars + how much it needs to display whole event


        public FormCustomConsole()
        {
            InitializeComponent();
            isActive = true;
            FormCustomConsolePtr = this; // Static pointer to last opened instance
        }

        public static void Write(string text) // Add text to console from every object thru static method
        {
            if (isActive) // If active, update text
            {
                FormCustomConsolePtr.UpdateText(text);
            }
        }

        public static void WriteLine(string text) // Add text to console from every object thru static method
        {
            if (isActive) // If active, update text
            {
                FormCustomConsolePtr.UpdateTextLn(text);
            }
        }

        public static void WriteLineWithConsole(string text) // Add text to console from every object thru static method
        {
            if (isActive) // If active, update text
            {
                FormCustomConsolePtr.UpdateTextLn(text);
            }
            Console.WriteLine(text);
        }

        private void FormCustomConsole_FormClosed(object sender, FormClosedEventArgs e)
        {
            isActive = false;
        }

        private void UpdateText(string text) // This method must not be static, and it ensures that all threads can manipulate it
        {
            if (this.textBoxConsole.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Write);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBoxConsole.Text += text;
            }
        }

        private void UpdateTextLn(string text) // This method must not be static, and it ensures that all threads can manipulate it
        {
            if (this.textBoxConsole.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(WriteLine);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBoxConsole.Text += text + "\r\n";
            }
        }

        private void textBoxConsole_TextChanged(object sender, EventArgs e)
        {
            int ptr;
            if (textBoxConsole.Text.Length > texboxMaxChars)
            {
                textBoxConsole.Text = textBoxConsole.Text.Substring(texboxDeleteOffset);
                ptr = textBoxConsole.Text.IndexOf(Environment.NewLine); // Make a clean cut, from newline
                textBoxConsole.Text = textBoxConsole.Text.Substring(ptr + 1);
            }
            // Make sure we are showing last line in textbox
            textBoxConsole.SelectionStart = textBoxConsole.Text.Length;
            textBoxConsole.ScrollToCaret();

        }

        private void buttonDiag_Click(object sender, EventArgs e)
        {
            // First update all configurations
            ConfigClass.InitializeConfiguration();

            // For now i want to print all config values
            UpdateTextLn("--------------------------------");
            UpdateTextLn("Colors are:");
            UpdateTextLn("Duplicate: " + ConfigClass.colors.colorDuplicate.ToString());
            UpdateTextLn("Invalid: " + ConfigClass.colors.colorInvalid.ToString());
            UpdateTextLn("Missing: " + ConfigClass.colors.colorMissing.ToString());
            UpdateTextLn("Null: " + ConfigClass.colors.colorNull.ToString());
            UpdateTextLn("Repeat: " + ConfigClass.colors.colorRepeat.ToString());
            UpdateTextLn("Valid: " + ConfigClass.colors.colorValid.ToString());
            UpdateTextLn("Zero: " + ConfigClass.colors.colorZero.ToString());
            UpdateTextLn("--------------------------------");

            UpdateTextLn("Delta values are :");
            UpdateTextLn("Delta current: " + ConfigClass.delta.deltaCurrent);
            UpdateTextLn("Delta voltage: " + ConfigClass.delta.deltaVoltage);
            UpdateTextLn("Zero delta current: " + ConfigClass.delta.zeroDeltaCurrent);
            UpdateTextLn("Zero delta voltage: " + ConfigClass.delta.zeroDeltaVoltage);
            UpdateTextLn("--------------------------------");

            UpdateTextLn("Other values are :");
            UpdateTextLn("Days to check in delivered energy: " + (ConfigClass.other.daysToCheck -1)); // -1 because it is stored with +1 value
            UpdateTextLn("Database wait for response timeout (in seconds): " + (ConfigClass.other.dbWaitTimeout)); // -1 because it is stored with +1 value
            UpdateTextLn("--------------------------------");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxConsole.Clear();
        }
    }
}
