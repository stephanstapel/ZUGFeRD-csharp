using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s2industries.ZUGFeRDRenderer
{
    class RazorTemplateManager : ITemplateManager
    {
        public ITemplateSource Resolve(ITemplateKey key)
        {
            // Resolve your template here (ie read from disk)
            // if the same templates are often read from disk you propably want to do some caching here.
            string template = "Hello @Model.Name, welcome to RazorEngine!";
            // Provide a non-null file to improve debugging
            return new LoadedTemplateSource(template, null);
        }

        public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            // If you can have different templates with the same name depending on the 
            // context or the resolveType you need your own implementation here!
            // Otherwise you can just use NameOnlyTemplateKey.
            return new NameOnlyTemplateKey(name, resolveType, context);
            // template is specified by full path
            //return new FullPathTemplateKey(name, fullPath, resolveType, context);
        }

        public void AddDynamic(ITemplateKey key, ITemplateSource source)
        {
            // You can disable dynamic templates completely. 
            // This just means all convenience methods (Compile and RunCompile) with
            // a TemplateSource will no longer work (they are not really needed anyway).
            throw new NotImplementedException("dynamic templates are not supported!");
        }
    }
}
