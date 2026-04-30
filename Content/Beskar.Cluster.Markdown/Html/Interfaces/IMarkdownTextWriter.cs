namespace Beskar.Cluster.Markdown.Html.Interfaces;

public interface IMarkdownTextWriter<TContext> : IMarkdownHtmlWriter<TContext>
{
   public bool IsParagraphOpen { get; set; }
}