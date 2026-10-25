# Toitumispäevik (Nutrition tracker) .NET MAUI App

Mitmekeelne toitumispäeviku rakendus, mis on loodud .NET MAUI ja C# abil MVVM arhitektuurimustrit kasutades.
Rakendus võimaldab kasutajatel hallata oma igapäevaseid toidukordi ja arvutada automaatselt päeva kogukaloreid.
Samuti on rakenduses võimalik kohandada keelt ja värviteemat.

Rakendus sisaldab ka liidestust USDA FoodData Central API-ga, et hankida päevakohast infot iga toiduaine toitumisalase
teabe kohta. Toiduaine sisestades tehake päring API-le ning saadud vastust kasutatakse, et välja arvutada
toiduaine kaloraaž kasutaja sisestatud koguses.

Kirjed salvestatakse vastavalt valitud toidukorrale valitud kuupäeva alla. Hetkel on vaikimisi kolm toidukorra valikut.

## Kasutatud tehnoloogiad
<li>.NET 10 (.NET MAUI)</li>
<li>SQlite v11.1.285</li>
<li>Android Studio Rabbit 1 | 2026.2.1</li>
<li>Android Emulator 37.1.11</li>
<li>Android 16.0 (API level 36.0)</li>