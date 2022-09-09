using PhotographersLittleHelper.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public class DirectorySource : Source<PhotoData>
    {
        public string Directory { get; set; }

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

            foreach (FileInfo photoFile in photoFiles)
            {
                await WorkInternalAsync(photoFile).ConfigureAwait(false);
            }
        }

        private async Task<bool> WorkInternalAsync(FileInfo photoFile)
        {
            PhotoData photoData = await PhotoService.ReadPhotoAsync(photoFile.FullName).ConfigureAwait(false);

            return await NextStep.WorkAsync(photoData).ConfigureAwait(false);
        }
    }
}
