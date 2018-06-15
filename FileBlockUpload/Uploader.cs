using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Threading.Tasks;

namespace FileBlockUpload
{
    public class Uploader
    {
        private readonly int BlockSize = 1024 * 1024 * 20;

        public async Task UploadUsingFileStream(string sourceFilepath, string destFilepath)
        {
            var sourceFileInfo = new FileInfo(sourceFilepath);
            var fileLength = sourceFileInfo.Length;
            var blocksCount = (int)Math.Ceiling(fileLength / (double)BlockSize);
            var sourceFileStream = sourceFileInfo.OpenRead();
            var destFileStream = File.OpenWrite(destFilepath);

            destFileStream.SetLength(sourceFileInfo.Length);

            for (int i = 0; i < blocksCount; i++)
            {
                var remainingLength = (fileLength - (i * BlockSize));

                // TODO: confirm
                if (remainingLength < 0)
                {
                    break;
                }
                var currentBlockSize = (int)(BlockSize > remainingLength ? remainingLength : BlockSize);
                var startPos = i * (long)BlockSize;
                var fileBlock = new byte[currentBlockSize];

                sourceFileStream.Position = startPos;
                await sourceFileStream.ReadAsync(fileBlock, 0, currentBlockSize);

                destFileStream.Position = startPos;
                await destFileStream.WriteAsync(fileBlock, 0, currentBlockSize);
            }

            destFileStream.Flush();
            destFileStream.Close();
            destFileStream.Dispose();
            sourceFileStream.Close();
            sourceFileStream.Dispose();
        }

        public async Task UploadUsingMemoryMappedFile(string sourceFilepath, string destFilepath)
        {
            var sourceFileInfo = new FileInfo(sourceFilepath);
            var fileLength = sourceFileInfo.Length;
            var blocksCount = (int)Math.Ceiling(fileLength / (double)BlockSize);
            var sourceFileStream = sourceFileInfo.OpenRead();
            var destFileStream = MemoryMappedFile.CreateFromFile(destFilepath, FileMode.Create, Guid.NewGuid().ToString(), fileLength);

            for (int i = 0; i < blocksCount; i++)
            {
                var remainingLength = fileLength - (i * BlockSize);

                // TODO: confirm
                if (remainingLength < 0)
                {
                    break;
                }

                var currentBlockSize = (int)(BlockSize > remainingLength ? remainingLength : BlockSize);
                long startPos = i * (long)BlockSize;
                var fileBlock = new byte[currentBlockSize];

                sourceFileStream.Position = startPos;
                await sourceFileStream.ReadAsync(fileBlock, 0, currentBlockSize);

                var destStreamAccessor = destFileStream.CreateViewAccessor(startPos, currentBlockSize);

                destStreamAccessor.WriteArray(0, fileBlock, 0, currentBlockSize);

                destStreamAccessor.Dispose();
            }

            destFileStream.Dispose();
            sourceFileStream.Close();
            sourceFileStream.Dispose();
        }

        public async Task UploadUsingCustomBufferedMemory(string sourceFilepath, string destFilepath)
        {
            var sourceFileInfo = new FileInfo(sourceFilepath);
            var fileLength = sourceFileInfo.Length;
            var blockSize = fileLength > BlockSize ? BlockSize : fileLength;
            var blocksCount = (int)Math.Ceiling(fileLength / (double)blockSize);
            var sourceFileStream = sourceFileInfo.OpenRead();
            var destWriter = new BufferedStreamWriter(destFilepath, fileLength);

            for (int i = 0; i < blocksCount; i++)
            {
                var remainingLength = fileLength - (i * blockSize);
                var currentBlockSize = (int)(blockSize > remainingLength ? remainingLength : blockSize);
                long startPos = i * (long)blockSize;
                var fileBlock = new byte[currentBlockSize];

                sourceFileStream.Position = startPos;
                await sourceFileStream.ReadAsync(fileBlock, 0, currentBlockSize);

                destWriter.WriteFileBlock(fileBlock, i);
            }

            destWriter.Dispose();
            sourceFileStream.Close();
            sourceFileStream.Dispose();
        }

    }
}
