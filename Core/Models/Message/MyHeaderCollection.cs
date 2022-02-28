using System.Collections;

namespace MyKafkaClient.Core.Models.Message ;

    public interface IMyHeaderCollection : IEnumerable<KeyValuePair<string, byte[]>>
    {
        byte[] this[string key] { get; set; }
    }

    public sealed class MyHeaderCollection : IMyHeaderCollection
    {
        private readonly IDictionary<string, byte[]> _headers;

        public MyHeaderCollection() : this(new Dictionary<string, byte[]>())
        {
        }

        public MyHeaderCollection(IDictionary<string, byte[]> headers) : this(headers.ToArray())
        {
        }

        public MyHeaderCollection(IEnumerable<KeyValuePair<string, byte[]>> headers) : this(headers.ToArray())
        {
        }

        public MyHeaderCollection(params KeyValuePair<string, byte[]>[] headers)
        {
            ArgumentNullException.ThrowIfNull(headers);

            _headers = new Dictionary<string, byte[]>(headers, StringComparer.OrdinalIgnoreCase);
        }

        public byte[] this[string key]
        {
            get => Get(key);
            set => Merge(key, value);
        }

        public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private byte[] Get(string key)
        {
            return _headers.TryGetValue(key, out var value) ? value : Array.Empty<byte>();
        }

        private void Merge(string key, byte[] value)
        {
            if (!_headers.TryAdd(key, value))
                _headers[key] = value;
        }
    }