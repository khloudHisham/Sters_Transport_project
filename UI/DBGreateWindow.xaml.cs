using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.SqlServer.Smo;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for DBGreateWindow.xaml
    /// </summary>
    public partial class DBGreateWindow : Window
    {


        public string scriptGeneratedPathmdf { get; set; }
        public string scriptGeneratedPathlog { get; set; }
        public string serverstr { get; set; }
        public string DBStr { get; set; }
        public string UsrStr { get; set; }
        public string PassStr { get; set; }

        public bool UseSqlAuth { get; set; }
        public DBGreateWindow()
        {
            InitializeComponent();
            //  scriptGeneratedPathmdf = "C:\\Program Files\\Microsoft SQL Server\\MSSQL15.SQLEXPRESS\\MSSQL\\DATA\\StersTransport_DB2.mdf";
            //  scriptGeneratedPathlog = "C:\\Program Files\\Microsoft SQL Server\\MSSQL15.SQLEXPRESS\\MSSQL\\DATA\\StersTransport_DB2_log.ldf";
            
            scriptGeneratedPathmdf = "D:\\Strs\\StersTransport_DB2.mdf";
            scriptGeneratedPathlog = "D:\\Strs\\StersTransport_DB2_log.ldf";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // schema 
             var ofdlg = new Microsoft.Win32.OpenFileDialog();

            if (ofdlg.ShowDialog() == true)
            {
                txtsc.Text = ofdlg.FileName;

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

 
      
            try
            {  // execute schema
               // string scriptFile = txtdata.Text;
                SqlConnectionStringBuilder builderentity = new SqlConnectionStringBuilder();
                builderentity.DataSource = serverstr;
                builderentity.InitialCatalog = DBStr;
                if (UseSqlAuth)
                {
                    builderentity.IntegratedSecurity = false;
                }
                else
                {
                    builderentity.IntegratedSecurity = true;
                }
                builderentity.UserID = UsrStr;
                builderentity.Password = PassStr;

                string sqlConnectionString = builderentity.ToString();


                //sqlcmd -v varMDF="C:\dev\SAMPLE.mdf" varLDF="C:\dev\SAMPLE_log.ldf"

                // first replace storage path in script
                /*
                string text = File.ReadAllText(txtsc.Text);
                string newpath = txtdest.Text + "\\" + "StersTransport_DB2.mdf";
                string newpathlog = txtdest.Text + "\\" + "StersTransport_DB2_log.ldf";
                // text = text.Replace("C:\\Program Files\\Microsoft SQL Server\\MSSQL15.SQLEXPRESS\\MSSQL\\DATA\\StersTransport_DB2.mdf", newpath);
                // text = text.Replace("C:\\Program Files\\Microsoft SQL Server\\MSSQL15.SQLEXPRESS\\MSSQL\\DATA\\StersTransport_DB2_log.ldf", newpathlog);

                text = text.Replace(scriptGeneratedPathmdf, newpath);
                text = text.Replace(scriptGeneratedPathlog, newpathlog);

                File.WriteAllText(txtsc.Text, text);
                var milliseconds = 5000;
                Thread.Sleep(milliseconds);
                */


                string newpath = txtdest.Text + "\\" + "StarsTransport_DB2.mdf";
                string newpathlog = txtdest.Text + "\\" + "StarsTransport_DB2_log.ldf";


                string strCmdText;

                /*
                strCmdText = string.Format("/C sqlcmd  -S {0} -U {1} -P {2} -i {3}", serverstr, UsrStr, PassStr, txtsc.Text);
                if (!UseSqlAuth)
                { strCmdText = string.Format("/C sqlcmd -S {0} -E -i {1}", serverstr, txtsc.Text); }
                */

                strCmdText = string.Format("/K sqlcmd  -S {0} -U {1} -P {2} -i {3} -v varMDF=\"{4}\" varLDF=\"{5}\"", serverstr, UsrStr, PassStr, txtsc.Text, newpath,newpathlog);
                if (!UseSqlAuth)
                { strCmdText = string.Format("/K sqlcmd -S {0} -E -i {1} -v varMDF=\"{2}\" varLDF=\"{3}\"", serverstr, txtsc.Text, newpath, newpathlog); }


              //  Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "CMD.EXE";
                psi.Arguments = strCmdText;
               // p.StartInfo = psi;

                Process p = Process.Start(psi);

                p.WaitForExit();

                // p.Start();
                // p.WaitForExit();


                //   System.Diagnostics.Process.Start("CMD.exe", strCmdText);


            }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
      

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            /*
            // data 
            var ofdlg = new Microsoft.Win32.OpenFileDialog();

            if (ofdlg.ShowDialog() == true)
            {
                txtdata.Text = ofdlg.FileName;

            }
            */
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                string scriptFile = txtdata.Text;
                SqlConnectionStringBuilder builderentity = new SqlConnectionStringBuilder();
                builderentity.DataSource = serverstr;
                builderentity.InitialCatalog = DBStr;
                if (UseSqlAuth)
                {
                    builderentity.IntegratedSecurity = false;
                }
                else
                {
                    builderentity.IntegratedSecurity = true;
                }
                builderentity.UserID = UsrStr;
                builderentity.Password = PassStr;

                string sqlConnectionString = builderentity.ToString();
 
                string strCmdText;
                strCmdText = string.Format("/C sqlcmd -S {0} -U {1} -P {2} -i {3}", serverstr, UsrStr, PassStr, txtdata.Text);
                if (!UseSqlAuth)
                { strCmdText = string.Format("/C sqlcmd -S {0} -E -i {1}", serverstr, txtdata.Text); }
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }

            */
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            var connstr = config.ConnectionStrings.ConnectionStrings["StersDB"].ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connstr);

            //     MessageBox.Show(string.Format("server:{0},db:{1},user:{2},pass:{3},IS:{4},App{5}",builder.DataSource,builder.InitialCatalog,builder.UserID,builder.Password,builder.IntegratedSecurity.ToString(),builder.ApplicationName));

            serverstr = builder.DataSource;
            DBStr = builder.InitialCatalog;
            UseSqlAuth= builder.IntegratedSecurity ? false : true;
            UsrStr = builder.UserID;
            PassStr= builder.Password; // need encryption methode



            try
            {
               // txtsc.Text = System.AppDomain.CurrentDomain.BaseDirectory + "Scripts\\" + "SCandDATA.sql";
              //  txtdata.Text = System.AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + "sters2Data.sql";
            }
            catch (Exception) { }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtdest.Text = fbd.SelectedPath;
                }
            }
        }

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            DBGreateWindow_Advanced dBGreateWindow_ = new DBGreateWindow_Advanced(this);
            dBGreateWindow_.ShowDialog();
        }
    }
}
