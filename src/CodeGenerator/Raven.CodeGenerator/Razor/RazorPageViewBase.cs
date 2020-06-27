using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Raven.CodeGenerator.Razor
{
    public abstract class RazorPageViewBase<TModel>
    {
        protected TModel Model;

        public async Task ExecuteViewAsync(TModel model)
        {
            this.Model = model;
            Output = new StringBuilder();

            await ExecuteAsync();

            _isCanWrite = true;

            await WriteStream();
        }

        private static readonly Encoding UTF8NoBOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        protected StringBuilder Output { get; private set; }

        private volatile bool _isCanWrite = false;

        public Stream Stream { get; } = new MemoryStream();
        protected HtmlEncoder HtmlEncoder { get; set; } = HtmlEncoder.Default;

        private async Task WriteStream()
        {
            if (_isCanWrite)
            {
                using (var writer = new StreamWriter(Stream, UTF8NoBOM, 4096, leaveOpen: true))
                {
                    await writer.WriteAsync(Output.ToString());
                }
            }
        }

        public abstract Task ExecuteAsync();

        protected string Raw(string value)
        {
            return value + Environment.NewLine;
        }

        protected void WriteLiteral(string value)
        {
            WriteLiteralTo(Output, value);
        }

        protected void WriteLiteral(object value)
        {
            WriteLiteralTo(Output, value);
        }

        private string AttributeEnding { get; set; }

        protected void BeginWriteAttribute(string name, string begining, int startPosition, string ending, int endPosition, int thingy)
        {
            Output.Append(begining);
            AttributeEnding = ending;
        }


        protected void Write(object value)
        {
            WriteTo(Output, value);
        }

        protected void Write(string value)
        {
            WriteTo(Output, value);
        }


        protected void WriteTo(StringBuilder writer, object value)
        {
            if (value != null)
            {
                WriteTo(writer, Convert.ToString(value, CultureInfo.InvariantCulture));
            }
        }

        protected void WriteTo(StringBuilder writer, string value)
        {
            if (value == null)
            {
                return;
            }

            WriteLiteralTo(writer, value);
        }

        protected void WriteLiteralTo(StringBuilder writer, object value)
        {
            if (value == null)
            {
                return;
            }

            WriteLiteralTo(writer, Convert.ToString(value, CultureInfo.InvariantCulture));
        }


        protected void WriteLiteralTo(StringBuilder writer, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.Append(value);
            }
        }
    }
}
