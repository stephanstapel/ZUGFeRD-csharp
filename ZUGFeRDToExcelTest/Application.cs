using s2industries.ZUGFeRD;
using System;
using System.IO;

namespace ZUGFeRDToExcel
{
    internal class Application
    {
        internal void Run(Options options)
        {            
            if (options.InputFile.Contains("*"))
            {
                string baseDirectory = System.IO.Path.GetDirectoryName(options.InputFile);
                string searchPattern = System.IO.Path.GetFileName(options.InputFile);
                foreach (string _inputPath in System.IO.Directory.GetFiles(baseDirectory, searchPattern, options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                {
                    string _outputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_inputPath), String.Format("{0}.{1}", System.IO.Path.GetFileNameWithoutExtension(_inputPath), "xlsx"));
                    InvoiceConverter.ConvertZUGFeRDToExcel(_inputPath, _outputPath);
                }
            }
            else
            {
                string _outputFile = options.OutputFile;
                if (String.IsNullOrEmpty(_outputFile))
                {
                    _outputFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(options.InputFile), String.Format("{0}.{1}", System.IO.Path.GetFileNameWithoutExtension(options.InputFile), "xlsx"));
                }

                InvoiceConverter.ConvertZUGFeRDToExcel(options.InputFile, _outputFile);
            }
        }        
    }
}