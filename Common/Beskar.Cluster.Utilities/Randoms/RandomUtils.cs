using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Utilities.Randoms;

public static class RandomUtils
{
   private const string DefaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
   
   public static string GenerateRandomBase64String(int length)
   {
      using var owner = length < 512
         ? new SpanOwner<byte>(stackalloc byte[length])
         : new SpanOwner<byte>(length);
      
      var span = owner.Span;
      RandomNumberGenerator.Fill(span);
      
      return Convert.ToBase64String(span);
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static byte[] GenerateRandomBytes(int length)
   {
      var bytes = new byte[length];
      RandomNumberGenerator.Fill(bytes);
      
      return bytes;
   }

   public static string GenerateRandomString(int length, string alphabet = DefaultAlphabet)
   {
      return string.Create(length, (alphabet, length), static (span, state) =>
      {
         var (alpha, len) = state;
         
         for (var i = 0; i < len; i++)
         {
            var index = RandomNumberGenerator.GetInt32(alpha.Length);
            span[i] = alpha[index];
         }
      });
   }
}