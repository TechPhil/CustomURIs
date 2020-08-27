using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

namespace URIEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            string xmlPath = Application.StartupPath + "/Links.xml";

            try
            {
                RegisterApplication(args[0], xmlPath);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("This is your first runtime. In order to register the application, please run as administrator", "First runtime", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            if (args.Length == 1)
            {
                MessageBox.Show("ShortURL not provided! Run goto://<shorturl>", "No ShortURL entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            //MessageBox.Show(xmlPath);
            string[] arguments;
            if (args[1].Contains("://"))
            {
                string tempStr = args[1].Substring(0, (args[1].Length) - 1);
                arguments = tempStr.Split(new string[] { "://" }, StringSplitOptions.None);
            }
            else
            {
                arguments = args[1].Split(':');
            }

            
            
            //MessageBox.Show(arguments[1]);

            

            if (arguments[1] == "edit") // if we run goto://edit
            {
                OpenEditor(xmlPath);
            } else
            {
                HandleShortLink(arguments[1],xmlPath);
            }
        }

        static void RegisterApplication(string myAppPath,string xmlPath)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("goto");

            if (key == null)
            {
                key = Registry.ClassesRoot.CreateSubKey("goto");
                key.SetValue(string.Empty, "URL: goto Protocol");
                key.SetValue("URL Protocol", string.Empty);

                key = key.CreateSubKey(@"shell\open\command");
                key.SetValue(string.Empty, Application.ExecutablePath + " " + "%1");
                MessageBox.Show("Successfully registered URL Protocol 'goto'. Run goto:edit to edit ShortURLs", "Successfully Registered", MessageBoxButtons.OK, MessageBoxIcon.Information);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Properties.Resources.Template);
                doc.Save(xmlPath);
            }

            key.Close();
            
        }


        static void OpenEditor(string xmlPath)
        {
            //MessageBox.Show("Opening editor");
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = xmlPath
            };

            process.Start();
        }

        static void HandleShortLink(string shortlink,string xmlPath)
        {
            //MessageBox.Show("Handling shortlink " + shortlink);
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            //MessageBox.Show(doc.ToString());
            bool foundLink = false;
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (foundLink)
                {
                    break;
                }
                string shorturl = node.FirstChild.InnerText;
                //MessageBox.Show(shorturl);
                if (shortlink == shorturl)
                {
                    //MessageBox.Show("FOUND IT");
                    string url = node.LastChild.InnerText;
                    //MessageBox.Show(url);
                    var process = new Process();
                    process.StartInfo = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = url
                    };

                    process.Start();
                    foundLink = true;
                }
            }
            if (!foundLink)
            {
                MessageBox.Show("ShortURL not found! Run goto://edit to view and edit ShortURLs", "ShortURL not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
