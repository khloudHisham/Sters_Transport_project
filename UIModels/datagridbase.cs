using System;
using System.Windows.Controls;

namespace StersTransport.UIModels
{
    class datagridbase :DataGrid
    {
        protected override void OnCurrentCellChanged(EventArgs e)
        {

            BeginEdit();

            base.OnCurrentCellChanged(e);
        }

    }
}
