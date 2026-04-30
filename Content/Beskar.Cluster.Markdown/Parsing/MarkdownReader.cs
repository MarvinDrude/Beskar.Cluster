using System.Runtime.CompilerServices;
using Beskar.Cluster.Markdown.Models;

namespace Beskar.Cluster.Markdown.Parsing;

public ref struct MarkdownReader(
   ReadOnlySpan<MarkdownToken> tokens, 
   ReadOnlySpan<char> source)
{
   public MarkdownToken Current => _tokens[_index];
   public ReadOnlySpan<char> Value => _source.Slice(Current.ValuePostion.Index, Current.ValuePostion.Length);
   
   private int _index = -1;
   
   private readonly ReadOnlySpan<MarkdownToken> _tokens = tokens;
   private readonly ReadOnlySpan<char> _source = source;

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public bool Read()
   {
      if (_index + 1 >= _tokens.Length)
         return false;
      
      _index++;
      return true;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public MarkdownToken Peek()
   {
      return _index + 1 < _tokens.Length 
         ? _tokens[_index + 1] : default;
   }
}