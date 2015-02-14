using System.ComponentModel;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.Contracts;
using DarklandsUiCommon.ViewModels;

namespace DarklandsUiCommon.Models
{
    public class EquipmentModel : ModelBase, IEditableObject, IValidableObject
    {
        private int _backupQuality;
        private int _backupQuantity;
        private bool _editing;

        public EquipmentModel()
        {
        }

        public EquipmentModel(Item item)
        {
            Item = item;
        }

        public Item Item { get; private set; }
        public string Name { get; set; }
        public EquipmentCategory Category { get; set; }

        #region IValidableObject

        public bool IsValid
        {
            get { return string.IsNullOrEmpty(Item.Error); }
        }

        #endregion

        #region IEditableObject

        public void BeginEdit()
        {
            if (_editing) return;

            _backupQuantity = Item.Quantity;
            _backupQuality = Item.Quality;

            _editing = true;
        }

        public void EndEdit()
        {
            if (_editing)
            {
                _editing = false;
            }
        }

        public void CancelEdit()
        {
            if (!_editing) return;

            Item.Quantity = _backupQuantity;
            Item.Quality = _backupQuality;
            _editing = false;
        }

        #endregion
    }
}