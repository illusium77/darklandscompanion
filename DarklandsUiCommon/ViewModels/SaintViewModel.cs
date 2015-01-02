using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsUiCommon.ViewModels
{
    public class SaintViewModel : ModelBase
    {
        private IEnumerable<SaintModel> m_saints;
        public IEnumerable<SaintModel> Saints
        {
            get { return m_saints; }
            set
            {
                m_saints = value;
                NotifyPropertyChanged();
            }
        }

        public SaintViewModel(IEnumerable<Saint> saints, SaintBitmask knownSaints)
        {
            var models = new List<SaintModel>();
            models.AddRange(from s in saints
                            select new SaintModel(s, knownSaints));

            Saints = models;
        }
    }
}
