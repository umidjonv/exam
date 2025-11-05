using System;
using NAudio.Wave;

namespace WaveFormRendererLib.Providers
{

    public class SavingWaveProvider : IWaveProvider, IDisposable
    {
        private readonly IWaveProvider _sourceWaveProvider;
        private readonly WaveFileWriter _writer;
        private bool _isWriterDisposed;

        public SavingWaveProvider(IWaveProvider sourceWaveProvider, string wavFilePath)
        {
            _sourceWaveProvider = sourceWaveProvider;
            _writer = new WaveFileWriter(wavFilePath, sourceWaveProvider.WaveFormat);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var read = _sourceWaveProvider.Read(buffer, offset, count);

            if (count > 0 && !_isWriterDisposed)
            {
                _writer.Write(buffer, offset, read);
            }

            if (count == 0)
            {
                Dispose();
            }

            return read;
        }

        public WaveFormat WaveFormat => _sourceWaveProvider.WaveFormat;

        public void Dispose()
        {
            if (!_isWriterDisposed)
            {
                _isWriterDisposed = true;
                _writer.Dispose();
            }
        }

    }
}
