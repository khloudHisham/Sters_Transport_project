using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using StersTransport.Helpers;
using StersTransport.GlobalData;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using StersTransport.Models;
using StersTransport.DataAccess;
using System.Windows.Media;

namespace StersTransport.Presentation
{
    public  class CodeLabel_PRSN
    {
        
        SolidColorBrush NotPaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD015B"));
         SolidColorBrush PaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#23C99C"));
        SolidColorBrush BorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#404040"));

        double lineheight = 25.0;
        double lineheight2 = 30.0;

        ClientCodeDA clientCodeDA = new ClientCodeDA();
        AgentDa agentDa = new AgentDa();
        CountryDa countryDa = new CountryDa();
        BranchDa branchDa = new BranchDa();


        ClientCode ClientCode { get; set; }
        Country countryAgent { get; set; }
        Country countryPost { get; set; }

        Agent agent { get; set; }

        List<Branch> Branches = new List<Branch>();

        public Table Generate_Header_Table()
        {
            Table etable = new Table();

            etable.TextAlignment = TextAlignment.Center;
           
            // add table columns....
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(2, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });



            string currentvaluestr = string.Empty;

            etable.RowGroups.Add(new TableRowGroup());
            etable.RowGroups[0].Rows.Add(new TableRow());
            TableRow Row = etable.RowGroups[0].Rows[0];


            var bitimage= ImageHelpercs.LoadImage(CompanyData.Logo2_Sters);
            Image image = new Image();
            image.Source = bitimage;
            image.Width = 100;
            image.Height = 100;
            var block = new BlockUIContainer(image);
            Row.Cells.Add(new TableCell(block) { LineHeight = lineheight, RowSpan=3 });

            currentvaluestr = CompanyData.ArabicName;

            Row.Cells.Add(
                      new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
                  );

            bitimage = ImageHelpercs.LoadImage(CompanyData.Logo1_EU);
            image = new Image();
            image.Source = bitimage;
            image.Width = 100;
            image.Height = 100;
            block = new BlockUIContainer(image);
            Row.Cells.Add(new TableCell(block) { LineHeight = lineheight, RowSpan = 2 });

            // new row
            // adding data
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[1];

            currentvaluestr = CompanyData.ArabicDiscription;
            Row.Cells.Add(
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );


            bool havelocalpost = ClientCode.Have_Local_Post.HasValue ? (bool)ClientCode.Have_Local_Post : false;

            // adding data
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[2];

            if (havelocalpost)
            {
                currentvaluestr = "Post Label";

                Row.Cells.Add(
                 new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
             );

                string postdatestr = ClientCode.PostDate.HasValue ? ClientCode.PostDate.Value.ToShortDateString() : string.Empty;

                currentvaluestr = string.Format("{0} : {1}", "Date", postdatestr);
                Row.Cells.Add(
                new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
            );

            }
            else
            {
                currentvaluestr = "Box Label";

                Row.Cells.Add(
                 new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
             );

               
                Row.Cells.Add(
                new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight }
            );
            }

           
 
