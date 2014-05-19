﻿using Baasic.Client.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Baasic.Client
{
    /// <summary>
    /// Baasic client.
    /// </summary>
    public class BaasicClient : IBaasicClient
    {
        #region Properties

        /// <summary>
        /// Gets or sets client configuration.
        /// </summary>
        public IClientConfiguration Configuration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the HTTP client factory.
        /// </summary>
        /// <value>The HTTP client factory.</value>
        protected IHttpClientFactory HttpClientFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default serializer.
        /// </summary>
        /// <value>Default serializer.</value>
        protected virtual JsonSerializer Serializer
        {
            get;
            private set;
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaasicClient" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        public BaasicClient(IClientConfiguration configuration,
            IHttpClientFactory httpClientFactory
            )
        {
            Configuration = configuration;
            HttpClientFactory = httpClientFactory;

            this.Serializer = JsonSerializer.Create(this.Configuration.SerializerSettings);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Create string content.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="mthv">Media type.</param>
        /// <returns>String content.</returns>
        public StringContent CreateStringContent(string data, string mthv)
        {
            var result = new StringContent(data, Configuration.DefaultEncoding, mthv);
            //TODO: Add authentication header
            //result.Headers
            return result;
        }

        /// <summary>
        /// Asynchronously deletes the object from the system.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <returns>True if object is deleted, false otherwise.</returns>
        public virtual Task<bool> DeleteAsync(string requestUri)
        {
            return DeleteAsync(requestUri, new CancellationToken());
        }

        /// <summary>
        /// Asynchronously deletes the object from the system.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if object is deleted, false otherwise.</returns>
        public virtual async Task<bool> DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                InitializeClient(client, Configuration.DefaultMediaType);

                var response = await client.DeleteAsync(requestUri, cancellationToken);
                return response.StatusCode.Equals(HttpStatusCode.OK);
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Gets the API URL.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public string GetApiUrl(string relativeUrl, params object[] parameters)
        {
            return GetApiUrl(false, Configuration.ApplicationIdentifier, relativeUrl, parameters);
        }

        /// <summary>
        /// Gets the API URL.
        /// </summary>
        /// <param name="ssl">if set to <c>true</c> [SSL].</param>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public string GetApiUrl(bool ssl, string relativeUrl, params object[] parameters)
        {
            return String.Format("{0}/{1}", ssl ? Configuration.SecureBaseAddress.TrimEnd('/') : Configuration.BaseAddress.TrimEnd('/'), String.Format(relativeUrl, parameters));
        }

        /// <summary>
        /// Asynchronously gets the <typeparamref name="T" /> from the system.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="requestUri">Request URI.</param>
        /// <returns><typeparamref name="T" />.</returns>
        public virtual Task<T> GetAsync<T>(string requestUri)
        {
            return GetAsync<T>(requestUri, new CancellationToken());
        }

        /// <summary>
        /// Asynchronously gets the <typeparamref name="T" /> from the system.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><typeparamref name="T" />.</returns>
        public virtual async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                InitializeClient(client, Configuration.DefaultMediaType);

                var response = await client.GetAsync(requestUri, cancellationToken);
                response.EnsureSuccessStatusCode();
                //TODO: Add HAL Converter
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Gets the secure API URL.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public string GetSecureApiUrl(string relativeUrl, params object[] parameters)
        {
            return GetApiUrl(true, Configuration.ApplicationIdentifier, relativeUrl, parameters);
        }

        /// <summary>
        /// Asynchronously insert the <typeparamref name="T" /> into the system.
        /// </summary>
        /// <typeparam name="T">Resource type.</typeparam>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="content">Resource instance.</param>
        /// <returns>Newly created <typeparamref name="T" />.</returns>
        public virtual Task<T> PostAsync<T>(string requestUri, T content)
        {
            return PostAsync<T>(requestUri, content, new CancellationToken());
        }

        /// <summary>
        /// Asynchronously insert the <typeparamref name="T" /> into the system.
        /// </summary>
        /// <typeparam name="T">Resource type.</typeparam>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="content">Resource instance.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Newly created <typeparamref name="T" />.</returns>
        public virtual async Task<T> PostAsync<T>(string requestUri, T content, CancellationToken cancellationToken)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                InitializeClient(client, Configuration.DefaultMediaType);

                var response = await client.PostAsync(requestUri, CreateStringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(content)), Configuration.DefaultMediaType), cancellationToken);
                response.EnsureSuccessStatusCode();

                //TODO: Add HAL Converter
                var stringContent = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew<T>(() => JsonConvert.DeserializeObject<T>(stringContent, Configuration.SerializerSettings));
            }
        }

        /// <summary>
        /// Asynchronously update the <typeparamref name="T" /> in the system.
        /// </summary>
        /// <typeparam name="T">Resource type.</typeparam>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="content">Resource instance.</param>
        /// <returns>Updated <typeparamref name="T" />.</returns>
        public virtual Task<T> PutAsync<T>(string requestUri, T content)
        {
            return PutAsync<T>(requestUri, content, new CancellationToken());
        }

        /// <summary>
        /// Asynchronously update the <typeparamref name="T" /> in the system.
        /// </summary>
        /// <typeparam name="T">Resource type.</typeparam>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="content">Resource instance.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Updated <typeparamref name="T" />.</returns>
        public virtual async Task<T> PutAsync<T>(string requestUri, T content, CancellationToken cancellationToken)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                InitializeClient(client, Configuration.DefaultMediaType);

                var response = await client.PutAsync(requestUri, CreateStringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(content)), Configuration.DefaultMediaType), cancellationToken);
                response.EnsureSuccessStatusCode();

                var reader = new JsonTextReader(new System.IO.StreamReader(await response.Content.ReadAsStreamAsync()));

                return await Task.Factory.StartNew(() => this.Serializer.Deserialize<T>(reader));
                //TODO: Add HAL Converter
                //return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), Configuration.SerializerSettings);
            }
        }

        /// <summary>
        /// Asynchronously sends http request.
        /// </summary>
        /// <typeparam name="request">Http request.</typeparam>
        /// <returns>Http respnse message.</returns>
        public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return this.SendAsync(request, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously sends http request.
        /// </summary>
        /// <typeparam name="request">Http request.</typeparam>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Http respnse message.</returns>
        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                InitializeClient(client, Configuration.DefaultMediaType);

                var response = await client.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();
                //TODO: Add HAL Converter
                return response;
            }
        }

        /// <summary>
        /// Initialize HTTP client instance.
        /// </summary>
        /// <param name="client">HTTP client.</param>
        /// <param name="mthv">Media type header value.</param>
        protected virtual void InitializeClient(HttpClient client, string mthv)
        {
            client.Timeout = Configuration.DefaultTimeout;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));

            //TODO: Add authentication header
            var token = Configuration.TokenHandler.Get();
            if (token != null && token.IsValid)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.Scheme, token.Token);
            }
        }

        private string GetApiUrl(bool ssl, string applicationIdentifier, string relativeUrl, params object[] parameters)
        {
            return GetApiUrl(ssl, applicationIdentifier.TrimEnd('/') + "/" + relativeUrl, parameters);
        }

        #endregion Methods
    }

    /// <summary>
    /// <see cref="IBaasicClient" /> factory.
    /// </summary>
    public class BaasicClientFactory : IBaasicClientFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaasicClientFactory" /> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public BaasicClientFactory(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Gets or sets the dependency resolver.
        /// </summary>
        /// <value>The dependency resolver.</value>
        private IDependencyResolver DependencyResolver { get; set; }

        /// <summary>
        /// Creates the specified Baasic client.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns><see cref="IBaasicClient" /> instance.</returns>
        public virtual IBaasicClient Create(IClientConfiguration configuration)
        {
            IBaasicClient client = DependencyResolver.GetService<IBaasicClient>();
            client.Configuration = configuration;
            return client;
        }
    }

    /// <summary>
    /// <see cref="HttpClient" /> factory.
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaasicClientFactory" /> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public HttpClientFactory(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Gets or sets the dependency resolver.
        /// </summary>
        /// <value>The dependency resolver.</value>
        private IDependencyResolver DependencyResolver { get; set; }

        /// <summary>
        /// Creates <see cref="HttpClient" /> instance.
        /// </summary>
        /// <returns><see cref="HttpClient" /> instance.</returns>
        public virtual HttpClient Create()
        {
            return DependencyResolver.GetService<HttpClient>();
        }
    }
}