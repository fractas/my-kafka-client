using FluentAssertions;

using MyKafkaClient.Core.Models;

using NUnit.Framework;

namespace KafkaClient.Test.UnitTest.Models;

[TestFixture]
public class MyHeaderCollectionTest
{
  [Test]
  public void Should_Not_Require_Any_Arguments()
  {
    MyHeaderCollection headers = new();

    headers.Should().BeEmpty();
  }

  [Test]
  public void Should_Add_Items_With_Index_Property_Only()
  {
    MyHeaderCollection headers = new()
    {
      ["key1"] = Array.Empty<byte>(),
      ["key2"] = Array.Empty<byte>(),
      ["key3"] = Array.Empty<byte>()
    };

    headers.Should().HaveCount(3);

    typeof(MyHeaderCollection).Should().NotHaveMethod("Add", new[] { typeof(string), typeof(byte[]) });
    typeof(MyHeaderCollection).Should().NotHaveMethod("Add", new[] { typeof(KeyValuePair<string, byte[]>) });
    typeof(MyHeaderCollection).Should().NotHaveMethod("Add", new[] { typeof(KeyValuePair<string, byte[]>[]) });
  }

  [Test]
  public void Should_Receive_Dictionary_Of_String_And_ByteArray()
  {
    Dictionary<string, byte[]> dictionaryOfStringAndByteArray = new()
    {
      { "key1", Guid.NewGuid().ToByteArray() },
      { "key2", Guid.NewGuid().ToByteArray() },
      { "key3", Guid.NewGuid().ToByteArray() }
    };

    MyHeaderCollection headers = new(dictionaryOfStringAndByteArray);

    headers.Should().NotBeEmpty();
    headers.Should().HaveCount(3);
  }

  [Test]
  public void Should_Receive_List_Of_KeyValuePairs_Of_String_And_ByteArray()
  {
    List<KeyValuePair<string, byte[]>> listOfKeyValuePairsOfStringAndByteArray = new()
    {
      new KeyValuePair<string, byte[]>("key1", Guid.NewGuid().ToByteArray()),
      new KeyValuePair<string, byte[]>("key2", Guid.NewGuid().ToByteArray()),
      new KeyValuePair<string, byte[]>("key3", Guid.NewGuid().ToByteArray())
    };

    MyHeaderCollection headers = new(listOfKeyValuePairsOfStringAndByteArray);

    headers.Should().NotBeEmpty();
    headers.Should().HaveCount(3);
  }

  [Test]
  public void Should_Receive_Array_Of_KeyValuePairs_Of_String_And_ByteArray()
  {
    KeyValuePair<string, byte[]>[] arrayOfKeyValuePairsOfStringAndByteArray =
    {
      new("key1", Guid.NewGuid().ToByteArray()), new("key2", Guid.NewGuid().ToByteArray()),
      new("key3", Guid.NewGuid().ToByteArray())
    };

    MyHeaderCollection headers = new(arrayOfKeyValuePairsOfStringAndByteArray);

    headers.Should().NotBeEmpty();
    headers.Should().HaveCount(3);
  }
}
