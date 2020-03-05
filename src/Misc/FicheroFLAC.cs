using CSCore.Codecs.FLAC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace CSCore.Codecs.FLAC
{
    public class FlacMetadataVorbisComment : FlacMetadata
    {
        protected override unsafe void InitializeByStream(Stream stream)

        {

            //http://flac.sourceforge.net/format.html#metadata_block_streaminfo

            var reader = new BinaryReader(stream, Encoding.UTF8);

            try

            {

                MinBlockSize = reader.ReadInt16();

                MaxBlockSize = reader.ReadInt16();

            }

            catch (IOException e)

            {

                throw new FlacException(e, FlacLayer.Metadata);

            }

            const int bytesToRead = (240 / 8) - 16;

            byte[] buffer = reader.ReadBytes(bytesToRead);

            if (buffer.Length != bytesToRead)

            {

                throw new FlacException(new EndOfStreamException("Could not read VorbisComment-content"),

                    FlacLayer.Metadata);

            }
        }

        /// <summary>

        /// Gets the minimum size of the block in samples.

        /// </summary>

        /// <value>

        /// The minimum size of the block in samples.

        /// </value>

        public short MinBlockSize { get; private set; }



        /// <summary>

        /// Gets the maximum size of the block in samples.

        /// </summary>

        /// <value>

        /// The maximum size of the block in samples.

        /// </value>

        public short MaxBlockSize { get; private set; }
    }

}
*/