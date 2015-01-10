using System;
using System.Collections.Generic;
using System.Linq;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.ViewModels;

namespace DarklandsUiCommon.Models
{
    public enum QuestType
    {
        KillRaubritter,
        RetrieveItem
    }

    public class QuestModel : ModelBase
    {
        private const string Invalid = "[Unknown]";
        private readonly Event _event;

        private QuestModel(Event quest, IEnumerable<Location> locations, IEnumerable<ItemDefinition> items)
        {
            if (!quest.IsQuest)
            {
                throw new ArgumentException("Quest event required");
            }

            _event = quest;

            InitializeQuest(locations, items);
        }

        public QuestType Type { get; private set; }
        public bool IsCompleted { get; private set; }
        public string PickUpDate { get; private set; }
        public string DeliverByDate { get; private set; }
        public string TimeLeftComplete { get; private set; }
        public string QuestGiverName { get; private set; }
        public string Destination { get; private set; }
        public string Source { get; private set; }
        public string QuestItem { get; private set; }
        public string Tooltip { get; private set; }
        public string Description { get; private set; }

        private void InitializeQuest(IEnumerable<Location> locations, IEnumerable<ItemDefinition> items)
        {
            var locs = locations.ToList();
            var dest = locs.FirstOrDefault(l => l.Id == _event.DestinationLocationId);

            Destination = GetDestination(locs, dest);

            if (_event.ItemId == 0)
            {
                Type = QuestType.KillRaubritter;

                IsCompleted = dest != null && dest.Type != LocationType.CastleRaubritter;
            }
            else
            {
                Type = QuestType.RetrieveItem;

                var item = items.FirstOrDefault(i => i.Id == _event.ItemId);
                QuestItem = item != null ? item.Name : Invalid;

                IsCompleted = !_event.ExpireDate.IsInfinite; // when item is found, delived time chages to infinite
            }

            PickUpDate = _event.CreateDate.ToString();
            DeliverByDate = _event.ExpireDate.IsInfinite == false
                ? _event.ExpireDate.ToString()
                : string.Empty;
            TimeLeftComplete = GetTimeDelta();

            QuestGiverName = GetQuestGiver();

            var source = locs.FirstOrDefault(l => l.Id == _event.SourceLocationId);
            Source = source != null ? source.Name : Invalid;

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
                return dest.Name + " (" + dest.GetDirections(locations) + ")";
            }

            return Invalid;
        }

        private string GetTimeDelta()
        {
            if (_event.ExpireDate.IsInfinite)
            {
                return string.Empty;
            }

            var delta = _event.ExpireDate.DayDifference(_event.CreateDate);
            if (delta < 90)
            {
                return delta + "d";
            }
            return _event.ExpireDate.MonthDifference(_event.CreateDate) + "m";
        }

        private string GetQuestGiver()
        {
            switch (_event.QuestGiver)
            {
                case QuestGiver.Na:
                    return Invalid;
                case QuestGiver.ForeignTrader:
                    return "Foreign Trader";
                case QuestGiver.Hanse:
                    return "Hanseatic League";
                default:
                    return _event.QuestGiver.ToString();
            }
        }

        private string GetDescription()
        {
            switch (Type)
            {
                case QuestType.RetrieveItem:
                    if (IsCompleted)
                    {
                        return "Return " + QuestItem + " to " + QuestGiverName + ".";
                    }
                    return "Find " + QuestItem + " for " + QuestGiverName + " in " + Source + ".";
                case QuestType.KillRaubritter:
                    if (IsCompleted)
                    {
                        return "Return to " + QuestGiverName + ".";
                    }
                    return "Kill raubritter for " + QuestGiverName + " in " + Source + ".";
            }

            return Invalid;
        }

        private string GetTooltip()
        {
            switch (Type)
            {
                case QuestType.RetrieveItem:
                    if (IsCompleted)
                    {
                        return "Return " + QuestItem + " to " + QuestGiverName + " in " + Destination +
                               " and claim reward by " + DeliverByDate + ".";
                    }
                    return "Get " + QuestItem + " from " + Destination + " for " + QuestGiverName + " in " + Source +
                           ".";
                case QuestType.KillRaubritter:
                    if (IsCompleted)
                    {
                        return "Return to " + QuestGiverName + " in " + Destination + " and claim reward by " +
                               DeliverByDate + ".";
                    }
                    return "Kill raubritter at " + Destination + " for " + QuestGiverName + " in " + Source + " by " +
                           DeliverByDate + ".";
            }

            return Invalid;
        }

        public static IEnumerable<QuestModel> FromEvents(IEnumerable<Event> events, IEnumerable<Location> locations,
            IEnumerable<ItemDefinition> items)
        {
            var quests = new List<QuestModel>(
                from e in events
                where e.IsActiveQuest
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