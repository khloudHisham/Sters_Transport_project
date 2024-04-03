using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;



using StersTransport.Models;
using StersTransport.UIModels;
using StersTransport.DataAccess;
using StersTransport.GlobalData;
using System.Windows.Media.Animation;
using StersTransport.Enumerations;
using System.Windows.Controls.Primitives;

namespace StersTransport
{



   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool LogoutIsCalled = false;

        object original_Darkmodebackground;
        object original_lightmodebackground;
        object original_LabelTitleForeColorDarkmode;
        object original_LabelTitleForeColor;


        object original_lightmodebackgroundUpperPart;
        object original_lightmodeforegroundUpperPart;
        object original_LabelTitleForeColorUpper;
        object original_LabelTitleForeColorUpperHover;
        object original_LabelTitleForeColorUpperHoverDarkMode;

        public double MainZoom
        { get; set; }


        public TabItemEXvm tabitemexviewmodel;


        List<ToggleButton> sideMenuButtons;

        public MainWindow()
        {
            InitializeComponent();
            original_Darkmodebackground = Application.Current.Resources["Darkmodebackground"];
            original_lightmodebackground = Application.Current.Resources["lightmodebackground"];
            original_LabelTitleForeColorDarkmode= Application.Current.Resources["LabelTitleForeColorDarkmode"];
            original_LabelTitleForeColor = Application.Current.Resources["LabelTitleForeColor"];

            original_lightmodebackgroundUpperPart= Application.Current.Resources["lightmodebackgroundUpperPart"];
            original_lightmodeforegroundUpperPart = Application.Current.Resources["lightmodeforegroundUpperPart"];
            original_LabelTitleForeColorUpper = Application.Current.Resources["LabelTitleForeColorUpper"];
            original_LabelTitleForeColorUpperHover = Application.Current.Resources["LabelTitleForeColorUpperHover"];
            original_LabelTitleForeColorUpperHoverDarkMode = Application.Current.Resources["LabelTitleForeColorUpperHoverDarkMode"];



            tabitemexviewmodel = new UIModels.TabItemEXvm();
            tab_windows.ItemsSource = tabitemexviewmodel.tabs;

            LogoutIsCalled = false;

            sideMenuButtons = new List<ToggleButton>();
            sideMenuButtons.Add(btnHome);
            sideMenuButtons.Add(btnSend);
            sideMenuButtons.Add(btnLabel);
            sideMenuButtons.Add(btnInvoice);
            sideMenuButtons.Add(btnReports);
            sideMenuButtons.Add(btnAdmin);
            sideMenuButtons.Add(btnSettings);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            show_dash_ui();
           
        }

        private void Show_SendWindow_RelatedControls(bool IsVisible)
        {
            if (IsVisible)
            {
                grd_darkMode.Visibility = Visibility.Visible;
                btn_zommIN.Visibility = Visibility.Visible;
                btn_zoomOut.Visibility = Visibility.Visible;
                zoom.Visibility = Visibility.Visible;
            }
            else
            {
                grd_darkMode.Visibility = Visibility.Collapsed;
                btn_zommIN.Visibility = Visibility.Collapsed;
                btn_zoomOut.Visibility = Visibility.Collapsed;
                zoom.Visibility = Visibility.Collapsed;
            }
        }

        public void showsendwindow()
        {
            Show_SendWindow_RelatedControls(true);
            var r = from tab in tabitemexviewmodel.tabs
                    where tab.content is UI.Send
                    select tab;
            if (r.ToList().Count > 0)
            {
                TabItemEX selecteditemex = r.Single();
                tab_windows.SelectedItem = selecteditemex;

                // check 
                btnSend.IsChecked = true;
                // uncheck
                var btns = sideMenuButtons.Where(xx => xx.Name != (btnSend).Name);
                foreach (var b in btns)
                {
                    b.IsChecked = false;
                }

                return;
            }

            UI.Send uisend = new UI.Send();
            string header = "send";
            tabitemexviewmodel.addtab(header, uisend);
            int x = tabitemexviewmodel.tabs.Count - 1;
            tabitemexviewmodel.tabs[x].tbindex = x;
            tab_windows.SelectedItem = tabitemexviewmodel.tabs[x];

            Binding binding = new Binding();
            binding.Path = new PropertyPath("Value");
            binding.Source = zoom;
            BindingOperations.SetBinding(uisend, UI.Send.ZoomFactorProperty, binding);


            // check 
            btnSend.IsChecked = true;
            // uncheck
            var _btns = sideMenuButtons.Where(xx => xx.Name != (btnSend).Name);
            foreach (var b in _btns)
            {
                b.IsChecked = false;
            }


        }


