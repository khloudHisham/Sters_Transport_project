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
using System.Data;
using System.Windows.Shapes;
using ZXing;

namespace StersTransport.Presentation
{

    /// <summary>
    /// Stars Copy Invoice Report 
    /// </summary>
    public class CodeInvoice3PRSN
    {
        enum BranchLanguages
        {
            ar, ku
        }
        BranchLanguages branchLanguage;
        SolidColorBrush BlueColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#236BC9"));
        SolidColorBrush RedColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD015B"));
        SolidColorBrush BorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#404040"));
        SolidColorBrush Orangecolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E89933"));
        SolidColorBrush GreenColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00BB7E"));
        //#2C2D57
        SolidColorBrush NavyColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#2C2D57"));
        //#E45C32
        SolidColorBrush Orangecolor2 = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E45C32"));

        double lineheight = 20.0;
        double lineheightsmall = 5.0;
        double lineheight2 = 25.0;
        double marginthicness = 1.0;
        double paddingthicness = 0.0;
        double cellspacing = 4;
        double FontsizeLarge = 14;
        double FontsizeExtraLarge = 17;

        ClientCodeDA clientCodeDA = new ClientCodeDA();
        AgentDa agentDa = new AgentDa();
        CountryDa countryDa = new CountryDa();
        BranchDa branchDa = new BranchDa();
        CurrencyDa currencyDa = new CurrencyDa();


        ClientCode ClientCode { get; set; }

        Country countryBranch { get; set; }
        Country countryAgent { get; set; }
        Country countryPost { get; set; }
        Agent agent { get; set; }

        Agent branch { get; set; }

        Currency LocalCurrency { get; set; }
        Currency OuterCurrency { get; set; }

        City AgentCity { get; set; }




        List<Agent> Branches = new List<Agent>();


        public void generateDocument(FlowDocument flowdocument, string code, double lh, double fs)
        {
            flowdocument.FontSize = fs;
            lineheight = lh;
            ClientCode = clientCodeDA.GetClientCode(code);
            if (ClientCode == null) { return; }
            // get branches data 

            //  List<Branch> allbranches = branchDa.GetBranches();
            List<Agent> allbranches = agentDa.GetAgents();
            Branches = allbranches.Where(x => x.IsLocalCompanyBranch == true).ToList();
            long _CountryAgentId = ClientCode.CountryAgentId.HasValue ? (long)ClientCode.CountryAgentId : 0;
            countryAgent = countryDa.GetCountry(_CountryAgentId);


            long _CountryPostId = ClientCode.CountryPostId.HasValue ? (long)ClientCode.CountryPostId : 0;
            countryPost = countryDa.GetCountry(_CountryPostId);

            long _agentID = ClientCode.AgentId.HasValue ? (long)ClientCode.AgentId : 0;
            agent = agentDa.GetAgent(_agentID);

            long _branchID = ClientCode.BranchId.HasValue ? (long)ClientCode.BranchId : 0;
            //  branch = branchDa.GetBranch(_branchID);
            branch = agentDa.GetAgent(_branchID);


            if (agent != null)
            {
                long agentCityId = agent.CityId.HasValue ? (long)agent.CityId : 0;
                CityDa cityDa = new CityDa();
                AgentCity= cityDa.GetCity(agentCityId);

            }

            // local currency
            if (branch != null)
            {
                long _CountryBranchId = branch.CountryId.HasValue ? (long)branch.CountryId : 0;
                countryBranch = countryDa.GetCountry(_CountryBranchId);
                LocalCurrency = currencyDa.GetCurrency((long)countryBranch.CurrencyId);
            }
            // outer currency
            if (countryAgent != null)
            {
                OuterCurrency = currencyDa.GetCurrency((long)countryAgent.CurrencyId);
            }


            if (branch == null) { return; }
            if (agent == null) { return; }



            flowdocument.Blocks.Clear();

            // Have Local Post?
            bool havelocalpost = ClientCode.Have_Local_Post.HasValue ? (bool)ClientCode.Have_Local_Post : false;
            if (havelocalpost)
            { cellspacing = 2; }

            bool addRowForAgent = false;

            //Branch Language...
            string branchinvoiceLanguage = branch.InvoiceLanguage;//Ku,Ar
            branchLanguage = BranchLanguages.ar;
            string labelLanguageColumnName = "Arabic";

            if (branchinvoiceLanguage == "Ar")
            {
                branchLanguage = BranchLanguages.ar; labelLanguageColumnName = "Arabic";
            }
            else if (branchinvoiceLanguage == "Ku")
            {
                branchLanguage = BranchLanguages.ku; labelLanguageColumnName = "Kurdish";
            }




            // Main (Containing Table..)
            var containingTable = new Table();
            containingTable.CellSpacing = 0;
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            containingTable.RowGroups.Add(new TableRowGroup());

            int rowscount = 8;
            if (addRowForAgent)
            {
                rowscount = 9;// one more row for agent..
            }

            for (int c = 0; c < rowscount; c++)
            {
                containingTable.RowGroups[0].Rows.Add(new TableRow());
            }

            int lastaddedRowIndex = 0;

            string currentLabelStr = string.Empty;
            string currentvaluestr = string.Empty;


            #region Header Section
            // header table
            Table etable = new Table();
            etable.FlowDirection = FlowDirection.RightToLeft;
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = 0;
            etable.Margin = new Thickness(2,8,2,5);
            // add table columns....
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1.2, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1.3, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(.5, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(.75, GridUnitType.Star) });

            etable.RowGroups.Add(new TableRowGroup());

            etable.RowGroups[0].Rows.Add(new TableRow());
            TableRow Row = etable.RowGroups[0].Rows[0];

            // company label...
            currentLabelStr = getlabel("StersCompany", labelLanguageColumnName);
            var grid = new Grid();
            var blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            TextBlock txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            var tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor};
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            // Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontSize = FontsizeLarge, FontWeight = FontWeights.Bold, LineHeight = lineheight });


