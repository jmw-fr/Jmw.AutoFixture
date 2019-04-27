// <copyright file="MyClassMocks.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Jmw.AutoFixture.Test
{
    using Moq;

    /// <summary>
    /// Type of test emulated.
    /// </summary>
    public enum TestType
    {
        /// <summary>
        /// Nominal functionning.
        /// </summary>
        Nominal,

        /// <summary>
        /// Throw an exception
        /// </summary>
        ThrowException,
    }

    /// <summary>
    /// Mocks for <see cref="MyClassUnitTest" />.
    /// </summary>
    public class MyClassMocks : Mocks<TestType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyClassMocks"/> class.
        /// </summary>
        /// <param name="typeTest">Type of test to emulate.</param>
        public MyClassMocks(TestType typeTest)
            : base(typeTest)
        {
            Param = CreateFixture<string>();
            ExpectedResult = CreateFixture<string>();

            switch (typeTest)
            {
                case TestType.Nominal:
                    {
                        CreateMock<IRepository>(true)
                            .Setup(m => m.GetSomeDataAsync(It.Is<string>(p => p == Param)))
                            .ReturnsAsync(ExpectedResult);

                        break;
                    }

                case TestType.ThrowException:
                    {
                        CreateMock<IRepository>(true)
                            .Setup(m => m.GetSomeDataAsync(It.Is<string>(p => p == Param)))
                            .Throws(new System.InvalidOperationException());

                        break;
                    }

                default:
                    {
                        throw new System.ArgumentException(nameof(typeTest));
                    }
            }
        }

        /// <summary>
        /// Gets some parameter.
        /// </summary>
        public string Param { get; }

        /// <summary>
        /// Gets some expected result.
        /// </summary>
        public string ExpectedResult { get; }
    }
}