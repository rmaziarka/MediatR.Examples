namespace KnightFrank.Antares.Dal.Seed
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;
	using System.Linq;

	using KnightFrank.Antares.Dal.Model;
	using KnightFrank.Antares.Dal.Model.Resource;
	using KnightFrank.Antares.Dal.Model.User;

    internal class BusinessData
    {
		public static void Seed(KnightFrankContext context)
	    {
			List<Country> countries = context.Country.ToList();

			SeedBusiness(context, countries);

			context.SaveChanges();
		}

		private static void SeedBusiness(KnightFrankContext context, List<Country> countries)
		{
			SeedBusinessData(context, countries, "Abode", "IT");
			SeedBusinessData(context, countries, "Agence Boan", "FR");
			SeedBusinessData(context, countries, "Agence Val d'Isere", "FR");
			SeedBusinessData(context, countries, "Altitude Immobilier", "CH");
			SeedBusinessData(context, countries, "Bajan Services", "BB");
			SeedBusinessData(context, countries, "Belair Property", "MT");
			SeedBusinessData(context, countries, "Belles Demeures de France", "FR");
			SeedBusinessData(context, countries, "Broersma", "NL");
			SeedBusinessData(context, countries, "Casa di Campagna", "IT");
			SeedBusinessData(context, countries, "CIMALPES", "FR");
			SeedBusinessData(context, countries, "Classic French Homes", "FR");
			SeedBusinessData(context, countries, "Damianos Realty", "BS");
			SeedBusinessData(context, countries, "Dexter Realty", "CA");
			SeedBusinessData(context, countries, "Diana Morales Properties", "ES");
			SeedBusinessData(context, countries, "Douglas Elliman", "US");
			SeedBusinessData(context, countries, "Ek", "NO");
			SeedBusinessData(context, countries, "Elite Havens", "ID");
			SeedBusinessData(context, countries, "Emerald Casa", "CH");
			SeedBusinessData(context, countries, "Estate Net Prestige | Knight Frank", "FR");
			SeedBusinessData(context, countries, "Gerax Real Estate", "CH");
			SeedBusinessData(context, countries, "Ginesta Immobilien", "CH");
			SeedBusinessData(context, countries, "Guinnard Immobilier", "CH");
			SeedBusinessData(context, countries, "Holmes Sotogrande", "ES");
			SeedBusinessData(context, countries, "Hugo Skillington", "FR");
			SeedBusinessData(context, countries, "IIN Liguria", "IT");
			SeedBusinessData(context, countries, "Immobili di Prestigio", "IT");
			SeedBusinessData(context, countries, "Immobiliare Brunati", "IT");
			SeedBusinessData(context, countries, "Immobiliere Le Lion", "BE");
			SeedBusinessData(context, countries, "Ionian International", "GR");
			SeedBusinessData(context, countries, "IRG", "KY");
			SeedBusinessData(context, countries, "Janssens Immobilier", "FR");
			SeedBusinessData(context, countries, "Jumby Bay Island", "AG");
			SeedBusinessData(context, countries, "Knight Frank Australia", "AU");
			SeedBusinessData(context, countries, "Knight Frank Austria", "AT");
			SeedBusinessData(context, countries, "Knight Frank Bahrain", "BH");
			SeedBusinessData(context, countries, "Knight Frank Belgium", "BE");
			SeedBusinessData(context, countries, "Knight Frank Botswana", "BW");
			SeedBusinessData(context, countries, "Knight Frank Cambodia", "KH");
			SeedBusinessData(context, countries, "Knight Frank China", "CN");
			SeedBusinessData(context, countries, "Knight Frank Czech Republic", "CZ");
			SeedBusinessData(context, countries, "Knight Frank España", "ES");
			SeedBusinessData(context, countries, "Knight Frank France", "FR");
			SeedBusinessData(context, countries, "Knight Frank Germany", "DE");
			SeedBusinessData(context, countries, "Knight Frank Hong Kong", "HK");
			SeedBusinessData(context, countries, "Knight Frank India", "IN");
			SeedBusinessData(context, countries, "Knight Frank Indonesia", "ID");
			SeedBusinessData(context, countries, "Knight Frank Investment Management", "GB");
			SeedBusinessData(context, countries, "Knight Frank Ireland", "IE");
			SeedBusinessData(context, countries, "Knight Frank Italy", "IT");
			SeedBusinessData(context, countries, "Knight Frank Kenya", "KE");
			SeedBusinessData(context, countries, "Knight Frank KFAP Team", "SG");
			SeedBusinessData(context, countries, "Knight Frank Korea", "KR");
			SeedBusinessData(context, countries, "Knight Frank LLP", "GB");
			SeedBusinessData(context, countries, "Knight Frank Malawi", "MW");
			SeedBusinessData(context, countries, "Knight Frank Malaysia", "MY");
			SeedBusinessData(context, countries, "Knight Frank New Zealand", "NZ");
			SeedBusinessData(context, countries, "Knight Frank Nigeria", "NG");
			SeedBusinessData(context, countries, "Knight Frank Poland", "PL");
			SeedBusinessData(context, countries, "Knight Frank Romania", "RO");
			SeedBusinessData(context, countries, "Knight Frank Russia", "RU");
			SeedBusinessData(context, countries, "Knight Frank Singapore", "SG");
			SeedBusinessData(context, countries, "Knight Frank South Africa", "ZA");
			SeedBusinessData(context, countries, "Knight Frank Tanzania", "TZ");
			SeedBusinessData(context, countries, "Knight Frank Thailand", "TH");
			SeedBusinessData(context, countries, "Knight Frank UAE", "AE");
			SeedBusinessData(context, countries, "Knight Frank Uganda", "UG");
			SeedBusinessData(context, countries, "Knight Frank Vietnam", "VN");
			SeedBusinessData(context, countries, "Knight Frank Zambia", "ZM");
			SeedBusinessData(context, countries, "Knight Frank Zimbabwe", "ZW");
			SeedBusinessData(context, countries, "La Porte Property", "FR");
			SeedBusinessData(context, countries, "La Reale Domus", "IT");
			SeedBusinessData(context, countries, "Laurent Guerineau Immobilier", "FR");
			SeedBusinessData(context, countries, "Lucas Fox", "ES");
			SeedBusinessData(context, countries, "Mallorca Gold", "ES");
			SeedBusinessData(context, countries, "Menager Hug", "FR");
			SeedBusinessData(context, countries, "MJ Property", "MU");
			SeedBusinessData(context, countries, "Morzine Immobilier", "FR");
			SeedBusinessData(context, countries, "Mountain Base", "FR");
			SeedBusinessData(context, countries, "Naef Prestige | Knight Frank", "CH");
			SeedBusinessData(context, countries, "NL Real Estate", "NL");
			SeedBusinessData(context, countries, "OTTO Immobilien", "AT");
			SeedBusinessData(context, countries, "Partner Real Estate", "CH");
			SeedBusinessData(context, countries, "Ploumis Sotiropoulos", "GR");
			SeedBusinessData(context, countries, "Prime Properties", "PT");
			SeedBusinessData(context, countries, "Purslows Gascony", "FR");
			SeedBusinessData(context, countries, "PVN Real Estate Investments", "MC");
			SeedBusinessData(context, countries, "Regie Turrian SA", "CH");
			SeedBusinessData(context, countries, "Rego Realtors", "BM");
			SeedBusinessData(context, countries, "Riedel Immobilien", "DE");
			SeedBusinessData(context, countries, "Sadlers Property", "PT");
			SeedBusinessData(context, countries, "Sanda Marisuc", "HR");
			SeedBusinessData(context, countries, "Ser.Imm", "IT");
			SeedBusinessData(context, countries, "Sibarth Real Estate", "BL");
			SeedBusinessData(context, countries, "SLiguria Homes", "IT");
			SeedBusinessData(context, countries, "Smiths Gore", "VG");
			SeedBusinessData(context, countries, "The Advisers", "RO");
			SeedBusinessData(context, countries, "The Buying Solution", "GB");
			SeedBusinessData(context, countries, "The Mustique Company", "VC");
			SeedBusinessData(context, countries, "thinkSicily", "IT");
			SeedBusinessData(context, countries, "Tuscany Inside Out", "IT");
			SeedBusinessData(context, countries, "USA - External", "US");
			SeedBusinessData(context, countries, "Venice Real Estate", "IT");
			SeedBusinessData(context, countries, "Villa Contact", "ES");
			SeedBusinessData(context, countries, "Worx", "PT");
		}

		private static void SeedBusinessData(KnightFrankContext context, IEnumerable<Country> countries, string name, string countryCode)
		{
			var business = new Business
			{
				Name = name,
				CountryId = GetCountryIdByCode(countries, countryCode)
			};

			context.Business.AddOrUpdate(x => x.Name, business);
		}

		private static Guid GetCountryIdByCode(IEnumerable<Country> countries, string countryCode)
		{
			Country country = countries.FirstOrDefault(c => c.IsoCode == countryCode);

			return country?.Id ?? default(Guid);
		}
	}
}