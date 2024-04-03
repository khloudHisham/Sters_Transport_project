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
using ZXing;
using System.IO;

namespace StersTransport.Presentation
{
   

    public class CodeLabelPost_PRSN
    {
       


        SolidColorBrush NotPaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD015B"));
        SolidColorBrush PaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#23C99C"));
        SolidColorBrush BorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#404040"));


     
        double lineheight = 25.0;
        double lineheight2 = 30.0;

 
        double lineheightsmall = 3.0;

        double marginthicness = 0;
        double paddingthicness = 0;
        double cellspacing = 0;
        double FontsizeLarge = 14;
        double FontsizeextraLarge = 22;
        ClientCodeDA clientCodeDA = new ClientCodeDA();
        AgentDa agentDa = new AgentDa();
        CountryDa countryDa = new CountryDa();
        BranchDa branchDa = new BranchDa();


        ClientCode ClientCode { get; set; }
        Country countryAgent { get; set; }
        Country countryPost { get; set; }

        Agent agent { get; set; }


        Agent branch { get; set; }

       // List<Branch> Branches = new List<Branch>();
        List<Agent> Branches = new List<Agent>();



        public Table generate_Header_With_ourOffices_Table()
        {

            // Main (Containing Table..)
            var containingTable = new Table();
            containingTable.CellSpacing = 0;
            containingTable.Margin = new Thickness(1);
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            containingTable.RowGroups.Add(new TableRowGroup());
            containingTable.RowGroups[0].Rows.Add(new TableRow());



            TableRow ContainingRow = containingTable.RowGroups[0].Rows[0];
            // Add The EU Image
            var bitimage = ImageHelpercs.LoadImage(CompanyData.Logo1_EU);
            var image = new Image();
            image.Source = bitimage;
          
            image.Width = 100;
            image.Height = 100;
            image.Stretch = Stretch.Fill;
            var block = new BlockUIContainer(image);
            
            ContainingRow.Cells.Add(new TableCell(block) { LineHeight = lineheight2 });


            //.................................
            Table etable = new Table();
            etable.TextAlignment = TextAlignment.Left;
            etable.FontSize = 8;
            etable.Margin = new Thickness(0);
            // add table columns....
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.RowGroups.Add(new TableRowGroup());



            etable.RowGroups[0].Rows.Add(new TableRow());
            TableRow Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count-1];// last added


