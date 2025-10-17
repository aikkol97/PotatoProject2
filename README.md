# PotatoProject2 API

Μια απλή αλλά ολοκληρωμένη RESTful API φτιαγμένη με .NET 8 και FastEndpoints,  
η οποία επιτρέπει τη διαχείριση "πατατών" (Potatoes) και τη σύνδεση χρηστών μέσω JWT Authentication.  
Το project περιλαμβάνει endpoints για εγγραφή/είσοδο χρηστών, δημιουργία και ανάκτηση πατατών, καθώς και πλήρη υποστήριξη Swagger UI για εύκολο testing.

---

## Χαρακτηριστικά

-  Χρησιμοποιεί FastEndpoints για καθαρή και απλή αρχιτεκτονική
-  JWT Authentication για ασφαλή πρόσβαση
-  MySQL βάση δεδομένων μέσω Entity Framework Core
-  Swagger UI ενσωματωμένο για εύκολη εξερεύνηση API
-  Γρήγορη ανάπτυξη και εύκολη επέκταση

---

## Endpoints

###  Login
POST `/api/login`  
Επιστρέφει JWT Token για τον χρήστη.

Body:
```json
{
  "username": "admin",
  "password": "12345"
}
