// <copyright file="AutoFixtureExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Jmw.AutoFixture
{
    using System;
    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    /// <summary>
    /// Some extensions for AutoFixture.
    /// </summary>
    public static class AutoFixtureExtensions
    {
        /// <summary>
        /// Registers a class as the interface type.
        /// </summary>
        /// <typeparam name="TInterface">Type of the interface.</typeparam>
        /// <typeparam name="TClass">Type of the class implementing the class.</typeparam>
        /// <param name="fixture">Instance of Autofixture.</param>
        /// <returns>The instance of Autofixture configured.</returns>
        public static Fixture RegisterInterface<TInterface, TClass>(this Fixture fixture)
            where TInterface : class
            where TClass : class
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(TInterface),
                    typeof(TClass)));

            return fixture;
        }
    }
}
