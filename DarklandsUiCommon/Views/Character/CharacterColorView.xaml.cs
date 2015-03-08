namespace DarklandsUiCommon.Views.Character
{
    /// <summary>
    /// Interaction logic for CharacterColors.xaml
    /// </summary>
    public partial class CharacterColorView
    {
        public CharacterColorView()
        {
            InitializeComponent();

            DataContextChanged += CharacterColorView_DataContextChanged;
        }

        void CharacterColorView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
