using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBlockUpload.Tests
{
    [TestClass]
    public class SettingsExtensionTests
    {
        [TestMethod]
        public void ToKeyValuePairs_Should_Create_A_Dictionary_With_No_KeyPrefix()
        {
            var sample = new SampleClass1
            {
                IntProp = 1234,
                FloatProp = 1234.56f,
                StringProp = "Prop"
            };

            var result = sample.ToKeyValuePairs();

            Assert.AreEqual(sample.IntProp, result[nameof(sample.IntProp)]);
            Assert.AreEqual(sample.FloatProp, result[nameof(sample.FloatProp)]);
            Assert.AreEqual(sample.StringProp, result[nameof(sample.StringProp)]);
        }

        [TestMethod]
        public void ToKeyValuePairs_Should_Create_A_Dictionary_With_KeyPrefix()
        {
            var prefix = $"{nameof(SampleClass1)}.";
            var sample = new SampleClass1
            {
                IntProp = 1234,
                FloatProp = 1234.56f,
                StringProp = "Prop"
            };

            var result = sample.ToKeyValuePairs(prefix);

            Assert.AreEqual(sample.IntProp, result[$"{prefix}{nameof(sample.IntProp)}"]);
            Assert.AreEqual(sample.FloatProp, result[$"{prefix}{nameof(sample.FloatProp)}"]);
            Assert.AreEqual(sample.StringProp, result[$"{prefix}{nameof(sample.StringProp)}"]);
        }

        [TestMethod]
        public void FromKeyValuePairs_Should_Create_Instance_Of_Settings()
        {
            var prefix = $"{nameof(SampleClass1)}.";
            var keyValuePairs = new Dictionary<string, object>
            {
                { $"{prefix}{nameof(SampleClass1.IntProp)}",  1234 },
                { $"{prefix}{nameof(SampleClass1.FloatProp)}",  1234.56f },
                { $"{prefix}{nameof(SampleClass1.StringProp)}",  "my prop" }
            };

            var result = keyValuePairs.FromKeyValuePairs<SampleClass1>(prefix);

            Assert.AreEqual(keyValuePairs[$"{prefix}{nameof(SampleClass1.IntProp)}"], result.IntProp);
            Assert.AreEqual(keyValuePairs[$"{prefix}{nameof(SampleClass1.FloatProp)}"], result.FloatProp);
            Assert.AreEqual(keyValuePairs[$"{prefix}{nameof(SampleClass1.StringProp)}"], result.StringProp);
        }


        private class SampleClass1
        {
            public int IntProp { get; set; }

            public float FloatProp { get; set; }

            public string StringProp { get; set; }
        }
    }
}
