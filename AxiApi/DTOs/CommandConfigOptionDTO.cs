namespace AxiApi.DTOs
{
    /// <summary>One Configure / SDK option as listed on the AxiCMD Config page (active or not).</summary>
    public class CommandConfigOptionDTO : CommandConfigDTO
    {
        /// <summary>'T' for a Built-in Option shipped with the product, 'F' for a Custom Option.</summary>
        public string IsSystem { get; set; } = "F";

        /// <summary>Name of the data source that lists the records of this option (e.g. axi_userlist); empty when none.</summary>
        public string RecordSource { get; set; } = string.Empty;
    }
}
