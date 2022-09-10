using MaterialDesignExtensions.Controls;
using NuniToolbox.Collections;
using NuniToolbox.Ui.Commands;
using PhotographersLittleHelper.Core.Pipe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhotographersLittleHelper.App.GuiLayer.ViewModel
{
    public class BuildPipeViewModel : ViewModelObject
    {
        private DirectorySource m_source;
        private DirectorySink m_sink;

        private readonly ExtendedObservableCollection<IStep<PhotoData, PhotoData>> m_steps;

        public ICommand AddStepCommand { get; init; }

        public ICommand SelectSourceDirectoryCommand { get; init; }

        public ICommand SelectSinkDirectoryCommand { get; init; }

        public ICommand RunPipeCommand { get; init; }

        public DirectorySource Source
        {
            get
            {
                return m_source;
            }

            set
            {
                m_source = value;

                OnPropertyChanged();
            }
        }

        public DirectorySink Sink
        {
            get
            {
                return m_sink;
            }

            set
            {
                m_sink = value;

                OnPropertyChanged();
            }
        }

        public ExtendedObservableCollection<IStep<PhotoData, PhotoData>> Steps
        {
            get
            {
                return m_steps;
            }
        }

        public BuildPipeViewModel(WindowViewModel windowViewModel)
            : base(windowViewModel)
        {
            AddStepCommand = new DelegateCommand<Action>(AddStepHandler);
            SelectSourceDirectoryCommand = new DelegateCommand(SelectSourceDirectoryHandler);
            SelectSinkDirectoryCommand = new DelegateCommand(SelectSinkDirectoryHandler);
            RunPipeCommand = new DelegateCommand(RunPipeHandler);

            m_source = new();
            m_sink = new();

            m_steps = new();

#if DEBUG
            m_source.Directory = @"C:\temp\photos\in";
            m_sink.Directory = @"C:\temp\photos\out";
            //m_steps.Add(new PhotoCompressionStep());
#endif
        }

        private async void SelectSourceDirectoryHandler()
        {
            OpenDirectoryDialogResult result = await SelectDirectoryAsync(m_source.Directory).ConfigureAwait(false);

            if (result.Confirmed)
            {
                m_source.Directory = result.Directory;
            }
        }

        private async void SelectSinkDirectoryHandler()
        {
            OpenDirectoryDialogResult result = await SelectDirectoryAsync(m_sink.Directory).ConfigureAwait(false);

            if (result.Confirmed)
            {
                m_sink.Directory = result.Directory;
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

        private void AddStepHandler(Action action)
        {
            action();
        }

        public Action AddResizingStep => () =>
        {
            m_steps.Add(new PhotoResizingStep());
        };

        public Action AddCompressionStep => () =>
        {
            m_steps.Add(new PhotoCompressionStep());
        };

        private async void RunPipeHandler()
        {
            IsBusy = true;

            try
            {
                if (!string.IsNullOrWhiteSpace(m_source.Directory) && new DirectoryInfo(m_source.Directory).Exists
                    && !string.IsNullOrWhiteSpace(m_sink.Directory) && new DirectoryInfo(m_sink.Directory).Exists)
                {
                    await Task.Run(async () =>
                    {
                        Pipe<PhotoData> pipe = Pipe<PhotoData>.Create(m_source, m_steps.ToList(), m_sink);
                        await pipe.RunAsync().ConfigureAwait(false);
                    }).ConfigureAwait(false);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
