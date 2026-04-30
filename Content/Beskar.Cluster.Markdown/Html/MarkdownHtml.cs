using Beskar.Cluster.Markdown.Enums;
using Beskar.Cluster.Markdown.Html.Default;
using Beskar.Cluster.Markdown.Html.Interfaces;
using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html;

public sealed class MarkdownHtml<TContext>
{
   public void Write(TContext context, ref MarkdownReader reader, ref TextWriterIndentSlim writer)
   {
      Clear();
      
      var inner = new MarkdownHtmlContext<TContext>
      {
         Context = context, 
         Html = this
      };
      
      while (reader.Read())
      {
         switch (reader.Current.Type)
         {
            case MarkdownTokenType.Heading1:
            case MarkdownTokenType.Heading2:
            case MarkdownTokenType.Heading3:
            case MarkdownTokenType.Heading4:
            case MarkdownTokenType.Heading5:
            case MarkdownTokenType.Heading6:
               HeaderWriter.Write(inner, ref reader, ref writer);
               break;
            case MarkdownTokenType.NewLine:
               NewLineWriter.Write(inner, ref reader, ref writer);
               break;
            case MarkdownTokenType.Bold:
               BoldWriter.Write(inner, ref reader, ref writer);
               break;
            case MarkdownTokenType.InlineCode:
               InlineCodeWriter.Write(inner, ref reader, ref writer);
               break;
            case MarkdownTokenType.Text:
               TextWriter.Write(inner, ref reader, ref writer);
               break;
            case MarkdownTokenType.HorizontalRule:
               HorizontalRuleWriter.Write(inner, ref reader, ref writer);
               break;
            case MarkdownTokenType.Strikethrough:
               StrikethroughWriter.Write(inner, ref reader, ref writer);
               break;
            case MarkdownTokenType.CodeBlock:
               CodeBlockWriter.Write(inner, ref reader, ref writer);
               break;
         }
      }
   }

   private void Clear()
   {
      HeaderWriter.Clear();
      NewLineWriter.Clear();
      BoldWriter.Clear();
      InlineCodeWriter.Clear();
      TextWriter.Clear();
      HorizontalRuleWriter.Clear();
      StrikethroughWriter.Clear();
      CodeBlockWriter.Clear();
   }
   
   public IMarkdownHtmlWriter<TContext> HeaderWriter { get; set; } = new HeaderWriter<TContext>();
   public IMarkdownHtmlWriter<TContext> NewLineWriter { get; set; } = new NewLineWriter<TContext>();
   public IMarkdownHtmlWriter<TContext> BoldWriter { get; set; } = new BoldWriter<TContext>();
   public IMarkdownHtmlWriter<TContext> InlineCodeWriter { get; set; } = new InlineCodeWriter<TContext>();
   public IMarkdownTextWriter<TContext> TextWriter { get; set; } = new TextWriter<TContext>();
   public IMarkdownHtmlWriter<TContext> HorizontalRuleWriter { get; set; } = new HorizontalRuleWriter<TContext>();
   public IMarkdownHtmlWriter<TContext> StrikethroughWriter { get; set; } = new StrikethroughWriter<TContext>();
   public IMarkdownHtmlWriter<TContext> CodeBlockWriter { get; set; } = new CodeBlockWriter<TContext>();
}