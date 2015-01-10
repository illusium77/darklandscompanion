using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DarklandsUiCommon.Commands;

namespace DarklandsUiCommon.ViewModels
{
    public class SelectFolderDialogModel : ModelBase
    {
        private readonly Action _onClose;
        private IEnumerable<string> _requiredFiles;
        private string _selectedPath;

        public SelectFolderDialogModel(Action onClose)
        {
            BrowseCommand = new UiCommand(OnBrowse);
            OkCommand = new UiCommand(OnOk, IsProperFolder);

            _onClose = onClose;
        }

        public string SelectedPath
        {
            get { return _selectedPath; }
            set
            {
                _selectedPath = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<string> RequiredFiles
        {
            get { return _requiredFiles; }
            set
            {
                _requiredFiles = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand BrowseCommand { get; private set; }
        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            if (_onClose != null)
            {
                _onClose();
            }
        }

        private bool IsProperFolder()
        {
            if (!Directory.Exists(SelectedPath))
            {
                return false;
            }
            if (RequiredFiles == null || !RequiredFiles.Any())
            {
                return true;
            }

            try
            {
                var filesInDirectory =
                    from f in Directory.EnumerateFiles(SelectedPath, "*", SearchOption.AllDirectories)
                    where f != null
                    select Path.GetFileName(f).ToLower();

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