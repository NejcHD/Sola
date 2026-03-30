using Grpc.Core;
using GrpcService1.Grpc; // Generiran namespace iz .proto datoteke

namespace GrpcService1.Services
{
    // DEDUJEMO iz generiranega baznega razreda
    public class AvtorService : Avtorji.AvtorjiBase 
    {
        // 1. SIMULACIJA BAZE (za začetek)
        private static List<Avtor> avtorji = new List<Avtor>();
        private static int nextId = 1;

        // 2. IMPLEMENTACIJA USTVARI AVTORJA (POST/Create)
        public override Task<AvtorResponse> UstvariAvtorja(UstvariAvtorjaRequest request, ServerCallContext context)
        {
            var novAvtor = new Avtor
            {
                Id = nextId++,
                Ime = request.Ime,
                Priimek = request.Priimek
            };
            
            avtorji.Add(novAvtor);
            
            return Task.FromResult(new AvtorResponse
            {
                Avtor = novAvtor,
                Sporocilo = $"Avtor {novAvtor.Ime} {novAvtor.Priimek} uspešno ustvarjen z ID: {novAvtor.Id}"
            });
        }

        // 3. IMPLEMENTACIJA PRIDOBI AVTORJA (GET/Read)
        public override Task<AvtorResponse> PridobiAvtorja(PridobiAvtorjaRequest request, ServerCallContext context)
        {
            var avtor = avtorji.FirstOrDefault(a => a.Id == request.Id);

            if (avtor == null)
            {
                // V gRPC za napake uporabljamo RpcException (Statusne kode, ki jih lahko obdela klient)
                throw new RpcException(new Status(StatusCode.NotFound, $"Avtor z ID {request.Id} ni najden."));
            }

            return Task.FromResult(new AvtorResponse
            {
                Avtor = avtor,
                Sporocilo = "Uspešno pridobljeno."
            });
        }
    }
}