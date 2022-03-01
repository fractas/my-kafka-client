using System.Collections;
using System.Text;

using FluentAssertions;

using MyKafkaClient.Core.Models.Message;

using NUnit.Framework;

namespace MyKafkaClient.Test.Models;

public class MyMessageTests
{
    [Test]
    public void MyHeaderCollection_ShouldInitializeCollection()
    {
        MyHeaderCollection collection1 = new();

        collection1.Should().NotBeNull();
        collection1.Should().BeEmpty();

        Dictionary<string, byte[]> someDictionary = new()
        {
            { "x-header-1", Encoding.UTF8.GetBytes("x-header-1") },
            { "x-header-2", Encoding.UTF8.GetBytes("x-header-2") },
            { "x-header-3", Encoding.UTF8.GetBytes("x-header-3") }
        };

        MyHeaderCollection collection2 = new(someDictionary);

        collection2.Should().NotBeEmpty();
        collection2.Should().HaveCount(3);
        collection2["x-header-2"].Should().BeSameAs(someDictionary["x-header-2"]);

        someDictionary.Add("x-header-4", Encoding.UTF8.GetBytes("x-header-4"));

        MyHeaderCollection collection3 = new(someDictionary.ToList());

        collection3.Should().NotBeEmpty();
        collection3.Should().HaveCount(4);
        collection3["x-header-4"].Should().BeSameAs(someDictionary["x-header-4"]);

        someDictionary.Add("x-header-5", Encoding.UTF8.GetBytes("x-header-5"));

        MyHeaderCollection collection4 = new(someDictionary.ToArray());

        collection4.Should().NotBeEmpty();
        collection4.Should().HaveCount(5);
        collection4["x-header-5"].Should().BeSameAs(someDictionary["x-header-5"]);

        IEnumerator enumerator = collection4.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, byte[]> item = enumerator.Current.As<KeyValuePair<string, byte[]>>();
            item.Key.Should().StartWith("x-header");
        }

        collection4["x-header-5"] = someDictionary["x-header-4"];
        collection4["x-header-5"].Should().BeSameAs(collection4["x-header-4"]);
    }

    [Test]
    public void MyTimestamp_ShouldHaveUtcDateTime()
    {
        MyTimestamp timestamp1 = new();
        timestamp1.UtcDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10));

        DateTime reference = DateTime.UtcNow;
        MyTimestamp timestamp2 = new(reference);
        timestamp2.UtcDateTime.Should().BeSameDateAs(reference);
    }
}