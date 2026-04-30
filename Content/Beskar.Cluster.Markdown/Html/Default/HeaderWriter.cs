using Beskar.Cluster.Markdown.Enums;
using Beskar.Cluster.Markdown.Html.Interfaces;
using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html.Default;

internal sealed class HeaderWriter<TContext> : IMarkdownHtmlWriter<TContext>
{
   public void Write(MarkdownHtmlContext<TContext> host, ref MarkdownReader reader, ref TextWriterIndentSlim writer)
   {
      var headerLevel = reader.Current.Type - MarkdownTokenType.Heading1 + 1;
      writer.WriteInterpolated($"<h{headerLevel}>");

      if (reader.Peek().Type is MarkdownTokenType.Text
          && reader.Read())
      {
         writer.Write(reader.Value);
      }
      
      writer.WriteLineInterpolated($"</h{headerLevel}>");
   }
   
   public void Clear()
   {
      
   }
}