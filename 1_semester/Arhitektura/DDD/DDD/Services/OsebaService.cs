using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcOseba;
using Google.Protobuf.WellKnownTypes;

// --- 1. C# ENTITETA OSEBA (DOMENSKI MODEL) ---
public class Oseba
{
    public int Id { get; set; }
    public string Ime { get; set; }
    public string Priimek { get; set; }
    public DateTime DatumRojstva { get; set; }

    // Preslikava iz C# modela v Protobuf sporočilo (za POŠILJANJE k odjemalcu)
    public OsebaMessage ToProto()
    {
        return new OsebaMessage
        {
            Id = this.Id,
            Ime = this.Ime,
            Priimek = this.Priimek,
            // Pretvori C# DateTime v Protobuf Timestamp
            DatumRojstva = Timestamp.FromDateTime(this.DatumRojstva.ToUniversalTime())
        };
    }

    // Preslikava iz Protobuf sporočila v C# model (za SHRANJEVANJE na strežniku)
    public static Oseba FromProto(OsebaMessage message)
    {
        return new Oseba
        {
            Id = message.Id,
            Ime = message.Ime,
            Priimek = message.Priimek,
            // Pretvori Protobuf Timestamp nazaj v C# DateTime
            DatumRojstva = message.DatumRojstva.ToDateTime()
        };
    }
}

// --- 2. GRPc SERVICE IMPLEMENTACIJA ---
// Uporabimo statično listo za shranjevanje podatkov (in-memory persistence)
public class OsebeServiceImpl : OsebeService.OsebeServiceBase
{
    private static readonly List<Oseba> _osebe = new List<Oseba>()
    {
        // Začetni podatki
        new Oseba { Id = 1, Ime = "Ana", Priimek = "Novak", DatumRojstva = new DateTime(1990, 5, 15) },
        new Oseba { Id = 2, Ime = "Jana", Priimek = "Kovač", DatumRojstva = new DateTime(1985, 10, 20) }
    };
    private static int _nextId = 3;

    // CREATE (UstvariOsebo)
    public override Task<OsebaMessage> UstvariOsebo(OsebaMessage request, ServerCallContext context)
    {
        var novaOseba = Oseba.FromProto(request);
        novaOseba.Id = _nextId++;

        _osebe.Add(novaOseba);
        Console.WriteLine($"[STREŽNIK] Ustvarjena nova oseba: {novaOseba.Ime} z ID {novaOseba.Id}");

        return Task.FromResult(novaOseba.ToProto());
    }

    // READ BY ID (PridobiOsebo)
    public override Task<OsebaMessage> PridobiOsebo(OsejaIdRequest request, ServerCallContext context)
    {
        var oseba = _osebe.FirstOrDefault(o => o.Id == request.Id);

        if (oseba == null)
        {
            // gRPC mehanizem za vračanje napake (Status.NotFound = 404 v REST)
            throw new RpcException(new Status(StatusCode.NotFound, $"Oseba z ID {request.Id} ni najdena."));
        }

        return Task.FromResult(oseba.ToProto());
    }

    // READ ALL (PridobiVseOsebe)
    public override Task<VseOsebeReply> PridobiVseOsebe(Empty request, ServerCallContext context)
    {
        var odgovor = new VseOsebeReply();

        // Preslikaj seznam C# objektov v seznam Protobuf sporočil
        odgovor.Osebe.AddRange(_osebe.Select(o => o.ToProto()));
        Console.WriteLine($"[STREŽNIK] Vrnjen seznam {odgovor.Osebe.Count} oseb.");

        return Task.FromResult(odgovor);
    }

    // UPDATE (PosodobiOsebo)
    public override Task<OsebaMessage> PosodobiOsebo(OsebaMessage request, ServerCallContext context)
    {
        var obstojece = _osebe.FirstOrDefault(o => o.Id == request.Id);

        if (obstojece == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Oseba z ID {request.Id} ni najdena za posodobitev."));
        }

        // Posodobitev C# modela s podatki iz Protobuf sporočila
        obstojece.Ime = request.Ime;
        obstojece.Priimek = request.Priimek;
        obstojece.DatumRojstva = request.DatumRojstva.ToDateTime();

        Console.WriteLine($"[STREŽNIK] Posodobljena oseba z ID: {obstojece.Id}");
        return Task.FromResult(obstojece.ToProto());
    }

    // DELETE (IzbrisiOsebo)
    public override Task<Empty> IzbrisiOsebo(OsebaIdRequest request, ServerCallContext context)
    {
        var oseba = _osebe.FirstOrDefault(o => o.Id == request.Id);

        if (oseba == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Oseba z ID {request.Id} ni najdena za brisanje."));
        }

        _osebe.Remove(oseba);
        Console.WriteLine($"[STREŽNIK] Izbrisana oseba z ID: {request.Id}");

        return Task.FromResult(new Empty());
    }
}