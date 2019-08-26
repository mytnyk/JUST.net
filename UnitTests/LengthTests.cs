﻿using NUnit.Framework;
using System;

namespace JUST.UnitTests
{
    [TestFixture]
    public class LengthTests
    {
        [Test]
        public void LengthString()
        {
            const string transformer = "{ \"length\": \"#length(somestring)\" }";

            var result = JsonTransformer.Transform(transformer, ExampleInputs.NumbersArray);

            Assert.AreEqual("{\"length\":10}", result);
        }

        [Test]
        public void LengthArray()
        {
            const string transformer = "{ \"length\": \"#length(#valueof($.numbers))\" }";

            var result = JsonTransformer.Transform(transformer, ExampleInputs.NumbersArray);

            Assert.AreEqual("{\"length\":5}", result);
        }

        [Test]
        public void LengthNotEnumerableValue()
        {
            const string transformer = "{ \"length\": \"#length(#valueof($.numbers[0]))\" }";

            var result = JsonTransformer.Transform(transformer, ExampleInputs.NumbersArray);

            Assert.AreEqual("{\"length\":0}", result);
        }

        [Test]
        public void LengthNotEnumerableConst()
        {
            const string transformer = "{ \"length\": \"#length(#tointeger(1))\" }";

            var result = JsonTransformer.Transform(transformer, ExampleInputs.NumbersArray);

            Assert.AreEqual("{\"length\":0}", result);
        }

        [Test]
        public void LengthNotEnumerableValueStrict()
        {
            const string transformer = "{ \"length\": \"#length(#valueof($.numbers[0]))\" }";

            var result = Assert.Throws<Exception>(() => JsonTransformer.Transform(transformer, ExampleInputs.NumbersArray, new JUSTContext { EvaluationMode = EvaluationMode.Strict }));

            Assert.AreEqual("Error while calling function : #length(#valueof($.numbers[0])) - Argument not elegible for #length: 1", result.Message);
        }

        [Test]
        public void LengthNotEnumerableConstStrict()
        {
            const string transformer = "{ \"length\": \"#length(#todecimal(1.44))\" }";

            var result = Assert.Throws<Exception>(() => JsonTransformer.Transform(transformer, ExampleInputs.NumbersArray, new JUSTContext { EvaluationMode = EvaluationMode.Strict }));

            var decimalSeparator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            Assert.AreEqual($"Error while calling function : #length(#todecimal(1.44)) - Argument not elegible for #length: 1{decimalSeparator}44", result.Message);
        }
    }
}
