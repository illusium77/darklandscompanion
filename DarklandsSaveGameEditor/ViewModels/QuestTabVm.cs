using DarklandsBusinessObjects.Save;
using DarklandsServices.Services;
using DarklandsUiCommon.Models;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsSaveGameEditor.ViewModels
{
    public class QuestTabVm : ModelBase
    {
        private QuestListViewModel m_questVm;
        public QuestListViewModel QuestVm
        {
            get { return m_questVm; }
            set
            {
                m_questVm = value;
                NotifyPropertyChanged();
            }
        }


        private string m_header;
        public string Header
        {
            get
            {
                return m_header;
            }
            set
            {
                m_header = value;
                NotifyPropertyChanged();
            }
        }

        public QuestTabVm(SaveEvents saveEvents)
        {
            var quests = QuestModel.FromEvents(
                saveEvents.Events, saveEvents.Locations, StaticDataService.ItemDefinitions);

            QuestVm = new QuestListViewModel(quests);

            Header = "Quests (" + quests.Count() + ")";
        }
    }
}
