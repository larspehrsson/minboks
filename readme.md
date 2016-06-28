# MinBoks
Program til automatisk at hente dokumenter fra eBoks og gemme dem i en mappe og evt. også videresende dem til en valgfri mail adresse.

Programmet skal installeres som en windows service og poller derefter hver 4. time EBoks og henter nye dokumenter. 

####Elementer i .config:

Key      | Value | Description
-------- | ----- | -------------
response | | Dannes automatisk ved første kørsel
brugernavn | 1234567890 | cprnummer uden -
password | 01010101 | adgangskode til eboks
aktiveringskode | abcdefgh | aktiveringskode til eboks
deviceid |  | Dannes automatisk ved første kørsel
mailserver | smtp.gmail.com | DNS til mailserver
mailserverport | 587 | Port til mailserver
mailserverssl | True | Skal der bruges SSL til mailserver
mailfrom | <din email> | Hvem skal stå som afsender
mailto | <din email>  | Hvem skal modtage mail
mailserveruser | <din email> | Brugernavn til mailserver
mailserverpassword | secret | Password til mailserver
savepath | c:\temp\ | Hvor skal modtagne dokumenter gemmes
opbyghentet | True | Ved første kørsel kan denne sættes til True. Så vil den ikke hente noget men blot markere alle dokumenter som hentet. Sættes automatisk til false derefter.
retrydelay | 240 | Antal minutter mellem hver login til EBoks
downloadonly | False | Download uden at maile.


## Related projects

- [MinEBoks](https://github.com/larspehrsson/MinEBoks) by [Lars Pehrsson] windows gui version af denne.
- [e-boks-mailer](https://github.com/christianpanton/eboks-mailer) by [Christian Panton](https://twitter.com/christianpanton) is written in Python and works by scraping the mobile website. Can forward messages by email.
- [Net-Eboks](https://github.com/dk/Net-Eboks) by [Dmitry Karasik](https://twitter.com/dmitrykarasik) is written in perl and also uses the mobile app API. Can also expose documents through POP3. Dmitry even hosts an open server and promises that it will not store your credentials or your documents on his server.
- [Postboks](https://github.com/olegam/Postboks) af [Ole Gam](https://twitter.com/olegam) MacOS project syncs your e-Boks documents to a folder on your mac in the background. You never have to log in to the e-Boks website using NemID again.
