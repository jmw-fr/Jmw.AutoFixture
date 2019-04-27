// <copyright file="MyClassUnitTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Jmw.AutoFixture.Test
{
    using global::AutoFixture;
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
    }
}