            // agent adrress
            // agent label 
            currentLabelStr = getlabel("Branch", labelLanguageColumnName);
            string branchstr = string.Empty;
            if (branchLanguage == BranchLanguages.ar)
            {
                branchstr = branch.AddressAR;
            }
            else if (branchLanguage == BranchLanguages.ku)
            {
                branchstr = branch.AddressKu;
            }
            currentvaluestr = string.Format("{0} {1}", currentLabelStr, branchstr);
             grid = new Grid();
             blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold;
            //txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

           // Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { FontSize = FontsizeLarge, FontWeight = FontWeights.Bold, LineHeight = lineheight });

            // sters company ...
            currentLabelStr = "Transport Company";
            grid = new Grid();
            grid.Height = lineheight * 2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, RowSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            //   Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontSize = FontsizeLarge, FontWeight = FontWeights.Bold, LineHeight = lineheight, RowSpan = 2, TextAlignment = TextAlignment.Right, Padding = new Thickness(paddingthicness) });


            //  Grid grid_ = new Grid(); grid_.Height = lineheight;
            //   grid_.Background = BlueColor;


            /*
            TextBlock tb = new TextBlock();
            tb.Text = "STARS"; tb.VerticalAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.Background = NavyColor;
            tb.Margin = new Thickness(2);
            tb.Foreground = Brushes.White;
            tb.FontSize = FontsizeLarge;
            tb.FontWeight = FontWeights.Bold;
            // grid_.Children.Add(tb);
            BlockUIContainer blockUIcntr = new BlockUIContainer(tb);

            var tablcell_ = new TableCell()
            {
                LineHeight = lineheight,
                Padding = new Thickness(paddingthicness),
            };
            tablcell_.Blocks.Add(blockUIcntr);
            Row.Cells.Add(tablcell_);
            */

            currentLabelStr = "STARS";
            grid = new Grid();
            grid.Height = lineheight * 2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.Background = NavyColor;
            txtblock.Margin = new Thickness(2);
            txtblock.Foreground = Brushes.White;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, RowSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            /*
            currentLabelStr = "EUKnet";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) 
            { 
                Background = BlueColor,Foreground=Brushes.White,
                LineHeight = lineheight, TextAlignment = TextAlignment.Left, 
                Padding=new Thickness(paddingthicness),
                 FontSize = FontsizeLarge,
                FontWeight = FontWeights.Bold
            }
            );
            */



            // Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight });
            // image logo

            grid = new Grid();
            grid.Height = lineheight * 2;
            var bitimage = ImageHelpercs.LoadImage(CompanyData.Logo2_Stars);
            var image = new Image();
            image.Source = bitimage;
           // image.Width = 65;
           // image.Height = 65;
            image.Stretch = Stretch.Uniform;
            grid.Children.Add(image);
            var block = new BlockUIContainer(grid);
            Row.Cells.Add(new TableCell(block) { LineHeight = lineheight, RowSpan = 2 });



            // second row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[1];

            // leading  in transprot label...
            currentLabelStr = getlabel("LeadingInTransport", labelLanguageColumnName);

             grid = new Grid();
            //  grid.Background = Orangecolor;
             blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
             txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.Foreground = Orangecolor2;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
             tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


           // Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { LineHeight = lineheightsmall,Background=Orangecolor2 });

            // branch telephones display string 

            currentLabelStr = branch.PhonesDisplayString;
            grid = new Grid();
            //  grid.Background = Orangecolor;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

          //  Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { LineHeight = lineheightsmall });


            // placeholder under the euknet 
            //  Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheightsmall });


            //  Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheightsmall });



            // third row

            /*

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[2];
            // image logo
            bitimage = ImageHelpercs.LoadImage(CompanyData.Logo2_Sters);
            image = new Image();
            image.Source = bitimage;
            image.Width = 50;
            image.Height = 50;
            block = new BlockUIContainer(image);
            Row.Cells.Add(new TableCell(block) { LineHeight = lineheight });


            currentLabelStr = getlabel("AgentInvoice", labelLanguageColumnName);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge }) { TextAlignment = TextAlignment.Center, LineHeight = lineheight, ColumnSpan = 3 });


            /*
            // placeholder  
            Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight });
            // placeholder  
            Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight });
            // placeholder  
             Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight });
            */




            /*
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_128;


            writer.Options = new ZXing.Common.EncodingOptions()
            {
                Height = 50,
                Width = 200
            };

            var result = writer.Write(ClientCode.Code);
            var barcodeBitmap = new System.Drawing.Bitmap(result);
            Image image2 = new Image();
            image2.Source = ImageHelpercs.ConvertBitmap(barcodeBitmap);
            var block2 = new BlockUIContainer(image2) { Margin = new Thickness(5) };
            Row.Cells.Add(new TableCell(block2) { LineHeight = lineheight });

            */

            // placeholder  
            //  Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight });
            #endregion

            var containingCell1 = new TableCell();

            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            TableRow trow = containingTable.RowGroups[0].Rows[lastaddedRowIndex];
            trow.Cells.Add(containingCell1);


