using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html;

public sealed class MarkdownHtml<TContext>
{
   public void Write(TContext context, ref MarkdownReader reader, ref TextWriterIndentSlim writer)
   {
      var inner = new MarkdownHtmlContext<TContext>
      {
         Context = context, 
         Html = this
      };
      
      while (reader.Read())
      {
         
      }
   }
}