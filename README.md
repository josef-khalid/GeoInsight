# GeoInsight
GeoInsight

GeoInsight är en webbapplikation byggd med ASP.NET Core Razor Pages där användaren kan skriva in en plats, till exempel en stad eller ett land, och få en samlad översikt över:

aktuellt väder på platsen

information om landet (till exempel befolkning, yta, valuta och språk)

kryptopriser (Bitcoin och Ethereum) i landets valuta om möjligt

### Tekniskt upplägg ### 

Projektet är uppdelat i flera lager:

Pages (UI): Razor Pages (Index.cshtml och Index.cshtml.cs) som tar emot användarens input och visar resultatet.

Business-lager: GeoInsightService som samordnar anrop till olika tjänster och bygger en GeoInsightViewModel.

Services: separata tjänster för geokodning, väder, landinformation och krypto som anropar externa API:er via HttpClient.

Models: dataklasser som beskriver väder-, land- och kryptoinformation samt vy-modellen.

### Köra projektet ###

1-Öppna lösningen i Visual Studio.
2-Välj rätt startprojekt om det finns flera.
3-Kör med F5 eller via Debug → Start Debugging.
4-Skriv in en plats i sökfältet på startsidan och klicka på “Sök”.

### Contributors ###
- Josef Khalid (Pages)
- Ayham Alshehabi (Business, models)
- Ali Wafa (services)

  
