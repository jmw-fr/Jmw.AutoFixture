// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Jmw.AutoFixture.Test
{
    using System.Threading.Tasks;

    /// <summary>
    /// Some repository.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Some sample function to be mmocked.
        /// </summary>
        /// <param name="param">Some parameter.</param>
        /// <returns>Some result from mock.</returns>
        Task<string> GetSomeDataAsync(string param);
    }
}
