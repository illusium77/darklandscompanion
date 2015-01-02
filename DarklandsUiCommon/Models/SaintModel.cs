using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Utils;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsUiCommon.Models
{
    public class SaintModel : ModelBase
    {
        private int m_id;
        private SaintBitmask m_bitmask;

        public string Name { get; private set; }
        public string Tip { get; private set; }

        public bool IsKnown
        {
            get { return m_bitmask.HasSaint(m_id); }
            set
            {
                if (value)
                {
                    m_bitmask.LearnSaint(m_id);
                }
                else
                {
                    m_bitmask.ForgetSaint(m_id);
                }
                NotifyPropertyChanged();
            }
        }

        public SaintModel(Saint saint, SaintBitmask bitmask)
        {
            Name = saint.ShortName;
            Tip = StringHelper.WordWrap(saint.Clue, 70);

            m_id = saint.Id;
            m_bitmask = bitmask;
        }
    }
}
