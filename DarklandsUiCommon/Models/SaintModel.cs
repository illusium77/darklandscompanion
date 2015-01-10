using System;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Utils;
using DarklandsUiCommon.ViewModels;

namespace DarklandsUiCommon.Models
{
    public class SaintModel : ModelBase
    {
        private readonly SaintBitmask _bitmask;
        private readonly int _id;

        public SaintModel(Saint saint, SaintBitmask bitmask)
        {
            Name = saint.ShortName;
            Tip = StringHelper.WordWrap(saint.Description + Environment.NewLine + Environment.NewLine + saint.Clue, 70);

            _id = saint.Id;
            _bitmask = bitmask;
        }

        public string Name { get; private set; }
        public string Tip { get; private set; }

        public bool IsKnown
        {
            get { return _bitmask.HasSaint(_id); }
            set
            {
                if (value)
                {
                    _bitmask.LearnSaint(_id);
                }
                else
                {
                    _bitmask.ForgetSaint(_id);
                }
                NotifyPropertyChanged();
            }
        }
    }
}