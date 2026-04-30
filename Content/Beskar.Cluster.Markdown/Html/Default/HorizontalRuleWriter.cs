using Beskar.Cluster.Markdown.Html.Interfaces;
using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html.Default;

internal sealed class HorizontalRuleWriter<TContext> : IMarkdownHtmlWriter<TContext>
{
   public void Write(MarkdownHtmlContext<TContext> host, ref MarkdownReader reader, ref TextWriterIndentSlim writer)
   {
      writer.WriteLine("<hr />");
   }

   public void Clear()
   {
      
   }
}