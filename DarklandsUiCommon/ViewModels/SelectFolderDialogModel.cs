using DarklandsUiCommon.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DarklandsUiCommon.ViewModels
{
    public class SelectFolderDialogModel : ViewModelBase
    {
        private Action m_onClose;

        private string m_selectedPath;
        public string SelectedPath
        {
            get { return m_selectedPath; }
            set
            {
                m_selectedPath = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<string> m_requiredFiles;
        public IEnumerable<string> RequiredFiles
        {
            get { return m_requiredFiles; }
            set
            {
                m_requiredFiles = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand BrowseCommand { get; private set; }
        public ICommand OkCommand { get; private set; }

        public SelectFolderDialogModel(Action OnClose)
        {
            BrowseCommand = new UiCommand(OnBrowse);
            OkCommand = new UiCommand(OnOk, IsProperFolder);

            m_onClose = OnClose;
        }

        private void OnOk()
        {
            if (m_onClose != null)
            {
                m_onClose();
            }
        }

        private bool IsProperFolder()
        {
            if (!Directory.Exists(SelectedPath))
            {
                return false;
            }
            else if (RequiredFiles == null || RequiredFiles.Count() == 0)
            {
                return true;
            }

            try
            {
                var filesInDirectory = Directory.EnumerateFiles(
                                SelectedPath, "*", SearchOption.AllDirectories).Select(f => Path.GetFileName(f).ToLower());

                return !RequiredFiles.Except(filesInDirectory).Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void OnBrowse()
        {
            // forms...
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectedPath = dialog.SelectedPath;
            }
        }
    }
}
