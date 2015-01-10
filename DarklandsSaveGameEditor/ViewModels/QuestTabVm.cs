using System.Linq;
using DarklandsBusinessObjects.Save;
using DarklandsServices.Services;
using DarklandsUiCommon.Models;
using DarklandsUiCommon.ViewModels;

namespace DarklandsSaveGameEditor.ViewModels
{
    public class QuestTabVm : ModelBase
    {
        private string _header;
        private QuestListViewModel _questVm;

        public QuestTabVm(SaveEvents saveEvents)
        {
            var quests = QuestModel.FromEvents(
                saveEvents.Events, saveEvents.Locations, StaticDataService.ItemDefinitions).ToList();

            QuestVm = new QuestListViewModel(quests);

            Header = "Quests (" + quests.Count() + ")";
        }

        public QuestListViewModel QuestVm
        {
            get { return _questVm; }
            set
            {
                _questVm = value;
                NotifyPropertyChanged();
            }
        }

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                NotifyPropertyChanged();
            }
        }
    }
}