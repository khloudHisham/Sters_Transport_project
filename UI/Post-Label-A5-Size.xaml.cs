using StersTransport.Models;
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
using System.Windows.Shapes;

namespace StersTransport.UI
{

    public partial class Post_Label_A5_Size : Window
    {
        private StersDB db;

        public Post_Label_A5_Size()
        {
            InitializeComponent();
            db = new StersDB();
            LoadCountryImage();
        }

        private void LoadCountryImage()
        {
            string countryName = countryName_txt.Text;
            
            try
            {
                var country = db.Country.FirstOrDefault(c => c.CountryName == countryName);
                if (country != null && country.ImgForPostLabel != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new System.IO.MemoryStream(country.ImgForPostLabel);
                    bitmapImage.EndInit();

                    countryImage_img.Source = bitmapImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading country image: " + ex.Message);
            }
        }
    }
}
