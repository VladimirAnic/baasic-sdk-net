﻿using Baasic.Client.TokenHandler;
using System;
using System.Threading.Tasks;

namespace Baasic.Client.Token
{
    /// <summary>
    /// Token Client.
    /// </summary>
    public interface ITokenClient
    {
        /// <summary>
        /// Asynchronously creates new <see cref="IAuthenticationToken" /> for specified used.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>New <see cref="IAuthenticationToken" />.</returns>
        Task<IAuthenticationToken> CreateAsync(string username, string password);

        /// <summary>
        /// Asynchronously destroys the <see cref="IAuthenticationToken" />.
        /// </summary>
        /// <param name="token">Token to destroy.</param>
        /// <returns>
        /// True if <see cref="IAuthenticationToken" /> is destoyed, false otherwise.
        /// </returns>
        Task<bool> DestroyAsync();

        /// <summary>
        /// Asynchronously refreshes the <see cref="IAuthenticationToken" />.
        /// </summary>
        /// <param name="token">Token to update.</param>
        /// <returns>New <see cref="IAuthenticationToken" />.</returns>
        Task<IAuthenticationToken> RefreshAsync();
    }
}