        public void show_dash_ui()
        {


            Show_SendWindow_RelatedControls(false);
            var r = from tab in tabitemexviewmodel.tabs
                    where tab.content is UI.UIDash
                    select tab;

            if (r.ToList().Count > 0)
            {
                // get the tab index 
                TabItemEX selecteditemex = r.Single();
                tab_windows.SelectedItem = selecteditemex;

                // check 
                btnHome.IsChecked = true;
                // uncheck
                var btns = sideMenuButtons.Where(xx => xx.Name != (btnHome).Name);
                foreach (var b in btns)
                {
                    b.IsChecked = false;
                }


                return;
            }

            UI.UIDash uidash = new UI.UIDash();
            string header = "Dash";
            tabitemexviewmodel.addtab(header, uidash);
            int x = tabitemexviewmodel.tabs.Count - 1;
            tabitemexviewmodel.tabs[x].tbindex = x;
            tab_windows.SelectedItem = tabitemexviewmodel.tabs[x];

            // check 
            btnHome.IsChecked = true;
            // uncheck
            var _btns = sideMenuButtons.Where(xx => xx.Name != (btnHome).Name);
            foreach (var b in _btns)
            {
                b.IsChecked = false;
            }

        }
        public void show_label_ui(string _code,bool ispost)
        {
            Show_SendWindow_RelatedControls(false);
            var r = from tab in tabitemexviewmodel.tabs
                    where tab.content is UI.UILabel
                    select tab;

            if (r.ToList().Count > 0)
            {
                // get the tab index 
                TabItemEX selecteditemex = r.Single();
                tab_windows.SelectedItem = selecteditemex;
                if (!string.IsNullOrEmpty(_code))
                {
                    (selecteditemex.content as UI.UILabel).GenerateDocument(_code, ispost);
                }

                // check 
                btnLabel.IsChecked = true;

                // uncheck
                var _btns = sideMenuButtons.Where(xx => xx.Name != (btnLabel).Name);
                foreach (var b in _btns)
                {
                    b.IsChecked = false;
                }


                return;
            }

            UI.UILabel uilabel = new UI.UILabel();
            string header = "Label";
            tabitemexviewmodel.addtab(header, uilabel);
            int x = tabitemexviewmodel.tabs.Count - 1;
            tabitemexviewmodel.tabs[x].tbindex = x;
            tab_windows.SelectedItem = tabitemexviewmodel.tabs[x];

            if (!string.IsNullOrEmpty(_code))
            {
                uilabel.GenerateDocument(_code, ispost);
            }




            // check 
            btnLabel.IsChecked = true;
            // uncheck
            var btns = sideMenuButtons.Where(xx => xx.Name != (btnLabel).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }



        }
        public void show_invoice_ui(string _code)
        {
            Show_SendWindow_RelatedControls(false);
            var r = from tab in tabitemexviewmodel.tabs
                    where tab.content is UI.UIInvoice
                    select tab;

            if (r.ToList().Count > 0)
            {
                // get the tab index 
                TabItemEX selecteditemex = r.Single();
                tab_windows.SelectedItem = selecteditemex;
                if (!string.IsNullOrEmpty(_code))
                {
                    (selecteditemex.content as UI.UIInvoice).GenerateDocument(_code);
                }

                // check 
                btnInvoice.IsChecked = true;
                // uncheck
                var _btns = sideMenuButtons.Where(xx => xx.Name != (btnInvoice).Name);
                foreach (var b in _btns)
                {
                    b.IsChecked = false;
                }

                return;
            }

            UI.UIInvoice uiinvoice = new UI.UIInvoice();
            string header = "Invoice";
            tabitemexviewmodel.addtab(header, uiinvoice);
            int x = tabitemexviewmodel.tabs.Count - 1;
            tabitemexviewmodel.tabs[x].tbindex = x;
            tab_windows.SelectedItem = tabitemexviewmodel.tabs[x];

            if (!string.IsNullOrEmpty(_code))
            {
                uiinvoice.GenerateDocument(_code);
            }

            // check 
            btnInvoice.IsChecked = true;
            // uncheck
            var btns = sideMenuButtons.Where(xx => xx.Name != (btnInvoice).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }


        }

