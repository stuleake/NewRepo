namespace Api.Planner.Core.ViewModels
{
    /// <summary>
    /// The full address search model class
    /// </summary>
    public class FullAddressSearchModel
    {
        /// <summary>
        /// Gets or sets the organisation name
        /// </summary>
        public string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the department name
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Gets or sets the sub building name
        /// </summary>
        public string SubBuildingName { get; set; }

        /// <summary>
        /// Gets or sets the building name
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the building number
        /// </summary>
        public int? BuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the PO box number
        /// </summary>
        public string PoBoxNumber { get; set; }

        /// <summary>
        /// Gets or sets the dependent thoroughfare
        /// </summary>
        public string DependentThoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the thoroughfare
        /// </summary>
        public string Thoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the double dependent locality
        /// </summary>
        public string DoubleDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the dependent locality
        /// </summary>
        public string DependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the post town
        /// </summary>
        public string PostTown { get; set; }

        /// <summary>
        /// Gets or sets the post code
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the welsh dependent thoroughfare
        /// </summary>
        public string WelshDependentThoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the welsh thoroughfare
        /// </summary>
        public string WelshThoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the welsh double dependent locality
        /// </summary>
        public string WelshDoubleDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the welsh dependent locality
        /// </summary>
        public string WelshDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the welsh post town
        /// </summary>
        public string WelshPostTown { get; set; }

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
        /// Gets or sets the welsh address line 1
        /// </summary>
        public string WelshAddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the welsh address line 2
        /// </summary>
        public string WelshAddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the welsh address line 3
        /// </summary>
        public string WelshAddressLine3 { get; set; }

        /// <summary>
        /// Gets or sets the single line address
        /// </summary>
        public string SingleLineAddress { get; set; }

        /// <summary>
        /// Gets or sets the welsh single line address
        /// </summary>
        public string WelshSingleLineAddress { get; set; }

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