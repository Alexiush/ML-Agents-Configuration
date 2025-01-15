using System;
using VYaml.Emitter;
using VYaml.Parser;
using VYaml.Serialization;

namespace MLAgents.Utilities
{
    public interface IDynamicallyYAMLSerialized
    {
        public void Serialize(ref Utf8YamlEmitter emitter, YamlSerializationContext context);
    }

    public abstract class WriteOnlyYamlFormatter<T> : IYamlFormatter<T>
    {
        public abstract void Serialize(ref Utf8YamlEmitter emitter, T value, YamlSerializationContext context);

        public T Deserialize(ref YamlParser parser, YamlDeserializationContext context)
        {
            throw new NotImplementedException("This formatter is write-only");
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultConfigurationFormatterAttribute : System.Attribute { }
}
