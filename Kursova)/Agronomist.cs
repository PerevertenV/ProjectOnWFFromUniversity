public class Agronomist : People //
{
    public static string BrunchName = "AgroWorld"; //
    public Agronomist(string FullName, string PhoneNumber) : base(FullName, PhoneNumber)
    {
        this.FullName = FullName;
        this.PhoneNumber = PhoneNumber;
        
    } //

    public static string getBrunchName() { return BrunchName; }

    public override string GiveInfo()//
    {
        string FullInfoAbout = "Повне ім'я замовника: " +this.getPeopleFullName() 
            + "\nНомер телефону агронома: "+ this.getPeoplePhoneNumber()
            + "\nНазва компанії: " + Agronomist.getBrunchName();
        return FullInfoAbout;
    }

}

