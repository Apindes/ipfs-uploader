using System;

namespace Uploader.Models
{
    public class FileItem
    {
        public static FileItem NewSourceVideoFileItem(FileContainer fileContainer, string sourceFilePath)
        {
            FileItem fileItem = new FileItem(fileContainer, true, "waiting in queue...");
            fileItem.FilePath = sourceFilePath;

            fileItem.VideoSize = VideoSize.Source;
            fileItem.EncodeProgress = "not available";
            fileItem.EncodeLastTimeProgressChanged = null;

            return fileItem;
        }

        public static FileItem NewEncodedVideoFileItem(FileContainer fileContainer, VideoSize videoSize)
        {
            if(videoSize == VideoSize.Undefined)
                throw new InvalidOperationException("VideoSize inconnu");

            FileItem fileItem = new FileItem(fileContainer, false, "waiting encode...");

            fileItem.VideoSize = videoSize;
            fileItem.EncodeProgress = "waiting in queue...";
            fileItem.EncodeLastTimeProgressChanged = null;

            return fileItem;
        }

        public static FileItem NewSpriteVideoFileItem(FileContainer fileContainer)
        {
            FileItem fileItem = new FileItem(fileContainer, false, "waiting sprite creation...");

            fileItem.ModeSprite = true;
            fileItem.VideoSize = VideoSize.Source;
            fileItem.EncodeProgress = "waiting in queue...";
            fileItem.EncodeLastTimeProgressChanged = null;

            return fileItem;
        }

        public static FileItem NewSourceImageFileItem(FileContainer fileContainer, string sourceFilePath)
        {
            FileItem fileItem = new FileItem(fileContainer, true, "waiting in queue...");
            fileItem.FilePath = sourceFilePath;
            fileItem.IpfsErrorMessage = "ipfs not asked";

            return fileItem;
        }

        public static FileItem NewAttachedImageFileItem(FileContainer fileContainer, string filePath)
        {
            FileItem fileItem = new FileItem(fileContainer, false, "waiting in queue...");
            fileItem.FilePath = filePath;

            return fileItem;
        }

        private FileItem(FileContainer fileContainer, bool isSource, string ipfsProgressInitialMessage)
        {
            IsSource = isSource;
            FileContainer = fileContainer;
            IpfsProgress = ipfsProgressInitialMessage;
            IpfsLastTimeProgressChanged = null;
        }

        public bool IsSource { get; }

        public long? FileSize { get; set; }

        public string FilePath
        { 
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;

                if(System.IO.File.Exists(_filePath))
                    FileSize = new System.IO.FileInfo(_filePath).Length;
            }
        }
        private string _filePath;

        public FileContainer FileContainer { get; }

        public bool ModeSprite { get; private set; }

        public bool WorkInProgress()
        {
            if(!string.IsNullOrWhiteSpace(IpfsErrorMessage))
                return false;
            if(!string.IsNullOrWhiteSpace(EncodeErrorMessage))
                return false;

            return string.IsNullOrWhiteSpace(IpfsHash);
        }


        public int? IpfsPositionInQueue { get; set; }

        public string IpfsHash { get; set; }

        private string _ipfsProgress;

        public string IpfsProgress
        {
            get
            {
                return _ipfsProgress;
            }

            set
            {
                _ipfsProgress = value;
                IpfsLastTimeProgressChanged = DateTime.UtcNow;
            }
        }

        public DateTime? IpfsLastTimeProgressChanged { get; private set; }

        public string IpfsErrorMessage { get; set; }




        public VideoSize VideoSize { get; private set; }

        public int? EncodePositionInQueue { get; set; }

        private string _encodeProgress;

        public string EncodeProgress
        {
            get
            {
                return _encodeProgress;
            }

            set
            {
                _encodeProgress = value;
                EncodeLastTimeProgressChanged = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// in seconds
        /// </summary>
        /// <returns></returns>
        public int? VideoDuration { get; set; }

        public DateTime? EncodeLastTimeProgressChanged { get; private set; }

        public string EncodeErrorMessage { get; set; }
    }
}