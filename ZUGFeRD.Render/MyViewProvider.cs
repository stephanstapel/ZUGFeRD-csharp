
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace s2industries.ZUGFeRD.Render
{
    internal class MyViewProvider : IFileProvider
    {
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new System.NotImplementedException();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken Watch(string filter)
        {
            throw new System.NotImplementedException();
        }
    }
}
