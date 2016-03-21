namespace KnightFrank.Antares.Dal.Seed
{
	using System;
	using System.Data.Entity.Migrations;
	using System.Linq;

	using KnightFrank.Antares.Dal.Model;

    internal class BusinessData
    {
		public static void Seed(KnightFrankContext context)
	    {
			SeedBusiness(context);
		}

		private static void SeedBusiness(KnightFrankContext context)
		{
			SeedBusinessData(context, "Abode", "IT");
			SeedBusinessData(context, "Agence Boan", "FR");
			SeedBusinessData(context, "Agence Val d'Isere", "FR");
			SeedBusinessData(context, "Altitude Immobilier", "CH");
			SeedBusinessData(context, "Bajan Services", "BB");
			SeedBusinessData(context, "Belair Property", "MT");
			SeedBusinessData(context, "Belles Demeures de France", "FR");
			SeedBusinessData(context, "Broersma", "NL");
			SeedBusinessData(context, "Casa di Campagna", "IT");
			SeedBusinessData(context, "CIMALPES", "FR");
			SeedBusinessData(context, "Classic French Homes", "FR");
			SeedBusinessData(context, "Damianos Realty", "BS");
			SeedBusinessData(context, "Dexter Realty", "CA");
			SeedBusinessData(context, "Diana Morales Properties", "ES");
			SeedBusinessData(context, "Douglas Elliman", "US");
			SeedBusinessData(context, "Ek", "NO");
			SeedBusinessData(context, "Elite Havens", "ID");
			SeedBusinessData(context, "Emerald Casa", "CH");
			SeedBusinessData(context, "Estate Net Prestige | Knight Frank", "FR");
			SeedBusinessData(context, "Gerax Real Estate", "CH");
			SeedBusinessData(context, "Ginesta Immobilien", "CH");
			SeedBusinessData(context, "Guinnard Immobilier", "CH");
			SeedBusinessData(context, "Holmes Sotogrande", "ES");
			SeedBusinessData(context, "Hugo Skillington", "FR");
			SeedBusinessData(context, "IIN Liguria", "IT");
			SeedBusinessData(context, "Immobili di Prestigio", "IT");
			SeedBusinessData(context, "Immobiliare Brunati", "IT");
			SeedBusinessData(context, "Immobiliere Le Lion", "BE");
			SeedBusinessData(context, "Ionian International", "GR");
			SeedBusinessData(context, "IRG", "KY");
			SeedBusinessData(context, "Janssens Immobilier", "FR");
			SeedBusinessData(context, "Jumby Bay Island", "AG");
			SeedBusinessData(context, "Knight Frank Australia", "AU");
			SeedBusinessData(context, "Knight Frank Austria", "AT");
			SeedBusinessData(context, "Knight Frank Bahrain", "BH");
			SeedBusinessData(context, "Knight Frank Belgium", "BE");
			SeedBusinessData(context, "Knight Frank Botswana", "BW");
			SeedBusinessData(context, "Knight Frank Cambodia", "KH");
			SeedBusinessData(context, "Knight Frank China", "CN");
			SeedBusinessData(context, "Knight Frank Czech Republic", "CZ");
			SeedBusinessData(context, "Knight Frank España", "ES");
			SeedBusinessData(context, "Knight Frank France", "FR");
			SeedBusinessData(context, "Knight Frank Germany", "DE");
			SeedBusinessData(context, "Knight Frank Hong Kong", "HK");
			SeedBusinessData(context, "Knight Frank India", "IN");
			SeedBusinessData(context, "Knight Frank Indonesia", "ID");
			SeedBusinessData(context, "Knight Frank Investment Management", "GB");
			SeedBusinessData(context, "Knight Frank Ireland", "IE");
			SeedBusinessData(context, "Knight Frank Italy", "IT");
			SeedBusinessData(context, "Knight Frank Kenya", "KE");
			SeedBusinessData(context, "Knight Frank KFAP Team", "SG");
			SeedBusinessData(context, "Knight Frank Korea", "KR");
			SeedBusinessData(context, "Knight Frank LLP", "GB");
			SeedBusinessData(context, "Knight Frank Malawi", "MW");
			SeedBusinessData(context, "Knight Frank Malaysia", "MY");
			SeedBusinessData(context, "Knight Frank New Zealand", "NZ");
			SeedBusinessData(context, "Knight Frank Nigeria", "NG");
			SeedBusinessData(context, "Knight Frank Poland", "PL");
			SeedBusinessData(context, "Knight Frank Romania", "RO");
			SeedBusinessData(context, "Knight Frank Russia", "RU");
			SeedBusinessData(context, "Knight Frank Singapore", "SG");
			SeedBusinessData(context, "Knight Frank South Africa", "ZA");
			SeedBusinessData(context, "Knight Frank Tanzania", "TZ");
			SeedBusinessData(context, "Knight Frank Thailand", "TH");
			SeedBusinessData(context, "Knight Frank UAE", "AE");
			SeedBusinessData(context, "Knight Frank Uganda", "UG");
			SeedBusinessData(context, "Knight Frank Vietnam", "VN");
			SeedBusinessData(context, "Knight Frank Zambia", "ZM");
			SeedBusinessData(context, "Knight Frank Zimbabwe", "ZW");
			SeedBusinessData(context, "La Porte Property", "FR");
			SeedBusinessData(context, "La Reale Domus", "IT");
			SeedBusinessData(context, "Laurent Guerineau Immobilier", "FR");
			SeedBusinessData(context, "Lucas Fox", "ES");
			SeedBusinessData(context, "Mallorca Gold", "ES");
			SeedBusinessData(context, "Menager Hug", "FR");
			SeedBusinessData(context, "MJ Property", "MU");
			SeedBusinessData(context, "Morzine Immobilier", "FR");
			SeedBusinessData(context, "Mountain Base", "FR");
			SeedBusinessData(context, "Naef Prestige | Knight Frank", "CH");
			SeedBusinessData(context, "NL Real Estate", "NL");
			SeedBusinessData(context, "OTTO Immobilien", "AT");
			SeedBusinessData(context, "Partner Real Estate", "CH");
			SeedBusinessData(context, "Ploumis Sotiropoulos", "GR");
			SeedBusinessData(context, "Prime Properties", "PT");
			SeedBusinessData(context, "Purslows Gascony", "FR");
			SeedBusinessData(context, "PVN Real Estate Investments", "MC");
			SeedBusinessData(context, "Regie Turrian SA", "CH");
			SeedBusinessData(context, "Rego Realtors", "BM");
			SeedBusinessData(context, "Riedel Immobilien", "DE");
			SeedBusinessData(context, "Sadlers Property", "PT");
			SeedBusinessData(context, "Sanda Marisuc", "HR");
			SeedBusinessData(context, "Ser.Imm", "IT");
			SeedBusinessData(context, "Sibarth Real Estate", "BL");
			SeedBusinessData(context, "SLiguria Homes", "IT");
			SeedBusinessData(context, "Smiths Gore", "VG");
			SeedBusinessData(context, "The Advisers", "RO");
			SeedBusinessData(context, "The Buying Solution", "GB");
			SeedBusinessData(context, "The Mustique Company", "VC");
			SeedBusinessData(context, "thinkSicily", "IT");
			SeedBusinessData(context, "Tuscany Inside Out", "IT");
			SeedBusinessData(context, "USA - External", "US");
			SeedBusinessData(context, "Venice Real Estate", "IT");
			SeedBusinessData(context, "Villa Contact", "ES");
			SeedBusinessData(context, "Worx", "PT");
		}

		private static void SeedBusinessData(KnightFrankContext context, string name, string countryCode)
		{
			var business = new Business
			{
				Name = name,
				CountryId = GetCountryIdByCode(context, countryCode)
			};

			context.Business.AddOrUpdate(x => x.Name, business);
			context.SaveChanges();
		}

		private static Guid GetCountryIdByCode(KnightFrankContext context, string countryCode)
		{
			Country country = context.Country.FirstOrDefault(c => c.IsoCode == countryCode);

			return country?.Id ?? default(Guid);
		}
	}
}