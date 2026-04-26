using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using Beskar.CodeGeneration.PacketGenerator.Marker.Common;
using Me.Memory.Buffers;
using Me.Memory.Extensions;
using MessagePack;

namespace Beskar.Cluster.Sockets.Registries;

public abstract class MessagePackPacketRegistry<TState>(
   MessagePackSerializerOptions? options = null,
   PacketRegistryOptions? registryOptions = null)
   : BasePacketRegistry<TState>(registryOptions)
{
   private readonly MessagePackSerializerOptions _options = options ?? _defaultOptions;

   public override void Serialize<T>(ref BufferWriter<byte> writer, T packet)
   {
      var arrayWriter = new ArrayBufferWriter<byte>(1024); // double heap is ok for now
      
      MessagePackSerializer.Serialize(arrayWriter, packet, _options);

      Span<byte> lengthBytes = stackalloc byte[sizeof(int)];
      arrayWriter.WrittenCount.WriteLittleEndian(lengthBytes);
      
      writer.Write(lengthBytes);
      writer.Write(arrayWriter.WrittenSpan);
   }

   public override bool TryDeserialize<T>(ref SequenceReader<byte> reader, [MaybeNullWhen(false)] out T packet)
   {
      try
      {
         if (!reader.TryReadLittleEndian(out int length))
         {
            packet = default;
            return false;
         }
         
         if (reader.Remaining < length)
         {
            packet = default;
            reader.Rewind(sizeof(int));
            return false;
         }
         
         var packetSlice = reader.UnreadSequence.Slice(0, length);
         packet = MessagePackSerializer.Deserialize<T>(packetSlice, _options);

         reader.Advance(length);
         return true;
      }
      catch (Exception)
      {
         packet = default;
         return false;
      }
   }

   private static readonly MessagePackSerializerOptions _defaultOptions = MessagePackSerializerOptions.Standard;
}