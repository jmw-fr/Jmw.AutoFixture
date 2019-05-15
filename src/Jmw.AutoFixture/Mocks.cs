// <copyright file="Mocks.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Jmw.AutoFixture
{
    using System.Collections.Generic;
    using System.Reflection;
    using global::AutoFixture;
    using global::AutoFixture.Dsl;
    using Moq;

    /// <summary>
    /// Helper class to create mocks and fixtures inside a "mock" class.
    /// </summary>
    /// <typeparam name="TEnumTestType">Test type</typeparam>
    public abstract class Mocks<TEnumTestType>
        where TEnumTestType : System.Enum
    {
        private Dictionary<System.Type, Mock> mocks;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mocks{TEnumTestType}"/> class.
        /// </summary>
        /// <param name="typeTest">Type of test emulated.</param>
        public Mocks(TEnumTestType typeTest)
        {
            TypeTest = typeTest;

            mocks = new Dictionary<System.Type, Mock>();

            Fixture = new Fixture();
        }

        /// <summary>
        ///  Gets the AutoFixture instance.
        /// </summary>
        public Fixture Fixture { get; }

        /// <summary>
        /// Gets the type of test emulated.
        /// </summary>
        public TEnumTestType TypeTest { get; }

        /// <summary>
        /// Call <see cref="Mock.VerifyAll()"/> for the setup mocks.
        /// </summary>
        /// <param name="verifyNoOtherCalls">Also call <see cref="Mock{T}.VerifyNoOtherCalls()"/></param>
        public void VerifyAll(bool verifyNoOtherCalls)
        {
            foreach (var m in mocks)
            {
                m.Value.VerifyAll();
                if (verifyNoOtherCalls)
                {
                    m.Value.GetType().GetMethod("VerifyNoOtherCalls").Invoke(m.Value, null);
                }
            }
        }

        /// <summary>
        /// Returns the instance of the mocked object or <c>null</c> if the object is not mocked.
        /// </summary>
        /// <typeparam name="T">Type of the mocked instance.</typeparam>
        /// <returns>Mocked instance or <c>null</c>.</returns>
        public T GetMock<T>()
            where T : class
        {
            if (mocks.ContainsKey(typeof(Mock<T>)))
            {
                return ((Mock<T>)mocks[typeof(T)]).Object;
            }

            return null;
        }

        /// <summary>
        /// Creates a new fixture using AutoFixture.
        /// </summary>
        /// <param name="setupAction">Setup action of the fixture.</param>
        /// <typeparam name="T">Type of the fixture to create.</typeparam>
        /// <returns>Created fixture.</returns>
        public T CreateFixture<T>(System.Func<IPostprocessComposer<T>, IPostprocessComposer<T>> setupAction = null)
        {
            return CreateFixture(false, setupAction);
        }

        /// <summary>
        /// Creates a new fixture using AutoFixture.
        /// </summary>
        /// <param name="freeze">Create a freezed fixture.</param>
        /// <param name="setupAction">Setup action of the fixture.</param>
        /// <typeparam name="T">Type of the fixture to create.</typeparam>
        /// <returns>Created fixture.</returns>
        public T CreateFixture<T>(bool freeze, System.Func<IPostprocessComposer<T>, IPostprocessComposer<T>> setupAction = null)
        {
            if (freeze)
            {
                if (setupAction != null)
                {
                    return Fixture.Freeze<T>(f => setupAction.Invoke(f));
                }
                else
                {
                    return Fixture.Freeze<T>();
                }
            }
            else
            {
                if (setupAction != null)
                {
                    IPostprocessComposer<T> b = Fixture.Build<T>();

                    b = setupAction?.Invoke(b);

                    return b.Create();
                }
                else
                {
                    return Fixture.Create<T>();
                }
            }
        }

        /// <summary>
        /// Setup a Moq instance.
        /// </summary>
        /// <typeparam name="T">Mocked type.</typeparam>
        /// <param name="strict">If <c>true</c>, setup the strict mode of Moq.</param>
        /// <returns>Instance of <see cref="Mock{T}"/>.</returns>
        /// <remarks>
        /// <para>I don't rely on Autofixture.AutoMoq to setup the Mock instance.</para>
        /// <para>I prefer to create the Mock with the strict behavior and inject it inside AutoFixture.</para>
        /// <para>AutoFixture.AutoMoq behavior is by default to return a mocked instance instead of <c>null</c> which is not the best way for testing (as far as I'm concerned).</para>
        /// </remarks>
        protected Mock<T> CreateMock<T>(bool strict)
            where T : class
        {
            if (mocks.ContainsKey(typeof(Mock<T>)))
            {
                return (Mock<T>)mocks[typeof(T)];
            }

            var mock = new Mock<T>(strict ? MockBehavior.Strict : MockBehavior.Default);
            Fixture.Inject(mock.Object);

            mocks.Add(typeof(T), mock);

            return mock;
        }

        /// <summary>
        /// Setup a Moq instance and calls setup Actions.
        /// </summary>
        /// <typeparam name="T">Mocked type.</typeparam>
        /// <param name="strict">If <c>true</c>, setup the strict mode of Moq.</param>
        /// <param name="setupActions">List of actions to execute after mock creation.</param>
        /// <returns>Instance of <see cref="Mock{T}"/>.</returns>
        protected Mock<T> CreateMock<T>(bool strict, params System.Action<Mock<T>>[] setupActions)
            where T : class
        {
            var mock = CreateMock<T>(true);

            foreach (var action in setupActions)
            {
                action?.Invoke(mock);
            }

            return mock;
        }
    }
}