            return etable;
        }


        public Table generate_data_table(int current_boxno)
        {
            Table etable = new Table();
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = 0;
            // add table columns....
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            string currentvaluestr = string.Empty;


            bool havelocalpost = ClientCode.Have_Local_Post.HasValue ? (bool)ClientCode.Have_Local_Post : false;



            if (havelocalpost)
            {
               

                etable.RowGroups.Add(new TableRowGroup());


                // adding row by row
                etable.RowGroups[0].Rows.Add(new TableRow());
                TableRow Row = etable.RowGroups[0].Rows[0];
                currentvaluestr = "Information";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) {  BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight,ColumnSpan=3 }) ;



                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1]; // the last added row
                currentvaluestr = "Sender";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.SenderName;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });



                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Weight KG";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                double Weight_Total = ClientCode.Weight_Total.HasValue ? (double)ClientCode.Weight_Total : 0;
                currentvaluestr = Weight_Total.ToString();
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });



                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "No. Of Parcel";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                double boxno = ClientCode.Box_No.HasValue ? (double)ClientCode.Box_No : 0;
                currentvaluestr = boxno.ToString();
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });



                currentvaluestr = string.Format("Unit NO {0} Of Units {1}", (current_boxno+1).ToString(), ((int)ClientCode.Box_No).ToString());


                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });



                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Receiver Name";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.ReceiverName;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });

                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Address";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.Street_Name_No;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });



                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Postal Code";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.ZipCode;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });


                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "City Name";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.CityPost;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });


                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Country Name";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });



             //   long CountryPostId=  ClientCode.CountryPostId.HasValue?(long)ClientCode.CountryPostId:0;
             //   Country country=  countryDa.GetCountry(CountryPostId);
                currentvaluestr = string.Empty;
                if (countryPost != null)
                { currentvaluestr = countryPost.CountryName; } // another approach is to load data within clientda.getclient()
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });


                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Receiver Phone";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.Receiver_Tel;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });



                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Code";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.Code;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });

                double EuropeToPay= ClientCode.EuropaToPay.HasValue ? (double)ClientCode.EuropaToPay : 0;
                if (EuropeToPay > 0)
                {
                    currentvaluestr = "Pay In Europe";
                    Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { Background=NotPaidColor, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                }
                else
                {
                    currentvaluestr = "All Paid";
                    Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { Background = PaidColor, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                }

 

            }

            else
            {
                etable.RowGroups.Add(new TableRowGroup());
                // adding row by row
                etable.RowGroups[0].Rows.Add(new TableRow());
                TableRow Row = etable.RowGroups[0].Rows[0];
                currentvaluestr = "Information";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight, ColumnSpan = 3 });

                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1]; // the last added row
                currentvaluestr = "Sender";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.SenderName;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });

                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Receiver";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.ReceiverName;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });


                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Phone Number";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.Receiver_Tel;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });


                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Item(s)";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.Goods_Description;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });


                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Weight KG";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                double Weight_Total = ClientCode.Weight_Total.HasValue ? (double)ClientCode.Weight_Total : 0;
                currentvaluestr = Weight_Total.ToString();
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });



                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Code";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = ClientCode.Code;
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });

                double EuropeToPay = ClientCode.EuropaToPay.HasValue ? (double)ClientCode.EuropaToPay : 0;
                if (EuropeToPay > 0)
                {
                    currentvaluestr = "Pay In Europe";
                    Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { Background = NotPaidColor, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                }
                else
                {
                    currentvaluestr = "All Paid";
                    Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { Background = PaidColor, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                }


                // city of agent office ...
                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "City";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });               
                currentvaluestr = string.Empty;
                if (agent != null)
                { currentvaluestr = agent.AgentName; } // another approach is to load data within clientda.getclient()
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });

                // country 
                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
                currentvaluestr = "Country";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });
                currentvaluestr = string.Empty;
                if (countryAgent != null)
                { currentvaluestr = countryAgent.CountryName; } // another approach is to load data within clientda.getclient()
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = BorderColor, LineHeight = lineheight });










            }





            return etable;
        }


        public Table generate_second_data_table()
        {
            Table etable = new Table();
            etable.TextAlignment = TextAlignment.Center;
            // add table columns....
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            string currentvaluestr = string.Empty;

            bool havelocalpost = ClientCode.Have_Local_Post.HasValue ? (bool)ClientCode.Have_Local_Post : false;

            etable.RowGroups.Add(new TableRowGroup());


            // adding row by row
            etable.RowGroups[0].Rows.Add(new TableRow());
            TableRow Row = etable.RowGroups[0].Rows[0];
            currentvaluestr = "Truck No.";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5), BorderBrush = BorderColor, LineHeight = lineheight2 });


            if (havelocalpost)
            {
                if (countryPost != null)
                {
                    var bitimage = ImageHelpercs.LoadImage(countryPost.ImgForPostLabel);
                    if (bitimage != null)

                    {
                        Image image = new Image();
                        image.Source = bitimage;// handle nulls
                        image.Width = 100;
                         image.Height = 100;
                        var block = new BlockUIContainer(image);
                        Row.Cells.Add(new TableCell(block) { LineHeight = lineheight, RowSpan = 2 });
                    } // no image
                    else
                    {
                        Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight, RowSpan = 2 });
                    }
           
                } // no country in case of deleted ...(no need to print in the first place)
                else
                {
                    Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight, RowSpan = 2 });
                }
                 
            }
            else
            {
                if (countryAgent != null)
                {
                    var bitimage = ImageHelpercs.LoadImage(countryAgent.ImgForBoxLabel);
                    if (bitimage != null)

                    {
                        Image image = new Image();
                        image.Source = bitimage;// handle nulls
                        image.Width = 100;
                        image.Height = 100;
                        var block = new BlockUIContainer(image);
                        Row.Cells.Add(new TableCell(block) { LineHeight = lineheight, RowSpan = 2 });
                    } // no image
                    else
                    {
                        Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight, RowSpan = 2 });
                    }

                } // no country in case of deleted ...(no need to print in the first place)
                else
                {
                    Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight, RowSpan = 2 });
                }
            }

        

            currentvaluestr = "Items";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5), BorderBrush = BorderColor, LineHeight = lineheight2 });

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            double Shipment_No = ClientCode.Shipment_No.HasValue ? (double)ClientCode.Shipment_No : 0;
            currentvaluestr = Shipment_No.ToString();
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5), BorderBrush = BorderColor, LineHeight = lineheight2 });

            currentvaluestr = ClientCode.Goods_Description ;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5), BorderBrush = BorderColor, LineHeight = lineheight2 });



            // our offices....

            return etable;
        }


        public Table generate_ourOffices_Table()
        {
            Table etable = new Table();
            etable.TextAlignment = TextAlignment.Center;
            // add table columns....
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1.5, GridUnitType.Star) });

            string currentvaluestr = string.Empty;

            etable.RowGroups.Add(new TableRowGroup());


            etable.RowGroups[0].Rows.Add(new TableRow());
            TableRow Row = etable.RowGroups[0].Rows[0];

            currentvaluestr = "Our Offices In Iraq And Europe";
            Row.Cells.Add(
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight,ColumnSpan=3 }
              );


            // now generating rows equal to branches count... with minimum rows equal to four to contain company info 
            int rowtoadd = Branches.Count;
            if (rowtoadd < 4) { rowtoadd = 4; }

            for (int c = 0; c < rowtoadd; c++)
            {
                etable.RowGroups[0].Rows.Add(new TableRow());
            }
            int startindex = 1;
            // initialize rows with empty strings
            for (int c = startindex; c < etable.RowGroups[0].Rows.Count ; c++)
            {
                Row = etable.RowGroups[0].Rows[c];
                for (int cc = 0; cc < etable.Columns.Count; cc++)
                {
                   
                    Row.Cells.Add(
                 new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight }
             );

                }
            }




                // fill with company setttings
                Row = etable.RowGroups[0].Rows[startindex];

            currentvaluestr = CompanyData.EnglishName;
            Row.Cells[0]=(
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );
            currentvaluestr = CompanyData.Tel1;
            Row.Cells[1] = (
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );



            Row = etable.RowGroups[0].Rows[startindex+1];
            currentvaluestr = "De Steiger 98";
            Row.Cells[0] = (
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );
            currentvaluestr = CompanyData.Tel2;
            Row.Cells[1] = (
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );

            Row = etable.RowGroups[0].Rows[startindex + 2];
            currentvaluestr = "Almere - The Netherlands";
            Row.Cells[0] = (
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );
            currentvaluestr = string.Format("{0} : {1}","Email:",CompanyData.Email);
            Row.Cells[1] = (
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );

            Row = etable.RowGroups[0].Rows[startindex + 3];
            currentvaluestr = "";
            Row.Cells[0] = (
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );
            currentvaluestr = string.Format("{0} : {1}", "Web:", CompanyData.Website);
            Row.Cells[1] = (
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );

            // last column

            for (int c = startindex; c < etable.RowGroups[0].Rows.Count; c++)
            {
                string branchname = Branches[c-startindex].BranchName;
                string displayphones = Branches[c - startindex].PhonesDisplayString;
                currentvaluestr = string.Format("{0} : {1}", branchname, displayphones);
                Row = etable.RowGroups[0].Rows[c];
                Row.Cells[2]= (
                  new TableCell(new Paragraph(new Run(currentvaluestr))) { LineHeight = lineheight }
              );
            }






            return etable;
        }



        public void generateDocument(FlowDocument flowdocument,string code)
        {
            ClientCode = clientCodeDA.GetClientCode(code);
            if (ClientCode == null) { return; }

            // get branches data 

            List<Branch> allbranches = branchDa.GetBranches();
            Branches = allbranches.Where(x => x.IsLocalCompanyBranch == true).ToList();

            long _CountryAgentId = ClientCode.CountryAgentId.HasValue ? (long)ClientCode.CountryAgentId : 0;
            countryAgent = countryDa.GetCountry(_CountryAgentId);

            long _CountryPostId = ClientCode.CountryPostId.HasValue ? (long)ClientCode.CountryPostId : 0;
            countryPost = countryDa.GetCountry(_CountryPostId);

            long _agentID = ClientCode.AgentId.HasValue ? (long)ClientCode.AgentId : 0;
            agent = agentDa.GetAgent(_agentID);





            flowdocument.Blocks.Clear();


            int boxcount = ClientCode.Box_No.HasValue ? (int)ClientCode.Box_No : 0;


            for (int c = 0; c < boxcount; c++)
            {
                Section headersection = new Section();
               
                Table headertable = Generate_Header_Table();
                headersection.Blocks.Add(headertable);
            
                if (c != 0)
                {
                    if (c < boxcount)
                    {
                        headersection.BreakPageBefore = true;
                    }
                }
             

                flowdocument.Blocks.Add(headersection);

                Table datatable = generate_data_table(c);
                flowdocument.Blocks.Add(datatable);
                Table secondtable = generate_second_data_table();
                flowdocument.Blocks.Add(secondtable);
                Table ourofficetable = generate_ourOffices_Table();
                flowdocument.Blocks.Add(ourofficetable);


                /*
                if (c + 1 < boxcount)
                {

                    // new page break...
                    Section section = new Section();
                    section.BreakPageBefore = true;
                    flowdocument.Blocks.Add(section);
                }
               */
              
                
            }

            



        }
    }
}
