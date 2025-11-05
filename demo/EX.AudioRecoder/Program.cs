using NAudio.Wave;
using System;
using System.IO;
using System.Threading;

namespace EX.AudioRecoder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var elapsed = new TimeSpan(0, 0, 60);
            var id = Guid.NewGuid();
            var capture = new WaveInEvent
            {
                DeviceNumber = 0,
                BufferMilliseconds = 1000,
                WaveFormat = new WaveFormat(44100, 2),
            };
            var path = Path.Combine(Environment.CurrentDirectory, $"record-{id}.wav");
            var writer = new WaveFileWriter(path, capture.WaveFormat);

            capture.DataAvailable += (s, a) =>
            {
                writer.Write(a.Buffer, 0, a.BytesRecorded);
            };

            capture.RecordingStopped += (s, a) =>
            {
                writer.Dispose();
                writer = null;
                capture.Dispose();
            };

            capture.StartRecording();

            Thread.Sleep(elapsed);

            Console.WriteLine("Done!", args);

            capture.StopRecording();
             
        }
    }
}
