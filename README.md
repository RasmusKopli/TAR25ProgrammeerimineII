# TAR25ProgrammeerimineII

<h2>Milline on DbContext-i ülesanne?</h2>
DbContext on keskne klass mis aitab ühendada C# koodi andmebaasiga.


<h2>Miks ei ole hea paigutada kõiki andmebaasipäringuid otse kasutajaliidese koodi?</h2>
Sellega võib tekkida raske arendada, võib tekkida andme lekkeid kasutaja poolel, selle testimine võib muutuda aeglasemaks ja on raskem testida.


<h2>Millisesse kihti paigutaksid andmebaasiga suhtlemise MVC- või VIPER-arhitektuuris? Põhjenda oma vastust.</h2>
MVC-arhitektuuris paigutaks model kihti, sest MVC-s esindab model andmeid--
