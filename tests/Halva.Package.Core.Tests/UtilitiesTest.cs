using System;
using System.IO;
using Xunit;

namespace Halva.Package.Core.Tests
{
    public class UtilitiesTest
    {
        [Fact]
        public void CompressionCheck()
        {
            bool finished = true;
            if (Directory.Exists("SampleFiles1")) Directory.Delete("SampleFiles1", true);
            try
            {
               PackageUtilities.CreateArchiveFromFolder("SampleFiles", "SampleFiles.halva");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                finished = false;
            }
            Assert.True(finished);
        }

        [Fact]
        public void DecompressionCheck()
        {
            bool finished = true;
            if (Directory.Exists("SampleFiles1")) Directory.Delete("SampleFiles1", true);
            try
            {
                PackageUtilities.ExportFromArchive("SampleFiles.halva", "SampleFiles1");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                finished = false;
            }
            Assert.True(finished);
        }
    }
}
