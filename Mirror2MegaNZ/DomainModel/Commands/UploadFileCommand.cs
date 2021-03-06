﻿using CG.Web.MegaApiClient;
using Mirror2MegaNZ.Logic;
using System;

namespace Mirror2MegaNZ.DomainModel.Commands
{
    internal class UploadFileCommand  : ICommand
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the last modified date of the file to upload.
        /// We need this information because the remote filename will containt
        /// the last modified date of the original file
        /// </summary>
        /// <value>
        /// The last modified date.
        /// </value>
        public DateTime LastModifiedDate { get; set; }

        public void Execute(IMegaApiClient megaApiClient, 
            IMegaNzItemCollection megaNzItemCollection,
            IFileManager fileManager,
            IProgress<double> progressNotifier)
        {
            using (var filestream = fileManager.GetStreamToReadFile(SourcePath))
            {
                var sourceFileName = System.IO.Path.GetFileName(SourcePath);
                var remoteFileName = NameHandler.BuildRemoteFileName(sourceFileName, LastModifiedDate);
                var parentMegaNzNode = megaNzItemCollection.GetByPath(DestinationPath);
                var newMegaNzNode = megaApiClient.UploadAsync(filestream, remoteFileName, parentMegaNzNode, progressNotifier).Result;
                megaNzItemCollection.Add(newMegaNzNode);
            }
        }

        public override string ToString()
        {
            return $"# Upload File Command\r\nSourcePath: {SourcePath}\r\nDestinationPath: {DestinationPath}\r\nLastModifiedDate: {LastModifiedDate.ToString()}\r\nSize: {FileSizeFormatter.Format(Size)}";
        }
    }
}
