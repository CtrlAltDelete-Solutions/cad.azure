using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD.Azure
{
    public interface IStorage
    {
        string DownloadToTemp(string container, string azureFile);
        void Download(string container, string azureFile, string targetFile);
        void Upload(string container, string localFile, string targetFile, string contentType = null);
    }
}
