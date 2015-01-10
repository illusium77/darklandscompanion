using System.Collections.Generic;
using System.Linq;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.Models;

namespace DarklandsUiCommon.ViewModels
{
    public class SaintViewModel : ModelBase
    {
        private IEnumerable<SaintModel> _saints;

        public SaintViewModel(IEnumerable<Saint> saints, SaintBitmask knownSaints)
        {
            var models = new List<SaintModel>();
            models.AddRange(from s in saints
                select new SaintModel(s, knownSaints));

            Saints = models;
        }

        public IEnumerable<SaintModel> Saints
        {
            get { return _saints; }
            set
            {
                _saints = value;
                NotifyPropertyChanged();
            }
        }
    }
}