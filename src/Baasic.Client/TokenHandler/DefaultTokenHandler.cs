﻿using System;
using System.Threading;

namespace Baasic.Client.TokenHandler
{
    /// <summary>
    /// Default token handler.
    /// </summary>
    public class DefaultTokenHandler : ITokenHandler
    {
        private static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
        private static string token = null;

        /// <summary>
        /// Clear token storage.
        /// </summary>
        /// <returns>True if token has been cleared, false otherwise.</returns>
        public virtual bool Clear()
        {
            if (rwl.TryEnterWriteLock(Timeout.Infinite))
            {
                try
                {
                    DefaultTokenHandler.token = null;
                }
                finally
                {
                    rwl.ExitWriteLock();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the token from a storage.
        /// </summary>
        /// <returns>Token.</returns>
        public virtual string Get()
        {
            if (rwl.TryEnterReadLock(Timeout.Infinite))
            {
                try
                {
                    return token;
                }
                finally
                {
                    rwl.ExitReadLock();
                }
            }
            return null;
        }

        /// <summary>
        /// Saves token to storage.
        /// </summary>
        /// <param name="token">Token to save.</param>
        /// <returns>True if token has been saved, false otherwise.</returns>
        public virtual bool Save(string token)
        {
            if (rwl.TryEnterWriteLock(Timeout.Infinite))
            {
                try
                {
                    DefaultTokenHandler.token = token;
                }
                finally
                {
                    rwl.ExitWriteLock();
                }
                return true;
            }
            return false;
        }
    }
}