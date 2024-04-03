using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace StersTransport.Helpers
{
    public class PrintHelper
    {
        public void showpreview(double w, double h,FlowDocument flowdocument)
        {

            flowdocument.PageHeight = h;
            flowdocument.PageWidth = w;
            flowdocument.PagePadding = new Thickness(10);
            
            flowdocument.ColumnGap = 0;
            // avoid multi column
        
            flowdocument.ColumnWidth = (flowdocument.PageWidth -
                                   flowdocument.ColumnGap -
                                   flowdocument.PagePadding.Left -
                                   flowdocument.PagePadding.Right);
       
            var paginator = ((IDocumentPaginatorSource)flowdocument).DocumentPaginator;

            string tempFileName = System.IO.Path.GetTempFileName();

            //GetTempFileName creates a file, the XpsDocument throws an exception if the file already
            //exists, so delete it. Possible race condition if someone else calls GetTempFileName
            File.Delete(tempFileName);
            using (XpsDocument xpsDocument = new XpsDocument(tempFileName, FileAccess.ReadWrite))
            {
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                writer.Write(paginator);

                UI.previewWindow previewWindow = new UI.previewWindow
                {
                    Document = xpsDocument.GetFixedDocumentSequence()
                };
                previewWindow.ShowDialog();
            }
        }
    }
}
