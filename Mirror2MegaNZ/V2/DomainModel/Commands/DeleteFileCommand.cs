﻿using CG.Web.MegaApiClient;
using Mirror2MegaNZ.Logic;
using Mirror2MegaNZ.V2.Logic;
using System;

namespace Mirror2MegaNZ.V2.DomainModel.Commands
{
    internal class DeleteFileCommand : ICommand
    {
        public string PathToDelete { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public void Execute(IMegaApiClient megaApiClient, 
            IMegaNzItemCollection megaNzItemCollection, 
            IFileManager fileManager,
            IProgress<double> progressNotifier)
        {
            var megaNzNodeToDelete = megaNzItemCollection.GetByPath(PathToDelete);
            megaApiClient.Delete(megaNzNodeToDelete, true);
        }

        public override string ToString()
        {
            return $"Delete File Command - PathToDelete: {PathToDelete} - LastModifiedDate: {LastModifiedDate.ToString()}";
        }
    }
}
