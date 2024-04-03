using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace StersTransport.UIModels
{
    public class TabItemEXvm
    {
        public ObservableCollection<TabItemEX> tabs { get; set; }

        public TabItemEXvm()
        {
            tabs = new ObservableCollection<UIModels.TabItemEX>();
        }

        public void addtab(string header_, UserControl content_)
        { tabs.Add(new UIModels.TabItemEX { header = header_, content = content_ }); }


        public void closetab(UserControl content_)
        {
            tabs.Remove(tabs.Where(x => x.content == content_).FirstOrDefault());
        }
    }
}
