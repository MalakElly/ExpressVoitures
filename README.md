# Express Voitures

Application de gestion de voitures pour une entreprise spécialisée dans l’achat, la réparation et la revente de véhicules.**

##  Contexte

L’entreprise achète des voitures aux enchères, effectue des réparations et revend les véhicules avec un bénéfice modéré basé sur un volume important.  
Cette application permet de gérer l’inventaire des voitures, les transactions d’achat/vente et les réparations associées.

---

##  Technologies utilisées

- **ASP.NET Core MVC** – Framework web robuste et sécurisé
- **C#** – Langage de développement
- **Entity Framework Core** – Accès aux données 
- **SQL Server** – Base de données relationnelle
- **Bootstrap** – Interface utilisateur responsive
- **Git** – Gestion du code source

---

##  Fonctionnalités principales

 Ajout, modification, suppression de voitures  
 Gestion des transactions (prix d’achat, prix de vente, dates)  
 Suivi des réparations : type, coût, date  
 Upload et gestion des images des véhicules  
 Filtrage des voitures :
- Par marque
- Par modèle
- Par année
- Par prix maximum  

 Sécurité : accès en écriture réservé à l’administrateur  
Validation côté client et serveur (prix positifs, dates cohérentes...)

---

## Modèle de données

- **Car**
  - Code VIN
  - Marque
  - Modèle
  - Année
  - Kilométrage
  - Image

- **Transaction**
  - Prix d’achat
  - Date d’achat
  - Prix de vente
  - Date de vente
  - Disponibilité

- **Repairing**
  - Type
  - Coût
  - Date

---




