# HistoricalEventsCase
![Animation1](https://user-images.githubusercontent.com/58739068/180646396-2f7733b7-5782-4f4b-9921-89d25e50e994.gif)

# Tasks
iki adet *.json dosyamız var. 
 - [x] Bir endpoint yardımıyla bu bilgileri import etmek istiyoruz. 
Dosyalar türkçe ve italyanca bilgiler içermekte. 
- [x] Dosyaları import et! 
	**I used webclient for reading json files. i also imported files by using staticfiles but it has not been needed. maybe i will sooner**
	sonra 
- [x] _Request Localization_ yardımıyla 
	- [x] türkçe kullanıcılar için türkçe 
	- [x] italyanca kullanıcılar için italyanca 
	bilgilerin sorgulanabilmesini istiyoruz.

### Gerekli teknolojiler;

-   [Asp.Net](http://Asp.Net) Core **I used .Net6**
-   Entity Framework Core
-   Any Relational Database (MySql, PostgreSql, MsSql etc.) ** I used sqlite for portabelity **

### **Ödevi gerçekleştirirken beklediğimiz temel yapıları maddeler halinde özetledik;**

-   Request Localization
-   Authentication **i used jwt and basic auth**
-   Cache (In-memory or Distributed) ** I used in-memory **
-   Exception Handler
-   Api & Code documentation

### Olursa harika olur dediklerimiz;

-   Unit Test
-   Object Mapping
-   Validation
-   Git Usage **i used `git flow`**
-   Code Complexity **i wanted to implement clean achitecture but there wasn't enough time and i guess this case isn't so complex...**
-   Code Readability
-   Multi-Tenancy
