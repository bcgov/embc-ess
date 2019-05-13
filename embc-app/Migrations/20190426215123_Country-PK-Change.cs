using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class CountryPKChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Countries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"UPDATE Countries SET CountryCode = 'AFG' WHERE Name = 'Afghanistan'
                                                UPDATE Countries SET CountryCode = 'ALA' WHERE Name = 'Åland Islands'
                                                UPDATE Countries SET CountryCode = 'ALB' WHERE Name = 'Albania'
                                                UPDATE Countries SET CountryCode = 'DZA' WHERE Name = 'Algeria'
                                                UPDATE Countries SET CountryCode = 'ASM' WHERE Name = 'American Samoa'
                                                UPDATE Countries SET CountryCode = 'AND' WHERE Name = 'Andorra'
                                                UPDATE Countries SET CountryCode = 'AGO' WHERE Name = 'Angola'
                                                UPDATE Countries SET CountryCode = 'AIA' WHERE Name = 'Anguilla'
                                                UPDATE Countries SET CountryCode = 'ATA' WHERE Name = 'Antarctica'
                                                UPDATE Countries SET CountryCode = 'ATG' WHERE Name = 'Antigua and Barbuda'
                                                UPDATE Countries SET CountryCode = 'ARG' WHERE Name = 'Argentina'
                                                UPDATE Countries SET CountryCode = 'ARM' WHERE Name = 'Armenia'
                                                UPDATE Countries SET CountryCode = 'ABW' WHERE Name = 'Aruba'
                                                UPDATE Countries SET CountryCode = 'AUS' WHERE Name = 'Australia'
                                                UPDATE Countries SET CountryCode = 'AUT' WHERE Name = 'Austria'
                                                UPDATE Countries SET CountryCode = 'AZE' WHERE Name = 'Azerbaijan'
                                                UPDATE Countries SET CountryCode = 'BHS' WHERE Name = 'Bahamas'
                                                UPDATE Countries SET CountryCode = 'BHR' WHERE Name = 'Bahrain'
                                                UPDATE Countries SET CountryCode = 'BGD' WHERE Name = 'Bangladesh'
                                                UPDATE Countries SET CountryCode = 'BRB' WHERE Name = 'Barbados'
                                                UPDATE Countries SET CountryCode = 'BLR' WHERE Name = 'Belarus'
                                                UPDATE Countries SET CountryCode = 'BEL' WHERE Name = 'Belgium'
                                                UPDATE Countries SET CountryCode = 'BLZ' WHERE Name = 'Belize'
                                                UPDATE Countries SET CountryCode = 'BEN' WHERE Name = 'Benin'
                                                UPDATE Countries SET CountryCode = 'BMU' WHERE Name = 'Bermuda'
                                                UPDATE Countries SET CountryCode = 'BTN' WHERE Name = 'Bhutan'
                                                UPDATE Countries SET Name = 'Bolivia (Plurinational State of)' WHERE Name = 'Bolivia'
                                                UPDATE Countries SET CountryCode = 'BOL' WHERE Name = 'Bolivia (Plurinational State of)'
                                                UPDATE Countries SET CountryCode = 'BES' WHERE Name = 'Bonaire, Sint Eustatius and Saba'
                                                UPDATE Countries SET CountryCode = 'BIH' WHERE Name = 'Bosnia and Herzegovina'
                                                UPDATE Countries SET CountryCode = 'BWA' WHERE Name = 'Botswana'
                                                UPDATE Countries SET CountryCode = 'BVT' WHERE Name = 'Bouvet Island'
                                                UPDATE Countries SET CountryCode = 'BRA' WHERE Name = 'Brazil'
                                                UPDATE Countries SET CountryCode = 'IOT' WHERE Name = 'British Indian Ocean Territory'
                                                UPDATE Countries SET CountryCode = 'BRN' WHERE Name = 'Brunei Darussalam'
                                                UPDATE Countries SET CountryCode = 'BGR' WHERE Name = 'Bulgaria'
                                                UPDATE Countries SET CountryCode = 'BFA' WHERE Name = 'Burkina Faso'
                                                UPDATE Countries SET CountryCode = 'BDI' WHERE Name = 'Burundi'
                                                UPDATE Countries SET CountryCode = 'CPV' WHERE Name = 'Cabo Verde'
                                                UPDATE Countries SET CountryCode = 'KHM' WHERE Name = 'Cambodia'
                                                UPDATE Countries SET CountryCode = 'CMR' WHERE Name = 'Cameroon'
                                                UPDATE Countries SET CountryCode = 'CAN' WHERE Name = 'Canada'
                                                UPDATE Countries SET CountryCode = 'CYM' WHERE Name = 'Cayman Islands'
                                                UPDATE Countries SET CountryCode = 'CAF' WHERE Name = 'Central African Republic'
                                                UPDATE Countries SET CountryCode = 'TCD' WHERE Name = 'Chad'
                                                UPDATE Countries SET CountryCode = 'CHL' WHERE Name = 'Chile'
                                                UPDATE Countries SET CountryCode = 'CHN' WHERE Name = 'China'
                                                UPDATE Countries SET CountryCode = 'CXR' WHERE Name = 'Christmas Island'
                                                UPDATE Countries SET CountryCode = 'CCK' WHERE Name = 'Cocos (Keeling) Islands'
                                                UPDATE Countries SET CountryCode = 'COL' WHERE Name = 'Colombia'
                                                UPDATE Countries SET CountryCode = 'COM' WHERE Name = 'Comoros'
                                                UPDATE Countries SET CountryCode = 'COG' WHERE Name = 'Congo'
                                                UPDATE Countries SET Name = 'Congo, Democratic Republic of the' WHERE NAME = 'The Democratic Republic of Congo'
                                                UPDATE Countries SET CountryCode = 'COD' WHERE Name = 'Congo, Democratic Republic of the'
                                                UPDATE Countries SET CountryCode = 'COK' WHERE Name = 'Cook Islands'
                                                UPDATE Countries SET CountryCode = 'CRI' WHERE Name = 'Costa Rica'
                                                UPDATE Countries SET CountryCode = 'CIV' WHERE Name = N'Côte d''Ivoire'
                                                UPDATE Countries SET CountryCode = 'HRV' WHERE Name = 'Croatia'
                                                UPDATE Countries SET CountryCode = 'CUB' WHERE Name = 'Cuba'
                                                UPDATE Countries SET CountryCode = 'CUW' WHERE Name = 'Curaçao'
                                                UPDATE Countries SET CountryCode = 'CYP' WHERE Name = 'Cyprus'
                                                DELETE FROM Countries WHERE Name = 'Czech Republic'
                                                UPDATE Countries SET CountryCode = 'CZE' WHERE Name = 'Czechia'
                                                UPDATE Countries SET CountryCode = 'DNK' WHERE Name = 'Denmark'
                                                UPDATE Countries SET CountryCode = 'DJI' WHERE Name = 'Djibouti'
                                                UPDATE Countries SET CountryCode = 'DMA' WHERE Name = 'Dominica'
                                                UPDATE Countries SET CountryCode = 'DOM' WHERE Name = 'Dominican Republic'
                                                DELETE FROM Countries WHERE Name = 'East Timor'
                                                UPDATE Countries SET CountryCode = 'ECU' WHERE Name = 'Ecuador'
                                                UPDATE Countries SET CountryCode = 'EGY' WHERE Name = 'Egypt'
                                                DELETE FROM Countries WHERE Name = 'England'
                                                UPDATE Countries SET CountryCode = 'SLV' WHERE Name = 'El Salvador'
                                                UPDATE Countries SET CountryCode = 'GNQ' WHERE Name = 'Equatorial Guinea'
                                                UPDATE Countries SET CountryCode = 'ERI' WHERE Name = 'Eritrea'
                                                UPDATE Countries SET CountryCode = 'EST' WHERE Name = 'Estonia'
                                                UPDATE Countries SET CountryCode = 'SWZ' WHERE Name = 'Eswatini'
                                                UPDATE Countries SET CountryCode = 'ETH' WHERE Name = 'Ethiopia'
                                                UPDATE Countries SET Name = 'Falkland Islands (Malvinas)' WHERE Name = 'Falkland Islands'
                                                UPDATE Countries SET CountryCode = 'FLK' WHERE Name = 'Falkland Islands (Malvinas)'
                                                UPDATE Countries SET CountryCode = 'FRO' WHERE Name = 'Faroe Islands'
                                                UPDATE Countries SET Name = 'Fiji' WHERE Name = 'Fiji Islands'
                                                UPDATE Countries SET CountryCode = 'FJI' WHERE Name = 'Fiji'
                                                UPDATE Countries SET CountryCode = 'FIN' WHERE Name = 'Finland'
                                                UPDATE Countries SET CountryCode = 'FRA' WHERE Name = 'France'
                                                UPDATE Countries SET CountryCode = 'GUF' WHERE Name = 'French Guiana'
                                                UPDATE Countries SET CountryCode = 'PYF' WHERE Name = 'French Polynesia'
                                                UPDATE Countries SET CountryCode = 'ATF' WHERE Name = 'French Southern Territories'
                                                UPDATE Countries SET CountryCode = 'GAB' WHERE Name = 'Gabon'
                                                UPDATE Countries SET CountryCode = 'GMB' WHERE Name = 'Gambia'
                                                UPDATE Countries SET CountryCode = 'GEO' WHERE Name = 'Georgia'
                                                UPDATE Countries SET CountryCode = 'DEU' WHERE Name = 'Germany'
                                                UPDATE Countries SET CountryCode = 'GHA' WHERE Name = 'Ghana'
                                                UPDATE Countries SET CountryCode = 'GIB' WHERE Name = 'Gibraltar'
                                                UPDATE Countries SET CountryCode = 'GRC' WHERE Name = 'Greece'
                                                UPDATE Countries SET CountryCode = 'GRL' WHERE Name = 'Greenland'
                                                UPDATE Countries SET CountryCode = 'GRD' WHERE Name = 'Grenada'
                                                UPDATE Countries SET CountryCode = 'GLP' WHERE Name = 'Guadeloupe'
                                                UPDATE Countries SET CountryCode = 'GUM' WHERE Name = 'Guam'
                                                UPDATE Countries SET CountryCode = 'GTM' WHERE Name = 'Guatemala'
                                                UPDATE Countries SET CountryCode = 'GGY' WHERE Name = 'Guernsey'
                                                UPDATE Countries SET CountryCode = 'GIN' WHERE Name = 'Guinea'
                                                UPDATE Countries SET CountryCode = 'GNB' WHERE Name = 'Guinea-Bissau'
                                                UPDATE Countries SET CountryCode = 'GUY' WHERE Name = 'Guyana'
                                                UPDATE Countries SET CountryCode = 'HTI' WHERE Name = 'Haiti'
                                                UPDATE Countries SET CountryCode = 'HMD' WHERE Name = 'Heard Island and McDonald Islands'
                                                UPDATE Countries SET Name = 'Holy See' WHERE Name = 'Holy See (Vatican City State)'
                                                UPDATE Countries SET CountryCode = 'VAT' WHERE Name = 'Holy See'
                                                UPDATE Countries SET CountryCode = 'HND' WHERE Name = 'Honduras'
                                                UPDATE Countries SET CountryCode = 'HKG' WHERE Name = 'Hong Kong'
                                                UPDATE Countries SET CountryCode = 'HUN' WHERE Name = 'Hungary'
                                                UPDATE Countries SET CountryCode = 'ISL' WHERE Name = 'Iceland'
                                                UPDATE Countries SET CountryCode = 'IND' WHERE Name = 'India'
                                                UPDATE Countries SET CountryCode = 'IDN' WHERE Name = 'Indonesia'
                                                UPDATE Countries SET Name = 'Iran (Islamic Republic of)' WHERE Name = 'Iran'
                                                UPDATE Countries SET CountryCode = 'IRN' WHERE Name = 'Iran (Islamic Republic of)'
                                                UPDATE Countries SET CountryCode = 'IRQ' WHERE Name = 'Iraq'
                                                UPDATE Countries SET CountryCode = 'IRL' WHERE Name = 'Ireland'
                                                UPDATE Countries SET CountryCode = 'IMN' WHERE Name = 'Isle of Man'
                                                UPDATE Countries SET CountryCode = 'ISR' WHERE Name = 'Israel'
                                                UPDATE Countries SET CountryCode = 'ITA' WHERE Name = 'Italy'
                                                UPDATE Countries SET CountryCode = 'JAM' WHERE Name = 'Jamaica'
                                                UPDATE Countries SET CountryCode = 'JPN' WHERE Name = 'Japan'
                                                UPDATE Countries SET CountryCode = 'JEY' WHERE Name = 'Jersey'
                                                UPDATE Countries SET CountryCode = 'JOR' WHERE Name = 'Jordan'
                                                UPDATE Countries SET CountryCode = 'KAZ' WHERE Name = 'Kazakhstan'
                                                UPDATE Countries SET CountryCode = 'KEN' WHERE Name = 'Kenya'
                                                UPDATE Countries SET CountryCode = 'KIR' WHERE Name = 'Kiribati'
                                                UPDATE Countries SET CountryCode = 'PRK' WHERE Name = 'Korea (Democratic People''s Republic of)'
                                                UPDATE Countries SET CountryCode = 'KOR' WHERE Name = 'Korea, Republic of'
                                                UPDATE Countries SET CountryCode = 'KWT' WHERE Name = 'Kuwait'
                                                UPDATE Countries SET CountryCode = 'KGZ' WHERE Name = 'Kyrgyzstan'
                                                UPDATE Countries SET Name = 'Lao People''s Democratic Republic' WHERE Name = 'Laos'
                                                UPDATE Countries SET CountryCode = 'LAO' WHERE Name = 'Lao People''s Democratic Republic'
                                                UPDATE Countries SET CountryCode = 'LVA' WHERE Name = 'Latvia'
                                                UPDATE Countries SET CountryCode = 'LBN' WHERE Name = 'Lebanon'
                                                UPDATE Countries SET CountryCode = 'LSO' WHERE Name = 'Lesotho'
                                                UPDATE Countries SET CountryCode = 'LBR' WHERE Name = 'Liberia'
                                                UPDATE Countries SET Name = 'Libyan' WHERE Name = 'Libyan Arab Jamahiriya'
                                                UPDATE Countries SET CountryCode = 'LBY' WHERE Name = 'Libya'
                                                UPDATE Countries SET CountryCode = 'LIE' WHERE Name = 'Liechtenstein'
                                                UPDATE Countries SET CountryCode = 'LTU' WHERE Name = 'Lithuania'
                                                UPDATE Countries SET CountryCode = 'LUX' WHERE Name = 'Luxembourg'
                                                UPDATE Countries SET CountryCode = 'MAC' WHERE Name = 'Macao'
                                                DELETE FROM Countries WHERE Name = 'Macedonia'
                                                UPDATE Countries SET CountryCode = 'MDG' WHERE Name = 'Madagascar'
                                                UPDATE Countries SET CountryCode = 'MWI' WHERE Name = 'Malawi'
                                                UPDATE Countries SET CountryCode = 'MYS' WHERE Name = 'Malaysia'
                                                UPDATE Countries SET CountryCode = 'MDV' WHERE Name = 'Maldives'
                                                UPDATE Countries SET CountryCode = 'MLI' WHERE Name = 'Mali'
                                                UPDATE Countries SET CountryCode = 'MLT' WHERE Name = 'Malta'
                                                UPDATE Countries SET CountryCode = 'MHL' WHERE Name = 'Marshall Islands'
                                                UPDATE Countries SET CountryCode = 'MTQ' WHERE Name = 'Martinique'
                                                UPDATE Countries SET CountryCode = 'MRT' WHERE Name = 'Mauritania'
                                                UPDATE Countries SET CountryCode = 'MUS' WHERE Name = 'Mauritius'
                                                UPDATE Countries SET CountryCode = 'MYT' WHERE Name = 'Mayotte'
                                                UPDATE Countries SET CountryCode = 'MEX' WHERE Name = 'Mexico'
                                                UPDATE Countries SET Name = 'Micronesia (Federated States of)' WHERE Name = 'Micronesia, Federated States of'
                                                UPDATE Countries SET CountryCode = 'FSM' WHERE Name = 'Micronesia (Federated States of)'
                                                UPDATE Countries SET CountryCode = 'MDA' WHERE Name = 'Moldova, Republic of'
                                                UPDATE Countries SET CountryCode = 'MCO' WHERE Name = 'Monaco'
                                                UPDATE Countries SET CountryCode = 'MNG' WHERE Name = 'Mongolia'
                                                UPDATE Countries SET CountryCode = 'MNE' WHERE Name = 'Montenegro'
                                                UPDATE Countries SET CountryCode = 'MSR' WHERE Name = 'Montserrat'
                                                UPDATE Countries SET CountryCode = 'MAR' WHERE Name = 'Morocco'
                                                UPDATE Countries SET CountryCode = 'MOZ' WHERE Name = 'Mozambique'
                                                UPDATE Countries SET CountryCode = 'MMR' WHERE Name = 'Myanmar'
                                                UPDATE Countries SET CountryCode = 'NAM' WHERE Name = 'Namibia'
                                                UPDATE Countries SET CountryCode = 'NRU' WHERE Name = 'Nauru'
                                                UPDATE Countries SET CountryCode = 'NPL' WHERE Name = 'Nepal'
                                                UPDATE Countries SET CountryCode = 'NLD' WHERE Name = 'Netherlands'
                                                DELETE FROM Countries WHERE Name = 'Netherlands Antilles'
                                                UPDATE Countries SET CountryCode = 'NCL' WHERE Name = 'New Caledonia'
                                                UPDATE Countries SET CountryCode = 'NZL' WHERE Name = 'New Zealand'
                                                UPDATE Countries SET CountryCode = 'NIC' WHERE Name = 'Nicaragua'
                                                UPDATE Countries SET CountryCode = 'NER' WHERE Name = 'Niger'
                                                UPDATE Countries SET CountryCode = 'NGA' WHERE Name = 'Nigeria'
                                                UPDATE Countries SET CountryCode = 'NIU' WHERE Name = 'Niue'
                                                UPDATE Countries SET CountryCode = 'NFK' WHERE Name = 'Norfolk Island'
                                                DELETE FROM Countries WHERE Name = 'North Korea'
                                                DELETE FROM Countries WHERE Name = 'Northern Ireland'
                                                UPDATE Countries SET CountryCode = 'MKD' WHERE Name = 'North Macedonia'
                                                UPDATE Countries SET CountryCode = 'MNP' WHERE Name = 'Northern Mariana Islands'
                                                UPDATE Countries SET CountryCode = 'NOR' WHERE Name = 'Norway'
                                                UPDATE Countries SET CountryCode = 'OMN' WHERE Name = 'Oman'
                                                UPDATE Countries SET CountryCode = 'PAK' WHERE Name = 'Pakistan'
                                                UPDATE Countries SET CountryCode = 'PLW' WHERE Name = 'Palau'
                                                UPDATE Countries SET CountryCode = 'PSE' WHERE Name = 'Palestine, State of'
                                                UPDATE Countries SET CountryCode = 'PAN' WHERE Name = 'Panama'
                                                UPDATE Countries SET CountryCode = 'PNG' WHERE Name = 'Papua New Guinea'
                                                UPDATE Countries SET CountryCode = 'PRY' WHERE Name = 'Paraguay'
                                                UPDATE Countries SET CountryCode = 'PER' WHERE Name = 'Peru'
                                                UPDATE Countries SET CountryCode = 'PHL' WHERE Name = 'Philippines'
                                                UPDATE Countries SET CountryCode = 'PCN' WHERE Name = 'Pitcairn'
                                                UPDATE Countries SET CountryCode = 'POL' WHERE Name = 'Poland'
                                                UPDATE Countries SET CountryCode = 'PRT' WHERE Name = 'Portugal'
                                                UPDATE Countries SET CountryCode = 'PRI' WHERE Name = 'Puerto Rico'
                                                UPDATE Countries SET CountryCode = 'QAT' WHERE Name = 'Qatar'
                                                UPDATE Countries SET CountryCode = 'REU' WHERE Name = N'Réunion'
                                                UPDATE Countries SET CountryCode = 'ROU' WHERE Name = 'Romania'
                                                UPDATE Countries SET CountryCode = 'RUS' WHERE Name = 'Russian Federation'
                                                UPDATE Countries SET CountryCode = 'RWA' WHERE Name = 'Rwanda'
                                                UPDATE Countries SET CountryCode = 'BLM' WHERE Name = N'Saint Barthélemy'
                                                UPDATE Countries Set Name = 'Saint Helena, Ascension and Tristan da Cunha' WHERE Name = 'Saint Helena'
                                                UPDATE Countries SET CountryCode = 'SHN' WHERE Name = 'Saint Helena, Ascension and Tristan da Cunha'
                                                UPDATE Countries SET CountryCode = 'KNA' WHERE Name = 'Saint Kitts and Nevis'
                                                UPDATE Countries SET CountryCode = 'LCA' WHERE Name = 'Saint Lucia'
                                                UPDATE Countries SET CountryCode = 'MAF' WHERE Name = 'Saint Martin (French part)'
                                                UPDATE Countries SET CountryCode = 'SPM' WHERE Name = 'Saint Pierre and Miquelon'
                                                UPDATE Countries SET CountryCode = 'VCT' WHERE Name = 'Saint Vincent and the Grenadines'
                                                UPDATE Countries SET CountryCode = 'WSM' WHERE Name = 'Samoa'
                                                UPDATE Countries SET CountryCode = 'SMR' WHERE Name = 'San Marino'
                                                UPDATE Countries SET CountryCode = 'STP' WHERE Name = 'Sao Tome and Principe'
                                                UPDATE Countries SET CountryCode = 'SAU' WHERE Name = 'Saudi Arabia'
                                                DELETE FROM Countries WHERE Name = 'Scotland'
                                                UPDATE Countries SET CountryCode = 'SEN' WHERE Name = 'Senegal'
                                                UPDATE Countries SET CountryCode = 'SRB' WHERE Name = 'Serbia'
                                                UPDATE Countries SET CountryCode = 'SYC' WHERE Name = 'Seychelles'
                                                UPDATE Countries SET CountryCode = 'SLE' WHERE Name = 'Sierra Leone'
                                                UPDATE Countries SET CountryCode = 'SGP' WHERE Name = 'Singapore'
                                                UPDATE Countries SET CountryCode = 'SXM' WHERE Name = 'Sint Maarten (Dutch part)'
                                                UPDATE Countries SET CountryCode = 'SVK' WHERE Name = 'Slovakia'
                                                UPDATE Countries SET CountryCode = 'SVN' WHERE Name = 'Slovenia'
                                                UPDATE Countries SET CountryCode = 'SLB' WHERE Name = 'Solomon Islands'
                                                UPDATE Countries SET CountryCode = 'SOM' WHERE Name = 'Somalia'
                                                UPDATE Countries SET CountryCode = 'ZAF' WHERE Name = 'South Africa'
                                                UPDATE Countries SET CountryCode = 'SGS' WHERE Name = 'South Georgia and the South Sandwich Islands'
                                                DELETE FROM Countries WHERE Name = 'South Korea'
                                                UPDATE Countries SET CountryCode = 'SSD' WHERE Name = 'South Sudan'
                                                UPDATE Countries SET CountryCode = 'ESP' WHERE Name = 'Spain'
                                                UPDATE Countries SET Name = 'Sri Lanka' WHERE Name = 'SriLanka'
                                                UPDATE Countries SET CountryCode = 'LKA' WHERE Name = 'Sri Lanka'
                                                UPDATE Countries SET CountryCode = 'SDN' WHERE Name = 'Sudan'
                                                UPDATE Countries SET CountryCode = 'SUR' WHERE Name = 'Suriname'
                                                UPDATE Countries SET CountryCode = 'SJM' WHERE Name = 'Svalbard and Jan Mayen'
                                                DELETE FROM Countries WHERE Name = 'Swaziland'
                                                UPDATE Countries SET CountryCode = 'SWE' WHERE Name = 'Sweden'
                                                UPDATE Countries SET CountryCode = 'CHE' WHERE Name = 'Switzerland'
                                                UPDATE Countries SET Name = 'Syrian Arab Republic' WHERE Name = 'Syria'
                                                UPDATE Countries SET CountryCode = 'SYR' WHERE Name = 'Syrian Arab Republic'
                                                UPDATE Countries SET CountryCode = 'TWN' WHERE Name = 'Taiwan, Province of China'
                                                UPDATE Countries SET CountryCode = 'TJK' WHERE Name = 'Tajikistan'
                                                UPDATE Countries SET Name = 'Tanzania, United Republic of' WHERE Name = 'Tanzania'
                                                UPDATE Countries SET CountryCode = 'TZA' WHERE Name = 'Tanzania, United Republic of'
                                                UPDATE Countries SET CountryCode = 'THA' WHERE Name = 'Thailand'
                                                UPDATE Countries SET CountryCode = 'TLS' WHERE Name = 'Timor-Leste'
                                                UPDATE Countries SET CountryCode = 'TGO' WHERE Name = 'Togo'
                                                UPDATE Countries SET CountryCode = 'TKL' WHERE Name = 'Tokelau'
                                                UPDATE Countries SET CountryCode = 'TON' WHERE Name = 'Tonga'
                                                UPDATE Countries SET CountryCode = 'TTO' WHERE Name = 'Trinidad and Tobago'
                                                UPDATE Countries SET CountryCode = 'TUN' WHERE Name = 'Tunisia'
                                                UPDATE Countries SET CountryCode = 'TUR' WHERE Name = 'Turkey'
                                                UPDATE Countries SET CountryCode = 'TKM' WHERE Name = 'Turkmenistan'
                                                UPDATE Countries SET CountryCode = 'TCA' WHERE Name = 'Turks and Caicos Islands'
                                                UPDATE Countries SET CountryCode = 'TUV' WHERE Name = 'Tuvalu'
                                                UPDATE Countries SET CountryCode = 'UGA' WHERE Name = 'Uganda'
                                                UPDATE Countries SET CountryCode = 'UKR' WHERE Name = 'Ukraine'
                                                UPDATE Countries SET CountryCode = 'ARE' WHERE Name = 'United Arab Emirates'
                                                UPDATE Countries SET Name = 'United Kingdom of Great Britain and Northern Ireland' WHERE Name = 'United Kingdom'
                                                UPDATE Countries SET CountryCode = 'GBR' WHERE Name = 'United Kingdom of Great Britain and Northern Ireland'
                                                UPDATE Countries SET Name = 'United States of America' WHERE Name = 'United States'
                                                UPDATE Countries SET CountryCode = 'USA' WHERE Name = 'United States of America'
                                                UPDATE Countries SET CountryCode = 'UMI' WHERE Name = 'United States Minor Outlying Islands'
                                                UPDATE Countries SET CountryCode = 'URY' WHERE Name = 'Uruguay'
                                                UPDATE Countries SET CountryCode = 'UZB' WHERE Name = 'Uzbekistan'
                                                UPDATE Countries SET CountryCode = 'VUT' WHERE Name = 'Vanuatu'
                                                UPDATE Countries SET Name = 'Venezuela (Bolivarian Republic of)' WHERE Name = 'Venezuela'
                                                UPDATE Countries SET CountryCode = 'VEN' WHERE Name = 'Venezuela (Bolivarian Republic of)'
                                                UPDATE Countries SET Name = 'Viet Nam' WHERE Name = 'Vietnam'
                                                UPDATE Countries SET CountryCode = 'VNM' WHERE Name = 'Viet Nam'
                                                UPDATE Countries SET Name = 'Virgin Islands (British)' WHERE Name = 'Virgin Islands, British'
                                                UPDATE Countries SET CountryCode = 'VGB' WHERE Name = 'Virgin Islands (British)'
                                                UPDATE Countries SET Name = 'Virgin Islands (U.S.)' WHERE Name = 'Virgin Islands, U.S.'
                                                UPDATE Countries SET CountryCode = 'VIR' WHERE Name = 'Virgin Islands (U.S.)'
                                                DELETE FROM Countries WHERE Name = 'Wales'
                                                UPDATE Countries SET CountryCode = 'WLF' WHERE Name = 'Wallis and Futuna'
                                                UPDATE Countries SET CountryCode = 'ESH' WHERE Name = 'Western Sahara'
                                                UPDATE Countries SET CountryCode = 'YEM' WHERE Name = 'Yemen'
                                                DELETE FROM Countries WHERE Name = 'Yugoslavia'
                                                UPDATE Countries SET CountryCode = 'ZMB' WHERE Name = 'Zambia'
                                                UPDATE Countries SET CountryCode = 'ZWE' WHERE Name = 'Zimbabwe'
                                                DELETE FROM Countries WHERE CountryCode = ''
                                                ");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Addresses",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE Addresses SET CountryCode = c.CountryCode FROM Addresses a INNER JOIN Countries c ON a.CountryId = c.Id");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Countries_CountryId",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Addresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryCode",
                table: "Addresses",
                column: "CountryCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Countries_CountryCode",
                table: "Addresses",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "CountryCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Countries_CountryCode",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryCode",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Addresses");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Countries",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Addresses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Countries_CountryId",
                table: "Addresses",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
