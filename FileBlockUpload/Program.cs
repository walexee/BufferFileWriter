using System;
using System.Diagnostics;

namespace FileBlockUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            //var filepath = @"C:\Users\oadeleye002\Downloads\LargeFiles\sample_10g.dat";
            //var destPath = $@"C:\Users\oadeleye002\Downloads\Uploads/file_{Guid.NewGuid()}.dat";

            var filepath = @"C:\Users\oadeleye002\Downloads\backgroundservice_USGCOV3APPSWV01.log";
            var destPath = $@"C:\Users\oadeleye002\Downloads\Uploads/file_{Guid.NewGuid()}.log";

            var uploader = new Uploader();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //uploader.UploadUsingSingleBlockWrite(filepath, destPath).GetAwaiter().GetResult();
            //uploader.UploadUsingMemoryMappedFile(filepath, destPath).GetAwaiter().GetResult();
            uploader.UploadUsingCustomBufferedMemory(filepath, destPath).GetAwaiter().GetResult();

            stopwatch.Stop();
        }
    }
}