        public void show_admin_ui()
        {

            if (LoggedData.LoggedUser.Authorization != "Admin")
            {
                WpfMessageBox.Show("", "User Dont Have Permission To Perform Action..", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Warning);
                return;
            }


            Show_SendWindow_RelatedControls(false);

            var r = from tab in tabitemexviewmodel.tabs
                    where tab.content is UI.UIAdmin
                    select tab;

            if (r.ToList().Count > 0)
            {
                // get the tab index 
                TabItemEX selecteditemex = r.Single();
                tab_windows.SelectedItem = selecteditemex;

                // check 
                btnAdmin.IsChecked = true;
                // uncheck
                var _btns = sideMenuButtons.Where(xx => xx.Name != (btnAdmin).Name);
                foreach (var b in _btns)
                {
                    b.IsChecked = false;
                }


                return;
            }

            UI.UIAdmin uiadmin = new UI.UIAdmin();
            string header = "Admin";
            tabitemexviewmodel.addtab(header, uiadmin);
            int x = tabitemexviewmodel.tabs.Count - 1;
            tabitemexviewmodel.tabs[x].tbindex = x;
            tab_windows.SelectedItem = tabitemexviewmodel.tabs[x];


            // check 
            btnAdmin.IsChecked = true;
            // uncheck
            var btns = sideMenuButtons.Where(xx => xx.Name != (btnAdmin).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }




        }

        public void showSettingsUI()
        {
            if (LoggedData.LoggedUser.Authorization != "Admin")
            {
                WpfMessageBox.Show("", "User Dont Have Permission To Perform Action..", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Warning);
                return;
            }


            Show_SendWindow_RelatedControls(false);

            var r = from tab in tabitemexviewmodel.tabs
                    where tab.content is UI.UISettings
                    select tab;

            if (r.ToList().Count > 0)
            {
                // get the tab index 
                TabItemEX selecteditemex = r.Single();
                tab_windows.SelectedItem = selecteditemex;


                // check 
                btnSettings.IsChecked = true;
                // uncheck
                var _btns = sideMenuButtons.Where(xx => xx.Name != (btnSettings).Name);
                foreach (var b in _btns)
                {
                    b.IsChecked = false;
                }


                return;
            }

            UI.UISettings uisettings = new UI.UISettings();
            string header = "settings";
            tabitemexviewmodel.addtab(header, uisettings);
            int x = tabitemexviewmodel.tabs.Count - 1;
            tabitemexviewmodel.tabs[x].tbindex = x;
            tab_windows.SelectedItem = tabitemexviewmodel.tabs[x];


            // check 
            btnSettings.IsChecked = true;
            // uncheck
            var btns = sideMenuButtons.Where(xx => xx.Name != (btnSettings).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }

        }

        public void show_reports_ui()
        {
            Show_SendWindow_RelatedControls(false);
            var r = from tab in tabitemexviewmodel.tabs
                    where tab.content is UI.UIReports
                    select tab;

            if (r.ToList().Count > 0)
            {
                // get the tab index 
                TabItemEX selecteditemex = r.Single();
                tab_windows.SelectedItem = selecteditemex;


                // check 
                btnReports.IsChecked = true;
                // uncheck
                var _btns = sideMenuButtons.Where(xx => xx.Name != (btnReports).Name);
                foreach (var b in _btns)
                {
                    b.IsChecked = false;
                }



                return;
            }

            UI.UIReports uireports = new UI.UIReports();
            string header = "Reports";
            tabitemexviewmodel.addtab(header, uireports);
            int x = tabitemexviewmodel.tabs.Count - 1;
            tabitemexviewmodel.tabs[x].tbindex = x;
            tab_windows.SelectedItem = tabitemexviewmodel.tabs[x];


            // check 
            btnReports.IsChecked = true;
            // uncheck
            var btns = sideMenuButtons.Where(xx => xx.Name != (btnReports).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }

        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            toggledarkmode();
        }

