﻿using System.IO;

namespace NAudio.Wave
{
    /// <summary>
    /// WaveStream that simply passes on data from its source stream
    /// (e.g. a MemoryStream)
    /// </summary>
    public class RawSourceWaveStream : WaveStream
    {
        private readonly Stream sourceStream;
        private readonly WaveFormat waveFormat;

        /// <summary>
        /// Initialises a new instance of RawSourceWaveStream
        /// </summary>
        /// <param name="sourceStream">The source stream containing raw audio</param>
        /// <param name="waveFormat">The waveformat of the audio in the source stream</param>
        public RawSourceWaveStream(Stream sourceStream, WaveFormat waveFormat)
        {
            this.sourceStream = sourceStream;
            this.waveFormat = waveFormat;
        }

        /// <summary>
        /// The WaveFormat of this stream
        /// </summary>
        public override WaveFormat WaveFormat => this.waveFormat;

        /// <summary>
        /// The length in bytes of this stream (if supported)
        /// </summary>
        public override long Length => this.sourceStream.Length;

        /// <summary>
        /// The current position in this stream
        /// </summary>
        public override long Position
        {
            get => this.sourceStream.Position;
            set => this.sourceStream.Position = value;
        }

        /// <summary>
        /// Reads data from the stream
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count) => sourceStream.Read(buffer, offset, count);
    }
}