            #region Code Section
            etable = new Table();
            etable.FlowDirection = FlowDirection.RightToLeft;
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = 0;
            etable.Margin = new Thickness(2,8,2,8);
            // add table columns....
            etable.Columns.Add(new TableColumn() { Width = new GridLength(.35, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(.5, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(.35, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(.5, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(.35, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(.5, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });

            etable.RowGroups.Add(new TableRowGroup());
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[0];


            // code path
             grid = new Grid(); grid.Height = lineheight2;
             blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            var path = new Path();
            path.Fill = BorderColor;
            path.StrokeThickness = 1;
            path.Data = Geometry.Parse("M49.767,20.6,34.093,36.277l-3.134-3.134L43.5,20.6,30.958,8.063l3.134-3.134Zm-42.5,0,12.54,12.54-3.134,3.134L1,20.6,16.674,4.929l3.134,3.134Z");
            path.HorizontalAlignment = HorizontalAlignment.Right;
            ScaleTransform myScaleTransform = new ScaleTransform();
            path.RenderTransformOrigin = new Point(0.5, 0.5);
            myScaleTransform.ScaleY = 1;
            myScaleTransform.ScaleX = 1;
            path.RenderTransform = myScaleTransform;
            Viewbox vbx = new Viewbox();
            vbx.Child = path;
            grid.Children.Add(vbx);
             tablcell = new TableCell();
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // code textt
            currentLabelStr = getlabel("Code", labelLanguageColumnName);
            grid = new Grid();
            //  grid.Background = Orangecolor;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            /*
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr) { BaselineAlignment = BaselineAlignment.Center }) { FontWeight = FontWeights.Bold, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) }) { LineHeight = lineheight });
            */



            // code value
            currentvaluestr = ClientCode.Code;

            grid = new Grid();
           // grid.Background = Orangecolor2;
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Foreground = Brushes.White;
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeExtraLarge;
            txtblock.Margin = new Thickness(0, 5, 0, 0);
             txtblock.Background = Orangecolor2;

            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {Background=Orangecolor2, LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            /*
            Row.Cells.Add(new TableCell
                (new Paragraph(new Run(currentvaluestr) { BaselineAlignment = BaselineAlignment.Center }) { FontWeight = FontWeights.Bold, Margin =new Thickness (marginthicness),Padding=new Thickness (paddingthicness)})
            { LineHeight = lineheight,BorderBrush=BorderColor,BorderThickness=new Thickness (1) });
            */


            // shipmeny path
            grid = new Grid();
            grid.Height = lineheight2;
            StackPanel stackPanel = new StackPanel();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            path = new Path();
            path.Fill = BorderColor;
            path.StrokeThickness = 1;

            path.Data = Geometry.Parse("M17.594,32.083a7.292,7.292,0,0,1-14.437,0H1v-25A2.083,2.083,0,0,1,3.083,5H32.25a2.083,2.083,0,0,1,2.083,2.083V11.25h6.25l6.25,8.45V32.083h-4.24a7.292,7.292,0,0,1-14.438,0ZM30.167,9.167h-25V25.938a7.292,7.292,0,0,1,11.8,1.979H28.785a7.347,7.347,0,0,1,1.381-1.979Zm4.167,12.5h8.333v-.594l-4.183-5.656h-4.15Zm1.042,12.5a3.126,3.126,0,1,0-3.126-3.126,3.126,3.126,0,0,0,3.126,3.126ZM13.5,31.042a3.125,3.125,0,1,0-3.125,3.125A3.125,3.125,0,0,0,13.5,31.042Z");
            path.HorizontalAlignment = HorizontalAlignment.Right;
            path.RenderTransformOrigin = new Point(0.5, 0.5);
            myScaleTransform = new ScaleTransform();
            myScaleTransform.ScaleY = 1;
            myScaleTransform.ScaleX = 1;
            path.RenderTransform = myScaleTransform;
            vbx = new Viewbox();
            vbx.Child = path;
            grid.Children.Add(vbx);

            tablcell = new TableCell();
            tablcell.Blocks.Add(blockUI);

            Row.Cells.Add(tablcell);

            /*
                 Row.Cells.Add(new TableCell
                        (new Paragraph(new Run(string.Empty)) { Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                 { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

                 */

            //    currentLabelStr = "tr";
            //    Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, 10, 0, 10), Padding = new Thickness(0, 10, 0, 10) }) { LineHeight = lineheight });



            // shipmenty text
            currentLabelStr = getlabel("ShipmentNO", labelLanguageColumnName);
            grid = new Grid();
            //  grid.Background = Orangecolor;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock(); txtblock.FontWeight = FontWeights.Bold;
            txtblock.Text = currentLabelStr; txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            /*
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontWeight = FontWeights.Bold, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0,paddingthicness, 0,paddingthicness) }) { LineHeight = lineheight });
            */

            // shipment value
            double shipmentno = ClientCode.Shipment_No.HasValue ? (double)ClientCode.Shipment_No : 0;
            currentvaluestr = shipmentno.ToString();
            grid = new Grid();
          //  grid.Background = Orangecolor;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock(); txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeExtraLarge;
            txtblock.Margin = new Thickness(0, 5, 0, 0);
            txtblock.Text = currentvaluestr;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            /*

            Row.Cells.Add(new TableCell
                (new Paragraph(new Run(currentvaluestr)) { FontWeight = FontWeights.Bold, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
            { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

            */


            // date path
            grid = new Grid(); grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            blockUI.TextAlignment = TextAlignment.Left;
            path = new Path();
            path.Fill = BorderColor;
            path.StrokeThickness = 1;
            path.Data = Geometry.Parse("M32,5h8a2,2,0,0,1,2,2V39a2,2,0,0,1-2,2H4a2,2,0,0,1-2-2V7A2,2,0,0,1,4,5h8V1h4V5H28V1h4ZM28,9H16v4H12V9H6v8H38V9H32v4H28ZM38,21H6V37H38Z");
            path.HorizontalAlignment = HorizontalAlignment.Right;
            path.RenderTransformOrigin = new Point(0.5, 0.5);
            myScaleTransform = new ScaleTransform();
            myScaleTransform.ScaleY = 1;
            myScaleTransform.ScaleX = 1;
            path.RenderTransform = myScaleTransform;
            vbx = new Viewbox();
            vbx.Child = path;
            grid.Children.Add(vbx);

            tablcell = new TableCell();
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // currentLabelStr = "dt";
            // Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, 10, 0, 10), Padding = new Thickness(0, 10, 0, 10) }) { LineHeight = lineheight });


            // date text

            currentLabelStr = getlabel("Date", labelLanguageColumnName);
            grid = new Grid();
            // grid.Background = Orangecolor;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            /*
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontWeight = FontWeights.Bold, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) }) { LineHeight = lineheight });
            */


            // date value
            string datestr = ClientCode.PostDate.HasValue ? ClientCode.PostDate.Value.ToShortDateString() : string.Empty;
            currentvaluestr = datestr;

            grid = new Grid();
        //    grid.Background = Orangecolor;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentvaluestr;
            txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeExtraLarge;
            txtblock.Margin = new Thickness(0, 5, 0, 0);
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            /*
            Row.Cells.Add(new TableCell
                 (new Paragraph(new Run(currentvaluestr)) { FontWeight = FontWeights.Bold, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
            { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });
            */


            #endregion

            containingCell1 = new TableCell();

            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            lastaddedRowIndex++;
            trow = containingTable.RowGroups[0].Rows[lastaddedRowIndex];
            trow.Cells.Add(containingCell1);


            #region Receiver Section
            etable = new Table();
            etable.FlowDirection = FlowDirection.RightToLeft;
            etable.Margin = new Thickness(2,5,2,5);
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = cellspacing;
            etable.Columns.Add(new TableColumn() { Width = new GridLength(2, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(4, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(4, GridUnitType.Star) });



            etable.RowGroups.Add(new TableRowGroup());
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[0];

            int rowspanforreceiverinfo = 2;
            if (havelocalpost) { rowspanforreceiverinfo = 3; }

            currentLabelStr = getlabel("ReceiverInfo", labelLanguageColumnName);
        
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr) ) {FontSize=FontsizeExtraLarge, FontWeight = FontWeights.Bold, Foreground = BlueColor, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight, RowSpan = rowspanforreceiverinfo });


            currentLabelStr = getlabel("Name", labelLanguageColumnName);
            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */

            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });

            currentvaluestr = ClientCode.ReceiverName;
            /*
            grid = new Grid();
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */
           Row.Cells.Add(new TableCell
               (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
         { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

            currentLabelStr = getlabel("Phone", labelLanguageColumnName);
            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */

            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
           { LineHeight = lineheight });

            currentvaluestr = ClientCode.Receiver_Tel;
            /*
            grid = new Grid();
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */
             Row.Cells.Add(new TableCell
             (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
          { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

            if (havelocalpost) // add more rows
            {
                // new row
                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[1];
                // country
                currentLabelStr = getlabel("Country", labelLanguageColumnName);

                /*
                grid = new Grid();
                blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
                txtblock.FontSize = FontsizeLarge;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);
                */
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                currentvaluestr = string.Empty;
                if (countryPost != null)
                {
                    currentvaluestr = countryPost.CountryName;
                }

                grid = new Grid();
                /*
                grid.Height = lineheight2;
                blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);
                */
                Row.Cells.Add(new TableCell
               (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

                // city
                currentLabelStr = getlabel("City", labelLanguageColumnName);

                /*
                grid = new Grid();
                blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
                txtblock.FontSize = FontsizeLarge;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);
                */
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                currentvaluestr = ClientCode.CityPost;
                /*
                grid = new Grid();
                grid.Height = lineheight2;
                blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);
                */

                Row.Cells.Add(new TableCell
            (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });


                // new row
                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[2];



                // address
                currentLabelStr = getlabel("Address", labelLanguageColumnName);
                /*
                grid = new Grid();
                blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
                txtblock.FontSize = FontsizeLarge;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);
                */



                //
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                if (!string.IsNullOrEmpty(ClientCode.Dep_Appar))
                { currentvaluestr = string.Format("{0} / {1}", ClientCode.Street_Name_No, ClientCode.Dep_Appar); }
                else
                { currentvaluestr = string.Format("{0}", ClientCode.Street_Name_No); }
               
                /*
                grid = new Grid();
                grid.Height = lineheight2;
                blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                tablcell = new TableCell() {  LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);
                */

                Row.Cells.Add(new TableCell
            (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

                // zipcode
                currentLabelStr = getlabel("PostalCode", labelLanguageColumnName);
                /*
                grid = new Grid();
                blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
                txtblock.FontSize = FontsizeLarge;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);
                */

                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                currentvaluestr = ClientCode.ZipCode;
                /*
                grid = new Grid();
                grid.Height = lineheight2;
                blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(txtblock);
                tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);
                */

                Row.Cells.Add(new TableCell
             (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });
            }
            else // No Local Post ... Deliver in Agent Office.. Add Country And City
            {
                // new row
                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[1];
                // country
                currentLabelStr = getlabel("Country", labelLanguageColumnName);


                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                currentvaluestr = string.Empty;
                if (countryAgent != null)
                {
                    currentvaluestr = countryAgent.CountryName;
                }
                Row.Cells.Add(new TableCell
              (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });


                // city
                currentLabelStr = getlabel("City", labelLanguageColumnName);


                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                currentvaluestr = string.Empty;
                if (AgentCity != null)
                {
                    currentvaluestr = AgentCity.CityName;
                }
               
                Row.Cells.Add(new TableCell
            (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

            }


            #endregion


            containingCell1 = new TableCell();

            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            lastaddedRowIndex++;
            trow = containingTable.RowGroups[0].Rows[lastaddedRowIndex];
            trow.Cells.Add(containingCell1);



            #region sender Section
            etable = new Table();
            etable.FlowDirection = FlowDirection.RightToLeft;
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = cellspacing;
            etable.Columns.Add(new TableColumn() { Width = new GridLength(2, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(4, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(4, GridUnitType.Star) });


            etable.Margin = new Thickness(2,5,2,5);
            etable.RowGroups.Add(new TableRowGroup());
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[0];


            currentLabelStr = getlabel("SenderInfo", labelLanguageColumnName);
           Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeExtraLarge, FontWeight = FontWeights.Bold, Foreground = BlueColor, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
         { LineHeight = lineheight });

            currentLabelStr = getlabel("Name", labelLanguageColumnName);


            /*
            grid = new Grid();
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */



        Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr) { FontSize=FontsizeLarge}) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
           { LineHeight = lineheight });

            currentvaluestr = ClientCode.SenderName;


            /*
            grid = new Grid();
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */

            Row.Cells.Add(new TableCell
                (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge,  Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
            { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

            currentLabelStr = getlabel("Phone", labelLanguageColumnName);
            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });

            currentvaluestr = ClientCode.Sender_Tel;

            /*
            grid = new Grid();
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */
            Row.Cells.Add(new TableCell
                (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
            { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });


            #endregion

            containingCell1 = new TableCell();
            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            lastaddedRowIndex++;
            trow = containingTable.RowGroups[0].Rows[lastaddedRowIndex];
            trow.Cells.Add(containingCell1);


           
            if (addRowForAgent)
            {

                #region agent
                etable = new Table();
                etable.FlowDirection = FlowDirection.RightToLeft;
                etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = cellspacing;
                etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
                etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
                etable.Columns.Add(new TableColumn() { Width = new GridLength(3, GridUnitType.Star) });
                etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
                etable.Columns.Add(new TableColumn() { Width = new GridLength(3, GridUnitType.Star) });
                etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
                etable.Columns.Add(new TableColumn() { Width = new GridLength(3, GridUnitType.Star) });

                etable.RowGroups.Add(new TableRowGroup());
                etable.RowGroups[0].Rows.Add(new TableRow());
                Row = etable.RowGroups[0].Rows[0];

                currentLabelStr = getlabel("Agent", labelLanguageColumnName);
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontWeight = FontWeights.Bold, Foreground = BlueColor, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                currentLabelStr = getlabel("Name", labelLanguageColumnName);
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                currentvaluestr = string.Empty;
                if (agent != null)
                {
                    currentvaluestr = agent.ContactPersonName;
                }
                Row.Cells.Add(new TableCell
                    (new Paragraph(new Run(currentvaluestr)) { Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

                // office
                currentLabelStr = getlabel("Office", labelLanguageColumnName);
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                currentvaluestr = string.Empty;
                if (countryAgent != null)
                {
                    currentvaluestr = agent.AgentName;
                }
                Row.Cells.Add(new TableCell
                    (new Paragraph(new Run(currentvaluestr)) { Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

                // phone

                currentLabelStr = getlabel("Phone", labelLanguageColumnName);
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
                { LineHeight = lineheight });

                currentvaluestr = string.Empty;
                if (agent != null)
                {
                    currentvaluestr = agent.PhoneNo1;
                    if (!string.IsNullOrEmpty(agent.PhoneNo2))
                    {
                        currentvaluestr += "/" + agent.PhoneNo2;
                    }
                }
                Row.Cells.Add(new TableCell
                    (new Paragraph(new Run(currentvaluestr)) { Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
                { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });


                #endregion

                containingCell1 = new TableCell();
                containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
                lastaddedRowIndex++;
                trow = containingTable.RowGroups[0].Rows[lastaddedRowIndex];
                trow.Cells.Add(containingCell1);



            }




            #region  ShipmentDetails
            etable = new Table();
            etable.FlowDirection = FlowDirection.RightToLeft;
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = cellspacing;
            etable.Columns.Add(new TableColumn() { Width = new GridLength(2, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(4, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(4, GridUnitType.Star) });
            etable.Margin = new Thickness(2,5,2,5);
            etable.RowGroups.Add(new TableRowGroup());
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[0];

            currentLabelStr = getlabel("ShipmentDetails", labelLanguageColumnName);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) {FontSize=FontsizeExtraLarge, FontWeight = FontWeights.Bold, Foreground = BlueColor, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight, RowSpan = 4 });

            // box count
            currentLabelStr = getlabel("NOBoxes", labelLanguageColumnName);

            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */

            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });


            double boxno = ClientCode.Box_No.HasValue ? (double)ClientCode.Box_No : 0;
            double Palletno = ClientCode.Pallet_No.HasValue ? (double)ClientCode.Pallet_No : 0;
            string boxstr = string.Format("{0} Box(s)", boxno.ToString());
            string palletstr = string.Empty;
            if (Palletno > 0)
            {
                palletstr = string.Format(" ,{0} Pallet(s)", Palletno.ToString());
            }
            currentvaluestr = string.Format("{0}{1}", boxstr, palletstr);
            /*
            grid = new Grid();
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FlowDirection = FlowDirection.LeftToRight;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */


            Row.Cells.Add(new TableCell
               (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge,FlowDirection = FlowDirection.LeftToRight, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
           { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });


            // weight
            currentLabelStr = getlabel("WeightKG", labelLanguageColumnName);
            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */


            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
           { LineHeight = lineheight });


            double weighttotal = ClientCode.Weight_Total.HasValue ? (double)ClientCode.Weight_Total : 0;
            currentvaluestr = weighttotal.ToString();
            /*
            grid = new Grid();
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {  LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */
           Row.Cells.Add(new TableCell
                (new Paragraph(new Run(currentvaluestr)) { FontSize = FontsizeLarge, Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
            { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });





            // new row...
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[1];



            // goods discription
            currentLabelStr = getlabel("GoodsDiscription", labelLanguageColumnName);
            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */


             Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });


            currentvaluestr = ClientCode.Goods_Description;
            /*
            grid = new Grid();
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.TextWrapping = TextWrapping.WrapWithOverflow;
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */
          
            var p = new Paragraph();
            p.Inlines.Add(new TextBlock()
            {
                Text = currentvaluestr,
                TextWrapping = TextWrapping.NoWrap,
                Margin = new Thickness(marginthicness),
                Padding = new Thickness(paddingthicness)
            });


            Row.Cells.Add(new TableCell
                (p)
            { FontSize = FontsizeLarge, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });

          
            // goods value
            currentLabelStr = getlabel("GoodsValue", labelLanguageColumnName);
            /*
            
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
           { LineHeight = lineheight });



            currentvaluestr = ClientCode.Goods_Value.HasValue ? string.Format("{0:#,0.##}", ClientCode.Goods_Value.ToString())  : string.Empty;
            /*
            grid = new Grid();
            grid.Height = lineheight2;
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentvaluestr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {   LineHeight = lineheight2, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */


           Row.Cells.Add(new TableCell
               (new Paragraph(new Run(currentvaluestr)) { Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
           { FontSize = FontsizeLarge, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });



            // new row...

            /*

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[2];



            // have insurance ?
            currentLabelStr = getlabel("HaveInsurance", labelLanguageColumnName);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });


            bool haveinsurance = ClientCode.Have_Insurance.HasValue ? (bool)ClientCode.Have_Insurance : false;
            stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            blockUI = new BlockUIContainer(stackPanel);
            blockUI.Margin = new Thickness(marginthicness);
            CheckBox checkBoxinsuranceyes = new CheckBox();
            checkBoxinsuranceyes.IsChecked = haveinsurance;
            checkBoxinsuranceyes.Margin = new Thickness(marginthicness, 0, marginthicness, 0);
            checkBoxinsuranceyes.IsHitTestVisible = false;
            checkBoxinsuranceyes.Focusable = false;
            checkBoxinsuranceyes.FlowDirection = FlowDirection.LeftToRight;
            checkBoxinsuranceyes.Content = getlabel("Yes", labelLanguageColumnName);
            stackPanel.Children.Add(checkBoxinsuranceyes);

            CheckBox checkBoxinsuranceno = new CheckBox();
            checkBoxinsuranceno.IsChecked = !haveinsurance;
            checkBoxinsuranceno.Margin = new Thickness(marginthicness, 0, marginthicness, 0);
            checkBoxinsuranceno.IsHitTestVisible = false;
            checkBoxinsuranceno.Focusable = false;
            checkBoxinsuranceno.FlowDirection = FlowDirection.LeftToRight;

            checkBoxinsuranceno.Content = getlabel("No", labelLanguageColumnName);
            stackPanel.Children.Add(checkBoxinsuranceno);
            tablcell = new TableCell();
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });
            Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });




            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[3];

            currentLabelStr = getlabel("Notes", labelLanguageColumnName);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });



            currentvaluestr = ClientCode.Note_Send;


            p = new Paragraph();
            p.Inlines.Add(new TextBlock()
            {
                Text = currentvaluestr,
                TextWrapping = TextWrapping.NoWrap,
                Margin = new Thickness(marginthicness),
                Padding = new Thickness(paddingthicness)
            });


            Row.Cells.Add(new TableCell
                (p)
            { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 3 });


            */


            /*
            Row.Cells.Add(new TableCell
                (new Paragraph(new Run(currentvaluestr)) { Margin = new Thickness(marginthicness), Padding = new Thickness(paddingthicness) })
            { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1),ColumnSpan=3 });
          */

            #endregion

            containingCell1 = new TableCell();
            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            lastaddedRowIndex++;
            trow = containingTable.RowGroups[0].Rows[lastaddedRowIndex];
            trow.Cells.Add(containingCell1);



            #region TransportCostDetails

            string localcurrencyAR = "";
            if (LocalCurrency != null)
            {
                if (branchLanguage == BranchLanguages.ar)
                { localcurrencyAR = LocalCurrency.NameAR; }
                else
                if (branchLanguage == BranchLanguages.ku)
                {
                    localcurrencyAR = LocalCurrency.NameKU;
                }
                else
                { localcurrencyAR = LocalCurrency.Name; }

            }
            string outerCurrencyENG = "";
            if (OuterCurrency != null)
            {
                outerCurrencyENG = OuterCurrency.Name; // always english 
                /*
                if (branchLanguage == BranchLanguages.ar)
                { outerCurrencyENG = OuterCurrency.NameAR; }
                else
                if (branchLanguage == BranchLanguages.ku)
                {
                    outerCurrencyENG = OuterCurrency.NameKU;
                }
                else
                { outerCurrencyENG = OuterCurrency.Name; }
                */

            }

            etable = new Table();
            etable.FlowDirection = FlowDirection.RightToLeft;
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = cellspacing;
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1.5, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1.5, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1.5, GridUnitType.Star) });


            etable.Margin = new Thickness(2,2,2,0);
            etable.RowGroups.Add(new TableRowGroup());


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // have insurance ?
            currentLabelStr = getlabel("HaveInsurance", labelLanguageColumnName);
            // Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            //  { LineHeight = lineheight });


            Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)))
             { LineHeight = lineheight });
            Row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)))
            { LineHeight = lineheight });


            bool haveinsurance = ClientCode.Have_Insurance.HasValue ? (bool)ClientCode.Have_Insurance : false;
            stackPanel = new StackPanel();
            
            stackPanel.Orientation = Orientation.Horizontal;
            blockUI = new BlockUIContainer(stackPanel);
            blockUI.Margin = new Thickness(8, 0, 8, 0);

            TextBlock tb1 = new TextBlock();
            tb1.Text = currentLabelStr;
            tb1.FontSize = FontsizeExtraLarge;
            tb1.FontWeight = FontWeights.Bold;
            tb1.Margin = new Thickness(5);
            stackPanel.Children.Add(tb1);

            CheckBox checkBoxinsuranceyes = new CheckBox();
            checkBoxinsuranceyes.IsChecked = haveinsurance;
            checkBoxinsuranceyes.FontSize = FontsizeExtraLarge;
            checkBoxinsuranceyes.FontWeight = FontWeights.Bold;

            checkBoxinsuranceyes.Margin = new Thickness(8, 0, 8, 0);
            checkBoxinsuranceyes.IsHitTestVisible = false;
            checkBoxinsuranceyes.Focusable = false;
            checkBoxinsuranceyes.FlowDirection = FlowDirection.LeftToRight;
            checkBoxinsuranceyes.Content = getlabel("Yes", labelLanguageColumnName);
            stackPanel.Children.Add(checkBoxinsuranceyes);

            CheckBox checkBoxinsuranceno = new CheckBox();
            checkBoxinsuranceno.IsChecked = !haveinsurance;
            checkBoxinsuranceno.Margin = new Thickness(8, 0, 8, 0);
            checkBoxinsuranceno.IsHitTestVisible = false;
            checkBoxinsuranceno.Focusable = false;
            checkBoxinsuranceno.FlowDirection = FlowDirection.LeftToRight;

            checkBoxinsuranceno.Content = getlabel("No", labelLanguageColumnName);
            checkBoxinsuranceno.FontSize = FontsizeExtraLarge;
            checkBoxinsuranceno.FontWeight = FontWeights.Bold;
            stackPanel.Children.Add(checkBoxinsuranceno);
            tablcell = new TableCell();
            tablcell.ColumnSpan = 4;
            tablcell.TextAlignment = TextAlignment.Center;
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);





            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            currentLabelStr = getlabel("TransportCostDetails", labelLanguageColumnName);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeExtraLarge, FontWeight = FontWeights.Bold, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight, Foreground = BlueColor, ColumnSpan = 6 });

            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added row



            currentLabelStr = getlabel("TransportCost", labelLanguageColumnName);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });



            DockPanel dp = new DockPanel();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;
            double Sub_Post_Cost_IQD = ClientCode.Sub_Post_Cost_IQD.HasValue ? (double)ClientCode.Sub_Post_Cost_IQD : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.##}", Sub_Post_Cost_IQD);
            dp.Children.Add(txtblock);


            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { FontSize = FontsizeLarge, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

          
            currentLabelStr = getlabel("InsuranceCost", labelLanguageColumnName);
            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */



           Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
           { LineHeight = lineheight });



            dp = new DockPanel();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;
            double Insurance_Amount = ClientCode.Insurance_Amount.HasValue ? (double)ClientCode.Insurance_Amount : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.##}", Insurance_Amount);
            dp.Children.Add(txtblock);


            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { FontSize = FontsizeLarge, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);




            currentLabelStr = getlabel("RemainingAmount", labelLanguageColumnName);
            /*
          
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */

            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
           { LineHeight = lineheight });

            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;


            double Remaining_IQD = ClientCode.Remaining_IQD.HasValue ? (double)ClientCode.Remaining_IQD : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.##}", Remaining_IQD);
            dp.Children.Add(txtblock);


            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { FontSize = FontsizeLarge, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


 

        

            //new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added row

            currentLabelStr = getlabel("DoorToDoorCost", labelLanguageColumnName);
            /*
           
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            */

            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge,Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
           { LineHeight = lineheight });



            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;
            

            double POST_DoorToDoor_IQD = ClientCode.POST_DoorToDoor_IQD.HasValue ? (double)ClientCode.POST_DoorToDoor_IQD : 0;
           
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.##}", POST_DoorToDoor_IQD);
            dp.Children.Add(txtblock);

            if (POST_DoorToDoor_IQD > 0)
            {
                txtblock.Foreground = Brushes.White;
            }
            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR;
            if (POST_DoorToDoor_IQD > 0)
            {
                txtblock.Foreground = Brushes.White;
            }
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { FontSize = FontsizeLarge, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            if (POST_DoorToDoor_IQD > 0)
            { tablcell.Background = BlueColor; }

            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


   


            currentLabelStr = getlabel("AdminExportCost", labelLanguageColumnName);
            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            */


            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge,Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });



            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;


            double Custome_Cost_Qomrk = ClientCode.Custome_Cost_Qomrk.HasValue ? (double)ClientCode.Custome_Cost_Qomrk : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.##}", Custome_Cost_Qomrk);
            dp.Children.Add(txtblock);


            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { FontSize = FontsizeLarge,LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

        



            string countryagentname = string.Empty;
            countryagentname = countryAgent.CountryName;
            // if the country is europe contininet relpace with europe



            if (branchLanguage == BranchLanguages.ar)
            { countryagentname = countryAgent.CountryNameAR; }
            else if (branchLanguage == BranchLanguages.ku)
            { countryagentname = countryAgent.CountryNameKU; }
            else
            { countryagentname = countryAgent.CountryName; }


            if (countryAgent.continent == StaticData.Continent_Europe)
            {
                countryagentname = StaticData.Continent_Europe;
            }
            currentLabelStr = string.Format("{0} {1}", getlabel("PaidINEurope", labelLanguageColumnName), countryagentname);


         
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold; 
            txtblock.FontSize = FontsizeLarge-1;// Notice The Minus Here...
            txtblock.TextWrapping = TextWrapping.NoWrap;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            //txtblock.TextTrimming = TextTrimming.
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
           

            /*
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr) ) 
            {
                
                FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness),
                Padding = new Thickness(0, paddingthicness, 0, paddingthicness) }
            )
            { LineHeight = lineheight });
            */



            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;


            double EuropaToPay = ClientCode.EuropaToPay.HasValue ? (double)ClientCode.EuropaToPay : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.##}", EuropaToPay);
            dp.Children.Add(txtblock);

            if (EuropaToPay > 0)
            {
                txtblock.Foreground = Brushes.White;
            }

            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;

            //  txtblock.Text = "EUR";
            txtblock.Text = outerCurrencyENG;


            dp.Children.Add(txtblock);

            if (EuropaToPay > 0)
            {
                txtblock.Foreground = Brushes.White;
            }

            tablcell = new TableCell() { FontSize = FontsizeLarge,LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            if (EuropaToPay > 0)
            {
                tablcell.Background = RedColor;
            }

            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

         


            //new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added row




          


            currentLabelStr = getlabel("TotalCost", labelLanguageColumnName);

            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge,Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });




            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;
           

            double Total_Post_Cost_IQD = ClientCode.Total_Post_Cost_IQD.HasValue ? (double)ClientCode.Total_Post_Cost_IQD : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            // txtblock.Text = string.Format("{0:n0}", Total_Post_Cost_IQD);
            txtblock.Text = string.Format("{0:#,0.##}", Total_Post_Cost_IQD);
            dp.Children.Add(txtblock);
            txtblock.Foreground = Brushes.White;

            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Foreground = Brushes.White;
            txtblock.Text = localcurrencyAR;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { FontSize = FontsizeLarge, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            tablcell.Background = Orangecolor2;

            Row.Cells.Add(tablcell);



            currentLabelStr = getlabel("PaidAmount", labelLanguageColumnName);

            /*
            grid = new Grid();
            blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold; txtblock.FontSize = FontsizeLarge;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight2, BorderBrush = BorderColor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            */

            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });



            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;


            double TotalPaid_IQD = ClientCode.TotalPaid_IQD.HasValue ? (double)ClientCode.TotalPaid_IQD : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.##}", TotalPaid_IQD);
            dp.Children.Add(txtblock);

            if (TotalPaid_IQD > 0)
            {
                dp.Background = GreenColor;
            }
            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { FontSize = FontsizeLarge, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);



            Row.Cells.Add(new TableCell(new Paragraph(new Run("")) { Margin = new Thickness(0, marginthicness, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight });


            currentLabelStr = getlabel("Sign", labelLanguageColumnName);
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { FontSize = FontsizeLarge, Margin = new Thickness(0, lineheight*3, 0, marginthicness), Padding = new Thickness(0, paddingthicness, 0, paddingthicness) })
            { LineHeight = lineheight,RowSpan=3, BorderBrush = BorderColor, BorderThickness = new Thickness(1) });



            //new row
            // empty row : a hack method
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added row
            stackPanel = new StackPanel();
            stackPanel.Height = lineheight ;
            blockUI = new BlockUIContainer(stackPanel);
            tablcell = new TableCell() {   LineHeight = lineheight , BorderBrush = BorderColor,   ColumnSpan = 5 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            //new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added row



            stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Height = lineheight * 2;
           // stackPanel.Margin = new Thickness(0, 5, 0, 0);
            txtblock = new TextBlock();
            currentLabelStr = string.Format("{0} :", getlabel("Disallowed Items", labelLanguageColumnName));
            txtblock.Text = currentLabelStr;
            txtblock.Foreground = RedColor; txtblock.VerticalAlignment = VerticalAlignment.Center;
             txtblock.FontSize = FontsizeExtraLarge;
            txtblock.FontWeight = FontWeights.Bold;
            stackPanel.Children.Add(txtblock);
            txtblock = new TextBlock();
            currentLabelStr = getlabel("Disallowed Items List Values", labelLanguageColumnName);
            txtblock.Text = currentLabelStr;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.FontWeight = FontWeights.Bold;
            stackPanel.Children.Add(txtblock);
            blockUI = new BlockUIContainer(stackPanel);

            tablcell = new TableCell() {Padding=new Thickness (0,5,0,0), LineHeight = lineheight*2, BorderBrush = BorderColor, BorderThickness = new Thickness(0,1,0,0),ColumnSpan=5 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
           


            #endregion

            containingCell1 = new TableCell();
            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            lastaddedRowIndex++;
            trow = containingTable.RowGroups[0].Rows[lastaddedRowIndex];
            trow.Cells.Add(containingCell1);




            flowdocument.Blocks.Add(new Section(containingTable));

        }

        private string form_agreemetPoints(string labelLanguageColumnName)
        {
            string result = string.Empty;
            int pointnumber = 0;
            string currentLabelStr = getlabel("agreement1", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result += string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                result += Environment.NewLine;
            }

            currentLabelStr = getlabel("agreement2", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result += string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                result += Environment.NewLine;
            }

            currentLabelStr = getlabel("agreement3", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result += string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                result += Environment.NewLine;
            }
            currentLabelStr = getlabel("agreement4", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result += string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                result += Environment.NewLine;
            }

            currentLabelStr = getlabel("agreement4a", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {

                result += string.Format("   {0}{1} {2}", "أ", "-", currentLabelStr);
                result += Environment.NewLine;
            }
            currentLabelStr = getlabel("agreement4b", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {

                result += string.Format("   {0}{1} {2}", "ب", "-", currentLabelStr);
                result += Environment.NewLine;
            }


            currentLabelStr = getlabel("agreement5", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result += string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                result += Environment.NewLine;
            }
            currentLabelStr = getlabel("agreement6", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result += string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                result += Environment.NewLine;
            }
            currentLabelStr = getlabel("agreement7", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result += string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);

            }

            return result;
        }


        private List<Paragraph> form_agreemetPointstoparagraphs(string labelLanguageColumnName)
        {
            var paragraphs = new List<Paragraph>();
            var paragraph = new Paragraph();
            string result = string.Empty;
            int pointnumber = 0;
            string currentLabelStr = getlabel("agreement1", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result = string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);

                paragraph.Inlines.Add(new Run(result));
                paragraphs.Add(paragraph);
                //  result += Environment.NewLine;
            }

            currentLabelStr = getlabel("agreement2", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result = string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(result));
                paragraphs.Add(paragraph);
                //  result += Environment.NewLine;
            }

            currentLabelStr = getlabel("agreement3", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result = string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(result));
                paragraphs.Add(paragraph);
                //  result += Environment.NewLine;
            }
            currentLabelStr = getlabel("agreement4", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result = string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(result));
                paragraphs.Add(paragraph);
                //   result += Environment.NewLine;
            }

            currentLabelStr = getlabel("agreement4a", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {

                result = string.Format("   {0}{1} {2}", "أ", "-", currentLabelStr);
                paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(result));
                paragraphs.Add(paragraph);
                //   result += Environment.NewLine;
            }
            currentLabelStr = getlabel("agreement4b", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {

                result = string.Format("   {0}{1} {2}", "ب", "-", currentLabelStr);
                paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(result));
                paragraphs.Add(paragraph);
                //  result += Environment.NewLine;
            }


            currentLabelStr = getlabel("agreement5", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result = string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(result));
                paragraphs.Add(paragraph);
                //   result += Environment.NewLine;
            }
            currentLabelStr = getlabel("agreement6", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result = string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(result));
                paragraphs.Add(paragraph);
                //   result += Environment.NewLine;
            }
            currentLabelStr = getlabel("agreement7", labelLanguageColumnName);
            if (!string.IsNullOrEmpty(currentLabelStr))
            {
                pointnumber++;
                result = string.Format("{0}{1} {2}", pointnumber, "-", currentLabelStr);
                paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(result));
                paragraphs.Add(paragraph);

            }

            foreach (var p in paragraphs)
            {
                p.TextAlignment = TextAlignment.Left;
            }

            return paragraphs;
        }

        private string getlabel(string key, string labelLanguageColumnName)
        {
            string lbl = string.Empty;
            DataRow DR_Label = StaticData.Labels.Select("Keyword='" + key + "'").FirstOrDefault();

            if (DR_Label != null)
            {
                //labelLanguageColumnName
                lbl = DR_Label[labelLanguageColumnName].ToString();
            }
            return lbl;
        }

        private string generate_branches_string(List<Agent> branches)
        {
            string result = string.Empty;
            int branchcntr = 0;
            for (int c = 0; c < branches.Count; c++)
            {
                result += string.Format("{0} : {1}  ", branches[c].AgentName, branches[c].PhonesDisplayString);
                branchcntr++;
                if (branchcntr > 1)
                {
                    result += Environment.NewLine;
                    branchcntr = 0;
                }

            }
            return result;
        }
    }
}
