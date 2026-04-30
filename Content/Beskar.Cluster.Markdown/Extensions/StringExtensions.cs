using System.Runtime.CompilerServices;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Extensions;

public static class StringExtensions
{
   extension(ReadOnlySpan<char> value)
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void WriteHtmlEncoded(ref TextWriterIndentSlim writer)
      {
         var lastIndex = 0;

         for (int i = 0; i < value.Length; i++)
         {
            var c = value[i];
            var entity = c switch
            {
               '<' => "&lt;",
               '>' => "&gt;",
               '&' => "&amp;",
               '"' => "&quot;",
               '\'' => "&#39;",
               _ => null
            };

            if (entity is not null)
            {
               if (i > lastIndex)
               {
                  writer.Write(value.Slice(lastIndex, i - lastIndex));
               }

               writer.Write(entity);
               lastIndex = i + 1;
            }
         }

         if (lastIndex < value.Length)
         {
            writer.Write(value[lastIndex..]);
         }
      }
   }
}