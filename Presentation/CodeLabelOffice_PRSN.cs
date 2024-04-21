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
using System.Data;

namespace StersTransport.Presentation
{
    public class CodeLabelOffice_PRSN
    {
        enum BranchLanguages { ar, ku }
        BranchLanguages branchLanguage;

        SolidColorBrush NotPaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD015B"));
        SolidColorBrush PaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#23C99C"));
        SolidColorBrush BorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#404040"));
        
        double lineheight = 15.0;
        double lineheight2 = 20.0;

        double lineheightsmall = 3.0;

        double marginthicness = 1.0;
        double paddingthicness = 1.0;
        double cellspacing = 0;
        double FontsizeLarge = 14;
        double Fontsizeextralarge = 16;
        double Fontsizesmall = 11;
        double Fontsizeextrasmall = 8;

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

        public Table generate_Header_With_ourOffices_Table()
        {


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
            containingTable.Margin = new Thickness(1);
            containingTable.CellSpacing = 0;
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });

            containingTable.RowGroups.Add(new TableRowGroup());


            containingTable.RowGroups[0].Rows.Add(new TableRow());

            TableRow ContainingRow = containingTable.RowGroups[0].Rows[0];

            var containingCell1 = new TableCell();

          //  string currentLabelStr = GlobalData.CompanyData.EnglishName + "  " + GlobalData.CompanyData.ArabicName; // need kurdish name to add to database
            string currentLabelStr = getlabel("StersCompany for General Transport", "Arabic")+"-"+ getlabel("StersCompany for General Transport", "Kurdish");
            ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))
            {

                FontSize=Fontsizesmall,
                Margin = new Thickness(0, marginthicness, 0, marginthicness),
                Padding = new Thickness(0, paddingthicness, 0, paddingthicness)
            })
            { ColumnSpan = 2, LineHeight = lineheight2 }
  );

            // new cell
            // 
            currentLabelStr = GlobalData.CompanyData.EnglishName + " Company";
            ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontSize = Fontsizesmall, LineHeight = lineheight2 });

            // second row....
            containingTable.RowGroups[0].Rows.Add(new TableRow());
             ContainingRow = containingTable.RowGroups[0].Rows[1];

            // Add The EU Image
            var bitimage = ImageHelpercs.LoadImage(CompanyData.Logo2_Sters);
            var image = new Image();
            image.Source = bitimage;
            image.Width = 100;
            image.Height = 100;
            var block = new BlockUIContainer(image);

            ContainingRow.Cells.Add(new TableCell(block) { LineHeight = lineheight2 });
           
            Table etable = new Table();
            etable.FontSize = 7;
            etable.Margin = new Thickness(0);
            //  etable.FlowDirection = FlowDirection.RightToLeft;
            etable.TextAlignment = TextAlignment.Center; etable.CellSpacing = cellspacing;
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.RowGroups.Add(new TableRowGroup());

            for (int c = 0; c < Branches.Count; c++)
            {
                etable.RowGroups[0].Rows.Add(new TableRow());
                TableRow Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];


                double clh = lineheightsmall;
                if (Branches[c].PhoneNo1.Length > 0 && Branches[c].PhoneNo2.Length > 0)
                {
                    clh *= 2;
                }
                currentLabelStr = Branches[c].AgentName;
                var grid = new Grid(); var blockUI = new BlockUIContainer(grid);
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
                grid = new Grid(); blockUI = new BlockUIContainer(grid);
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

            containingCell1 = new TableCell();
            containingCell1.Blocks.Add(new Section(etable) { BorderBrush = BorderColor });
            ContainingRow.Cells.Add(containingCell1);
            //

            // Add The EU Image
            bitimage = ImageHelpercs.LoadImage(CompanyData.Logo1_EU);
            image = new Image();
            image.Source = bitimage;
            image.Stretch = Stretch.Fill;
            image.Width = 100;
            image.Height = 100;
            block = new BlockUIContainer(image);

            ContainingRow.Cells.Add(new TableCell(block) { LineHeight = lineheight2 });


            return containingTable;
        }

        public Table generate_data_table(int nop)
        {
            // Main (Containing Table..)
            var containingTable = new Table();
            containingTable.CellSpacing = 0;
            containingTable.Margin = new Thickness(1);
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(4, GridUnitType.Star) });
           // containingTable.Columns.Add(new TableColumn() { Width = new GridLength(1.5, GridUnitType.Star) });

            containingTable.RowGroups.Add(new TableRowGroup());
            containingTable.RowGroups[0].Rows.Add(new TableRow());
            TableRow ContainingRow = containingTable.RowGroups[0].Rows[0];

            string currentLabelStr = string.Empty;

            Table etable = new Table();
            etable.TextAlignment = TextAlignment.Left;
            etable.Margin = new Thickness(0);

            etable.Columns.Add(new TableColumn() { Width = new GridLength(.75, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) });
            etable.Columns.Add(new TableColumn() { Width = new GridLength(.75, GridUnitType.Star) });

            etable.RowGroups.Add(new TableRowGroup());

            etable.RowGroups[0].Rows.Add(new TableRow());
            TableRow Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added

            currentLabelStr = "Sender :";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontWeight=FontWeights.Bold,FontSize=Fontsizesmall, LineHeight = lineheight2, BorderThickness=new Thickness (0),BorderBrush= BorderColor });

            currentLabelStr = ClientCode.SenderName;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { LineHeight = lineheight2, 
                FontWeight = FontWeights.Bold,
                FontSize=Fontsizeextralarge,
                BorderThickness = new Thickness(0),
                BorderBrush = BorderColor,
                ColumnSpan = 2});

            currentLabelStr = "Agent City";

            var grid = new Grid();
            //  grid.Background = Orangecolor;
            var blockUI = new BlockUIContainer(grid);
            grid.Height = lineheight2;
            blockUI.Margin = new Thickness(marginthicness);
            TextBlock txtblock = new TextBlock();
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = Fontsizeextrasmall;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock . HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(txtblock);
            var tablcellHeader1 = new TableCell() { LineHeight = lineheight2 };
            tablcellHeader1.Blocks.Add(blockUI);
            Row.Cells.Add(tablcellHeader1);
            
            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Receiver :";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontSize = Fontsizesmall, FontWeight = FontWeights.Bold, LineHeight = lineheight2, BorderThickness = new Thickness(0), BorderBrush = BorderColor });

            currentLabelStr = ClientCode.ReceiverName;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontWeight = FontWeights.Bold,
                BorderThickness = new Thickness(0),
                BorderBrush = BorderColor,
                FontSize = Fontsizeextralarge,
                ColumnSpan = 2, LineHeight = lineheight2
            });

            // Agent Name Value
            var grid1 = new Grid();
           
            TextBlock tb = new TextBlock();
            string brname = string.Empty;
            if (branch != null)
            {
                brname = branch.AgentName;
            }
            tb.Text = brname;
             
            tb.FontSize = FontsizeLarge;
                       
          //  grid1.Width= lineheight * 4;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.FontWeight = FontWeights.Bold;

            tb.RenderTransformOrigin = new Point(0.5,0.5);
            RotateTransform rotateTransform1 = new RotateTransform(270);
            tb.RenderTransform = rotateTransform1;
            grid1.Children.Add(tb);
            BlockUIContainer blockUIcntr = new BlockUIContainer(grid1);
            //  blockUIcntr.TextEffects = effects;

            tb.Margin = new Thickness(0, 30, 0, 0);
            var tablcell_ = new TableCell()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = BorderColor,
                RowSpan = 4
            };
            tablcell_.Blocks.Add(blockUIcntr);
            Row.Cells.Add(tablcell_);


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Phone NO:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontSize = Fontsizesmall, FontWeight = FontWeights.Bold,LineHeight = lineheight2, BorderThickness = new Thickness(0), BorderBrush = BorderColor });

            currentLabelStr = ClientCode.Receiver_Tel;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontWeight = FontWeights.Bold,
                BorderThickness = new Thickness(0),
                BorderBrush = BorderColor,
                FontSize = Fontsizeextralarge,
                ColumnSpan = 2, LineHeight = lineheight2
            });

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Items :";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontSize = Fontsizesmall, FontWeight = FontWeights.Bold, LineHeight = lineheight2, BorderThickness = new Thickness(0), BorderBrush = BorderColor });

            currentLabelStr = ClientCode.Goods_Description;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontWeight = FontWeights.Bold,
                BorderThickness = new Thickness(0),
                BorderBrush = BorderColor,
                FontSize = Fontsizeextralarge,
                ColumnSpan = 2, LineHeight = lineheight2
            });


            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added


            currentLabelStr = "Weight KG:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontSize = Fontsizesmall, FontWeight = FontWeights.Bold,LineHeight = lineheight2, BorderThickness = new Thickness(0), BorderBrush = BorderColor });

            double wkg = ClientCode.Weight_Total.HasValue ? (double)ClientCode.Weight_Total : 0;
            // add a stackpanel 
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            TextBlock _tb1 = new TextBlock();
            _tb1.Text = wkg.ToString();
            _tb1.FontSize = 12; _tb1.FontWeight = FontWeights.Bold;
            sp.Children.Add(_tb1);

            TextBlock _tb2 = new TextBlock();
            _tb2.Text = "UnitNO."; _tb2.FontSize = Fontsizesmall;
            _tb2.Margin = new Thickness(14, 0, 0, 0);
            sp.Children.Add(_tb2);

            BlockUIContainer buc = new BlockUIContainer(sp);
            TableCell tc1 = new TableCell();
            tc1.LineHeight = lineheight2;
            tc1.Blocks.Add(buc);
            Row.Cells.Add(tc1);

            double bxn = ClientCode.Box_No.HasValue ? (double)ClientCode.Box_No : 0;
            double pln = ClientCode.Pallet_No.HasValue ? (double)ClientCode.Pallet_No : 0;
            double boxno = pln + bxn;
            sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
             _tb1 = new TextBlock();
            _tb1.Text = nop.ToString();
            _tb1.FontSize = 12; _tb1.FontWeight = FontWeights.Bold;
            sp.Children.Add(_tb1);

             _tb2 = new TextBlock();
            _tb2.Text = "of Units";
            _tb2.FontSize = Fontsizesmall;
            _tb2.Margin = new Thickness(8, 0, 0, 0);
            sp.Children.Add(_tb2);

            TextBlock _tb3 = new TextBlock();
            _tb3.Text =  boxno.ToString();
            _tb3.FontSize = 12; _tb3.FontWeight = FontWeights.Bold;
            _tb3.Margin = new Thickness(8, 0, 0, 0);
            sp.Children.Add(_tb3);

            buc = new BlockUIContainer(sp);
             tc1 = new TableCell();
            tc1.LineHeight = lineheight2;
            tc1.Blocks.Add(buc);
            Row.Cells.Add(tc1);

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Code:";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontSize = Fontsizesmall, FontWeight = FontWeights.Bold, LineHeight = lineheight2, BorderThickness = new Thickness(0), BorderBrush = BorderColor });

            currentLabelStr = ClientCode.Code;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontWeight = FontWeights.Bold,
                BorderThickness = new Thickness(0),
                BorderBrush = BorderColor,
                FontSize = Fontsizeextralarge,
                LineHeight = lineheight2
            });

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
                    currentLabelStr = "Pay In EU";
                }
                else
                {
                    currentLabelStr = "Pay In " + countryAgent.CountryName;
                }
                txtblockHeader.Text = currentLabelStr; txtblockHeader.FontWeight = FontWeights.Bold;
                var tablcellHeader = new TableCell() { LineHeight = lineheight2 , FontWeight = FontWeights.Bold, Background = NotPaidColor };
                tablcellHeader.Blocks.Add(blockUIHeader);
                Row.Cells.Add(tablcellHeader); 
            }

            else
            {
                currentLabelStr = "All Paid";
                txtblockHeader.Text = currentLabelStr; txtblockHeader.FontWeight = FontWeights.Bold;
                var tablcellHeader = new TableCell() { LineHeight = lineheight2, FontWeight = FontWeights.Bold, Background = PaidColor };
                tablcellHeader.Blocks.Add(blockUIHeader);
                Row.Cells.Add(tablcellHeader);  
            }

            // Have Insurance Issue
            bool haveinsurance = ClientCode.Have_Insurance.HasValue ? (bool)ClientCode.Have_Insurance : false;

            StackPanel stackPnl = new StackPanel();
            stackPnl.Orientation = Orientation.Vertical;


            tb = new TextBlock(); tb.FontSize = 12;
            
            tb.FontWeight = FontWeights.Bold;

            var tb2 = new TextBlock(); tb2.FontSize = 12;
            tb2.FontWeight = FontWeights.Bold;
           

            if (haveinsurance)
            {
                tb.Text = "Insurance";
                tb2.Text = "Yes";
            }
            else
            {
                tb.Text = string.Empty;
                tb2.Text = string.Empty;

            }
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb2.HorizontalAlignment = HorizontalAlignment.Center;

           
            stackPnl.Children.Add(tb);
            stackPnl.Children.Add(tb2);

            stackPnl.RenderTransformOrigin = new Point(.5,.5);
            rotateTransform1 = new RotateTransform(270);
            stackPnl.RenderTransform = rotateTransform1;

            stackPnl.Margin = new Thickness(0, 15, 0, 0);
          
            blockUIcntr = new BlockUIContainer(stackPnl);
            tablcell_ = new TableCell()
            {
                RowSpan=3,
                BorderThickness = new Thickness(1),
                BorderBrush = BorderColor,
            };
            // dont totch it.
            tablcell_.Blocks.Add(blockUIcntr);
            Row.Cells.Add(tablcell_);
            if (haveinsurance) { tablcell_.Background = PaidColor; }
            
            string citystr = string.Empty;
            if (agent != null)
            {
                citystr = agent.AgentName;
            }

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "City :";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontSize = Fontsizesmall, LineHeight = lineheight2, FontWeight=FontWeights.Bold });

            currentLabelStr = citystr;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) {
                FontWeight = FontWeights.Bold,
                  ColumnSpan = 2, LineHeight = lineheight2,

                BorderThickness = new Thickness(0),
                BorderBrush = BorderColor
            });
            
            string cntrystr = string.Empty;
            if (countryAgent != null)
            {
                cntrystr = countryAgent.CountryName;
            }

            etable.RowGroups[0].Rows.Add(new TableRow());
            Row = etable.RowGroups[0].Rows[etable.RowGroups[0].Rows.Count - 1];// last added
            currentLabelStr = "Country :";
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)))
            { FontSize = Fontsizesmall, FontWeight = FontWeights.Bold, LineHeight = lineheight2, BorderThickness = new Thickness(0), BorderBrush = BorderColor });

            currentLabelStr = cntrystr;
            Row.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { FontWeight = FontWeights.Bold,
                BorderThickness = new Thickness(0),
                BorderBrush = BorderColor,
                FontSize = Fontsizeextralarge,
                ColumnSpan = 2,  LineHeight = lineheight2
            });

            var containingCell1 = new TableCell();
            containingCell1.Blocks.Add(new Section(etable) { BorderThickness = new Thickness(1), BorderBrush = BorderColor });
            ContainingRow.Cells.Add(containingCell1);

            return containingTable;
        }

        public Table generate_second_table()
        {
            var containingTable = new Table();
            containingTable.Margin = new Thickness(1);
            
            containingTable.CellSpacing = 0;
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(.75, GridUnitType.Star) });
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(2, GridUnitType.Star) });
            containingTable.Columns.Add(new TableColumn() { Width = new GridLength(.75, GridUnitType.Star) });

            containingTable.RowGroups.Add(new TableRowGroup());
            containingTable.RowGroups[0].Rows.Add(new TableRow());
            TableRow ContainingRow = containingTable.RowGroups[0].Rows[0];

            double Shipment_No = ClientCode.Shipment_No.HasValue ? (double)ClientCode.Shipment_No : 0;
            string currentLabelStr = string.Format("{0}", "Truck NO.");

            /*
            var gridHeader = new Grid();
            //  grid.Background = Orangecolor;
            var blockUIHeader = new BlockUIContainer(gridHeader);
            blockUIHeader.Margin = new Thickness(4);
            TextBlock txtblockHeader = new TextBlock();
            txtblockHeader.Text = currentLabelStr; txtblockHeader.FontWeight = FontWeights.Bold;
            txtblockHeader.VerticalAlignment = VerticalAlignment.Center;
            
            gridHeader.Children.Add(txtblockHeader);
            gridHeader.Height = 75;
            var tablcellHeader = new TableCell() { LineHeight = lineheight2 };
            tablcellHeader.Blocks.Add(blockUIHeader);
            ContainingRow.Cells.Add(tablcellHeader);
            */
            StackPanel dp = new StackPanel();
            
            dp.Height = 60;
          //  dp.Margin = new Thickness(0, 20, 0, 0);
            var blockUIHeader = new BlockUIContainer(dp);
            blockUIHeader.Margin = new Thickness(4);
            dp.Orientation = Orientation.Vertical;
            TextBlock txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center;
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            txtblock.FontSize = Fontsizesmall;
            dp.Children.Add(txtblock);

            currentLabelStr = string.Format("{0}",Shipment_No.ToString());
            txtblock = new TextBlock(); txtblock.HorizontalAlignment = HorizontalAlignment.Center;
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            var tablcellHeader = new TableCell() { LineHeight = lineheight2 };
            tablcellHeader.Blocks.Add(blockUIHeader);
            ContainingRow.Cells.Add(tablcellHeader);

            // ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr))) { LineHeight = lineheight2, FontWeight = FontWeights.Bold });


            // add flag 
            var bitimage = ImageHelpercs.LoadImage(countryAgent.ImgForBoxLabel);
            if (bitimage != null)
            {
                Image image = new Image();
                image.Source = bitimage;// handle nulls
                image.Width = 250;
                image.Height = 60;
                image.Stretch = Stretch.Fill;
                var block = new BlockUIContainer(image);
                ContainingRow.Cells.Add(new TableCell(block) { LineHeight = lineheight2, ColumnSpan=1 });
            } // no image
            else
            {
                ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))) { LineHeight = lineheight2, ColumnSpan = 1 });
            }


            // 
            string dateportion = DateTime.Now.Date.ToString("dd/MM/yyyy");
            string timeportion = DateTime.Now.ToString("t");
            currentLabelStr = string.Format("{0}", dateportion);
          

            dp = new StackPanel();
            dp.Height = 60;
            // dp.Margin = new Thickness(0, 20, 0, 0);
            blockUIHeader = new BlockUIContainer(dp);
            blockUIHeader.Margin = new Thickness(4);
            dp.Orientation = Orientation.Vertical;
            
            txtblock = new TextBlock(); 
            txtblock.HorizontalAlignment = HorizontalAlignment.Center;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.Text = currentLabelStr; txtblock.FontWeight = FontWeights.Bold;
            dp.Children.Add(txtblock);

            currentLabelStr = string.Format("{0}", timeportion);
            txtblock = new TextBlock(); 
            txtblock.HorizontalAlignment = HorizontalAlignment.Center;
            txtblock.VerticalAlignment = VerticalAlignment.Center;
            txtblock.FontWeight = FontWeights.Bold;
            txtblock.Text = currentLabelStr; 
            dp.Children.Add(txtblock);

            tablcellHeader = new TableCell() { LineHeight = lineheight2, FontSize = Fontsizesmall, FontWeight = FontWeights.Bold };
            tablcellHeader.Blocks.Add(blockUIHeader);
            ContainingRow.Cells.Add(tablcellHeader);

            // second row for the barcode
            containingTable.RowGroups[0].Rows.Add(new TableRow());
            ContainingRow = containingTable.RowGroups[0].Rows[1];

            ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)))
            { LineHeight = lineheight2 });

            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_128;


            writer.Options = new ZXing.Common.EncodingOptions()
            {
               Height = 40,
              //Width = 250
              PureBarcode=true
            };

            var result = writer.Write(ClientCode.Code);
            var barcodeBitmap = new System.Drawing.Bitmap(result);

            var grd = new Grid();

            Image image2 = new Image();
            image2.Source = ImageHelpercs.ConvertBitmap(barcodeBitmap);
            image2.Stretch = Stretch.Fill;
         //   image2.Width = 300;
             image2.Height = 40;
            image2.HorizontalAlignment = HorizontalAlignment.Center;
            grd.Children.Add(image2);
            var block2 = new BlockUIContainer(grd) { Margin = new Thickness(0) };
            ContainingRow.Cells.Add(new TableCell(block2) { LineHeight = lineheight2, ColumnSpan=1 });

            ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)))
            { LineHeight = lineheight2 });



            // new row for barcode text 
            // third row for barcode label
            containingTable.RowGroups[0].Rows.Add(new TableRow());
            ContainingRow = containingTable.RowGroups[0].Rows[2];



            ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)))
            { LineHeight = lineheight2 });


            
            currentLabelStr = ClientCode.Code;
            ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(currentLabelStr)) { TextAlignment = TextAlignment.Center })
            { FontWeight = FontWeights.Bold, LineHeight = lineheight2 });

            ContainingRow.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)))
            { LineHeight = lineheight2 });





            return containingTable; 
        }

        public void generateDocument(FlowDocument flowdocument, string code)
        {
            ClientCode = clientCodeDA.GetClientCode(code);
            if (ClientCode == null) 
                return;

            // get branches data 

            // List<Branch> allbranches = branchDa.GetBranches();
            List<Agent> allbranches = agentDa.GetAgents();
            // Branches = allbranches.Where(x => x.IsLocalCompanyBranch == true).ToList();
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
                 
                Table datatable = generate_data_table(c + 1);
                flowdocument.Blocks.Add(datatable);
                Table secondtable = generate_second_table();
                flowdocument.Blocks.Add(secondtable);
            }
        }
    }
}
