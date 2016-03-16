namespace KnightFrank.Antares.Dal.Seed
{
	using System;
	using System.Data.Entity.Migrations;
	using System.Linq;

	using KnightFrank.Antares.Dal.Model;

    internal class CountryData
    {
		private static Guid CsLocaleId { get; set; }
		private static Guid DeLocaleId { get; set; }
		private static Guid EnLocaleId { get; set; }
		private static Guid EsLocaleId { get; set; }
		private static Guid FrLocaleId { get; set; }
		private static Guid PlLocaleId { get; set; }
		private static Guid SvLocaleId { get; set; }

		public static void Seed(KnightFrankContext context)
	    {
			SetLocaleIds(context);
			SeedCountry(context);
		}

	    private static void SetLocaleIds(KnightFrankContext context)
	    {
			CsLocaleId = context.Locale.Where(x => x.IsoCode == "cs").Select(x => x.Id).Single();
			DeLocaleId = context.Locale.Where(x => x.IsoCode == "de").Select(x => x.Id).Single();
			EnLocaleId = context.Locale.Where(x => x.IsoCode == "en").Select(x => x.Id).Single();
			EsLocaleId = context.Locale.Where(x => x.IsoCode == "es").Select(x => x.Id).Single();
			FrLocaleId = context.Locale.Where(x => x.IsoCode == "fr").Select(x => x.Id).Single();
			PlLocaleId = context.Locale.Where(x => x.IsoCode == "pl").Select(x => x.Id).Single();
			SvLocaleId = context.Locale.Where(x => x.IsoCode == "sv").Select(x => x.Id).Single();
		}

		private static void SeedCountry(KnightFrankContext context)
		{
			SeedCountryData(context, "AD", "Andorra", "Andorra", "Andorra", "Andorra", "Andorre", "Andora", "Andorra");
			SeedCountryData(context, "AE", "Spojené arabské emiráty", "die Vereinigten Arabischen Emirate", "United Arab Emirates", "Emiratos Árabes Unidos", "Émirats arabes unis ", "Zjednoczone Emiraty Arabskie", "Förenade Arabemiraten");
			SeedCountryData(context, "AF", "Afghánistán", "Afghanistan", "Afghanistan", "Afganistán", "Afghanistan", "Afganistan", "Afghanistan");
			SeedCountryData(context, "AG", "Antigua a Barbuda", "Antigua und Barbuda", "Antigua and Barbuda", "Antigua y Barbuda", "Antigua-et-Barbuda", "Antigua i Barbuda", "Antigua och Barbuda");
			SeedCountryData(context, "AI", "Anguilla", "Anguilla", "Anguilla", "Anguila", "Anguilla", "Anguilla", "Anguilla");
			SeedCountryData(context, "AL", "Albánie", "Albanien", "Albania", "Albania", "Albanie", "Albania", "Albanien");
			SeedCountryData(context, "AM", "Arménie", "Armenien", "Armenia", "Armenia", "Arménie", "Armenia", "Armenien");
			SeedCountryData(context, "AO", "Angola", "Angola", "Angola", "Angola", "Angola", "Angola", "Angola");
			SeedCountryData(context, "AR", "Argentina", "Argentinien", "Argentina", "Argentina", "Argentine", "Argentyna", "Argentina");
			SeedCountryData(context, "AT", "Rakousko", "Österreich ", "Austria", "Austria", "Autriche", "Austria", "Österrike");
			SeedCountryData(context, "AU", "Austrálie", "Australien", "Australia", "Australia", "Australie", "Australia", "Australien");
			SeedCountryData(context, "AW", "Aruba", "Aruba", "Aruba", "Aruba", "Aruba", "Aruba", "Aruba");
			SeedCountryData(context, "AZ", "Ázerbájdžán", "Aserbaidschan", "Azerbaijan", "Azerbaiyán", "Azerbaïdjan", "Azerbejdzan", "Azerbajdzjan");
			SeedCountryData(context, "BA", "Bosna a Hercegovina", "Bosnien-Herzegowina", "Bosnia & Herzegowina", "Bosnia y Herzegovina", "Bosnie-Herzégovine", "Bosnia i Hercegowina", "Bosnien och Hercegovina");
			SeedCountryData(context, "BB", "Barbados", "Barbados", "Barbados", "Barbados", "Barbade", "Barbados", "Barbados");
			SeedCountryData(context, "BD", "Bangladéš", "Bangladesch", "Bangladesh", "Bangladés", "Bangladesh", "Bangladesz", "Bangladesh");
			SeedCountryData(context, "BE", "Belgie", "Belgien", "Belgium", "Bélgica", "Belgique", "Belgia", "Belgien");
			SeedCountryData(context, "BF", "Burkina Faso", "Burkina Faso", "Burkina Faso", "Burkina Faso", "Burkina Faso", "Burkina Faso", "Burkina Faso");
			SeedCountryData(context, "BG", "Bulharsko", "Bulgarien", "Bulgaria", "Bulgaria", "Bulgarie", "Bulgaria", "Bulgarien");
			SeedCountryData(context, "BH", "Bahrajn", "Bahrain", "Bahrain", "Bahréin", "Bahreïn", "Bahrajn", "Bahrain");
			SeedCountryData(context, "BI", "Burundi", "Burundi", "Burundi", "Burundi", "Burundi", "Burundi", "Burundi");
			SeedCountryData(context, "BJ", "Republika Benin", "Republik Benin", "Republic of Benin", "República de Benín", "République du Bénin", "Benin", "Republiken Benin");
			SeedCountryData(context, "BL", "Saint Barthélemy", "Saint Barthélemy", "St Barts", "Saint Barthélemy", "Saint Barthélemy", "Saint Barthélemy", "Saint Barthélemy");
			SeedCountryData(context, "BM", "Bermudy", "Bermuda", "Bermuda", "Bermudas", "Bermudes", "Bermudy", "Bermuda");
			SeedCountryData(context, "BN", "Brunej Darussalam", "Brunei Darussalam", "Brunei Darussalam", "Brunei Darussalam", "Brunéi Darussalam", "Brunei", "Brunei Darussalam");
			SeedCountryData(context, "BO", "Bolívie", "Bolivien", "Bolivia", "Bolivia", "Bolivie", "Boliwia", "Bolivia");
			SeedCountryData(context, "BR", "Brazílie", "Brazilien", "Brazil", "Brasil", "Brésil", "Brazylia", "Brasilien");
			SeedCountryData(context, "BS", "Bahamy", "Bahamas", "Bahamas", "Bahamas", "Bahamas", "Bahamy", "Bahamas");
			SeedCountryData(context, "BT", "Bhútán", "Bhutan", "Bhutan", "Bután", "Bhoutan", "Bhutan", "Bhutan");
			SeedCountryData(context, "BW", "Botswana", "Botswana", "Botswana", "Botsuana", "Botswana", "Botswana", "Botswana");
			SeedCountryData(context, "BY", "Belorusko", "Weissrussland", "Republic of Belarus", "República de Bielorrusia", "République du Bélarus", "Bialorus", "Republiken Vitryssland");
			SeedCountryData(context, "BZ", "Belize", "Belize", "Belize", "Belice", "Bélize", "Belize", "Belize");
			SeedCountryData(context, "CA", "Kanada", "Kanada", "Canada", "Canadá", "Canada", "Kanada", "Kanada");
			SeedCountryData(context, "CC", "Kokosové ostrovy", "Kokosinseln (Keelinginseln)", "Cocos Islands", "Islas Cocos", "Îles Cocos ", "Wyspy Kokosowe", "Kokosöarna");
			SeedCountryData(context, "CD", "Republika Kongo", "Republik Kongo", "The Republic of the Congo", "República del Congo", "République du Congo", "Kongo", "Republiken Kongo");
			SeedCountryData(context, "CF", "Stredoafrická republika", "Zentralafrikanische Republik", "Central African Republic", "República centroafricana", "République centrafricaine", "Republika Srodkowoafrykanska", "Centralafrikanska Republiken");
			SeedCountryData(context, "CH", "Švýcarsko", "Schweiz", "Switzerland", "Suiza", "Suisse", "Szwajcaria", "Schweiz");
			SeedCountryData(context, "CI", "Pobreží slonoviny", "Elfenbeinküste", "Ivory Coast", "Costa de Marfil", "Côte d'Ivoire", "Wybrzeze Kosci Sloniowej", "Elfenbenskusten");
			SeedCountryData(context, "CK", "Cookovy ostrovy", "die Cookinseln", "The Cook Islands", "Islas Cook", "Les Îles Cook", "Wyspy Cooka", "Cooköarna");
			SeedCountryData(context, "CL", "Chile", "Chile", "Chile", "Chile", "Chili", "Chile", "Chile");
			SeedCountryData(context, "CM", "Kamerun", "Kamerun", "Cameroon", "Camerún", "Cameroun", "Kamerun", "Kamerun");
			SeedCountryData(context, "CN", "Cína", "China", "China", "China", "Chine", "Chiny", "Kina");
			SeedCountryData(context, "CO", "Kolumbie", "Kolumbien", "Colombia", "Colombia", "Colombie", "Kolumbia", "Colombia");
			SeedCountryData(context, "CR", "Kostarika", "Costa Rica", "Costa Rica", "Costa Rica", "Costa Rica", "Kostaryka", "Costa Rica");
			SeedCountryData(context, "CU", "Kuba", "Kuba", "Cuba", "Cuba", "Cuba", "Kuba", "Kuba");
			SeedCountryData(context, "CV", "Kapverdy", "Kap Verde", "Cape Verde", "Cabo Verde", "Cap Vert", "Republika Zielonego Przyladka", "Kap Verde");
			SeedCountryData(context, "CW", "Curaçao", "Curacao", "Curaçao", "Curazao", "Curaçao", "Curaçao", "Curaçao");
			SeedCountryData(context, "CX", "Vánocní ostrov", "Weihnachtsinsel", "Christmas Island", "Isla Christmas", "Îles Christmas ", "Wyspa Bozego Narodzenia", "Julön");
			SeedCountryData(context, "CY", "Kypr", "Zypern", "Cyprus", "Chipre", "Chypre", "Cypr", "Cypern");
			SeedCountryData(context, "CZ", "Ceská republika", "Tschechische Republik", "Czech Republic", "República Checa", "République tchèque", "Republika Czeska", "Republiken Tjeckien");
			SeedCountryData(context, "DE", "Nemecko", "Deutschland", "Germany", "Alemania", "Allemagne", "Niemcy", "Tyskland");
			SeedCountryData(context, "DJ", "Džibuti", "Dschibuti", "Djibouti", "Yibuti", "Djibouti", "Dzibuti", "Djibouti");
			SeedCountryData(context, "DK", "Dánsko", "Dänemark", "Denmark", "Dinamarca", "Danemark", "Dania", "Danmark");
			SeedCountryData(context, "DM", "Dominika", "Dominica", "Dominica", "Dominica", "Dominique", "Dominika", "Dominica");
			SeedCountryData(context, "DO", "Dominikánská republika", "Dominikanische Republik", "Dominican Republic", "República Dominicana", "République dominicaine", "Dominikana", "Dominikanska Republiken");
			SeedCountryData(context, "DZ", "Alžír", "Algerien", "Algeria", "Argelia", "Algérie", "Algieria", "Algeriet");
			SeedCountryData(context, "EC", "Ekvádor", "Ecuador", "Ecuador", "Ecuador", "Écuador", "Ekwador", "Ecuador");
			SeedCountryData(context, "EE", "Estonsko", "Estland", "Estonia", "Estonia", "Estonie", "Estonia", "Estland");
			SeedCountryData(context, "EG", "Egypt", "Ägypten", "Egypt", "Egipto", "Égypte", "Egipt", "Egypten");
			SeedCountryData(context, "EH", "Západní Sahara", "Westliche Sahara", "Western Sahara", "Sahara Occidental", "Sahara occidental", "Sahara Zachodnia", "Västsahara");
			SeedCountryData(context, "ER", "Eritrea", "Eritrea", "Eritrea", "Eritrea", "Érythrée", "Erytrea", "Eritrea");
			SeedCountryData(context, "ES", "Španelsko", "Spanien", "Spain", "España", "Espagne", "Hiszpania", "Spanien");
			SeedCountryData(context, "ET", "Etiopie", "Äthiopien", "Ethiopia", "Etiopía", "Éthiopie", "Etiopia", "Etiopien");
			SeedCountryData(context, "FI", "Finsko", "Finnland", "Finland", "Finlandia", "Finlande", "Finlandia", "Finland");
			SeedCountryData(context, "FJ", "Ostrovy Fidži", "Fidschi Inseln", "Fiji Islands", "Islas Fiji", "Îles Fidji ", "Fidzi", "Fijiöarna");
			SeedCountryData(context, "FK", "Falklandy", "Falklandinseln", "Falkland Islands", "Islas Malvinas", "Îles Falkland", "Falklandy", "Falklandsöarna");
			SeedCountryData(context, "FM", "Mikronésie", "Mikronesien", "Micronesia", "Micronesia", "Micronésie ", "Mikronezja", "Mikronesien");
			SeedCountryData(context, "FO", "Faerské ostrovy", "Färöer Inseln", "Faroe Islands", "Islas Feroe", "Îles Féroé", "Wyspy Owcze", "Färöarna");
			SeedCountryData(context, "FR", "Francie", "Frankreich", "France", "Francia", "France", "Francja", "Frankrike");
			SeedCountryData(context, "GA", "Gabon", "Gabun", "Gabon", "Gabón", "Gabon", "Gabon", "Gabon");
			SeedCountryData(context, "GB", "Velká Británie", "Großbritannien", "United Kingdom", "Reino Unido", "Royaume-Uni", "Wielka Brytania", "Storbritannien");
			SeedCountryData(context, "GD", "Grenada", "Grenada", "Grenada", "Granada", "Grenade", "Grenada", "Grenada");
			SeedCountryData(context, "GE", "Gruzie", "Georgien", "Georgia", "Georgia", "Géorgie ", "Gruzja", "Georgien");
			SeedCountryData(context, "GF", "Francouzská Guyana", "Französisch-Guayana", "French Guiana", "Guayana Francesa", "Guyane française", "Gujana Francuska", "Franska Guyana");
			SeedCountryData(context, "GG", "Guernsey", "Guernsey", "Guernsey", "Guernsey", "Guernsey", "Guernsey", "Guernsey");
			SeedCountryData(context, "GH", "Ghana", "Ghana", "Ghana", "Ghana", "Ghana", "Ghana", "Ghana");
			SeedCountryData(context, "GI", "Gibraltar", "Gibraltar", "Gibraltar", "Gibraltar", "Gibraltar", "Gibraltar", "Gibraltar");
			SeedCountryData(context, "GL", "Grónsko", "Grönland", "Greenland", "Groenlandia", "Groenland ", "Grenlandia", "Grönland");
			SeedCountryData(context, "GM", "Gambie", "Gambia", "Gambia", "Gambia", "Gambie", "Gambia", "Gambia");
			SeedCountryData(context, "GN", "Guinea", "Guinea", "Guinea", "Guinea", "Guinée", "Gwinea", "Guinea");
			SeedCountryData(context, "GP", "Guadeloupe", "Guadalupe", "Guadeloupe", "Guadalupe", "Guadeloupe", "Gwadelupa", "Guadeloupe");
			SeedCountryData(context, "GQ", "Rovníková Guinea", "Äquatorialguinea", "Equatorial Guinea", "Guinea Ecuatorial", "Guinée équatoriale", "Gwinea Równikowa", "Ekvatorialguinea");
			SeedCountryData(context, "GR", "Recko", "Griechenland", "Greece", "Grecia", "Grèce", "Grecja", "Grekland");
			SeedCountryData(context, "GT", "Guatemala", "Guatemala", "Guatemala", "Guatemala", "Guatemala", "Gwatemala", "Guatemala");
			SeedCountryData(context, "GU", "Guam", "Guam", "Guam", "Guam", "Guam", "Guam", "Guam");
			SeedCountryData(context, "GW", "Guinea-Bissau", "Guinea-Bissau", "Guinea-Bissau", "Guinea-Bisáu", "Guinée-Bissau", "Gwinea Bissau", "Guinea-Bissau");
			SeedCountryData(context, "GY", "Guyana", "Guyana", "Guyana", "Guyana", "Guyane", "Gujana", "Guyana");
			SeedCountryData(context, "HK", "Hong Kong", "Hongkong", "Hong Kong", "Hong Kong", "Hong Kong", "Hongkong", "Hong Kong");
			SeedCountryData(context, "HN", "Honduras", "Honduras", "Honduras", "Honduras", "Honduras", "Honduras", "Honduras");
			SeedCountryData(context, "HR", "Chorvatsko", "Kroatien", "Croatia", "Croacia", "Croatie", "Chorwacja", "Kroatien");
			SeedCountryData(context, "HT", "Haiti", "Haiti", "Haiti", "Haití", "Haïti", "Haiti", "Haiti");
			SeedCountryData(context, "HU", "Madarsko", "Ungarn", "Hungary", "Hungría", "Hongrie", "Wegry", "Ungern");
			SeedCountryData(context, "ID", "Indonésie", "Indonesien", "Indonesia", "Indonesia", "Indonésie", "Indonezja", "Indonesien");
			SeedCountryData(context, "IE", "Irsko", "Irland", "Ireland", "Irlanda", "Irlande", "Irlandia", "Irland");
			SeedCountryData(context, "IL", "Izrael", "Israel", "Israel", "Israel", "Israël ", "Izrael", "Israel");
			SeedCountryData(context, "IM", "Isle of Man", "Isle Of Man", "Isle of Man", "Isla de Man", "Île de Man", "Wyspa Man", "Isle of Man");
			SeedCountryData(context, "IN", "Indie", "Indien", "India", "India", "Inde", "Indie", "Indien");
			SeedCountryData(context, "IQ", "Irák", "Iraq", "Iraq", "Iraq", "Irak", "Irak", "Irak");
			SeedCountryData(context, "IR", "Írán, Islámská republika", "Islamische Republik Iran", "Iran, Islamic Republic of", "Irán, República Islámica de", "Iran, République islamique de", "Iran", "Islamiska republiken iran");
			SeedCountryData(context, "IS", "Island", "Island", "Iceland", "Islandia", "Islande", "Islandia", "Island");
			SeedCountryData(context, "IT", "Itálie", "Italien", "Italy", "Italia", "Italie", "Wlochy", "Italien");
			SeedCountryData(context, "JE", "Jersey", "Jersey", "Jersey", "Jersey", "Jersey", "Jersey", "Jersey");
			SeedCountryData(context, "JM", "Jamajka", "Jamaika", "Jamaica", "Jamaica", "Jamaïque ", "Jamajka", "Jamaica");
			SeedCountryData(context, "JO", "Jordánsko", "Jordanien", "Jordan", "Jordania", "Jordanie", "Jordania", "Jordanien");
			SeedCountryData(context, "JP", "Japonsko", "Japan", "Japan", "Japón", "Japon", "Japonia", "Japan");
			SeedCountryData(context, "KE", "Kena", "Kenia", "Kenya", "Kenia", "Kenya", "Kenia", "Kenya");
			SeedCountryData(context, "KG", "Kyrgyzstán", "Kirgisische Republik", "Kyrgyz Republic", "Kirguistán", "République kirghize", "Kirgistan", "Republiken Kirgizistan");
			SeedCountryData(context, "KH", "Kambodža", "Kambodscha", "Cambodia", "Camboya", "Cambodge", "Kambodża", "Kambodja");
			SeedCountryData(context, "KI", "Kiribati", "Kiribati", "Kiribati", "Kiribati", "Kiribati", "Kiribati", "Kiribati");
			SeedCountryData(context, "KM", "Komory", "Komoren", "Comoros", "Comoras", "Comores", "Komory", "Komorerna");
			SeedCountryData(context, "KN", "Saint Kitts and Nevis", "St. Kitt und Nevis", "Saint Kitts and Nevis", "San Cristóbal y Nieves", "Saint-Kitts-et-Nevis", "Saint Kitts i Nevis", "Saint Kitts och Nevis");
			SeedCountryData(context, "KP", "Korea (Severní)", "Nordkorea", "Korea (North)", "Corea (Norte)", "Corée du Nord ", "Korea (Pólnocna)", "Nordkorea");
			SeedCountryData(context, "KR", "Korea (Jižní)", "Südkorea", "Korea (South)", "Corea (Sur)", "Corée du Sud ", "Korea (Poludniowa)", "Sydkorea");
			SeedCountryData(context, "KW", "Kuvajt", "Kuwait", "Kuwait", "Kuwait", "Koweït", "Kuwejt", "Kuwait");
			SeedCountryData(context, "KY", "Kajmanské ostrovy", "Kaimaninseln", "Cayman Islands", "Islas Caimán", "Îles Caïmans", "Kajmany", "Caymanöarna");
			SeedCountryData(context, "KZ", "Kazachstán", "Kasachstan", "Kazakhstan", "Kazajistán", "Kazakhstan", "Kazachstan", "Kazakstan");
			SeedCountryData(context, "LA", "Laos", "Laos", "Laos", "Laos", "Laos", "Laos", "Laos");
			SeedCountryData(context, "LB", "Libanon", "Libanon", "Lebanon", "Líbano", "Liban", "Liban", "Libanon");
			SeedCountryData(context, "LC", "Saint Lucia", "St. Lucia", "Saint Lucia", "Santa Lucía", "Sainte-Lucie", "Saint Lucia", "Saint Lucia");
			SeedCountryData(context, "LI", "Lichtenštejnsko", "Lichtenstein", "Liechtenstein", "Liechtenstein", "Liechtenstein", "Liechtenstein", "Liechtenstein");
			SeedCountryData(context, "LK", "Srí Lanka", "Sri Lanka", "Sri Lanka", "Sri Lanka", "Sri Lanka", "Sri Lanka", "Sri Lanka");
			SeedCountryData(context, "LR", "Libérie", "Liberia", "Liberia", "Liberia", "Libéria", "Liberia", "Liberia");
			SeedCountryData(context, "LS", "Lesotho", "Lesotho", "Lesotho", "Lesoto", "Lesotho", "Lesotho", "Lesotho");
			SeedCountryData(context, "LT", "Litva", "Litauen", "Lithuania", "Lituania", "Lithuanie", "Litwa", "Litauen");
			SeedCountryData(context, "LU", "Lucembursko", "Luxemburg", "Luxembourg", "Luxemburgo", "Luxembourg", "Luksemburg", "Luxemburg");
			SeedCountryData(context, "LV", "Lotyšsko", "Lettland", "Latvia", "Letonia", "Lettonie", "Lotwa", "Lettland");
			SeedCountryData(context, "LY", "Velká libyjská arabská lidová republika", "Lybisch-Arabische Dschamahirjia", "Libyan Arab Jamahiriya", "Libia", "Jamahiriya arabe libyenne", "Libia", "Libyen");
			SeedCountryData(context, "MA", "Maroko", "Marokko", "Morocco", "Marruecos", "Maroc ", "Maroko", "Marocko");
			SeedCountryData(context, "MC", "Monako", "Monaco", "Monaco", "Mónaco", "Monaco", "Monako", "Monaco");
			SeedCountryData(context, "MD", "Moldavsko", "Moldawien/ Republik Moldau", "Moldova, Republic of", "Moldavia, República de", "Moldavie, République de", "Moldawia", "Moldavien");
			SeedCountryData(context, "ME", "Cerná Hora", "Montenegro", "Montenegro", "Montenegro", "Monténégro ", "Czarnogóra", "Montenegro");
			SeedCountryData(context, "MG", "Madagaskar", "Madagaskar", "Madagascar", "Madagascar", "Madagascar", "Madagaskar", "Madagaskar");
			SeedCountryData(context, "MH", "Marshallovy ostrovy", "Marshallinseln", "Marshall Islands", "Islas Marshall", "Îles Marshall", "Wyspy Marshalla", "Marshallöarna");
			SeedCountryData(context, "MK", "Makedonie", "Mazedonien", "Macedonia", "Macedonia", "Macédoine ", "Macedonia", "Makedonien");
			SeedCountryData(context, "ML", "Mali", "Mali", "Mali", "Mali", "Mali", "Mali", "Mali");
			SeedCountryData(context, "MM", "Burma", "Burma", "Myanmar", "Birmania", "Birmanie", "Birma", "Burma");
			SeedCountryData(context, "MN", "Mongolsko", "Mongolei", "Mongolia", "Mongolia", "Mongolie", "Mongolia", "Mongoliet");
			SeedCountryData(context, "MO", "Macao", "Macao", "Macao", "Macao", "Macao", "Makau", "Macao");
			SeedCountryData(context, "MQ", "Martinique", "Martinique", "Martinique", "Martinica", "Martinique", "Martynika", "Martinique");
			SeedCountryData(context, "MR", "Mauritánie", "Mauretanien", "Mauritania", "Mauritania", "Mauritanie", "Mauretania", "Mauretanien");
			SeedCountryData(context, "MT", "Malta", "Malta", "Malta", "Malta", "Malte", "Malta", "Malta");
			SeedCountryData(context, "MU", "Mauricius", "Mauritius", "Mauritius", "Mauricio", "Maurice ", "Mauritius", "Mauritius");
			SeedCountryData(context, "MV", "Maledivy", "Malediven", "Maldives", "Maldivas", "Maldives", "Malediwy", "Maldiverna");
			SeedCountryData(context, "MW", "Malawi", "Malawi", "Malawi", "Malaui", "Malawi", "Malawi", "Malawi");
			SeedCountryData(context, "MX", "Mexiko", "Mexiko", "Mexico", "México", "Mexique ", "Meksyk", "Mexiko");
			SeedCountryData(context, "MY", "Malajsie", "Malaysia", "Malaysia", "Malasia", "Malaisie", "Malezja", "Malaysia");
			SeedCountryData(context, "MZ", "Mozambik", "Mosambik", "Mozambique", "Mozambique", "Mozambique", "Mozambik", "Moçambique");
			SeedCountryData(context, "NA", "Namibie", "Nambibia", "Namibia", "Namibia", "Namibie", "Namibia", "Namibia");
			SeedCountryData(context, "NC", "Nová Kaledonie", "Neukaledonien", "New Caledonia", "Nueva Caledonia", "Nouvelle-Calédonie", "Nowa Kaledonia", "Nya Kaledonien");
			SeedCountryData(context, "NE", "Niger", "Niger", "Niger", "Níger", "Niger", "Niger", "Niger");
			SeedCountryData(context, "NG", "Nigérie", "Nigeria", "Nigeria", "Nigeria", "Nigéria", "Nigeria", "Nigeria");
			SeedCountryData(context, "NI", "Nikaragua", "Nicaragua", "Nicaragua", "Nicaragua", "Nicaragua", "Nikaragua", "Nicaragua");
			SeedCountryData(context, "NL", "Nizozemí", "Niederlande", "Netherlands", "Países Bajos", "Pays-Bas", "Holandia", "Nederländerna");
			SeedCountryData(context, "NO", "Norsko", "Norwegen", "Norway", "Noruega", "Norvège ", "Norwegia", "Norge");
			SeedCountryData(context, "NP", "Nepál", "Nepal", "Nepal", "Nepal", "Népal ", "Nepal", "Nepal");
			SeedCountryData(context, "NR", "Nauru", "Nauru", "Nauru", "Nauru", "Nauru", "Nauru", "Nauru");
			SeedCountryData(context, "NU", "Niue", "Niue", "Niue", "Niue", "Nioué ", "Niue", "Niue");
			SeedCountryData(context, "NZ", "Nový Zéland", "Neuseeland", "New Zealand", "Nueva Zelanda", "Nouvelle-Zélande ", "Nowa Zelandia", "Nya Zeeland");
			SeedCountryData(context, "OM", "Omán", "Oman", "Oman", "Omán", "Oman", "Oman", "Oman");
			SeedCountryData(context, "PA", "Panama", "Panama", "Panama", "Panamá", "Panama", "Panama", "Panama");
			SeedCountryData(context, "PE", "Peru", "Peru", "Peru", "Perú", "Pérou", "Peru", "Peru");
			SeedCountryData(context, "PF", "Francouzská Polynésie", "Französisch-Polynesien", "French Polynesia", "Polinesia Francesa", "Polynésie française", "Polinezja Francuska", "Franska Polynesien");
			SeedCountryData(context, "PG", "Papua Nová Guinea", "Papua-Neuguinea", "Papua New Guinea", "Papúa Nueva Guinea", "Papouasie-Nouvelle-Guinée", "Papua Nowa Gwinea", "Papua Nya Guinea");
			SeedCountryData(context, "PH", "Filipíny", "Philippinen", "Philippines", "Filipinas", "Philippines", "Filipiny", "Filippinerna");
			SeedCountryData(context, "PK", "Pákistán", "Pakistan", "Pakistan", "Pakistán", "Pakistan", "Pakistan", "Pakistan");
			SeedCountryData(context, "PL", "Polsko", "Polen", "Poland", "Polonia", "Polande", "Polska", "Polen");
			SeedCountryData(context, "PR", "Portoriko", "Puerto Rico", "Puerto Rico", "Puerto Rico", "Porto Rico", "Portoryko", "Puerto Rico");
			SeedCountryData(context, "PS", "Palestina", "Palästina", "Palestine", "Palestina", "Palestine", "Palestyna", "Palestina");
			SeedCountryData(context, "PT", "Portugalsko", "Portugal", "Portugal", "Portugal", "Portugal", "Portugalia", "Portugal");
			SeedCountryData(context, "PW", "Palau", "Palau", "Palau", "Palaos", "Palaos ", "Palau", "Palau");
			SeedCountryData(context, "PY", "Paraguay", "Paraquay", "Paraguay", "Paraguay", "Paraguay", "Paragwaj", "Paraguay");
			SeedCountryData(context, "QA", "Katar", "Qatar", "Qatar", "Qatar", "Qatar", "Katar", "Qatar");
			SeedCountryData(context, "RE", "Réunion", "Réunion", "Réunion", "Reunión", "Réunion", "Réunion", "Réunion");
			SeedCountryData(context, "RO", "Rumunsko", "Rumänien", "Romania", "Rumanía", "Roumanie ", "Rumunia", "Rumänien");
			SeedCountryData(context, "RS", "Srbsko", "Serbien", "Serbia", "Serbia", "Serbie", "Serbia", "Serbien");
			SeedCountryData(context, "RU", "Ruská federace", "Russische Föderation (Russland)", "Russian Federation", "Federación de Rusia", "Fédération de Russie", "Rosja", "Ryssland");
			SeedCountryData(context, "RW", "Rwanda", "Ruanda", "Rwanda", "Ruanda", "Rwanda", "Rwanda", "Rwanda");
			SeedCountryData(context, "SA", "Saúdská Arábie", "Saudi-Arabien", "Saudi Arabia", "Arabia Saudí", "Arabie saoudite", "Arabia Saudyjska", "Saudiarabien");
			SeedCountryData(context, "SB", "Šalamounovy ostrovy", "Salomon-Inseln", "Islas Salomón", "Islas Salomón", "Îles Salomon", "Wyspy Salomona", "Salomonöarna");
			SeedCountryData(context, "SC", "Seychely", "Seychellen", "Seychelles", "Seychelles", "Seychelles", "Seszele", "Seychellerna");
			SeedCountryData(context, "SD", "Súdán", "Sudan", "Sudan", "Sudán", "Soudan", "Sudan", "Sudan");
			SeedCountryData(context, "SE", "Švédsko", "Schweden", "Sweden", "Suecia", "Suède", "Szwecja", "Sverige");
			SeedCountryData(context, "SG", "Singapur", "Singapur", "Singapore", "Singapur", "Singapour", "Singapur", "Singapore");
			SeedCountryData(context, "SH", "Sv. Helena", "St. Helena", "St Helena", "Santa Elena", "Sainte-Hélène", "Wyspa Swietej Heleny", "Sankta Helena");
			SeedCountryData(context, "SI", "Slovinsko", "Slowenien", "Slovenia", "Eslovenia", "Slovénie", "Slowenia", "Slovenien");
			SeedCountryData(context, "SK", "Slovensko", "Slowakei", "Slovakia", "Eslovaquia", "Slovaquie ", "Slowacja", "Slovakien");
			SeedCountryData(context, "SL", "Sierra Leone", "Sierra Leone", "Sierra Leone", "Sierra Leona", "Sierra Leone", "Sierra Leone", "Sierra Leone");
			SeedCountryData(context, "SM", "San Marino", "San Marino", "San Marino", "San Marino", "Saint-Marin", "San Marino", "San Marino");
			SeedCountryData(context, "SN", "Senegal", "Senegal", "Senegal", "Senegal", "Sénégal", "Senegal", "Senegal");
			SeedCountryData(context, "SO", "Somálsko", "Somalia", "Somalia", "Somalia", "Somalie", "Somalia", "Somalia");
			SeedCountryData(context, "SR", "Surinam", "Surinam", "Suriname", "Surinam", "Suriname", "Surinam", "Surinam");
			SeedCountryData(context, "ST", "São Tomé", "São Tomé", "São Tomé", "Santo Tomé", "São Tomé", "Wyspa Swietego Tomasza", "São Tomé");
			SeedCountryData(context, "SV", "El Salvador", "El Salvador", "El Salvador", "El Salvador", "El Salvador", "Salwador", "El Salvador");
			SeedCountryData(context, "SX", "Saint Martin", "St. Martin", "Saint Martin", "San Martín", "Saint-Martin", "Saint-Martin", "Saint Martin");
			SeedCountryData(context, "SY", "Syrská arabská republika", "Syrische Arabische Republik (Syrien)", "Syrian Arab Republic", "República Árabe de Siria", "République arabe syrienne", "Syria", "Syrien");
			SeedCountryData(context, "SZ", "Svazijsko", "Swasiland", "Swaziland", "Suazilandia", "Swaziland", "Suazi", "Swaziland");
			SeedCountryData(context, "TC", "Turkovy a Caicosovy ostrovy", "Turks- und Caicosinseln", "Turks and Caicos Islands", "Islas Turcas y Caicos", "Îles Turques-et-Caïques", "Turks i Caicos", "Turks- och Caicosöarna");
			SeedCountryData(context, "TD", "Cad", "Tschad", "Chad", "Chad", "Tchad", "Czad", "Tchad");
			SeedCountryData(context, "TG", "Togo", "Togo", "Togo", "Togo", "Togo", "Togo", "Togo");
			SeedCountryData(context, "TH", "Thajsko", "Thailand", "Thailand", "Tailandia", "Thaïlande ", "Tajlandia", "Thailand");
			SeedCountryData(context, "TJ", "Tádžikistán", "Tadschikistan", "Tajikistan", "Tayikistán", "Tadjikistan ", "Tadzykistan", "Tadzjikistan");
			SeedCountryData(context, "TK", "Tokelau", "Tokelau", "Tokelau", "Tokelau", "Tokélaou ", "Tokelau", "Tokelauöarna");
			SeedCountryData(context, "TL", "Východní Timor", "Osttimor", "East Timor", "Timor Oriental", "Timor oriental", "Timor Wschodni", "Östtimor");
			SeedCountryData(context, "TM", "Turkmenistán", "Turkmenistan", "Turkmenistan", "Turkmenistán", "Turkménistan", "Turkmenistan", "Turkmenistan");
			SeedCountryData(context, "TN", "Tunisko", "Tunesien", "Tunisia", "Túnez", "Tunisie", "Tunezja", "Tunisien");
			SeedCountryData(context, "TO", "Tonga", "Tonga", "Tonga", "Tonga", "Tonga", "Tonga", "Tonga");
			SeedCountryData(context, "TR", "Turecko", "Türkey", "Turkey", "Turquía", "Turquie", "Turcja", "Turkiet");
			SeedCountryData(context, "TT", "Trinidad a Tobago", "Trinidad und Tobago", "Trinidad and Tobago", "Trinidad y Tobago", "Trinité-et-Tobago ", "Trynidad i Tobago", "Trinidad och Tobago");
			SeedCountryData(context, "TV", "Tuvalu", "Tuvalu", "Tuvalu", "Tuvalu", "Tuvalu", "Tuvalu", "Tuvalu");
			SeedCountryData(context, "TW", "Tchaj-wan", "Taiwan", "Taiwan", "Taiwán", "Taïwan ", "Tajwan", "Taiwan");
			SeedCountryData(context, "TZ", "Tanzanie", "Tansania", "Tanzania", "Tanzania", "Tanzanie", "Tanzania", "Tanzania");
			SeedCountryData(context, "UA", "Ukrajina", "Ukraine", "Ukraine", "Ucrania", "Ukraine", "Ukraina", "Ukraina");
			SeedCountryData(context, "UG", "Uganda", "Uganda", "Uganda", "Uganda", "Ouganda", "Uganda", "Uganda");
			SeedCountryData(context, "US", "Spojené státy americké", "USA -Vereinigte Staaten von Amerika", "United States of America", "Estados Unidos de América", "États-Unis d'Amérique", "Stany Zjednoczone Ameryki", "Amerikas förenta stater");
			SeedCountryData(context, "UY", "Uruguay", "Uruguay", "Uruguay", "Uruguay", "Uruguay", "Urugwaj", "Uruguay");
			SeedCountryData(context, "UZ", "Uzbekistán", "Usbekistan", "Uzbekistan", "Uzbekistán", "Ouzbékistan ", "Uzbekistan", "Uzbekistan");
			SeedCountryData(context, "VC", "Saint Vincent a Grenadines", "St. Vincent und die Grenadien", "Saint Vincent and the Grenadines", "San Vincente y las Granadinas", "Saint-Vincent-et-les-Grenadines ", "Saint Vincent i Grenadyny", "Saint Vincent och Grenadinerna");
			SeedCountryData(context, "VE", "Venezuela", "Venezuela", "Venezuela", "Venezuela", "Venezuela", "Wenezuela", "Venezuela");
			SeedCountryData(context, "VG", "Panenské ostrovy", " Jungferninseln", "Virgin Islands", "Islas Vírgenes", "Îles Vierges", "Wyspy Dziewicze", "Jungfruöarna");
			SeedCountryData(context, "VI", "Panenské ostrovy (USA)", "US Jungferninseln", "US Virgin Islands", "Islas Vírgenes de los Estados Unidos", "Îles Vierges américaines", "Wyspy Dziewicze Stanów Zjednoczonych", "Amerikanska Jungfruöarna");
			SeedCountryData(context, "VN", "Vietnam", "Vietnam", "Vietnam", "Vietnam", "Viet Nam", "Wietnam", "Vietnam");
			SeedCountryData(context, "VU", "Vanuatu", "Vanuatu", "Vanuatu", "Vanuatu", "Vanuatu", "Vanuatu", "Vanuatu");
			SeedCountryData(context, "WF", "Teritorium ostrovu Wallis a Futuna", "Wallis und Futuna Inseln", "Territory of the Wallis and Futuna Islands", "Territorio de las Islas Wallis y Futuna", "Territoire des îles Wallis et Futuna", "Wallis i Futuna", "Wallis och Futunaöarna");
			SeedCountryData(context, "WS", "Samoa", "Samoa", "Samoa", "Samoa", "Samoa", "Samoa", "Samoa");
			SeedCountryData(context, "YE", "Jemen", "Jemen", "Yemen", "Yemen", "Yémen", "Jemen", "Jemen");
			SeedCountryData(context, "YT", "Mayotte", "Mayotte", "Mayotte", "Mayotte", "Mayotte", "Majotta", "Mayotte");
			SeedCountryData(context, "YU", "Srbsko a Cerná hora", "Serbien und Montenegro", "Serbia and Montenegro", "Serbia y Montenegro", "Serbie-et-Monténégro", "Serbia i Czarnogóra", "Serbien och Montenegro");
			SeedCountryData(context, "ZA", "Jižní Afrika", "Südafrika", "South Africa", "Sudáfrica", "Afrique du Sud", "RPA", "Sydafrika");
			SeedCountryData(context, "ZM", "Zambie", "Sambia", "Zambia", "Zambia", "Zambie", "Zambia", "Zambia");
			SeedCountryData(context, "ZW", "Zimbabwe", "Simbabwe", "Zimbabwe", "Zimbabue", "Zimbabwé", "Zimbabwe", "Zimbabwe");
			SeedCountryData(context, "ZZ", "Neznámé", "unbekannt", "Unknown", "Desconocido", "Inconnu", "Nieznany", "Okänd");
		}

		private static void SeedCountryData(KnightFrankContext context, string isoCode, 
											string csValue, string deValue, string enValue, string esValue,
											string frValue, string plValue, string svValue)
		{
			var country = new Country { IsoCode = isoCode };

			context.Country.AddOrUpdate(x => x.IsoCode, country);
			context.SaveChanges();

			var countryLocaliseds = new[]
			{
				new CountryLocalised
				{
					CountryId = country.Id,
					LocaleId = CsLocaleId,
					Value = csValue
				},
				new CountryLocalised
				{
					CountryId = country.Id,
					LocaleId = DeLocaleId,
					Value = deValue
				},
				new CountryLocalised
				{
					CountryId = country.Id,
					LocaleId = EnLocaleId,
					Value = enValue
				},
				new CountryLocalised
				{
					CountryId = country.Id,
					LocaleId = EsLocaleId,
					Value = esValue
				},
				new CountryLocalised
				{
					CountryId = country.Id,
					LocaleId = FrLocaleId,
					Value = frValue
				},
				new CountryLocalised
				{
					CountryId = country.Id,
					LocaleId = PlLocaleId,
					Value = plValue
				},
				new CountryLocalised
				{
					CountryId = country.Id,
					LocaleId = SvLocaleId,
					Value = svValue
				}
			};
			context.CountryLocalised.AddOrUpdate(x => new { x.LocaleId, x.CountryId }, countryLocaliseds.ToArray());
			context.SaveChanges();
		}
	}
}