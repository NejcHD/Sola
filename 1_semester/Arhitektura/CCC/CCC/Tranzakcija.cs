namespace CCC
{
    public class Tranzakcija
    {
        public class PosodobitevStudentaInNalogeDto
        {
            public int StudentId { get; set; }
            public int NovaStarost { get; set; } // Podatek za tabelo 'Student'
            public int NalogaId { get; set; }
            public bool NovoStanjeNaloge { get; set; } = true; // Podatek za tabelo 'Naloga'
        }



    }

    

}
