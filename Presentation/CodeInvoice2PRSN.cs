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
    public class CodeInvoice2PRSN
    {
        enum BranchLanguages
        {
            ar, ku
        }
        BranchLanguages branchLanguage;

        SolidColorBrush BlueColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#236BC9"));
        SolidColorBrush RedColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD015B"));
        // SolidColorBrush BorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#404040"));
        SolidColorBrush BorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#000000"));
        SolidColorBrush Orangecolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E89933"));
        SolidColorBrush GreenColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00BB7E"));


  
        SolidColorBrush graycolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#D2D2D2"));
        SolidColorBrush lightbluecolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B6CFE6"));
        SolidColorBrush Orangecolor2 = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F7C663"));
   
        SolidColorBrush yellowcolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F7F700"));
        SolidColorBrush Orangecolor3 = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F7BA00"));
        SolidColorBrush lightpink = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F2C1A6"));
 

        double lineheight = 15.0;
        double lineheightsmall = 5.0;
        double lineheight2 = 15.0;
        double marginthicness = 0.0;
        double paddingthicness = 0.0;
        double cellspacing = 0.5;
        double FontsizeLarge = 14;
        double FontsizeExtraLarge = 16;
        double FontsizeExtraLarge2 = 18;
        double FontsizedoubleExtraLarge = 26;



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




        List<Agent> Branches = new List<Agent>();



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


            bool addRowForAgent = true;

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
            // add table columns....
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(2, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(2, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
 

            etable.RowGroups.Add(new TableRowGroup());

            etable.RowGroups[0].Rows.Add(new TableRow());
            TableRow Row = etable.RowGroups[0].Rows[0];


            // image logo
            var bitimage = ImageHelpercs.LoadImage(CompanyData.Logo2_Sters);
             var image = new Image();
            image.Source = bitimage;
            image.Width = 100;
            image.Height = 100;
            var block = new BlockUIContainer(image);
            Row.Cells.Add(new TableCell(block) { LineHeight = lineheight,RowSpan=5 });

            // second cell 
            // company label...



            currentvaluestr = getlabel("StersCompany", labelLanguageColumnName);

            /*
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontSize = FontsizeLarge, 
                ColumnSpan=2,
                FontWeight = FontWeights.Bold, LineHeight = lineheight });
            */

            var gridHeader = new Grid();
            //  grid.Background = Orangecolor;
            var blockUIHeader = new BlockUIContainer(gridHeader);
            blockUIHeader.Margin = new Thickness(marginthicness);
            TextBlock txtblockHeader = new TextBlock();
            txtblockHeader.Text = currentvaluestr; txtblockHeader.FontWeight = FontWeights.Bold;
            txtblockHeader.FontSize = FontsizeExtraLarge;
            txtblockHeader.VerticalAlignment = VerticalAlignment.Center;
            gridHeader.Children.Add(txtblockHeader);
            var tablcellHeader = new TableCell() { LineHeight = lineheight, ColumnSpan = 2 };
            tablcellHeader.Blocks.Add(blockUIHeader);
            Row.Cells.Add(tablcellHeader);



            // image EU
            bitimage = ImageHelpercs.LoadImage(CompanyData.Logo1_EU);
             image = new Image();
            image.Source = bitimage;
            image.Stretch = Stretch.Fill;
           image.Width = 100;
          image.Height = 100;
             block = new BlockUIContainer(image);
            Row.Cells.Add(new TableCell(block) { LineHeight = lineheight, RowSpan = 5 });


            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // leading  in transprot label...
            currentvaluestr = getlabel("LeadingInTransport", labelLanguageColumnName);


            //  Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontSize = FontsizeLarge, FontWeight = FontWeights.Bold, LineHeight = lineheight,ColumnSpan=2 });

             gridHeader = new Grid();
            //  grid.Background = Orangecolor;
             blockUIHeader = new BlockUIContainer(gridHeader);
            blockUIHeader.Margin = new Thickness(marginthicness);
             txtblockHeader = new TextBlock();
            txtblockHeader.Text = currentvaluestr; txtblockHeader.FontWeight = FontWeights.Bold;
            txtblockHeader.FontSize = FontsizeExtraLarge;
            txtblockHeader.VerticalAlignment = VerticalAlignment.Center;
            gridHeader.Children.Add(txtblockHeader);
             tablcellHeader = new TableCell() { LineHeight = lineheight, ColumnSpan = 2 };
            tablcellHeader.Blocks.Add(blockUIHeader);
            Row.Cells.Add(tablcellHeader);


            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


            // agent adrress
            // agent label 
            //currentLabelStr = getlabel("Branch", labelLanguageColumnName);
            string branchstr = string.Empty;
            if (branchLanguage == BranchLanguages.ar)
            {
                branchstr = branch.AddressAR;
            }
            else if (branchLanguage == BranchLanguages.ku)
            {
                branchstr = branch.AddressKu;
            }
            currentvaluestr = string.Format("{0}", branchstr);
            gridHeader = new Grid();
            //  grid.Background = Orangecolor;
            blockUIHeader = new BlockUIContainer(gridHeader);
            blockUIHeader.Margin = new Thickness(marginthicness);
            txtblockHeader = new TextBlock();
            txtblockHeader.Text = currentvaluestr; txtblockHeader.FontWeight = FontWeights.Bold;
            txtblockHeader.FontSize = FontsizeLarge;
            txtblockHeader.VerticalAlignment = VerticalAlignment.Center;
            gridHeader.Children.Add(txtblockHeader);
            tablcellHeader = new TableCell() { LineHeight = lineheight, ColumnSpan = 1 };
            tablcellHeader.Blocks.Add(blockUIHeader);
            Row.Cells.Add(tablcellHeader);
            //   Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) {  FontWeight = FontWeights.Bold, LineHeight = lineheight });


            //new cell 
            currentLabelStr = getlabel("Branch", labelLanguageColumnName);
            currentvaluestr = string.Format("{0} : {1}", currentLabelStr,branch.AgentName);
            //  Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) {   FontWeight = FontWeights.Bold, LineHeight = lineheight });
            gridHeader = new Grid();
            //  grid.Background = Orangecolor;
            blockUIHeader = new BlockUIContainer(gridHeader);
            blockUIHeader.Margin = new Thickness(marginthicness);
            txtblockHeader = new TextBlock();
            txtblockHeader.Text = currentvaluestr; txtblockHeader.FontWeight = FontWeights.Bold;
            txtblockHeader.FontSize = FontsizeLarge;
            txtblockHeader.VerticalAlignment = VerticalAlignment.Center;
            gridHeader.Children.Add(txtblockHeader);
            tablcellHeader = new TableCell() { LineHeight = lineheight, ColumnSpan = 1 };
            tablcellHeader.Blocks.Add(blockUIHeader);
            Row.Cells.Add(tablcellHeader);

            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];






            //tel1
            // currentvaluestr = string.Format("{0} : {1}", "Tel1", branch.PhoneNo1);
            // Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) { ColumnSpan = 2, LineHeight = lineheight });

            currentvaluestr = string.Format("{0} : {1}", "Tel1", branch.PhoneNo1);
             gridHeader = new Grid();
            gridHeader.Height = lineheight * 1.5;
            //  grid.Background = Orangecolor;
            blockUIHeader = new BlockUIContainer(gridHeader);
            blockUIHeader.Margin = new Thickness(marginthicness);
            txtblockHeader = new TextBlock();
            txtblockHeader.Text = currentvaluestr; txtblockHeader.FontWeight = FontWeights.Bold;
            txtblockHeader.VerticalAlignment = VerticalAlignment.Bottom;
            gridHeader.Children.Add(txtblockHeader);
            tablcellHeader = new TableCell() { LineHeight = lineheight, ColumnSpan = 2 };
            tablcellHeader.Blocks.Add(blockUIHeader);
            Row.Cells.Add(tablcellHeader);


            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


            //tel1
            //  currentvaluestr = string.Format("{0} : {1}", "Tel2", branch.PhoneNo2);
            //  Row.Cells.Add(new TableCell(new Paragraph(new Run(currentvaluestr))) {ColumnSpan=2, LineHeight = lineheight });

            currentvaluestr = string.Format("{0} : {1}", "Tel2", branch.PhoneNo2);
            gridHeader = new Grid(); gridHeader.Height = lineheight * 1.5;
            //  grid.Background = Orangecolor;
            blockUIHeader = new BlockUIContainer(gridHeader);
            blockUIHeader.Margin = new Thickness(marginthicness);
            txtblockHeader = new TextBlock();
            txtblockHeader.Text = currentvaluestr; txtblockHeader.FontWeight = FontWeights.Bold;
            txtblockHeader.VerticalAlignment = VerticalAlignment.Bottom;
            gridHeader.Children.Add(txtblockHeader);
             tablcellHeader = new TableCell() { LineHeight = lineheight, ColumnSpan = 2 };
            tablcellHeader.Blocks.Add(blockUIHeader);
            Row.Cells.Add(tablcellHeader);

            #endregion


            var containingCell1 = new TableCell();

            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            TableRow trow = containingTable.RowGroups[0].Rows[lastaddedRowIndex];
            trow.Cells.Add(containingCell1);



            // new table 
            etable = new Table();
            etable.FlowDirection = FlowDirection.RightToLeft;
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = 0;
            etable.Margin = new Thickness(0);

            // add table columns....
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1.5, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1.7, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1.7, GridUnitType.Star) });

            etable.RowGroups.Add(new TableRowGroup());


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count-1];

            currentLabelStr = getlabel("DeliverAndPayReceipt", labelLanguageColumnName);
            var grid = new Grid();
            //  grid.Background = Orangecolor;
            var blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            TextBlock txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            var tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, ColumnSpan = 6, Background = graycolor };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


            // code  title
            currentLabelStr = string.Format("{0} {1} {2}:", "Code:", Environment.NewLine, getlabel("Code", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight*2, BorderBrush = BorderColor,BorderThickness=new Thickness (1,1,0,1),
                Background = Orangecolor2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // code value
            currentLabelStr = ClientCode.Code;
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizedoubleExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Height = lineheight * 4;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight * 2, BorderBrush = BorderColor, BorderThickness = new Thickness(0, 1, 1, 1), Background = Orangecolor2,ColumnSpan=2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // truck title
            currentLabelStr = string.Format("{0} {1} {2}:", "Truck NO:", Environment.NewLine, getlabel("ShipmentNO", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight * 2, BorderBrush = BorderColor, Background = lightbluecolor,
                BorderThickness = new Thickness(1, 1, 0, 1)
            };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // truck value
            double shipmentno = ClientCode.Shipment_No.HasValue ? (double)ClientCode.Shipment_No : 0;
            currentLabelStr = shipmentno.ToString();
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Height = lineheight * 4; // this line to make text centered in textblock
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizedoubleExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight * 2, BorderBrush = BorderColor, Background = lightbluecolor,
                BorderThickness = new Thickness(0, 1, 1, 1)
            };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // date title and value

            string datestr = ClientCode.PostDate.HasValue ? ClientCode.PostDate.Value.ToShortDateString() : string.Empty;
            currentvaluestr = datestr;
            string dateportion = ClientCode.PostDate.HasValue? ClientCode.PostDate.Value.Date.ToString("dd/MM/yyyy"):"";
            string timeportion = ClientCode.PostDate.HasValue ? ClientCode.PostDate.Value.TimeOfDay.ToString():"";


            currentLabelStr = string.Format("{0}  :  {1}{2}{3}  :  {4}", "Date:", dateportion, Environment.NewLine,
                getlabel("Date", labelLanguageColumnName),timeportion);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight * 2, BorderBrush = BorderColor, Background = lightbluecolor,
                BorderThickness = new Thickness(1, 1, 0, 1)
            };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            //
            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // sender title
            currentLabelStr = string.Format("{0}:", getlabel("SenderInfo", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            //txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;

            txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);

            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight , BorderBrush = BorderColor ,ColumnSpan=2,BorderThickness=new Thickness(1)};
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // sender value
            currentLabelStr = ClientCode.SenderName;
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; 
            txtblock.FontWeight = FontWeights.Bold;
           // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);

            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight , BorderBrush = BorderColor, ColumnSpan = 2, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // receiver title
            currentLabelStr = string.Format("{0}:", getlabel("ReceiverInfo", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; 
            txtblock.FontWeight = FontWeights.Bold;
           // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            //receiver value 
            currentLabelStr = ClientCode.ReceiverName;
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; 
            txtblock.FontWeight = FontWeights.Bold;
           // txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);

            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // phone title 
            currentLabelStr = string.Format("{0}:", getlabel("Phone", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1),ColumnSpan=2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // phone
            currentLabelStr = ClientCode.Sender_Tel;
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);

            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // receiver tel title
            currentLabelStr = string.Format("{0}:", getlabel("Phone", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);

            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // receiver tel value
            currentLabelStr = ClientCode.Receiver_Tel;
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);

            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


            // please call our agent to get info 
            currentLabelStr = string.Format("{0}:", getlabel("PleaseContactAgent", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.Foreground = Brushes.Red ;
             txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 4 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // determine if have local post or not 
            if (!havelocalpost)
            {
                //  cell that spans 4 rows

                grid = new Grid();
                grid.Height = lineheight * 4;
                blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                var path = new Path();
                path.Stroke = Brushes.Red;
                path.StrokeThickness = 20;

                string src = @"M397,190.734L228.738,73.444c-6.599-5.632-11.996-3.147-11.996,5.528v49.068c0,8.672-7.099,15.77-15.77,15.77
				l-104.176,0.156H15.69v0.125C7.051,144.139,0.002,151.214,0,159.857l0.002,82.789c0,8.673,7.095,15.771,15.765,15.771
				l183.426-0.275h1.834c8.647,0.028,15.717,7.107,15.717,15.765v49.067c0,8.675,5.397,11.163,11.993,5.535l168.265-117.294
				C403.598,205.579,403.598,196.367,397,190.734z";
                path.Data = Geometry.Parse(src);
                path.HorizontalAlignment = HorizontalAlignment.Right;
                path.FlowDirection = FlowDirection.LeftToRight;
                path.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform myScaleTransform = new ScaleTransform();
              
                myScaleTransform.ScaleY = 1;
                myScaleTransform.ScaleX = 1;
                path.RenderTransform = myScaleTransform;
                Viewbox vbx = new Viewbox();
                vbx.Child = path;
                grid.Children.Add(vbx);

                tablcell = new TableCell() { RowSpan = 4 };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);

                //new cell 
                // post will be received in agent office title 
                currentLabelStr = string.Format("{0}:", getlabel("PostWillBeDeliveredInAgentOffice", labelLanguageColumnName));
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.Foreground = Brushes.Red;
                txtblock.FontSize = FontsizeLarge;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                grid.Height = lineheight * 4;
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, RowSpan = 4 };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);


            }
            else
            {
                // add cell with 1 row span and for the next  3 times adding rows check if dont have post to add a cell 
                //new cell 
                //address
                currentLabelStr = string.Format("{0}:", getlabel("Address", labelLanguageColumnName));
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                txtblock.HorizontalAlignment = HorizontalAlignment.Left;
                txtblock.Margin = new Thickness(2, 0, 0, 0);

                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);


                //new cell 
                //address value
                currentLabelStr = ClientCode.Street_Name_No;
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.VerticalAlignment = VerticalAlignment.Center;
                txtblock.HorizontalAlignment = HorizontalAlignment.Left;
                txtblock.Margin = new Thickness(2, 0, 0, 0);

                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);
            }

             
            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            //new cell 
            //Office title
            currentLabelStr = string.Format("{0}:", getlabel("Office", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor,BorderThickness=new Thickness (1),Background=Orangecolor2,ColumnSpan=2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            //agent office value
            currentLabelStr = string.Format("{0} - {1}", agent.AgentName, countryAgent.CountryName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), Background = Orangecolor2, ColumnSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            if (havelocalpost)
            {
                //new cell 
                // zipcode
                currentLabelStr = string.Format("{0}:", getlabel("PostalCode", labelLanguageColumnName));
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
                txtblock.Margin = new Thickness(2, 0, 0, 0);
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);


                //new cell 
                // zipcode value
                currentLabelStr = ClientCode.ZipCode;
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
                txtblock.Margin = new Thickness(2, 0, 0, 0);
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);



            }

            //new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            //new cell 
            //agent company title
            currentLabelStr = string.Format("{0}:", getlabel("Agent", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), Background = Orangecolor2, ColumnSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            //agent company value
            currentLabelStr = string.Format("{0}", agent.CompanyName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), Background = Orangecolor2, ColumnSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            if (havelocalpost)
            {
                //new cell 
                // city
                currentLabelStr = string.Format("{0}:", getlabel("City", labelLanguageColumnName));
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
                txtblock.Margin = new Thickness(2, 0, 0, 0);
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);


                //new cell 
                // city value
                currentLabelStr = ClientCode.CityPost;
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
                txtblock.Margin = new Thickness(2, 0, 0, 0);
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);

            }



            //new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
            //new cell 
            //agent phone title
            currentLabelStr = string.Format("{0}:", getlabel("Phone", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), Background = Orangecolor2, ColumnSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            //agent phone value
            currentLabelStr = string.Format("{0}", agent.PhonesDisplayString);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), Background = Orangecolor2, ColumnSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            if (havelocalpost)
            {
                //new cell 
                // country
                currentLabelStr = string.Format("{0}:", getlabel("Country", labelLanguageColumnName));
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
                txtblock.Margin = new Thickness(2, 0, 0, 0);
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);


                //new cell 
                // country value
                currentLabelStr = countryPost.CountryName;
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
                blockUI.Margin = new Thickness(marginthicness);
                txtblock = new TextBlock();
                txtblock.Text = currentLabelStr;
                txtblock.FontWeight = FontWeights.Bold;
                txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
                txtblock.Margin = new Thickness(2, 0, 0, 0);
                grid.Children.Add(txtblock);
                tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
                tablcell.Blocks.Add(blockUI);
                Row.Cells.Add(tablcell);

            }

            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // agent address title 
            currentLabelStr = string.Format("{0}:", getlabel("Address", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Center;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), Background = Orangecolor2, ColumnSpan = 4 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // transport costs title 
            currentLabelStr = string.Format("{0}:", getlabel("TransportCostDetails", labelLanguageColumnName));
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), Background = lightbluecolor, ColumnSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // agent address  value 
            currentLabelStr = agent.Address;
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;  
           
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), RowSpan = 4,ColumnSpan=4 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

           
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




            // transport cost title..
            currentLabelStr = getlabel("TransportCost", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new cell 
            //  transport cost value..
            DockPanel dp = new DockPanel();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;
            double Sub_Post_Cost_IQD = ClientCode.Sub_Post_Cost_IQD.HasValue ? (double)ClientCode.Sub_Post_Cost_IQD : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.####}", Sub_Post_Cost_IQD);
            txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);


            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR; txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // new cell 

            // packaging title 
            currentLabelStr = getlabel("PackigingCost", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            //  packiging cost value..
            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;


            double Packiging_cost_IQD = ClientCode.Packiging_cost_IQD.HasValue ? (double)ClientCode.Packiging_cost_IQD : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.####}", Packiging_cost_IQD); txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);


            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR; txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // new cell 

            // customeqmrk title 
            currentLabelStr = getlabel("AdminExportCost", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            //   qmrk value..
            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;


            double Custome_Cost_Qomrk = ClientCode.Custome_Cost_Qomrk.HasValue ? (double)ClientCode.Custome_Cost_Qomrk : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.####}", Custome_Cost_Qomrk); txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);


            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR; txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);



            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // new cell 

            // door to door title 
            currentLabelStr = getlabel("DoorToDoorCost", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // door to door value
            dp = new DockPanel();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;
            double POST_DoorToDoor_IQD = ClientCode.POST_DoorToDoor_IQD.HasValue ? (double)ClientCode.POST_DoorToDoor_IQD : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.####}", POST_DoorToDoor_IQD); txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);


            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR; txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // insurance value as checked radio button
            bool haveinsurance = ClientCode.Have_Insurance.HasValue ? (bool)ClientCode.Have_Insurance : false;
            // insuurance title ..
            //HaveInsurance ?
            currentLabelStr = getlabel("HaveInsurance", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);

            if (haveinsurance)
            {
              
                grid.Background = Brushes.Green; txtblock.Foreground = Brushes.White;
            }
             


            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1,1,0,1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // add empty string .. empty cell
            currentLabelStr = string.Empty;
            if (haveinsurance)
            {
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { Background = Brushes.Green, BorderBrush = BorderColor, BorderThickness = new Thickness(0, 1, 0, 1) })
                { LineHeight = lineheight });
            }
            else
            {
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { BorderBrush = BorderColor, BorderThickness = new Thickness(0, 1, 0, 1) })
                { LineHeight = lineheight });
            }

            // add another empty cell 
            if (haveinsurance)
            {
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) {Background=Brushes.Green, BorderBrush = BorderColor, BorderThickness = new Thickness(0, 1, 0, 1) })
                { LineHeight = lineheight });
            }
            else
            {
                Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { BorderBrush = BorderColor, BorderThickness = new Thickness(0, 1, 0, 1) })
                { LineHeight = lineheight });
            }
          

           
            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            blockUI = new BlockUIContainer(stackPanel);
            blockUI.Margin = new Thickness(marginthicness);
            RadioButton checkBoxinsuranceyes = new RadioButton();
            checkBoxinsuranceyes.IsChecked = true;
            checkBoxinsuranceyes.FontWeight = FontWeights.Bold;
            checkBoxinsuranceyes.Margin = new Thickness(marginthicness, 0, marginthicness, 0);
            checkBoxinsuranceyes.IsHitTestVisible = false;
            checkBoxinsuranceyes.Focusable = false;
            checkBoxinsuranceyes.FlowDirection = FlowDirection.LeftToRight;
            if (haveinsurance)
            {
                checkBoxinsuranceyes.Content = getlabel("Yes", labelLanguageColumnName);
                checkBoxinsuranceyes.Foreground = Brushes.White;
                //  checkBoxinsuranceyes.Background = Brushes.Green;
                stackPanel.Background = Brushes.Green;
            }
             
           
            stackPanel.Children.Add(checkBoxinsuranceyes);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(0, 1, 1, 1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // insurance value title 
            //
            currentLabelStr = getlabel("InsuranceCost", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            if (haveinsurance)
            {
                grid.Background = Brushes.Green; txtblock.Foreground = Brushes.White;
            }
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            // insurance value 
            dp = new DockPanel();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;
            double Insurance_Amount = ClientCode.Insurance_Amount.HasValue ? (double)ClientCode.Insurance_Amount : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.####}", Insurance_Amount); txtblock.FontWeight = FontWeights.Bold;
            if (haveinsurance)
            { txtblock.Foreground = Brushes.White; }
            dp.Children.Add(txtblock);

            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR; txtblock.FontWeight = FontWeights.Bold;

            if (haveinsurance)
            {
                dp.Background = Brushes.Green;
                txtblock.Foreground = Brushes.White;
            }


            dp.Children.Add(txtblock);

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


            // number of boxes 
            //
            currentLabelStr = getlabel("NOBoxes", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { Background=lightbluecolor, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            currentLabelStr = getlabel("WeightKG", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { Background = lightbluecolor, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            currentLabelStr = getlabel("GoodsDiscription", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { Background = lightbluecolor, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            currentLabelStr = getlabel("GoodsValue", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { Background = lightbluecolor, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);



            //TotalCost

            currentLabelStr = getlabel("TotalCost", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);




            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;


            double Total_Post_Cost_IQD = ClientCode.Total_Post_Cost_IQD.HasValue ? (double)ClientCode.Total_Post_Cost_IQD : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.####}", Total_Post_Cost_IQD); txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);


            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR; txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);






            // new row...
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            double boxno = ClientCode.Box_No.HasValue ? (double)ClientCode.Box_No : 0;
            currentLabelStr = string.Format("{0} {1}:", boxno.ToString(),"Box(s)");
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.FlowDirection = FlowDirection.LeftToRight;
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            grid.Height = lineheight * 3;
            tablcell = new TableCell() { RowSpan=3, LineHeight = lineheight , BorderBrush = BorderColor ,BorderThickness=new Thickness (1)};
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // new cell weight value display 
            double weight = ClientCode.Weight_Kg.HasValue ? (double)ClientCode.Weight_Kg : 0;
            double addweight = ClientCode.AdditionalWeight.HasValue ? (double)ClientCode.AdditionalWeight : 0;

            long L = ClientCode.Weight_L_cm.HasValue ? (long)ClientCode.Weight_L_cm : 0;
            long W = ClientCode.Weight_W_cm.HasValue ? (long)ClientCode.Weight_W_cm : 0;
            long H = ClientCode.Weight_H_cm.HasValue ? (long)ClientCode.Weight_H_cm : 0;

            string weightstr = string.Empty;
            weightstr = weight.ToString();
            if (addweight > 0)
            { weightstr += string.Format(" {0} {1} ","+",addweight.ToString()); }

            weightstr += " KG";

            if (L > 0 && W > 0 && H > 0)
            {
                string lwh = string.Format("{0}x{1}x{2}", L.ToString(), W.ToString(), H.ToString());
                weightstr += Environment.NewLine;
                weightstr += string.Format("{0} {1} {2} cm", "Volume Weight", Environment.NewLine, lwh);
            }
            currentLabelStr = weightstr;
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FlowDirection = FlowDirection.LeftToRight;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            grid.Height = lineheight * 3;
            tablcell = new TableCell() { RowSpan = 3, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new cell
            currentLabelStr = ClientCode.Goods_Description; ;
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            grid.Height = lineheight * 3;
            tablcell = new TableCell() { RowSpan = 3, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new cell
            currentLabelStr = ClientCode.Goods_Value.HasValue? ClientCode.Goods_Value.ToString():string.Empty ;
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            grid.Height = lineheight * 3;
            tablcell = new TableCell() { RowSpan = 3, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // new cell paid amount
            double TotalPaid_IQD = ClientCode.TotalPaid_IQD.HasValue ? (double)ClientCode.TotalPaid_IQD : 0;

            currentLabelStr = getlabel("PaidAmount", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            if (TotalPaid_IQD > 0)
            {
                grid.Background = GreenColor;
            }

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;


          
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.####}", TotalPaid_IQD); txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            if (TotalPaid_IQD > 0)
            {
                dp.Background = GreenColor;
            }
            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR; txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            //new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


            // remaining local

            double Remaining_IQD = ClientCode.Remaining_IQD.HasValue ? (double)ClientCode.Remaining_IQD : 0;


            currentLabelStr = getlabel("RemainingAmount", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            if (Remaining_IQD > 0)
            {
                grid.Background = Orangecolor3;
            }
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;


           // double Remaining_IQD = ClientCode.Remaining_IQD.HasValue ? (double)ClientCode.Remaining_IQD : 0;
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.####}", Remaining_IQD); txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            if (Remaining_IQD > 0)
            {
                dp.Background = Orangecolor3;
            }
            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            txtblock.Text = localcurrencyAR; txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new row
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


            // remaining outer

            double EuropaToPay = ClientCode.EuropaToPay.HasValue ? (double)ClientCode.EuropaToPay : 0;

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
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.VerticalAlignment = VerticalAlignment.Center; txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            txtblock.Margin = new Thickness(2, 0, 0, 0);
            grid.Children.Add(txtblock);
            if (EuropaToPay > 0)
            {
                grid.Background = Orangecolor3;
            }
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            dp = new DockPanel();
            // grid = new Grid();
            blockUI = new BlockUIContainer(dp);
            dp.LastChildFill = true;

            //{0:#,0.####}
            //{0:n0}
           
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center; txtblock.Margin = new Thickness(5, 0, 5, 0);
            txtblock.Text = string.Format("{0:#,0.####}", EuropaToPay); txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            if (EuropaToPay > 0)
            {
                dp.Background = Orangecolor3;
            }
            txtblock = new TextBlock(); txtblock.Margin = new Thickness(5, 0, 5, 0); txtblock.HorizontalAlignment = HorizontalAlignment.Right;
            //  txtblock.Text = "EUR";
            txtblock.Text = outerCurrencyENG;
            txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1) };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);




            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            //please read following
            currentLabelStr = getlabel("PleaseReadFollowingPointsBeforeSign", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.Foreground = Brushes.Red;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 6 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
            var paragraphs = form_agreemetPointstoparagraphs(labelLanguageColumnName);

            var _cell = new TableCell();
            for (int c = 0; c < paragraphs.Count; c++)
            {
                paragraphs[c].FontWeight = FontWeights.Bold;
                _cell.Blocks.Add(paragraphs[c]);
            }
            _cell.LineHeight = lineheight;
            _cell.RowSpan = 3;
            _cell.ColumnSpan = 5;
            _cell.Padding = new Thickness(0, 20, 0, 0);
            Row.Cells.Add(_cell);


            // new cell image agreement points 
            // image logo
            bitimage = ImageHelpercs.LoadImage(CompanyData.Agreement_Points_Image);
            image = new Image();
            image.Source = bitimage;
            image.Width = 150;
            image.Height = 200;
            block = new BlockUIContainer(image);
            Row.Cells.Add(new TableCell(block) { LineHeight = lineheight, RowSpan = 1 });

            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


            // sign space image with transpercy
            bitimage = ImageHelpercs.LoadImage(CompanyData.Logo1_EU);
            image = new Image();
            image.Source = bitimage;
            image.Opacity = 0.05;
            image.Stretch = Stretch.Fill;
            // image.Width = 100;
            //  image.Height = 50;
            block = new BlockUIContainer(image);
            Row.Cells.Add(new TableCell(block) { LineHeight = lineheight, RowSpan = 1 });


            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            // sign
            currentLabelStr = getlabel("Sign", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
           // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);



            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];

            currentLabelStr = getlabel("MainOffice", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            grid.Height = lineheight * 2;
            // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 2,RowSpan=2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);
            //
            //email
            currentLabelStr = string.Format("{0} {1}", "Email:", CompanyData.Email);
            grid = new Grid(); blockUI = new BlockUIContainer(grid);  
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 2, RowSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // new cell 
            // notes 
            currentLabelStr = getlabel("Notes", labelLanguageColumnName);
            grid = new Grid(); blockUI = new BlockUIContainer(grid); grid.Height = lineheight * 2;
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() {Background=lightpink, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1, RowSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);

            // notes value
            currentLabelStr = ClientCode.Note_Send;
            grid = new Grid(); blockUI = new BlockUIContainer(grid); grid.Height = lineheight*2;
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { Background = lightpink, LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 1, RowSpan = 2 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);


            // new row 
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];
            //web
            currentLabelStr = string.Format("{0} {1}", "Website:", CompanyData.Website);
            grid = new Grid(); blockUI = new BlockUIContainer(grid); grid.Height = lineheight;
            blockUI.Margin = new Thickness(marginthicness);
            txtblock = new TextBlock();
            txtblock.Text = currentLabelStr;
            txtblock.FontWeight = FontWeights.Bold;
            // txtblock.FontSize = FontsizeExtraLarge;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(txtblock);
            tablcell = new TableCell() { LineHeight = lineheight, BorderBrush = BorderColor, BorderThickness = new Thickness(1), ColumnSpan = 2, RowSpan = 1 };
            tablcell.Blocks.Add(blockUI);
            Row.Cells.Add(tablcell);





















            //
            containingCell1 = new TableCell();
            lastaddedRowIndex++;
            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            trow = containingTable.RowGroups[0].Rows[lastaddedRowIndex];
            trow.Cells.Add(containingCell1);











            //last thing
            flowdocument.Blocks.Add(new Section(containingTable));

        }

    }
}
