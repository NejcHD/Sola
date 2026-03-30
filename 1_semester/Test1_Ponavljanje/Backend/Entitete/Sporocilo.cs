namespace Backend.Entitete
{
    public class Sporocilo
    {
        public int Id { get; set; }
        public string Zadeva { get; set; }
        public string Vsebina { get; set; }
        public DateOnly CasPosiljanja { get; set; }
        public string PrejemnikEmail { get; set; }
        public Nabiralnik Prejemnik { get; set; }
        public string PosiljateljEmail { get; set; }
        public Nabiralnik Posiljatelj { get; set; }



         public void novoSporocilo(string prejemnikEmail, string posiljatelj, string zadeva, string vsebina )
        {
            Zadeva = zadeva;
            Vsebina = vsebina;
            PrejemnikEmail = prejemnikEmail;
            PosiljateljEmail = posiljatelj;

        }
       

        

    }




}
