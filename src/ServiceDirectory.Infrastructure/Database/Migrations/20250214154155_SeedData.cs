using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceDirectory.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        private const string ServiceDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer vel varius nibh. Mauris tincidunt vitae ligula ornare aliquam. Ut vestibulum orci libero, vel congue massa feugiat sed. Duis cursus eu nisl tempus ullamcorper. Praesent bibendum porttitor semper. Maecenas ultricies in tortor in dictum. Phasellus vel nisl lorem. Sed quis massa non elit dictum iaculis.";
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "Organisations",
                new[] { "Id", "Name", "Description", "Status" },
                new object[,]
                {
                    { 1, "Bristol County Council", "Bristol County Council", "Active" },
                    { 2, "Southampton County Council", "Southampton County Council", "Active" },
                    { 3, "Buckinghamshire Council", "Buckinghamshire Council", "Active" }
                });
            
            migrationBuilder.InsertData(
                "Locations",
                new[] { "Id", "AddressLine1", "AddressLine2", "TownOrCity", "County", "Postcode", "Latitude", "Longitude", "OrganisationId", "Status" },
                new object[,]
                {
                    { 1, "City Hall", "College Green", "Bristol", "Bristol", "BS1 5TR", 51.452605, -2.60207, 1, "Active" },
                    { 2, "Civic Centre", null, "Southampton", "Hampshire", "SO14 7LY", 50.907683, -1.406979, 2, "Active" },
                    { 3, "Walton St", null, "Aylesbury", "Buckinghamshire", "HP20 1UA", 51.814478, -0.8122, 3, "Active" }
                });
            
            migrationBuilder.InsertData(
                "Services",
                new[] { "Id", "Name", "Description", "Cost", "OrganisationId", "Status" },
                new object[,]
                {
                    { 1, "Symes Community Building", ServiceDescription, 0.00, 1, "Active" },
                    { 2, "AA Meeting - Discussion Group", ServiceDescription, 0.00, 1, "Active" },
                    { 3, "Activity Group - Redfield (St Anne's)", ServiceDescription, 0.00, 1, "Active" },
                    { 4, "Activity Group- Brislington", ServiceDescription, 0.00, 1, "Active" },
                    { 5, "Adult Visual Impairment Friendly Rowing Sessions", ServiceDescription, 0.00, 1, "Active" },
                    { 6, "ANDYSMANCLUB", ServiceDescription, 20.00, 1, "Active" },
                    { 7, "Art Ease Group - Tuesday", ServiceDescription, 5.00, 1, "Active" },
                    { 8, "Art Ease Group - Wednesday", ServiceDescription, 6.00, 1, "Active" },
                    { 9, "Art Therapeutic Community", ServiceDescription, 7.00, 1, "Active" },
                    { 10, "Autism Advice - Bristol", ServiceDescription, 2.00, 1, "Active" },
                    { 11, "Beginners Tai Chi", ServiceDescription, 10.00, 1, "Active" }
                });
            
            migrationBuilder.InsertData(
                "Locations",
                new[] { "Id", "AddressLine1", "AddressLine2", "TownOrCity", "County", "Postcode", "Latitude", "Longitude", "ServiceId", "Status" },
                new object[,]
                {
                    { 4, "Symes Community Building", "Peterson Avenue, Hartcliffe", "Bristol", "Bristol", "BS13 0BE", 51.4049, -2.59813, 1, "Active" },
                    { 5, "19A Stretford Rd", null, "Bristol", "Bristol", "BS5 7AW", 51.463252, -2.55083, 2, "Active" },
                    { 6, "ll-Aboard Watersports", "Baltic Wharf", "Bristol", "Bristol", "BS1 6XG", 51.4468911, -2.6158993, 3, "Active" },
                    { 7, "ll-Aboard Watersports", "Baltic Wharf", "Bristol", "Bristol", "BS1 6XG", 51.4468911, -2.6158993, 4, "Active" },
                    { 8, "ll-Aboard Watersports", "Baltic Wharf", "Bristol", "Bristol", "BS1 6XG", 51.4468911, -2.6158993, 5, "Active" },
                    { 9, "Ashton Gate Stadium", "Ashton Road", "Bristol", "Bristol", "BS3 2EJ", 51.4399, -2.62099, 6, "Active" },
                    { 10, "5 Knowle West Health Park", "Downton Road, Knowle West", "Bristol", "Bristol", "BS4 1WH", 51.4288, -2.59625, 7, "Active" },
                    { 11, "5 Knowle West Health Park", "Downton Road, Knowle West", "Bristol", "Bristol", "BS4 1WH", 51.4288, -2.59625, 8, "Active" },
                    { 12, "5 Knowle West Health Park", "Downton Road, Knowle West", "Bristol", "Bristol", "BS4 1WH", 51.4288, -2.59625, 9, "Active" },
                    { 13, "Spike Island", null, "Bristol", "Bristol", "BS1 6XN", 51.4471, -2.62224, 10, "Active" },
                    { 14, "The Greenway Centre", "Doncaster Road, Southmead", "Bristol", "Bristol", "BS10 5PY", 51.4994852, -2.60897120000004, 11, "Active" }
                });
            
            migrationBuilder.InsertData(
                "Services",
                new[] { "Id", "Name", "Description", "Cost", "OrganisationId", "Status" },
                new object[,]
                {
                    { 12, "0-25 Service - Special Educational Needs Team", ServiceDescription, 0.00, 2, "Active" },
                    { 13, "ABC Childminding - Channell, Andrea Evelyn", ServiceDescription, 0.00, 2, "Active" },
                    { 14, "AFASIC - Unlocking Speech & Language", ServiceDescription, 0.00, 2, "Active" },
                    { 15, "Abing Homecare", ServiceDescription, 0.00, 2, "Active" }
                });
            
            migrationBuilder.InsertData(
                "Locations",
                new[] { "Id", "AddressLine1", "AddressLine2", "TownOrCity", "County", "Postcode", "Latitude", "Longitude", "ServiceId", "Status" },
                new object[,]
                {
                    { 15, "SEND 0-25 Service", null, "Southampton", "Hampshire", "SO14 7LY", 50.9083824, -1.4069646, 12, "Active" },
                    { 16, "15 Church View Close", null, "Southampton", "Hampshire", "SO19 8SJ", 50.8988228, -1.3595737, 13, "Active" },
                    { 17, "Afasic", null, "Southampton", "Hampshire", "E2 9PJ", 51.5298457, -0.0545289, 14, "Active" },
                    { 18, "St. Denys Community Centre", null, "Southampton", "Hampshire", "SO17 2JZ", 50.9202022, -1.3885274, 15, "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
