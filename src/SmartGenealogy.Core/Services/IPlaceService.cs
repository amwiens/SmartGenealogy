namespace SmartGenealogy.Core.Services;

/// <summary>
/// Place service interface
/// </summary>
public interface IPlaceService
{
    /// <summary>
    /// Retrieves a list of master places from the database.
    /// </summary>
    /// <returns>A list of <see cref="Place"/> objects.</returns>
    Task<List<Place>?> ListMasterPlacesAsync();

    /// <summary>
    /// Get place by Id.
    /// </summary>
    /// <param name="id">Place identifier.</param>
    /// <returns>A <see cref="Place"/> object if found; otherwise, null.</returns>
    Task<Place?> GetPlaceAsync(int id);

    /// <summary>
    /// Saves a place to the database. If the place Id is 0, a new place is created; otherwise, the existing place is updated.
    /// </summary>
    /// <param name="place">The place to save.</param>
    /// <returns>The Id of the saved place.</returns>
    Task<int> SavePlaceAsync(Place place);

    /// <summary>
    /// Deletes a place from the database.
    /// </summary>
    /// <param name="place">The <see cref="Place"/> to delete.</param>
    /// <returns><see langword="true"> if deleted, otherwise <see langword="false"/>.</returns>
    Task<bool> DeletePlaceAsync(Place place);
}