        private void toggledarkmode()
        {

            Application.Current.Resources["lightmodebackground"] = original_Darkmodebackground;
            Application.Current.Resources["Darkmodebackground"] = original_lightmodebackground;
            Application.Current.Resources["LabelTitleForeColor"] = original_LabelTitleForeColorDarkmode;

            Application.Current.Resources["lightmodebackgroundUpperPart"] = original_Darkmodebackground;
            Application.Current.Resources["lightmodeforegroundUpperPart"] = original_LabelTitleForeColorDarkmode;
            Application.Current.Resources["LabelTitleForeColorUpper"] = original_LabelTitleForeColorDarkmode;
            Application.Current.Resources["LabelTitleForeColorUpperHover"] = original_LabelTitleForeColorUpperHoverDarkMode;
        }

        private void togglelightmode()
        {
          
            Application.Current.Resources["lightmodebackground"] = original_lightmodebackground;
            Application.Current.Resources["Darkmodebackground"] = original_Darkmodebackground;
            Application.Current.Resources["LabelTitleForeColor"] = original_LabelTitleForeColor;


            Application.Current.Resources["lightmodebackgroundUpperPart"] = original_lightmodebackgroundUpperPart;
            Application.Current.Resources["lightmodeforegroundUpperPart"] = original_lightmodeforegroundUpperPart;
            Application.Current.Resources["LabelTitleForeColorUpper"] = original_LabelTitleForeColorUpper;
            Application.Current.Resources["LabelTitleForeColorUpperHover"] = original_LabelTitleForeColorUpperHover;
          


        }

        private void tgl_darkmode_Checked(object sender, RoutedEventArgs e)
        {
            toggledarkmode();


            reassignSendWinowColors();


        }

        private void reassignSendWinowColors()
        {
            var r = from tab in tabitemexviewmodel.tabs
                    where tab.content is UI.Send
                    select tab;
            if (r.ToList().Count > 0)
            {
                var x = r.FirstOrDefault();
                var send = x.content as UI.Send;
                if (send.isCheckPaidChecked)
                { send.assign_TXTPaidcolors_Paid(); }
                else
                { send.assignTXTPaidCOlors_Unpaid(); }
                send.assignTXTRemainingColors();
                send.assignTXTToPayColors();
                send.assignTXTDiscountColors();
                send.assigndeletepathcolor();
            }
        }

