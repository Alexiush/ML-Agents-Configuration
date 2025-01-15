using MLAgents.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using VYaml.Annotations;
using VYaml.Serialization;

namespace MLAgents.Configuration
{
    public static class ConfigCreator
    {
        public static void CreateConfig(Config configuration, string path)
        {
            var options = YamlSerializerOptions.Standard;
            options.NamingConvention = NamingConvention.SnakeCase;

            options.Resolver = CompositeResolver.Create(
                AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.GetInterfaces().Contains(typeof(IYamlFormatter)))
                    .Where(t => t.IsDefined(typeof(DefaultConfigurationFormatterAttribute), false))
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Select(t => Activator.CreateInstance(t))
                    .OfType<IYamlFormatter>()
                    .ToArray(),
                new IYamlFormatterResolver[] {
                    StandardResolver.Instance,
                }
            );

            var config = YamlSerializer.SerializeToString(configuration, options);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using (FileStream dataStream = new FileStream(path, FileMode.Create))
                {
                    dataStream.Write(Encoding.UTF8.GetBytes(config));
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
