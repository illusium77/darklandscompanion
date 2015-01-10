using DarklandsUiCommon.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DarklandsUiCommon.ViewModels
{
    public class QuestListViewModel : ModelBase
    {
        private ICollectionView m_quests;
        public ICollectionView Quests
        {
            get
            {
                return m_quests;
            }
            set
            {
                m_quests = value;
                NotifyPropertyChanged();
            }
        }

        //http://msdn.microsoft.com/en-us/library/ff407126%28v=vs.110%29.aspx

        public QuestListViewModel(IEnumerable<QuestModel> quests)
        {
            Quests = CollectionViewSource.GetDefaultView(quests.OrderBy(q => q.Type));
            if (Quests != null && Quests.CanGroup == true)
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
    }
}
