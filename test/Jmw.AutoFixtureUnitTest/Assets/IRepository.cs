// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Jmw.AutoFixture.Test
{
    using System;
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

        /// <summary>
        /// Update data sample function.
        /// </summary>
        /// <param name="id">Some Id.</param>
        /// <param name="data">Some data.</param>
        /// <returns>Task.</returns>
        Task UpdateSomeDataAsync(Guid id, string data);
    }
}
