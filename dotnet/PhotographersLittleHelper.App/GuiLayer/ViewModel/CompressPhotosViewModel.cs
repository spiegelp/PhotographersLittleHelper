using MaterialDesignExtensions.Controls;
using NuniToolbox.Ui.Commands;
using PhotographersLittleHelper.Core.Pipe;
using PhotographersLittleHelper.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhotographersLittleHelper.App.GuiLayer.ViewModel
{
    public class CompressPhotosViewModel : ViewModelObject
    {
        private string m_sourceDirectory;
        private string m_sinkDirectory;
        private int m_quality;

        public ICommand SelectSourceDirectoryCommand { get; init; }

        public ICommand SelectSinkDirectoryCommand { get; init; }

        public ICommand CompressCommand { get; init; }

        public int MaxQuality => PhotoCompressionStep.MinQuality;

        public int MinQuality => PhotoCompressionStep.MaxQuality;

        public int Quality
        {
            get
            {
                return m_quality;
            }

            set
            {
                m_quality = value;

                OnPropertyChanged();
            }
        }

        public string SinkDirectory
        {
            get
            {
                return m_sinkDirectory;
            }

            set
            {
                m_sinkDirectory = value;

                OnPropertyChanged();
            }
        }

        public string SourceDirectory
        {
            get
            {
                return m_sourceDirectory;
            }

            set
            {
                m_sourceDirectory = value;

                OnPropertyChanged();
            }
        }

        public CompressPhotosViewModel(WindowViewModel windowViewModel)
            : base(windowViewModel)
        {
            SelectSourceDirectoryCommand = new DelegateCommand(SelectSourceDirectoryHandler);
            SelectSinkDirectoryCommand = new DelegateCommand(SelectSinkDirectoryHandler);
            CompressCommand = new DelegateCommand(CompressHandler);

            m_quality = 90;

#if DEBUG
            m_sourceDirectory = @"C:\temp\photos\in";
            m_sinkDirectory = @"C:\temp\photos\out";
#endif
        }

        private async void SelectSourceDirectoryHandler()
        {
            OpenDirectoryDialogResult result = await SelectDirectoryAsync(m_sourceDirectory).ConfigureAwait(false);

            if (result.Confirmed)
            {
                SourceDirectory = result.Directory;
            }
        }

        private async void SelectSinkDirectoryHandler()
        {
            OpenDirectoryDialogResult result = await SelectDirectoryAsync(m_sinkDirectory).ConfigureAwait(false);

            if (result.Confirmed)
            {
                SinkDirectory = result.Directory;
            }
        }

        private async Task<OpenDirectoryDialogResult> SelectDirectoryAsync(string directory)
        {
            OpenDirectoryDialogArguments args = new()
            {
                Width = 600,
                Height = 800,
                CurrentDirectory = directory ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };

            return await OpenDirectoryDialog.ShowDialogAsync(WindowViewModel.MainWindowDialogHostName, args);
        }

        private async void CompressHandler()
        {
            IsBusy = true;

            try
            {
                await Task.Run(async () =>
                {
                    await new PhotoService().CompressPhotosAsync(m_sourceDirectory, m_sinkDirectory, m_quality)
                        .ConfigureAwait(false);
                });
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
