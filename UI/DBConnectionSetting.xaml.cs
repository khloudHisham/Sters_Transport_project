using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for DBConnectionSetting.xaml
    /// </summary>
    public partial class DBConnectionSetting : Window
    {
        public DBConnectionSetting()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // read configuaration file
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
             var connstr=  config.ConnectionStrings.ConnectionStrings["StersDB"].ConnectionString;
              
           SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connstr);

            //     MessageBox.Show(string.Format("server:{0},db:{1},user:{2},pass:{3},IS:{4},App{5}",builder.DataSource,builder.InitialCatalog,builder.UserID,builder.Password,builder.IntegratedSecurity.ToString(),builder.ApplicationName));

            txtserver.Text = builder.DataSource;
            txt_database.Text = builder.InitialCatalog;
            chk_usersqlauth.IsChecked = builder.IntegratedSecurity ? false : true;
            txtsqlusername.Text = builder.UserID;
            passbx_sqluserpassword.Password = builder.Password; // need encryption methode

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnectionStringBuilder builderentity = new SqlConnectionStringBuilder();
            builderentity.DataSource = txtserver.Text;
            builderentity.InitialCatalog = txt_database.Text;
            if (chk_usersqlauth.IsChecked.HasValue)
            {
                builderentity.IntegratedSecurity = !(bool)chk_usersqlauth.IsChecked;
            }
            else
            {
                builderentity.IntegratedSecurity = true;
            }
            builderentity.UserID = txtsqlusername.Text;
            builderentity.Password = passbx_sqluserpassword.Password;
            builderentity.ApplicationName = "EntityFramework";

            //.......................................................
            SqlConnectionStringBuilder builderSQL = new SqlConnectionStringBuilder();
            builderSQL.DataSource = txtserver.Text;
            builderSQL.InitialCatalog = txt_database.Text;
            if (chk_usersqlauth.IsChecked.HasValue)
            {
                builderSQL.IntegratedSecurity = !(bool)chk_usersqlauth.IsChecked;
            }
            else
            {
                builderSQL.IntegratedSecurity = true;
            }
            builderSQL.UserID = txtsqlusername.Text;
            builderSQL.Password = passbx_sqluserpassword.Password;


            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["StersDB"].ConnectionString = builderentity.ToString();
            config.ConnectionStrings.ConnectionStrings["StersDBSQL"].ConnectionString = builderSQL.ToString();

            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");



            this.Close(); 


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DBGreateWindow dBGreateWindow = new DBGreateWindow();
            dBGreateWindow.ShowDialog();
        }
    }
}
