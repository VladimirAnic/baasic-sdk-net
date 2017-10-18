using Baasic.Client.Core;
using System;
using System.Threading.Tasks;
using Baasic.Client.Model;
using Baasic.Client.Configuration;
using Baasic.Client.Utility;
using System.Net;

namespace Baasic.Client.Clients.Metering
{
    /// <summary>
    /// Metering data client.
    /// </summary>
    public class MeteringDataClient<T> : ClientBase, IMeteringDataClient<T> where T : IModel
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MeteringDataClient" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="baasicClientFactory">The baasic client factory.</param>
        public MeteringDataClient(IClientConfiguration configuration,
            IBaasicClientFactory baasicClientFactory)
            : base(configuration)
        {
            BaasicClientFactory = baasicClientFactory;
        }

        #endregion Constructors


        #region Properties

        /// <summary>
        /// Gets or sets the baasic client factory.
        /// </summary>
        /// <value>The baasic client factory.</value>
        protected virtual IBaasicClientFactory BaasicClientFactory { get; set; }

        /// <summary>
        /// Gets the module relative path.
        /// </summary>
        protected override string ModuleRelativePath
        {
            get { return "metering/data"; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Asynchronously deletes the <see cref="T" /> from the system.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>True if <see cref="T" /> is deleted, false otherwise.</returns>
        public virtual Task<bool> DeleteAsync(object id)
        {
            using (IBaasicClient client = BaasicClientFactory.Create(Configuration))
            {
                return client.DeleteAsync(client.GetApiUrl(String.Format("{0}/{{0}}", ModuleRelativePath), id));
            }
        }

        /// <summary>
        /// Asynchronously find <see cref="T" /> s.
        /// </summary>
        /// <param name="searchQuery">Search query.</param>
        /// <param name="page">Page number.</param>
        /// <param name="rpp">Records per page limit.</param>
        /// <param name="sort">Sort by field.</param>
        /// <param name="embed">Embed related resources.</param>
        /// <param name="fields">The fields to include in response.</param>
        /// <returns>List of <see cref="T" /> s.</returns>
        public virtual Task<CollectionModelBase<T>> FindAsync(string searchQuery = DefaultSearchQuery,
            int page = DefaultPage, int rpp = DefaultMaxNumberOfResults,
            string sort = DefaultSorting, string embed = DefaultEmbed, string fields = DefaultFields)
        {
            return FindAsync<T>(searchQuery, page, rpp, sort, embed, fields);
        }

        /// <summary>
        /// Asynchronously find <see cref="T" /> s.
        /// </summary>
        /// <typeparam name="T">Type of extended <see cref="T" />.</typeparam>
        /// <param name="searchQuery">Search query.</param>
        /// <param name="page">Page number.</param>
        /// <param name="rpp">Records per page limit.</param>
        /// <param name="sort">Sort by field.</param>
        /// <param name="embed">Embed related resources.</param>
        /// <param name="fields">The fields to include in response.</param>
        /// <returns>List of <typeparamref name="T" /> s.</returns>
        public virtual async Task<CollectionModelBase<T>> FindAsync<T>(string searchQuery = DefaultSearchQuery,
            int page = DefaultPage, int rpp = DefaultMaxNumberOfResults,
            string sort = DefaultSorting, string embed = DefaultEmbed, string fields = DefaultFields) where T : IModel
        {
            using (IBaasicClient client = BaasicClientFactory.Create(Configuration))
            {
                UrlBuilder uriBuilder = new UrlBuilder(client.GetApiUrl(ModuleRelativePath));
                InitializeQueryString(uriBuilder, searchQuery, page, rpp, sort, embed, fields);
                var result = await client.GetAsync<CollectionModelBase<T>>(uriBuilder.ToString());
                if (result == null)
                {
                    result = new CollectionModelBase<T>();
                }
                return result;
            }
        }

        /// <summary>
        /// Asynchronously gets the <see cref="T" /> by provided id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="embed">The embed.</param>
        /// /// <param name="fields">The embed.</param>
        /// <returns><see cref="T" /> .</returns>
        public virtual Task<T> GetAsync(object id, string embed = DefaultEmbed, string fields = DefaultFields)
        {
            return GetAsync(id, embed, fields);
        }

        /// <summary>
        /// Asynchronously gets the <see cref="T" /> by provided id.
        /// </summary>
        /// <typeparam name="T">Type of extended <see cref="T" />.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="embed">The embed.</param>
        /// <returns><typeparamref name="T" /> .</returns>
        public virtual Task<T> GetAsync<T>(object id, string embed = DefaultEmbed) where T : IModel
        {
            using (IBaasicClient client = BaasicClientFactory.Create(Configuration))
            {
                UrlBuilder uriBuilder = new UrlBuilder(client.GetApiUrl(String.Format("{0}/{{0}}", ModuleRelativePath), id));
                InitializeQueryString(uriBuilder, embed, "");
                return client.GetAsync<T>(uriBuilder.ToString());
            }
        }

        /// <summary>
        /// Asynchronously insert the <see cref="T" /> into the system.
        /// </summary>
        /// <param name="content">Resource instance.</param>
        /// <returns>Newly created <see cref="T" /> .</returns>
        public virtual Task<T> InsertAsync(T content)
        {
            return InsertAsync<T>(content);
        }

        /// <summary>
        /// Asynchronously insert the <see cref="T" /> into the system.
        /// </summary>
        /// <typeparam name="T">Type of extended <see cref="T" />.</typeparam>
        /// <param name="content">Resource instance.</param>
        /// <returns>Newly created <typeparamref name="T" /> .</returns>
        public virtual Task<T> InsertAsync<T>(T content) where T : IModel
        {
            using (IBaasicClient client = BaasicClientFactory.Create(Configuration))
            {
                return client.PostAsync<T>(client.GetApiUrl(ModuleRelativePath), content);
            }
        }

        /// <summary>
        /// Asynchronously update the <see cref="T" /> in the system.
        /// </summary>
        /// <param name="content">Resource instance.</param>
        /// <returns>True if <see cref="T" /> is successfully updated, false otherwise.</returns>
        public virtual async Task<bool> UpdateAsync(T content)
        {
            using (IBaasicClient client = BaasicClientFactory.Create(Configuration))
            {
                var result = await client.PutAsync<T, HttpStatusCode>(client.GetApiUrl(String.Format("{0}/{1}", ModuleRelativePath, content.Id)), content);
                switch (result)
                {
                    case System.Net.HttpStatusCode.Created:
                    case System.Net.HttpStatusCode.NoContent:
                    case System.Net.HttpStatusCode.OK:
                        return true;

                    default:
                        return false;
                }
            }
        }

        #endregion Methods

    }
}
