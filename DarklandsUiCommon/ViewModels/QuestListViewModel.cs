using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DarklandsBusinessObjects.Utils;
using DarklandsUiCommon.Models;

namespace DarklandsUiCommon.ViewModels
{
    public class QuestListViewModel : ModelBase
    {
        private ICollectionView _quests;
        //http://msdn.microsoft.com/en-us/library/ff407126%28v=vs.110%29.aspx

        public QuestListViewModel(IEnumerable<QuestModel> quests)
        {
            Quests = CollectionViewSource.GetDefaultView(quests.OrderBy(q => q.Type));
            if (Quests != null && Quests.CanGroup)
            {
                Quests.GroupDescriptions.Add(new QuestTypeGroup());
                Quests.SortDescriptions.Add(new SortDescription("DeliverByDate", ListSortDirection.Descending));
            }
        }

        public ICollectionView Quests
        {
            get { return _quests; }
            set
            {
                _quests = value;
                NotifyPropertyChanged();
            }
        }

        private class QuestTypeGroup : PropertyGroupDescription
        {
            public override object GroupNameFromItem(object item, int level, CultureInfo culture)
            {
                var quest = item as QuestModel;

                return quest != null ? quest.Type.Description() : "INVALID GROUP";
            }
        }
    }
}