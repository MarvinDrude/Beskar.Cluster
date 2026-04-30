using System.Diagnostics;
using Beskar.Cluster.Markdown.Enums;
using Beskar.Cluster.Markdown.Html;
using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Models;

[DebuggerDisplay("{CreateDebugString(),nq}")]
public sealed class MarkdownLexResult
{
   public required string RawText { get; init; }
   
   public required MarkdownToken[] Tokens { get; init; }

   public void WriteHtml<TContext>(TContext context, ref TextWriterIndentSlim writer, MarkdownHtml<TContext>? host = null)
   {
      host ??= new MarkdownHtml<TContext>();

      var reader = new MarkdownReader(Tokens, RawText);
      host.Write(context, ref reader, ref writer);
   }

   public string CreateHtml<TContext>(TContext context, MarkdownHtml<TContext>? host = null)
   {
      var writer = new TextWriterIndentSlim(stackalloc char[512], stackalloc char[16]);
      try
      {
         WriteHtml(context, ref writer, host);
         return writer.ToString();
      }
      finally
      {
         writer.Dispose();
      }
   }
   
   public static MarkdownLexResult Create(string rawText)
   {
      var lexer = new MarkdownLexer(rawText, stackalloc MarkdownToken[128]);
      try
      {
         lexer.ScanTokens();
         
         return new MarkdownLexResult
         {
            RawText = rawText, 
            Tokens = lexer.WrittenTokens.ToArray()
         };
      }
      finally
      {
         lexer.Dispose();
      }
   }

   public string CreateDebugString()
   {
      var writer = new TextWriterIndentSlim(stackalloc char[512], stackalloc char[16]);
      try
      {
         var rawText = RawText.AsSpan();
         writer.WriteLineInterpolated($"TokenList ({Tokens.Length}): ");
         
         for (var index = 0; index < Tokens.Length; index++)
         {
            var token = Tokens[index];
            var slice = token.Type is MarkdownTokenType.NewLine
               ? "\\n"
               : rawText.Slice(token.ValuePostion.Index, token.ValuePostion.Length);
            
            writer.WriteLineInterpolated($"{index}: {token.Type} - '{slice}'");
         }
         
         return writer.ToString();
      }
      finally
      {
         writer.Dispose();
      }
   }
}