            string currentLabelStr = CompanyData.EnglishName;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { 
                Margin = new Thickness(0, marginthicness, 0, marginthicness),
                Padding = new Thickness(0, paddingthicness, 0, paddingthicness)
                ,FontWeight= FontWeights.Bold,
                 FontSize=12
            })
            { LineHeight = lineheightsmall });

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "De Steiger 98";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheightsmall });

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Almere";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheightsmall });


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Netherlands";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheightsmall });

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = string.Format("{0} {1}", "Tel1:", CompanyData.Tel1);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheightsmall }); ;

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = string.Format("{0} {1}", "Tel2:", CompanyData.Tel2);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheightsmall }); ;



            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = string.Format("{0} {1}", "Email:", CompanyData.Email);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheightsmall });


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = string.Format("{0} {1}", "Website:", CompanyData.Website);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheightsmall });


            //..........................
            var containingCell1 = new TableCell();
            containingCell1.Blocks.Add(new Section(etable) {   BorderBrush = BorderColor });
            ContainingRow.Cells.Add(containingCell1);


            //

            // Add The Sters Image
            bitimage = ImageHelpercs.LoadImage(CompanyData.Logo2_Sters);
            image = new Image();
            image.Source = bitimage;
            image.Width = 100;
            image.Height = 100;
            block = new BlockUIContainer(image);

            ContainingRow.Cells.Add(new TableCell(block) { LineHeight = lineheight2 });





            etable = new Table();
            etable.FontSize = 8;
            etable.Margin = new Thickness(0);
            //  etable.FlowDirection = FlowDirection.RightToLeft;
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = cellspacing;
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.RowGroups.Add(new TableRowGroup());

            // add in the first row the company Name english2 Feild (which represent 'STERS' in this case)
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
            currentLabelStr = CompanyData.EnglishName2;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))
            {
                Margin = new Thickness(0, marginthicness, 0, marginthicness),
                Padding = new Thickness(0, paddingthicness, 0, paddingthicness)
                ,
                FontWeight = FontWeights.Bold,
                FontSize = 12
            })
            { LineHeight = lineheightsmall });


            for (int c = 0; c < Branches.Count; c++)
            {
                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


                double clh = lineheightsmall;
                if (Branches[c].PhoneNo1.Length > 0 && Branches[c].PhoneNo2.Length > 0)
                {
                    clh *= 2;
                }
                currentLabelStr = Branches[c].AgentName;
                var grid = new Grid();var blockUI = new BlockUIContainer(grid);
               // blockUI.Margin = new Thickness(marginthicness);
                 var txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                 var tablcell = new TableCell()
                 { LineHeight = clh };
                tablcell.Padding = new Thickness(0, 3, 0, 0);
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);

                // new cell 
                currentLabelStr = Branches[c].PhoneNo1;
                if (Branches[c].PhoneNo2.Length > 0)
                {
                    currentLabelStr += Environment.NewLine;
                    currentLabelStr += Branches[c].PhoneNo2;
                }
                 grid = new Grid();  blockUI = new BlockUIContainer(grid);
                // blockUI.Margin = new Thickness(marginthicness);
                 txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                 tablcell = new TableCell()
                { LineHeight = clh };
                tablcell.Blocks.Add(blockUI);
                tablcell.Padding = new Thickness(0, 3, 0, 0);
                Row.Cells.Add(tablcell);
            }

            /*
              currentLabelStr = generate_branches_string(Branches);
              Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                  //  { LineHeight = lineheight2}
                  );
              */

            containingCell1 = new TableCell();
            containingCell1.Blocks.Add(new Section(etable) {  BorderBrush = BorderColor });
            ContainingRow.Cells.Add(containingCell1);


            return containingTable;
        }

        public Table generate_data_table(int nop)
        {
            double pt = 2;
            double brdrt = 0.2;

            // Main (Containing Table..)
            var containingTable = new Table();
            containingTable.Margin = new Thickness(1);
            containingTable.CellSpacing = 0;
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(3, GridUnitType.Star) });
           // containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });

            containingTable.RowGroups.Add(new TableRowGroup());



            containingTable.RowGroups[0].Rows.Add(new TableRow());
            TableRow ContainingRow = containingTable.RowGroups[0].Rows[0];

            string currentLabelStr = string.Empty;

            Table etable = new Table();
            etable.TextAlignment = TextAlignment.Left;
            etable.Margin = new Thickness(0); // important ....
            etable.CellSpacing = 0;

            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });

            etable.RowGroups.Add(new TableRowGroup());



            etable.RowGroups[0].Rows.Add(new TableRow());
            TableRow Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added


            currentLabelStr = "From:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) )
            { FontWeight = FontWeights.Regular, LineHeight = lineheight, Padding=new Thickness (pt, 0,0,0), BorderBrush = BorderColor,BorderThickness = new Thickness(brdrt) });

            currentLabelStr = string.Format("{0}  {1}", GlobalData.CompanyData.EnglishName, branch.AgentName);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { Padding = new Thickness(pt, 0, 0, 0), FontWeight =FontWeights.Bold,  ColumnSpan=2,  BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });


            currentLabelStr = DateTime.Now.ToString();
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { TextAlignment= TextAlignment.Center} )
            {  FontWeight = FontWeights.Regular,FontSize=10, LineHeight = lineheight });


            etable.RowGroups[0].Rows.Add(new TableRow());
             Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added


            currentLabelStr = "Sender Name:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Regular, Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight, BorderBrush = BorderColor,BorderThickness = new Thickness(brdrt) });

            currentLabelStr = ClientCode.SenderName;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { Padding = new Thickness(pt, 0, 0, 0),FontWeight = FontWeights.Bold,ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });


            currentLabelStr = ClientCode.Goods_Description;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { TextAlignment= TextAlignment.Center})
            { FontSize=10,FontWeight=FontWeights.Bold, Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight ,RowSpan=3});


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added


            currentLabelStr = "Number Of Parcel:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Regular, Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(brdrt) });


            double bxn = ClientCode.Box_No.HasValue ? (double)ClientCode.Box_No : 0;
            double pln = ClientCode.Pallet_No.HasValue ? (double)ClientCode.Pallet_No : 0;

            double boxno = bxn + pln;
           // double boxno = nop;

            // we need a stack panel 
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            Label l = new Label();
            l.BorderThickness = new Thickness(0.1);
            l.BorderBrush = BorderColor;
            l.Content = nop.ToString();
            sp.Children.Add(l);

            l = new Label();
            l.BorderThickness = new Thickness(0.1);
            l.BorderBrush = BorderColor;
            l.Content = "of";
            sp.Children.Add(l);

            l = new Label();
            l.BorderThickness = new Thickness(0.1);
            l.BorderBrush = BorderColor;
            l.Content = boxno.ToString();
            sp.Children.Add(l);

            BlockUIContainer buc = new BlockUIContainer(sp);
            //  Row.Cells.Add(new TableCell(block) { LineHeight = lineheight,RowSpan=5 });

        //    currentLabelStr = string.Format("{0} of {1}",nop.ToString(), boxno.ToString());
          //  Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { Padding = new Thickness(pt, 0, 0, 0),FontWeight = FontWeights.Bold,ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });
            Row.Cells.Add(new TableCell(buc) { Padding = new Thickness(pt, 0, 0, 0), FontWeight = FontWeights.Bold, ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });



            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added


            currentLabelStr = "Weight KG:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Regular, Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(brdrt) });

            double wkg = ClientCode.Weight_Total.HasValue ? (double)ClientCode.Weight_Total : 0;
            currentLabelStr = wkg.ToString();
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { Padding = new Thickness(pt, 0, 0, 0), FontWeight = FontWeights.Bold,ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });



            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added

            currentLabelStr = "To:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { Padding = new Thickness(pt, 0, 0, 0), ColumnSpan = 3, LineHeight = lineheight, FontWeight = FontWeights.Bold });


            bool haveinsurance = ClientCode.Have_Insurance.HasValue ? (bool)ClientCode.Have_Insurance : false;

            if (haveinsurance)
            {
                currentLabelStr = "Insurance = Yes";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { TextAlignment= TextAlignment.Center}) { Padding = new Thickness(pt, 0, 0, 0),Background = PaidColor, LineHeight = lineheight });
            }
            else
            {
                currentLabelStr = "";
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { LineHeight = lineheight });
            }




            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Receiver Name Full:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Bold,Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(brdrt) });

            currentLabelStr = ClientCode.ReceiverName;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) {FontSize=FontsizeextraLarge, Padding = new Thickness(pt, 0, 0, 0), FontWeight = FontWeights.Bold, ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });


            var bitimage = ImageHelpercs.LoadImage(countryPost.ImgForPostLabel);
            if (bitimage != null)

            {
                Image image_ = new Image();
                image_.Source = bitimage;// handle nulls
               image_.Stretch = Stretch.Uniform;
               // image_.Width = 70;
               // image_.Height = 70;
               // image.Stretch = Stretch.Fill;
                var block = new BlockUIContainer(image_);
                Row.Cells.Add(new TableCell(block) {   RowSpan =6, LineHeight = lineheight });
            } // no image
            else
            {
                Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { RowSpan = 6, LineHeight = lineheight });
            }



            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Address:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Bold, Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(brdrt) });

            if (!string.IsNullOrEmpty(ClientCode.Dep_Appar) )
            { currentLabelStr =string.Format("{0} / {1}", ClientCode.Street_Name_No, ClientCode.Dep_Appar); }
            else
            { currentLabelStr = ClientCode.Street_Name_No; }
           

            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { Padding = new Thickness(pt, 0, 0, 0), FontWeight = FontWeights.Bold, ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Postal Code:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Bold, Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(brdrt) });

            currentLabelStr = ClientCode.ZipCode;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { Padding = new Thickness(pt, 0, 0, 0),FontWeight = FontWeights.Bold, ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });



            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "City Name:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Bold, Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(brdrt) });

            currentLabelStr = ClientCode.CityPost;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) {FontSize=FontsizeextraLarge, Padding = new Thickness(pt, 0, 0, 0), FontWeight = FontWeights.Bold, ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });

            string cntrystr = string.Empty;
            if (countryPost != null)
            {
                cntrystr = countryPost.CountryName;

            }

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Country Name:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Bold, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(brdrt) });

            currentLabelStr = cntrystr;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontSize = FontsizeextraLarge, Padding = new Thickness(pt, 0, 0, 0), FontWeight = FontWeights.Bold, ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });

            


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Phone Number:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Bold,Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(brdrt) });

            currentLabelStr = ClientCode.Receiver_Tel;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { Padding = new Thickness(pt, 0, 0, 0), FontWeight = FontWeights.Bold, ColumnSpan = 2, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Code:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight = FontWeights.Bold,Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(brdrt) });

            currentLabelStr = ClientCode.Code;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontSize = FontsizeextraLarge, Padding = new Thickness(pt, 0, 0, 0), FontWeight = FontWeights.ExtraBold, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });



            var gridHeader = new Grid();
            var blockUIHeader = new BlockUIContainer(gridHeader);
            TextBlock txtblockHeader = new TextBlock();
            txtblockHeader.VerticalAlignment = VerticalAlignment.Center;
            txtblockHeader.HorizontalAlignment = HorizontalAlignment.Center;
            gridHeader.Children.Add(txtblockHeader);
            double EuropeToPay = ClientCode.EuropaToPay.HasValue ? (double)ClientCode.EuropaToPay : 0;
            if (EuropeToPay > 0)
            {
                if (countryAgent.continent == StaticData.Continent_Europe)
                {
                    currentLabelStr = "PAY IN " + StaticData.Continent_Europe;
                }
                else
                {
                    currentLabelStr = "PAY IN " + countryAgent.CountryName;
                }
                txtblockHeader.Text = currentLabelStr; txtblockHeader.FontWeight = FontWeights.Bold; txtblockHeader.FontSize = FontsizeextraLarge;
                var tablcellHeader = new TableCell() { BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight, FontWeight = FontWeights.Bold, Background = NotPaidColor };
                tablcellHeader.Blocks.Add(blockUIHeader);
                Row.Cells.Add(tablcellHeader);
                //  Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontWeight = FontWeights.Bold, Padding = new Thickness(pt, 0, 0, 0), Background = NotPaidColor, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });
            }
            else
            {
                currentLabelStr = "ALL PAID";
                // Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontWeight = FontWeights.Bold, Padding = new Thickness(pt, 0, 0, 0), Background = PaidColor, BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight });

                txtblockHeader.Text = currentLabelStr; txtblockHeader.FontWeight = FontWeights.Bold; txtblockHeader.FontSize = FontsizeextraLarge;
                 var tablcellHeader = new TableCell() { BorderThickness = new Thickness(brdrt), BorderBrush = BorderColor, LineHeight = lineheight, FontWeight = FontWeights.Bold, Background = PaidColor };
                tablcellHeader.Blocks.Add(blockUIHeader);
                Row.Cells.Add(tablcellHeader);
            }




            double shipmentno = ClientCode.Shipment_No.HasValue ? (double)ClientCode.Shipment_No : 0;
            currentLabelStr = string.Format("{0} {1}", "Truck NO.", shipmentno.ToString());
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { TextAlignment= TextAlignment.Center}) { FontWeight = FontWeights.Bold, Padding = new Thickness(pt, 0, 0, 0), LineHeight = lineheight });

            // truck no
            //..............................
            var containingCell1 = new TableCell();
            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            ContainingRow.Cells.Add(containingCell1);

            // second row for the barcode
            containingTable.RowGroups[0].Rows.Add(new TableRow());
            ContainingRow = containingTable.RowGroups[0].Rows[1];

            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_128;

           
            writer.Options = new ZXing.Common.EncodingOptions()
            {
               Height = 50,
              //  Width = 300
              PureBarcode=true
               
            };

            var result = writer.Write(ClientCode.Code);
            var barcodeBitmap = new System.Drawing.Bitmap(result);
            Image image2 = new Image();
            image2.Width = 300;
            image2.Height = 50;
            image2.Stretch = Stretch.Fill;
            image2.Source = ImageHelpercs.ConvertBitmap(barcodeBitmap);
            var block2 = new BlockUIContainer(image2) { Margin = new Thickness(4) };
            ContainingRow.Cells.Add(new TableCell(block2) { LineHeight = lineheight  });

            // third row for barcode label
            containingTable.RowGroups[0].Rows.Add(new TableRow());
            ContainingRow = containingTable.RowGroups[0].Rows[2];
            currentLabelStr = ClientCode.Code;
            ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { TextAlignment= TextAlignment.Center})
            { FontWeight = FontWeights.Bold,  LineHeight = lineheight  });


            return containingTable;
        }

        private string generate_branches_string(List<Branch> branches)
        {
            string result = string.Empty;
            int branchcntr = 0;
            for (int c = 0; c < branches.Count; c++)
            {
                result += string.Format("{0} : {1}  ", branches[c].BranchName, branches[c].PhonesDisplayString);
                branchcntr++;
                if (branchcntr > 1)
                {
                    result += Environment.NewLine;
                    branchcntr = 0;
                }

            }
            return result;
        }
        public void generateDocument(FlowDocument flowdocument, string code)
        {
            ClientCode = clientCodeDA.GetClientCode(code);
            if (ClientCode == null) { return; }

            // get branches data 

            // List<Branch> allbranches = branchDa.GetBranches();
            List<Agent> allbranches = agentDa.GetAgents();
            Branches = allbranches.Where(x => x.IsLocalCompanyBranch == true).ToList();

            long _CountryAgentId = ClientCode.CountryAgentId.HasValue ? (long)ClientCode.CountryAgentId : 0;
            countryAgent = countryDa.GetCountry(_CountryAgentId);

            long _CountryPostId = ClientCode.CountryPostId.HasValue ? (long)ClientCode.CountryPostId : 0;
            countryPost = countryDa.GetCountry(_CountryPostId);



            long _agentID = ClientCode.AgentId.HasValue ? (long)ClientCode.AgentId : 0;
            agent = agentDa.GetAgent(_agentID);

            long _branchID = ClientCode.BranchId.HasValue ? (long)ClientCode.BranchId : 0;

            branch = agentDa.GetAgent(_branchID);


            flowdocument.Blocks.Clear();


            int bxcnt = ClientCode.Box_No.HasValue ? (int)ClientCode.Box_No : 0;
            int pltcnt = ClientCode.Pallet_No.HasValue ? (int)ClientCode.Pallet_No : 0;

            int boxcount = bxcnt + pltcnt;

            for (int c = 0; c < boxcount; c++)
            {
                Section headersection = new Section();

                Table headertable = generate_Header_With_ourOffices_Table();
                headersection.Blocks.Add(headertable);

                if (c != 0)
                {
                    if (c < boxcount)
                    {
                        headersection.BreakPageBefore = true;
                    }
                }


                flowdocument.Blocks.Add(headersection);

                 Table datatable = generate_data_table(c+1);
                 flowdocument.Blocks.Add(datatable);
             //   Table secondtable = generate_second_data_table();
             //   flowdocument.Blocks.Add(secondtable);
             //   Table ourofficetable = generate_ourOffices_Table();
            //    flowdocument.Blocks.Add(ourofficetable);

            }





        }

    }
}
