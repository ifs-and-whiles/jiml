using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Object.Property
{
    public abstract class PropertyComposition
    {
        public JToken Value { get; }
        public string Name { get; }
        public bool IsSuccess { get; }
        public Jiml.Error[] Errors { get; }


        protected PropertyComposition(
            string name, 
            JToken value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            IsSuccess = true;
        }

        protected PropertyComposition(
            string name,
            params Jiml.Error[] errors)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            if (errors == null) throw new ArgumentNullException(nameof(errors));
            Errors = errors.ToArray();
        }

        public class Failure: PropertyComposition
        {
            public Failure(
                string name,
                params Jiml.Error[] errors) : base(
                name, 
                errors)
            {
            }
        }

        public class Success : PropertyComposition
        {
            public Success(
                string name,
                JToken value) : base(
                name,
                value)
            {
            }
        }
    }
}