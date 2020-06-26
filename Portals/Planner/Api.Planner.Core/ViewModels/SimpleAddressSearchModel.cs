namespace Api.Planner.Core.ViewModels
{
    /// <summary>
    /// The simple address search model
    /// </summary>
    public class SimpleAddressSearchModel
    {
        /// <summary>
        /// Gets or sets the single line address
        /// </summary>
        public string SingleLineAddress { get; set; }

        /// <summary>
        /// Gets or sets the address line 1
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line 2
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the address line 3
        /// </summary>
        public string AddressLine3 { get; set; }

        /// <summary>
        /// Gets or sets the town
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// Gets or sets the postcode
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the Uprn
        /// </summary>
        public long Uprn { get; set; }

        /// <summary>
        /// Gets or sets the latitude
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// Gets or sets the x coordinate easting
        /// </summary>
        public decimal XCoordinateEasting { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate northing
        /// </summary>
        public decimal YCoordinateNorthing { get; set; }
    }
}