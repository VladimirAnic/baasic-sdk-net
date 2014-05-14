﻿using Newtonsoft.Json;
using System;
using System.Text;

namespace Baasic.Client.Configuration
{
    /// <summary>
    /// Client configuration.
    /// </summary>
    public interface IClientConfiguration
    {
        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        string ApplicationIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets server base address.
        /// </summary>
        string BaseAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets default encoding.
        /// </summary>
        Encoding DefaultEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets default media type.
        /// </summary>
        string DefaultMediaType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets client default timeout period.
        /// </summary>
        TimeSpan DefaultTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets server secure base address.
        /// </summary>
        string SecureBaseAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the serializer settings.
        /// </summary>
        JsonSerializerSettings SerializerSettings
        {
            get;
            set;
        }
    }
}