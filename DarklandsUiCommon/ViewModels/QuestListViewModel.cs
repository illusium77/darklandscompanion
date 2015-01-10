using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
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
                Quests.GroupDescriptions.Add(new PropertyGroupDescription("Type"));
                Quests.SortDescriptions.Add(new SortDescription("DeliverByDate", ListSortDirection.Descending));
            }

            //var fetchQuests = from q in quests where q.Type == QuestType.FetchItem select q;
            //FetchItemQuests = new CollectionViewSource
            //{
            //    Source = fetchQuests,
            //}.View;
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
    }
}