using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html.Interfaces;

public interface IMarkdownHtmlWriter<TContext>
{
   public void Write(MarkdownHtmlContext<TContext> host, ref MarkdownReader reader, ref TextWriterIndentSlim writer);

   public void Clear();
}