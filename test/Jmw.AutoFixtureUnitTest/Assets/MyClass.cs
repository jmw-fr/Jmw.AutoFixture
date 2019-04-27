// <copyright file="MyClass.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Jmw.AutoFixture.Test
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Sample class under testing.
    /// </summary>
    public class MyClass
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyClass"/> class.
        /// </summary>
        /// <param name="repository">Instance of the <see cref="IRepository" />.</param>
        public MyClass(IRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Some function to test.
        /// </summary>
        /// <param name="param">Some parameter</param>
        /// <returns>Some result.</returns>
        public async Task<string> GetResultAsync(string param)
        {
            return await repository.GetSomeDataAsync(param);
        }
    }
}
