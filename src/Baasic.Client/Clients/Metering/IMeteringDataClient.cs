using Baasic.Client.Core;
using Baasic.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baasic.Client.Clients.Metering
{
    /// <summary>
    /// Data Metering Module Client.
    /// </summary>
    public interface IMeteringDataClient<T> where T : IModel
    {
        #region Methods

        /// <summary>
        /// Asynchronously deletes the <see cref="T" /> from the system.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>True if <see cref="T" /> is deleted, false otherwise.</returns>
        Task<bool> DeleteAsync(object id);

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
        Task<CollectionModelBase<T>> FindAsync(string searchQuery = ClientBase.DefaultSearchQuery, int page = ClientBase.DefaultPage, int rpp = ClientBase.DefaultMaxNumberOfResults, string sort = ClientBase.DefaultSorting, string embed = ClientBase.DefaultEmbed, string fields = ClientBase.DefaultFields);

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
        Task<CollectionModelBase<T>> FindAsync<T>(string searchQuery = ClientBase.DefaultSearchQuery,
            int page = ClientBase.DefaultPage, int rpp = ClientBase.DefaultMaxNumberOfResults,
            string sort = ClientBase.DefaultSorting, string embed = ClientBase.DefaultEmbed, string fields = ClientBase.DefaultFields) where T : IModel;

        /// <summary>
        /// Asynchronously gets the <see cref="T" /> by provided id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="embed">The embed.</param>
        /// /// <param name="fields">The fields.</param>
        /// <returns><see cref="T" /> .</returns>
        Task<T> GetAsync(object id, string embed = ClientBase.DefaultEmbed, string fields = ClientBase.DefaultFields);

        /// <summary>
        /// Asynchronously gets the <see cref="T" /> by provided id.
        /// </summary>
        /// <typeparam name="T">Type of extended <see cref="T" />.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="embed">The embed.</param>
        /// <returns><typeparamref name="T" /> .</returns>
        Task<T> GetAsync<T>(object id, string embed = ClientBase.DefaultEmbed) where T : IModel;

        /// <summary>
        /// Asynchronously insert the <see cref="T" /> into the system.
        /// </summary>
        /// <param name="content">Resource instance.</param>
        /// <returns>Newly created <see cref="T" /> .</returns>
        Task<T> InsertAsync(T content);

        /// <summary>
        /// Asynchronously insert the <see cref="T" /> into the system.
        /// </summary>
        /// <typeparam name="T">Type of extended <see cref="T" />.</typeparam>
        /// <param name="content">Resource instance.</param>
        /// <returns>Newly created <typeparamref name="T" /> .</returns>
        Task<T> InsertAsync<T>(T content) where T : IModel;

        /// <summary>
        /// Asynchronously update the <see cref="T" /> in the system.
        /// </summary>
        /// <param name="content">Resource instance.</param>
        /// <returns>True if <see cref="T" /> is successfully updated, false otherwise.</returns>
        Task<bool> UpdateAsync(T content);

        #endregion Methods
    }
}
