using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace INTSTDK009.Shared.PipelineComponents.Archiver
{
    /// <summary>
    /// This class is an implementation of ICSharpCode.SharpZipLib.Zip.IStaticDataSource.
    /// It is used in conjuction with ZipFile.Add to add a BizTalk message stream to a Zip archive.
    /// </summary>
    class StreamBasedDataSource : IStaticDataSource
    {
        private Stream stream = null;

        /// <summary>
        /// Public contructor.
        /// </summary>
        /// <param name="stream">Stream to be made available as a DataSource.</param>
        public StreamBasedDataSource(Stream stream)
        {
            this.stream = stream;
        }

        #region IStaticDataSource Members

        /// <summary>
        /// Method that returns the stream to add to Zip archive.
        /// </summary>
        /// <returns>Stream to add to Zip archive</returns>
        public System.IO.Stream GetSource()
        {
            return stream;
        }

        #endregion
    }

}
