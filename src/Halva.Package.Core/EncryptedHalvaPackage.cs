﻿using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Halva.Package.Core
{
    public class EncryptedHalvaPackage : HalvaPackageBase
    {
        private string Password { get; set; }

        /// <summary>
        /// Creates an encrypted Halva package in memory. This is used to update an archive or export some files.
        /// </summary>
        /// <param name="source">The archive you want to open.</param>
        /// <param name="pwsd">The password of the archive (if it's encrypted).</param>
        public EncryptedHalvaPackage(string source, string pwsd)
        {
            Password = pwsd;
            SourceLocation = new StringBuilder(Path.GetDirectoryName(source));
            DestinationLocation = new StringBuilder(source);
            EncryptedPackageUtilities.DecompressArchive(DestinationLocation.ToString(), Password);
            ArchiveMemoryStream = ZipFile.Open(PackageUtilities.TempArchive, ZipArchiveMode.Update);
            PullFiles(ArchiveMemoryStream);
        }

        /// <summary>
        /// Creates an encrypted Halva package using the source folder as the input. It will automatically put the files in the input folder to a temporary archive.
        /// </summary>
        /// <param name="source">The folders that will work as source.</param>
        /// <param name="destination">The location of the final archive.</param>
        /// <param name="pwsd">The password for the encrypted archive.</param>
        public EncryptedHalvaPackage(string source, string destination, string pwsd)
        {
            SourceLocation = new StringBuilder(source);
            DestinationLocation = new StringBuilder(destination);
            Password = pwsd;
            List<string> foundFilesList = PullFiles(source);
            ArchiveMemoryStream = ZipFile.Open(PackageUtilities.TempArchive, ZipArchiveMode.Create);
            foreach (string file in foundFilesList)
            {
                AddFileToList(file);
            }
        }

        /// <summary>
        /// Saves current changes to the destination archive.
        /// </summary>
        public void Save()
        {
            CloseArchive();
            ReloadArchive();
        }

        /// <summary>
        /// Closes the archive.
        /// </summary>
        public void CloseArchive()
        {
            ArchiveMemoryStream.Dispose();
            EncryptedPackageUtilities.CompressArchive(PackageUtilities.TempArchive, DestinationLocation.ToString(), Password);
        }

        /// <summary>
        /// Reloads the archive for editing.
        /// </summary>
        public void ReloadArchive()
        {
            EncryptedPackageUtilities.DecompressArchive(DestinationLocation.ToString(), Password);
            ArchiveMemoryStream = ZipFile.Open(PackageUtilities.TempArchive, ZipArchiveMode.Update);
        }


    }
}