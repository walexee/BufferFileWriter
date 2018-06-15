using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileBlockUpload
{
    public class BufferedStreamWriter : IDisposable
    {
        private readonly LinkedList<FileBlock> _pendingFileBlocks;
        private readonly Stream _destFileStream;
        private static int _blocksUploadCompleted = 0;
        private static int _writingInprogress = 0;
        private static ReaderWriterLockSlim _listLock = new ReaderWriterLockSlim();
        private static Thread _writeThread;
        private const int MaxFileBlocksAllowedInList = 10;
        private readonly bool _externalStream = false;

        public BufferedStreamWriter(string destFilePath, long capacity)
        {
            _pendingFileBlocks = new LinkedList<FileBlock>();
            _destFileStream = File.OpenWrite(destFilePath);
            _destFileStream.SetLength(capacity);
        }

        public BufferedStreamWriter(Stream stream, long capacity)
        {
            _externalStream = true;
            _pendingFileBlocks = new LinkedList<FileBlock>();
            
            if (!stream.CanSeek || !stream.CanWrite)
            {
                // TODO: change exception type
                throw new InvalidOperationException("The stream needs to be writable and seekable");
            }

            _destFileStream = stream;
            _destFileStream.SetLength(capacity);
        }

        public void WriteFileBlock(byte[] blockContent, long blockIndex)
        {
            var fileBlock = new FileBlock(blockContent, blockIndex);

            AddBlockToList(fileBlock);

            ExecuteWrite();
        }

        private void CompleteWrite()
        {
            Interlocked.Exchange(ref _blocksUploadCompleted, 1);

            while (_writingInprogress == 1)
            {
                Thread.Sleep(50);
            }
        }

        private void AddBlockToList(FileBlock fileBlock)
        {
            _listLock.EnterWriteLock();

            try
            {
                if (_pendingFileBlocks.Count >= MaxFileBlocksAllowedInList)
                {
                    Thread.Sleep(100);
                }

                _pendingFileBlocks.AddLast(fileBlock);
            }
            finally
            {
                if (_listLock.IsWriteLockHeld)
                {
                    _listLock.ExitWriteLock();
                }
            }
        }

        private void RemoveFromBlockList(FileBlock fileBlock)
        {
            _listLock.EnterWriteLock();

            try
            {
                _pendingFileBlocks.Remove(fileBlock);
            }
            finally
            {
                if (_listLock.IsWriteLockHeld)
                {
                    _listLock.ExitWriteLock();
                }
            }
        }

        private FileBlock GetFirstFromList()
        {
            _listLock.EnterReadLock();

            try
            {
                if (_pendingFileBlocks.Count > 0)
                    return _pendingFileBlocks.First();
            }
            finally
            {
                if (_listLock.IsReadLockHeld)
                {
                    _listLock.ExitReadLock();
                }
            }

            // TODO: check if this works
            return null;
        }

        private void ExecuteWrite()
        {
            //0  method is not in use.
            if (0 == Interlocked.Exchange(ref _writingInprogress, 1))
            {
                _writeThread = new Thread(new ThreadStart(WriteListToFile));

                _writeThread.Start();
            }
        }

        private void WriteListToFile()
        {
            while (true)
            {
                var fileBlock = GetFirstFromList();

                if (fileBlock == null)
                {
                    if (_blocksUploadCompleted == 1)
                    {
                        _writingInprogress = 0;
                        break;
                    }

                    Thread.Sleep(100);
                    continue;
                }

                //write block
                
                var blockLength = fileBlock.Content.Length;

                _destFileStream.Position = blockLength * fileBlock.Index;
                _destFileStream.WriteAsync(fileBlock.Content, 0, blockLength).GetAwaiter().GetResult();

                //remove from list
                RemoveFromBlockList(fileBlock);
            }

            //Release the lock
            Interlocked.Exchange(ref _writingInprogress, 0);
        }

        public void Dispose()
        {
            CompleteWrite();

            if (_writeThread != null && _writeThread.IsAlive)
            {
                _writeThread.Abort();
            }

            if (!_externalStream)
            {
                _destFileStream.Flush();
                _destFileStream.Close();
                _destFileStream.Dispose();
            }
        }

        private class FileBlock
        {
            public FileBlock (byte[] content, long index)
            {
                Content = content;
                Index = index;
            }

            public byte[] Content { get; set; }

            /// <summary>
            /// Zero-based index
            /// </summary>
            public long Index { get; set; }
        }
    }
}
