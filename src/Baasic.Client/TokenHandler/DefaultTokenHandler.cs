﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baasic.Client.TokenHandler
{
    /// <summary>
    /// Default token handler.
    /// </summary>
    public class DefaultTokenHandler : ITokenHandler
    {
        private static string token = null;
        private static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
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
    }
}
