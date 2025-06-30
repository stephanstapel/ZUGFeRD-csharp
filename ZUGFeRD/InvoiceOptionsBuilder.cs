using System;
using System.Collections.Generic;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public sealed class InvoiceOptionsBuilder
    {
        private readonly InvoiceFormatOptions _options;

        private InvoiceOptionsBuilder()
        {
            _options = new InvoiceFormatOptions();
        } // !InvoiceOptionsBuilder()


        private InvoiceOptionsBuilder(InvoiceFormatOptions baseOptions)
        {
            _options = baseOptions?.Clone() ?? throw new ArgumentNullException(nameof(baseOptions));
        } // !InvoiceOptionsBuilder()


        public static InvoiceOptionsBuilder Create()
        {
            return new InvoiceOptionsBuilder();
        } // !Create()


        public static InvoiceOptionsBuilder From(InvoiceFormatOptions options)
        {
            return new InvoiceOptionsBuilder(options);
        } // !From()


        public static InvoiceOptionsBuilder CreateDefault()
        {
            return Create().UseRecommendedDefaults();
        } // !CreateDefault()


        public InvoiceFormatOptions Build()
        {
            return _options;
        } // !Build()


        public InvoiceOptionsBuilder EnableXmlComments(bool enable = true)
        {
            _options.IncludeXmlComments = enable;
            return this;
        } // !EnableXmlComments()


        public InvoiceOptionsBuilder AddHeaderXmlComment(List<string> comments)
        {
            _options.XmlHeaderComments.AddRange(comments);
            return this;
        } // !AddHeaderXmlComment()


        public InvoiceOptionsBuilder AddHeaderXmlComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return this;
            }
            _options.XmlHeaderComments.Add(comment);
            return this;
        } // !AddHeaderXmlComment()


        public InvoiceOptionsBuilder UseRecommendedDefaults()
        {
            return EnableXmlComments(false);
        } // !UseRecommendedDefaults()  
    }
}
