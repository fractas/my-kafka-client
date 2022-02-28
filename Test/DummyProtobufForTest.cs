using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace MyKafkaClient.Test ;

    public class DummyProtobufForTest : IMessage<DummyProtobufForTest>
    {
        public void MergeFrom(DummyProtobufForTest dummyProtobufFor)
        {
            throw new NotImplementedException();
        }

        public void MergeFrom(CodedInputStream input)
        {
            throw new NotImplementedException();
        }

        public void WriteTo(CodedOutputStream output)
        {
            throw new NotImplementedException();
        }

        public int CalculateSize()
        {
            throw new NotImplementedException();
        }

        public MessageDescriptor Descriptor { get; }

        public bool Equals(DummyProtobufForTest? other)
        {
            throw new NotImplementedException();
        }

        public DummyProtobufForTest Clone()
        {
            throw new NotImplementedException();
        }
    }