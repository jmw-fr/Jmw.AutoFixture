// <copyright file="MyClassUnitTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Jmw.AutoFixture.Test
{
    using System.Linq;
    using global::AutoFixture;
    using Jmw.AutoFixtureUnitTest.Assets;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="MyClass" />.
    /// </summary>
    public class MyClassUnitTest
    {
        /// <summary>
        /// Function nominal test (i.e. normal functioning).
        /// </summary>
        [Fact]
        [Trait(nameof(MyClass.GetResultAsync), nameof(MyClass))]
        public async void GetResultAsync_Must_Nominal()
        {
            // Arrange
            var moq = new MyClassMocks(TestType.Nominal);
            var sut = moq.Fixture.Create<MyClass>();

            // Act
            var r = await sut.GetResultAsync(moq.Param);

            // Assert
            Assert.NotNull(r);
            Assert.Equal(moq.ExpectedResult, r);
            moq.VerifyAll(true);
        }

        /// <summary>
        /// Test if the function correctly thow exceptions.
        /// </summary>
        /// <param name="testType">Type of test.</param>
        /// <param name="expectedException">Type of expected exception.</param>
        [Theory]
        [Trait(nameof(MyClass.GetResultAsync), nameof(MyClass))]
        [InlineData(TestType.ThrowException, typeof(System.InvalidOperationException))]
        public async void GetResultAsync_Must_ThrowException(TestType testType, System.Type expectedException)
        {
            // Arrange
            var moq = new MyClassMocks(testType);
            var sut = moq.Fixture.Create<MyClass>();

            // Act
            var ex = await Assert.ThrowsAnyAsync<System.Exception>(async () => await sut.GetResultAsync(moq.Param));

            // Assert
            Assert.NotNull(ex);
            Assert.Equal(expectedException, ex.GetType());
        }

        /// <summary>
        /// Test that <c>CreateFixture</c>
        /// freezes the fixture.
        /// </summary>
        [Fact]
        [Trait("CreateFixture", nameof(MyClass))]
        public void Mocks_Must_Freeze()
        {
            // Arrange
            var moq = new MyClassMocks(TestType.Nominal);

            // Act
            var frozen = moq.CreateFixture<string>(true);
            var result = moq.CreateFixture<string>();

            // Act
            Assert.Same(frozen, result);

            // Assert
        }

        /// <summary>
        /// Test that <c>CreateFixture</c>
        /// freezes the fixture.
        /// </summary>
        [Fact]
        [Trait("CreateFixture", nameof(MyClass))]
        public void Mocks_Must_Freeze_WithBuild()
        {
            // Arrange
            var moq = new MyClassMocks(TestType.Nominal);

            // Act
            var frozen = moq.CreateFixture<MyData>(true, f => f.With(s => s.SomeProperty, "123"));
            var result = moq.CreateFixture<MyData>();

            // Assert
            Assert.Same(frozen, result);
        }

        /// <summary>
        /// Test that <c>CreateManyFixtures</c>
        /// correctly creates fixtures.
        /// </summary>
        [Fact]
        [Trait("CreateManyFixtures", nameof(MyClass))]
        public void Mocks_Must_CreateManyFixtures()
        {
            // Arrange
            var moq = new MyClassMocks(TestType.Nominal);

            // Act
            var result = moq.CreateManyFixtures<MyData>();

            // Assert
            Assert.NotEmpty(result);
        }

        /// <summary>
        /// Test that <c>CreateManyFixtures</c>
        /// correctly creates fixtures.
        /// </summary>
        [Fact]
        [Trait("CreateManyFixtures", nameof(MyClass))]
        public void Mocks_Must_CreateManyFixtures_Setup()
        {
            // Arrange
            var moq = new MyClassMocks(TestType.Nominal);

            // Act
            var result = moq.CreateManyFixtures<MyData>(f => f.With(d => d.SomeProperty, "123"));

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, r => Assert.Equal("123", r.SomeProperty));
        }

        /// <summary>
        /// Test that <c>CreateManyFixtures</c>
        /// correctly creates fixtures.
        /// </summary>
        [Fact]
        [Trait("CreateManyFixtures", nameof(MyClass))]
        public void Mocks_Must_CreateManyFixtures_WithCount()
        {
            // Arrange
            var moq = new MyClassMocks(TestType.Nominal);

            // Act
            var result = moq.CreateManyFixtures<MyData>(2);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        /// <summary>
        /// Test that <c>CreateManyFixtures</c>
        /// correctly creates fixtures.
        /// </summary>
        [Fact]
        [Trait("CreateManyFixtures", nameof(MyClass))]
        public void Mocks_Must_CreateManyFixtures_WithCountAndSetup()
        {
            // Arrange
            var moq = new MyClassMocks(TestType.Nominal);

            // Act
            var result = moq.CreateManyFixtures<MyData>(2, f => f.With(d => d.SomeProperty, "123"));

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.Equal("123", r.SomeProperty));
        }
    }
}
