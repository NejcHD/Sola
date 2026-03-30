using Grpc.Net.Client;
using GrpcOseba;
using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

// Naslov gRPC strežnika (preverite, če je to vaš pravilen port: npr. 7001)
const string ServerAddress = "https://localhost:7001";

// --- Pomožna funkcija za lepši izpis ---
void IzpisiOsebo(OsebaMessage oseba, string operacija)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n--- {operacija} USPEŠNO ({oseba.Id}) ---");
    Console.ResetColor();
    Console.WriteLine($"Ime: {oseba.Ime} {oseba.Priimek}, Rojena: {oseba.DatumRojstva.ToDateTime().ToShortDateString()}");
}

// --- Pomožna funkcija za izpis vseh oseb ---
async Task IzpisiVseOsebe(OsebeService.OsebeServiceClient client)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\n==============================================");
    Console.WriteLine("PRIDOBIVANJE VSEH OSEB");
    Console.WriteLine("==============================================");
    Console.ResetColor();

    var reply = await client.PridobiVseOsebeAsync(new Empty());

    if (reply.Osebe.Count == 0)
    {
        Console.WriteLine("Seznam oseb je prazen.");
        return;
    }

    foreach (var oseba in reply.Osebe)
    {
        IzpisiOsebo(oseba, "PRIDOBITEV");
    }
}

// --------------------------------------------------------------------------------
// GLAVNA LOGIKA (CRUD DEMONSTRACIJA)
// --------------------------------------------------------------------------------

using var channel = GrpcChannel.ForAddress(ServerAddress);
var client = new OsebeService.OsebeServiceClient(channel);

Console.WriteLine($"Povezovanje na gRPC strežnik na naslovu: {ServerAddress}\n");

// 1. USTVARJANJE OSEB (CREATE)
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("--- 1. USTVARJANJE NOVIH OSEB ---");
Console.ResetColor();

var novaOseba1 = new OsebaMessage
{
    Ime = "Marko",
    Priimek = "Zupan",
    DatumRojstva = Timestamp.FromDateTime(new DateTime(1995, 8, 1).ToUniversalTime())
};

var novaOseba2 = new OsebaMessage
{
    Ime = "Petra",
    Priimek = "Horvat",
    DatumRojstva = Timestamp.FromDateTime(new DateTime(2000, 3, 22).ToUniversalTime())
};

// Dodajanje 1. osebe (Marko)
var reply1 = await client.UstvariOseboAsync(novaOseba1);
IzpisiOsebo(reply1, "USTVARJANJE");

// Dodajanje 2. osebe (Petra)
var reply2 = await client.UstvariOseboAsync(novaOseba2);
IzpisiOsebo(reply2, "USTVARJANJE");

int markoId = reply1.Id; // Shranimo ID za nadaljnje operacije

// 2. PRIDOBIVANJE VSEH OSEB (READ ALL)
await IzpisiVseOsebe(client);


// 3. POSODABLJANJE OSEBE (UPDATE)
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("\n--- 3. POSODABLJANJE OSEBE ---");
Console.ResetColor();

var posodobljenaOseba = new OsebaMessage
{
    Id = markoId,
    Ime = "Marko Posodobljen", // Nova vrednost
    Priimek = reply1.Priimek,
    DatumRojstva = reply1.DatumRojstva // Ostalo ostane enako
};

var posodobljenReply = await client.PosodobiOseboAsync(posodobljenaOseba);
IzpisiOsebo(posodobljenReply, "POSODOBITEV");


// 4. PRIDOBIVANJE POSAMEZNE OSEBE (READ BY ID)
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("\n--- 4. PRIDOBIVANJE POSAMEZNE OSEBE ---");
Console.ResetColor();

try
{
    var request = new OsebaIdRequest { Id = markoId };
    var osebaMarko = await client.PridobiOseboAsync(request);
    IzpisiOsebo(osebaMarko, "PRIDOBITEV");
}
catch (RpcException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Napaka pri pridobivanju: {ex.Status.Detail}");
    Console.ResetColor();
}


// 5. BRISANJE OSEBE (DELETE)
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("\n--- 5. BRISANJE OSEBE ---");
Console.ResetColor();

try
{
    var deleteRequest = new OsebaIdRequest { Id = markoId };
    await client.IzbrisiOseboAsync(deleteRequest);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Oseba z ID {markoId} uspešno izbrisana.");
    Console.ResetColor();
}
catch (RpcException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Napaka pri brisanju: {ex.Status.Detail}");
    Console.ResetColor();
}

// 6. KONČNI PREGLED (Marko bi moral manjkati)
await IzpisiVseOsebe(client);

Console.WriteLine("\nDemonstracija CRUD operacij zaključena.");