        private void tgl_darkmode_Unchecked(object sender, RoutedEventArgs e)
        {
            togglelightmode();
            reassignSendWinowColors();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {



           
            UserDa userda = new UserDa();
            BranchDa branchda = new BranchDa();
            AgentDa agentda = new AgentDa();


            LoggedData.LoggedUser = userda.Getuser(LoggedData.LoggedUserID);
            LoggedData.LogggedBranch = agentda.GetAgent(LoggedData.LoggedBranchID);



           
            LabelsDa labelsDa = new LabelsDa();
            CompanyDa companyda = new CompanyDa();
            IdentityTypeDA identityTypeDA = new IdentityTypeDA();


            StaticData.continents = new List<string>();
            StaticData.continents.Add(StaticData.Continent_Europe);
            StaticData.continents.Add(StaticData.Continent_Asia);
            StaticData.continents.Add(StaticData.Continent_North_America);
            StaticData.continents.Add(StaticData.Continent_South_America);
            StaticData.continents.Add(StaticData.Continent_Africa);
            StaticData.continents.Add(StaticData.Continent_Australia);
            DutchZipCodesDa dutchZipCodesDa = new DutchZipCodesDa();
            StaticData.Special_Countries = dutchZipCodesDa.get_Special_Countries();
            StaticData.Labels = labelsDa.Get_Labels();
            StaticData.IdentityTypes = identityTypeDA.GetIdentityTypes();


            companyda.get_companyData();// will set the static variables in company class...
            txtblock_branch.Text = LoggedData.LogggedBranch.AgentName;
            txtblock_user.Text = LoggedData.LoggedUser.UserName;
            txtblock_authorization.Text = LoggedData.LoggedUser.Authorization;


            LoadZipcodesData(new object(), new RoutedEventArgs());

            show_dash_ui();
        }



        private async void LoadZipcodesData(object sender, RoutedEventArgs e)
        {

            DutchZipCodesDa dutchZipCodesDa = new DutchZipCodesDa();
          //  adorner1.IsAdornerVisible = true;
            await Task.Run(() => StaticData.DE_ZipCodes = dutchZipCodesDa.get_DE_ZipCodes());
            await Task.Run(() => StaticData.NL_ZipCode = dutchZipCodesDa.get_NL_ZipCodes());
          //  adorner1.IsAdornerVisible = false;


        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            show_label_ui(string.Empty,false);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            show_invoice_ui(string.Empty);
                
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            show_admin_ui();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            show_reports_ui();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dlg = WpfMessageBox.Show("", "Are You Sure You Want To Log out ?", MessageBoxButton.YesNo, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Question);
            if (dlg == MessageBoxResult.No) { return; }


            // logout
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            LogoutIsCalled = true;
            this.Close();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            // zoom in
            try { zoom.Value += zoom.TickFrequency; }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
           
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            // zoom out
            try { zoom.Value -= zoom.TickFrequency; }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void show_notification_message(string title, string message, MessagesTypes.messagestypes messagetype, int seconds)
        {

            notificationmessage2.Title = title;
            notificationmessage2.MessageType = messagetype;
            notificationmessage2.Message = message;
            notificationmessage2.Visibility = System.Windows.Visibility.Visible;

            Storyboard displayAnimation = FindResource("displayAnimation") as Storyboard;
            displayAnimation.Completed -= displayAnimation_Completed;
            displayAnimation.Completed += displayAnimation_Completed; // before the begin
            displayAnimation.Begin();

        }
        void displayAnimation_Completed(object sender, EventArgs e)
        {

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(3)
            };

            timer.Tick += delegate (object sender2, EventArgs e2)
            {
                ((System.Windows.Threading.DispatcherTimer)timer).Stop();

                performhideanimation();

            };
            timer.Start();



        }

        private void performhideanimation()
        {
            Storyboard hideAnimation = FindResource("hideAnimation") as Storyboard;
            hideAnimation.Completed -= hideAnimation_Completed;
            hideAnimation.Completed += hideAnimation_Completed;
            hideAnimation.Begin();
        }

        void hideAnimation_Completed(object sender, EventArgs e)
        {
            notificationmessage2.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void notificationmessage2_OnClose_1(object sender, EventArgs e)
        {
           //
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            showsendwindow();
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

         if (LogoutIsCalled) { return; }
         MessageBoxResult dlg=    WpfMessageBox.Show("", "Are You Sure You Want To Exit Application ?", MessageBoxButton.YesNo, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Question);
            if (dlg == MessageBoxResult.No) { e.Cancel = true; }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            // go to settings
            showSettingsUI();
        }

        private void btnHome_Checked(object sender, RoutedEventArgs e)
        {
            // uncheck the rest of buttons (an alternative soluton is to use radio buttons)
            var btns= sideMenuButtons.Where(x=>x.Name!=(sender as ToggleButton).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }
        }

        private void btnSend_Checked(object sender, RoutedEventArgs e)
        {
            /*
            var btns = sideMenuButtons.Where(x => x.Name != (sender as ToggleButton).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }
            */
        }

        private void btnInvoice_Checked(object sender, RoutedEventArgs e)
        {
            /*
            var btns = sideMenuButtons.Where(x => x.Name != (sender as ToggleButton).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }
            */
        }

        private void btnLabel_Checked(object sender, RoutedEventArgs e)
        {
            /*
            var btns = sideMenuButtons.Where(x => x.Name != (sender as ToggleButton).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }
            */

        }

        private void btnReports_Checked(object sender, RoutedEventArgs e)
        {
            /*
            var btns = sideMenuButtons.Where(x => x.Name != (sender as ToggleButton).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }
            */
        }

        private void btnAdmin_Checked(object sender, RoutedEventArgs e)
        {
            /*
            var btns = sideMenuButtons.Where(x => x.Name != (sender as ToggleButton).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }
            */
        }

        private void btnSettings_Checked(object sender, RoutedEventArgs e)
        {
            /*
            var btns = sideMenuButtons.Where(x => x.Name != (sender as ToggleButton).Name);
            foreach (var b in btns)
            {
                b.IsChecked = false;
            }
            */
        }
    }
}
