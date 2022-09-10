using PhotographersLittleHelper.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public class DirectorySource : Source<PhotoData>
    {
        private string m_directoy;

        public string Directory
        {
            get
            {
                return m_directoy;
            }

            set
            {
                m_directoy = value;

                OnPropertyChanged();
            }
        }

        public PhotoService PhotoService { get; init; }

        public DirectorySource()
            : base()
        {
            PhotoService = new();
        }

        public override async Task WorkAsync()
        {
            DirectoryInfo directory = new(Directory);

            List<FileInfo> photoFiles = directory.GetFiles()
                .Where(file => PhotoService.SupportedImageFormats.Contains(PhotoService.GetImageFormatForFilename(file.Extension)))
                .ToList();

            if (Environment.ProcessorCount >= 2 && photoFiles.Count >= 4)
            {
                List<FileInfo[]> lists = new() {
                    photoFiles.Take(photoFiles.Count / 2).ToArray(),
                    photoFiles.Take(new Range(photoFiles.Count / 2, photoFiles.Count - 1)).ToArray()
                };

                var tasks = lists
                    .Select(list => Task.Run(async () => await WorkInternalAsync(list).ConfigureAwait(false)))
                    .ToList();

                foreach (var task in tasks)
                {
                    await task.ConfigureAwait(false);
                }
            }
            else
            {
                foreach (FileInfo photoFile in photoFiles)
                {
                    await WorkInternalAsync(photoFile).ConfigureAwait(false);
                }
            }
        }

        private async Task<bool> WorkInternalAsync(FileInfo[] photoFiles)
        {
            foreach (FileInfo photoFile in photoFiles)
            {
                PhotoData photoData = await PhotoService.ReadPhotoAsync(photoFile.FullName).ConfigureAwait(false);

                await NextStep.WorkAsync(photoData).ConfigureAwait(false);
            }

            return true;
        }

        private async Task<bool> WorkInternalAsync(FileInfo photoFile)
        {
            PhotoData photoData = await PhotoService.ReadPhotoAsync(photoFile.FullName).ConfigureAwait(false);

            return await NextStep.WorkAsync(photoData).ConfigureAwait(false);
        }
    }
}
