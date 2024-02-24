public class Customer : People, ISphereOfOrder
{
    private string adress, ID, AgronomistFN;
    public Customer(string FullName, string PhoneNumber, string adress, string ID, string AgronomistFN) 
        : base(FullName, PhoneNumber)
    {
        this.adress = adress;
        this.FullName = FullName;
        this.PhoneNumber = PhoneNumber;
        this.ID = ID;
        this.AgronomistFN = AgronomistFN;
    }

    public string getAdress() { return adress; }   
    public string getAgronomisFN() { return AgronomistFN; }
    public string getCustomerID() { return ID; }   
    public void setAdress(string adress) { this.adress = adress; }  
    public void setCustomerID(string ID) { this.ID = ID; }

    public override string GiveInfo()
    {
        string FullInfoAbout =  "Повне ім'я замовника: " + this.getPeopleFullName() +
            "\nАдрес замовника: "+ this.getAdress() +
            "\nНомер телефону замовника: " + this.getPeoplePhoneNumber() + 
            "\nІD замовника: " + this.getCustomerID() +
            "\nАгроном який буде опрацьовувати замовлення: " + this.getAgronomisFN();

        return FullInfoAbout;
    }

}

