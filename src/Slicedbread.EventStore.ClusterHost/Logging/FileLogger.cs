namespace Slicedbread.EventStore.ClusterHost.Logging
{
    using System;
    using System.IO;

    public class FileLogger : ILogger
    {
        private readonly string filename;

        public FileLogger(string filename)
        {
            this.filename = filename;

            using (var writer = File.CreateText(filename))
            {
                writer.WriteLine("Starting log: {0}", DateTime.Now.ToString("G"));
            }
        }

        public void Log(string message)
        {
            File.AppendAllLines(this.filename, new[] { string.Format("{0}\t{1}", DateTime.Now.ToString("G"), message) });
        }
    }
}