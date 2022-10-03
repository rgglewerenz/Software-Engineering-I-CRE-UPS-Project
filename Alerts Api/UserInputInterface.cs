using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts_Api
{
    public class UserInputInterface
    {
        public static bool AskUserConsent(string prompt)
        {
            bool worked = false;
            bool answer = false;
            do
            {
                Console.Write(prompt);
                string response = Console.ReadLine().ToLower();
                if (response.StartsWith('y') || response.StartsWith('t'))
                {
                    answer = true;
                    worked = true;
                }
                else if (response.StartsWith('n') || response.StartsWith('f'))
                {
                    answer = false;
                    worked = true;
                }
            } while (!worked);
            return answer;
        }
        public static bool AskUserConsentPrompt(string prompt, string Title, MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel, MessageBoxIcon Icon = MessageBoxIcon.Question)
        {
            var _prompt = MessageBox.Show(prompt, Title, buttons, Icon);
            if (_prompt == DialogResult.Cancel)
            {
                Console.Clear();
                Console.WriteLine("Thank you for using the application...\nPress any key to close the program");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return _prompt == DialogResult.Yes;
        }
    }
}
