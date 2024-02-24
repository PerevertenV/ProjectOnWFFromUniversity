public abstract class People //абстрактний клас Люди
{ 
    protected  string FullName;// Повне Ім'я
    protected string PhoneNumber;//Номер Телефону
    protected People(string FullName, string PhoneNumber) // конструктор для даного класу
    {
        this.FullName = FullName;
        this.PhoneNumber = PhoneNumber;
    }   
    public string getPeopleFullName() { return this.FullName; }
    public string getPeoplePhoneNumber() { return this.PhoneNumber; }
    public void setPeopleFullName(string NewFullName) { this.FullName = NewFullName; }
    public void setPeoplePhoneNumber(string NewPhoneNumber) { this.PhoneNumber = NewPhoneNumber; }

    public virtual string GiveInfo() 
    { 
        string FullInfoAbout = this.getPeopleFullName()+"\\"+this.getPeoplePhoneNumber() + "\n"; 
        return FullInfoAbout; 
    }
}