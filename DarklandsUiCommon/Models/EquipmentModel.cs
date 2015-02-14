using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.Contracts;
using DarklandsUiCommon.ViewModels;

namespace DarklandsUiCommon.Models
{
    public class EquipmentModel : ModelBase, IEditableObject, IValidableObject
    {
        private bool _editing;
        private int _backupQuantity;
        private int _backupQuality;
        public Item Item { get; private set; }

        public EquipmentModel()
        {
        }

        public EquipmentModel(Item item)
        {
            Item = item;
        }
        public string Name { get; set; }

        public EquipmentCategory Category { get; set; }

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

        #region IValidableObject

        public bool IsValid
        {
            get { return string.IsNullOrEmpty(Item.Error); }
        }

        #endregion
    }
}