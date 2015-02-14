using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DarklandsBusinessObjects.Objects;

namespace DarklandsUiCommon.Models
{
    public class EquipmentListModel : ObservableCollection<EquipmentModel>
    {
        private static IReadOnlyList<ItemDefinition> _itemDefinitions;
        private readonly Character _character;
        private ICollectionView _view;

        public EquipmentListModel(Character character, IReadOnlyList<ItemDefinition> itemDefinitions)
        {
            _character = character;

            if (_itemDefinitions == null)
            {
                _itemDefinitions = itemDefinitions;
            }

            foreach (var item in character.ItemList.Items)
            {
                var definition = itemDefinitions.FirstOrDefault(d => d.Id == item.Id);
                if (definition == null || item.IsEmpty) continue;

                var model = new EquipmentModel(item)
                {
                    Name = definition.Name,
                    Category = MapCategory(definition)
                };

                Add(model);
            }
        }

        public ICollectionView View
        {
            get
            {
                if (_view == null)
                {
                    _view = CollectionViewSource.GetDefaultView(this);

                    if (_view != null)
                    {
                        _view.GroupDescriptions.Add(new ItemTypeGroup());
                    }
                }
                return _view;
            }
        }

        private static EquipmentCategory MapCategory(ItemDefinition definition)
        {
            if (definition.MaskA.HasFlag(ItemMaskA.IsEdged)
                || definition.MaskA.HasFlag(ItemMaskA.IsFlail)
                || definition.MaskA.HasFlag(ItemMaskA.IsImpact)
                || definition.MaskA.HasFlag(ItemMaskA.IsPolearm))
            {
                return EquipmentCategory.MeleeWeapon;
            }

            if (definition.MaskA.HasFlag(ItemMaskA.IsThrown)
                || definition.MaskA.HasFlag(ItemMaskA.IsBow)
                || definition.MaskD.HasFlag(ItemMaskD.IsMissileWeapon))
            {
                return EquipmentCategory.MissileWeapon;
            }

            if (definition.MaskC.HasFlag(ItemMaskC.IsArrow)
                || definition.MaskC.HasFlag(ItemMaskC.IsBall)
                || definition.MaskC.HasFlag(ItemMaskC.IsQuarrel))
            {
                return EquipmentCategory.Ammunition;
            }

            if (definition.MaskA.HasFlag(ItemMaskA.IsMetalArmor)
                || definition.MaskD.HasFlag(ItemMaskD.IsNoMetalArmor))
            {
                if (definition.Name.StartsWith("V:"))
                {
                    return EquipmentCategory.VitalArmor;
                }

                return EquipmentCategory.LegArmor;
            }

            if (definition.MaskA.HasFlag(ItemMaskA.IsShield))
            {
                return EquipmentCategory.Shield;
            }

            if (definition.MaskB.HasFlag(ItemMaskB.IsComponent))
            {
                return EquipmentCategory.Component;
            }

            if (definition.MaskB.HasFlag(ItemMaskB.IsPotion))
            {
                return EquipmentCategory.Potion;
            }

            if (definition.MaskB.HasFlag(ItemMaskB.IsQuestIndoor)
                || definition.MaskC.HasFlag(ItemMaskC.IsQuestOutdoor))
            {
                return EquipmentCategory.QuestItem;
            }

            if (definition.MaskB.HasFlag(ItemMaskB.IsPotion))
            {
                return EquipmentCategory.Potion;
            }

            if (definition.MaskB.HasFlag(ItemMaskB.IsRelic))
            {
                return EquipmentCategory.Relic;
            }

            return EquipmentCategory.Miscellaneous;
        }

        private class ItemTypeGroup : PropertyGroupDescription
        {
            public override object GroupNameFromItem(object item, int level, CultureInfo culture)
            {
                var e = item as EquipmentModel;
                if (e == null) return "Unknown";

                switch (e.Category)
                {
                    case EquipmentCategory.MeleeWeapon:
                        return "Weapons";
                    case EquipmentCategory.MissileWeapon:
                    case EquipmentCategory.Ammunition:
                        return "Ranged Weapons and Ammunition";
                    case EquipmentCategory.VitalArmor:
                    case EquipmentCategory.LegArmor:
                    case EquipmentCategory.Shield:
                        return "Armor and Shields";
                    case EquipmentCategory.Component:
                        return "Alchemic Components";
                    case EquipmentCategory.Potion:
                        return "Potions";
                    case EquipmentCategory.QuestItem:
                    case EquipmentCategory.Relic:
                        return "Quest Items and Relics";
                    default:
                        return "Miscellaneous";
                }
            }
        }
    }
}