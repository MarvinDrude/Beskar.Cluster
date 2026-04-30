using Beskar.Cluster.Markdown.Html.Interfaces;
using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html.Default;

internal sealed class BoldWriter<TContext> : IMarkdownHtmlWriter<TContext>
{
   private bool _isOpen;

   public void Write(MarkdownHtmlContext<TContext> host, ref MarkdownReader reader, ref TextWriterIndentSlim writer)
   {
      writer.Write(_isOpen ? "</strong>" : "<strong>");
      _isOpen = !_isOpen;
   }
   
   public void Clear() => _isOpen = false;
}