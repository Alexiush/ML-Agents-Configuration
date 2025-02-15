using MLAgents.Utilities;
using System.Collections.Generic;
using UnityEngine;
using VYaml.Annotations;
using VYaml.Emitter;
using VYaml.Serialization;

namespace MLAgents.Configuration
{
    public enum Measure
    {
        reward = 0,
        Elo = 1
    }

    [YamlObject]
    [System.Serializable]
    public partial class CompletionCriteria
    {
        public Measure Measure = Measure.reward;
        public BehaviorName Behavior;
        public float Threshold;
        public int MinLessonLength;
        public bool SignalSmoothing = false;
        public bool RequireReset = false;
    }

    [YamlObject]
    [System.Serializable]
    [YamlObjectUnion("!float", typeof(FloatValue))]
    [YamlObjectUnion("!sample", typeof(SampledValue))]
    public abstract partial class LessonValue : IDynamicallyYAMLSerialized
    {
        public abstract void Serialize(ref Utf8YamlEmitter emitter, YamlSerializationContext context);
    }

    [DefaultConfigurationFormatter]
    public class LessonValueFormatter : WriteOnlyYamlFormatter<LessonValue>
    {
        public override void Serialize(ref Utf8YamlEmitter emitter, LessonValue value, YamlSerializationContext context)
        {
            value.Serialize(ref emitter, context);
        }
    }

    [YamlObject]
    [System.Serializable]
    public partial class FloatValue : LessonValue
    {
        public float Value;

        public override void Serialize(ref Utf8YamlEmitter emitter, YamlSerializationContext context)
        {
            context.Serialize(ref emitter, Value);
        }
    }

    [YamlObject]
    [System.Serializable]
    public partial class SampledValue : LessonValue
    {
        [SubclassSelector, SerializeReference]
        public Sampler Value;

        public override void Serialize(ref Utf8YamlEmitter emitter, YamlSerializationContext context)
        {
            context.Serialize(ref emitter, Value);
        }
    }

    [YamlObject]
    [System.Serializable]
    public partial class Lesson
    {
        public string Name;
        public CompletionCriteria CompletionCriteria;
        [SubclassSelector, SerializeReference]
        public LessonValue Value;
    }

    [YamlObject]
    [System.Serializable]
    public partial class Curriculum
    {
        public List<Lesson> Lessons;
    }

    [YamlObject]
    [System.Serializable]
    public partial class CurriculumParameter : EnvironmentParameter
    {
        public Curriculum Curriculum;

        public override void Serialize(ref Utf8YamlEmitter emitter, YamlSerializationContext context)
        {
            emitter.WriteString(Name);
            emitter.BeginMapping();
            {
                emitter.WriteString("curriculum");
                emitter.BeginSequence();
                {
                    foreach (var lesson in Curriculum.Lessons)
                    {
                        context.Serialize(ref emitter, lesson);
                    }
                }
                emitter.EndSequence();
            }
            emitter.EndMapping();
        }
    }
}
