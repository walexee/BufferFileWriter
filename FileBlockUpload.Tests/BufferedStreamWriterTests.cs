using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileBlockUpload.Tests
{
    [TestClass]
    public class BufferedStreamWriterTests
    {
        [TestMethod]
        public void Writer_Should_Accurately_Write_Files()
        {
            var fileLength = 10000000;
            var fileBuffer = CreateRandomFile(fileLength);
            var destFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.dat");
            var blocksCount = 100;
            var blockSize = fileLength / blocksCount;

            using (var writer = new BufferedStreamWriter(destFilePath, fileLength))
            {
                for (var i = 0; i < blocksCount; i++)
                {
                    var startPos = i * blockSize;
                    var endPos = startPos + blockSize;
                    var blockBuffer = GetBlock(fileBuffer, startPos, endPos);

                    writer.WriteFileBlock(blockBuffer, i);
                }
            }

            var originalFileHash = Helpers.ComputeMd5Hash(fileBuffer);
            var destFileHash = Helpers.ComputeFileMd5Hash(destFilePath);

            //cleanup
            File.Delete(destFilePath);

            Assert.AreEqual(originalFileHash, destFileHash);
        }

        [TestMethod]
        public void Writer_Should_Accurately_Write_Files_When_Blocks_Received_Randomly()
        {
            var fileLength = 10000000;
            var fileBuffer = CreateRandomFile(fileLength);
            var destFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.dat");
            var blocksCount = 100;
            var blockSize = fileLength / blocksCount;
            var blockIndexes = new int[blocksCount];

            // set the indexes
            blockIndexes.ForEach((item, index) => blockIndexes[index] = index);

            // shuffle the indexes
            blockIndexes.Shuffle();

            using (var writer = new BufferedStreamWriter(destFilePath, fileLength))
            {
                foreach (var index in blockIndexes)
                {
                    var startPos = index * blockSize;
                    var endPos = startPos + blockSize;
                    var blockBuffer = GetBlock(fileBuffer, startPos, endPos);

                    writer.WriteFileBlock(blockBuffer, index);
                }
            }

            var originalFileHash = Helpers.ComputeMd5Hash(fileBuffer);
            var destFileHash = Helpers.ComputeFileMd5Hash(destFilePath);

            //cleanup
            File.Delete(destFilePath);

            Assert.AreEqual(originalFileHash, destFileHash);
        }

        [TestMethod]
        public void Writer_Should_Be_Thread_Safe()
        {
            var fileLength = 10000000;
            var fileBuffer = CreateRandomFile(fileLength);
            var destFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.dat");
            var blocksCount = 100;
            var blockSize = fileLength / blocksCount;
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 5
            };

            using (var writer = new BufferedStreamWriter(destFilePath, fileLength))
            {
                Parallel.For(0, blocksCount, index =>
                {
                    var startPos = index * blockSize;
                    var endPos = startPos + blockSize;
                    var blockBuffer = GetBlock(fileBuffer, startPos, endPos);

                    writer.WriteFileBlock(blockBuffer, index);
                });
            }

            var originalFileHash = Helpers.ComputeMd5Hash(fileBuffer);
            var destFileHash = Helpers.ComputeFileMd5Hash(destFilePath);

            //cleanup
            File.Delete(destFilePath);

            Assert.AreEqual(originalFileHash, destFileHash);
        }

        #region Helpers

        private byte[] CreateRandomFile(long fileSize)
        {
            var fileBuffer = new byte[fileSize];
            var random = new Random();

            random.NextBytes(fileBuffer);

            //for (var i = 0; i < fileSize; i++)
            //{
            //    fileBuffer[i] = random.NextBytes()
            //}

            return fileBuffer;
        }

        private byte[] GetBlock(byte[] fileBuffer, long startPos, long endPos)
        {
            var blockSize = endPos - startPos;
            var blockBuffer = new byte[blockSize];

            for (var i = 0; i < blockSize; i++)
            {
                blockBuffer[i] = fileBuffer[i + startPos];
            }

            return blockBuffer;
        } 
        #endregion
    }
}
