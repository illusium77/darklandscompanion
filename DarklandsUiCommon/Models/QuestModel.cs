using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsUiCommon.Models
{
    public enum QuestType
    {
        KillRaubritter,
        RetrieveItem
    }

    public class QuestModel : ModelBase
    {
        private Event m_event;
        private static string INVALID = "[Unknown]";

        public QuestType Type { get; private set; }

        public bool IsCompleted { get; private set; }
        public string PickUpDate { get; private set; }
        public string DeliverByDate { get; private set; }
        public string QuestGiverName { get; private set; }
        public string Destination { get; private set; }
        public string Source { get; private set; }
        public string QuestItem { get; private set; }
        public string Tooltip { get; private set; }
        public string Description { get; private set; }

        public QuestModel(Event quest, IEnumerable<Location> locations, IEnumerable<ItemDefinition> items)
        {
            if (!quest.IsQuest)
            {
                throw new ArgumentException("Quest event required");
            }

            m_event = quest;

            InitializeQuest(locations, items);
        }

        private void InitializeQuest(IEnumerable<Location> locations, IEnumerable<ItemDefinition> items)
        {
            var dest = locations.FirstOrDefault(l => l.Id == m_event.DestinationLocationId);

            Destination = GetDestination(locations, dest);

            if (m_event.ItemId == 0)
            {
                Type = QuestType.KillRaubritter;

                IsCompleted = dest != null ? dest.Type != LocationType.CastleRaubritter : false;
            }
            else
            {
                Type = QuestType.RetrieveItem;

                var item = items.FirstOrDefault(i => i.Id == m_event.ItemId);
                QuestItem = item != null ? item.Name : INVALID;

                IsCompleted = !m_event.ExpireDate.IsInfinite; // when item is found, delived time chages to infinite
            }

            PickUpDate = m_event.CreateDate.ToString();

            DeliverByDate = m_event.ExpireDate.IsInfinite == false
                ? m_event.ExpireDate.ToString() : string.Empty;

            QuestGiverName = GetQuestGiver(m_event.QuestGiver);

            var source = locations.FirstOrDefault(l => l.Id == m_event.SourceLocationId);
            Source = source != null ? source.Name : INVALID;

            Description = GetDescription();
            Tooltip = GetTooltip();
        }

        private string GetDestination(IEnumerable<Location> locations, Location dest)
        {

            if (dest != null)
            {
                if (dest.Type == LocationType.City)
                {
                    return dest.Name;
                }
                else
                {
                    return dest.Name + " (" + dest.GetDirections(locations) + ")";
                }
            }

            return INVALID;
        }

        private string GetQuestGiver(QuestGiver questGiver)
        {
            switch (questGiver)
            {
                case QuestGiver.NA:
                    return INVALID;
                case QuestGiver.ForeignTrader:
                    return "Foreign Trader";
                case QuestGiver.Hanse:
                    return "Hanseatic League";
                default:
                    return questGiver.ToString();
            }
        }

        private string GetDescription()
        {
            switch (Type)
            {
                case QuestType.RetrieveItem:
                    if (IsCompleted)
                    {
                        return "Return " + QuestItem + " to " + QuestGiverName + "." ;
                    }
                    else
                    {
                        return "Find " + QuestItem + " for " + QuestGiverName + " in " + Source + ".";
                    }
                case QuestType.KillRaubritter:
                    if (IsCompleted)
                    {
                        return "Return to " + QuestGiverName + ".";
                    }
                    else
                    {
                        return "Kill raubritter for " + QuestGiverName + " in " + Source + ".";
                    }
            }

            return INVALID;
        }

        private string GetTooltip()
        {
            switch (Type)
            {
                case QuestType.RetrieveItem:
                    if (IsCompleted)
                    {
                        return "Return " + QuestItem + " to " + QuestGiverName + " in " + Destination + " and claim reward by " + DeliverByDate + ".";
                    }
                    else
                    {
                        return "Get " + QuestItem + " from " + Destination + " for " + QuestGiverName + " in " + Source + ".";
                    }
                case QuestType.KillRaubritter:
                    if (IsCompleted)
                    {
                        return "Return to " + QuestGiverName + " in " + Destination + " and claim reward by " + DeliverByDate + ".";
                    }
                    else
                    {
                        return "Kill raubritter at " + Destination + " for " + QuestGiverName + " in " + Source + " by " + DeliverByDate + ".";
                    }
            }

            return INVALID;
        }

        public static IEnumerable<QuestModel> FromEvents(IEnumerable<Event> events, IEnumerable<Location> locations, IEnumerable<ItemDefinition> items)
        {
            var quests = new List<QuestModel>(
                from e in events
                where e.IsQuest
                select new QuestModel(e, locations, items));

            return quests;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case QuestType.RetrieveItem:
                    return "Find item: " + Tooltip;
                case QuestType.KillRaubritter:
                    return "Raubritter: " + Tooltip;
            }

            return base.ToString();
        }
    }
}
